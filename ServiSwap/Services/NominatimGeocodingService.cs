using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using ServiSwap.Exceptions;

namespace ServiSwap.Services;

public class NominatimGeocodingService(IHttpClientFactory httpClientFactory, IMemoryCache cache) : IGeocodingService
{
    private static readonly TimeSpan MinimumRequestInterval = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(30);

    private readonly SemaphoreSlim requestLock = new(1, 1);
    private DateTimeOffset lastRequestStarted = DateTimeOffset.MinValue;

    public async Task<GeoCoordinates?> GeocodeAddressAsync(string streetNumber, string streetName, string city, 
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"nominatim:{streetNumber.Trim().ToUpperInvariant()}|{streetName.Trim().ToUpperInvariant()}|{city.Trim().ToUpperInvariant()}";

        if (cache.TryGetValue(cacheKey, out GeoCoordinates? cachedCoordinates))
        {
            return cachedCoordinates;
        }

        await WaitForRequestTurnAsync(cancellationToken);

        string query = $"search?street={Uri.EscapeDataString($"{streetNumber} {streetName}")}" +
            $"&city={Uri.EscapeDataString(city)}&format=jsonv2&limit=1";

        try
        {
            using HttpResponseMessage response = await httpClientFactory.CreateClient("Geocoding").GetAsync(query, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            NominatimResult[]? results = await JsonSerializer.DeserializeAsync<NominatimResult[]>(responseStream, cancellationToken: cancellationToken);

            if (results is null || results.Length == 0)
            {
                return null;
            }

            NominatimResult result = results[0];
            if (!double.TryParse(result.Latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude)
                || !double.TryParse(result.Longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude)
                || latitude is < -90 or > 90 || longitude is < -180 or > 180)
            {
                throw new TechnicalException("The geocoding provider returned invalid coordinates.");
            }

            GeoCoordinates coordinates = new(latitude, longitude);
            cache.Set(cacheKey, coordinates, CacheDuration);

            return coordinates;
        }
        catch (HttpRequestException exception)
        {
            throw new TechnicalException("The geocoding provider could not process the address.", exception);
        }
        catch (JsonException exception)
        {
            throw new TechnicalException("The geocoding provider returned an invalid response.", exception);
        }
    }

    private async Task WaitForRequestTurnAsync(CancellationToken cancellationToken)
    {
        await requestLock.WaitAsync(cancellationToken);

        try
        {
            TimeSpan elapsed = DateTimeOffset.UtcNow - lastRequestStarted;
            TimeSpan waitTime = MinimumRequestInterval - elapsed;

            if (waitTime > TimeSpan.Zero)
            {
                await Task.Delay(waitTime, cancellationToken);
            }

            lastRequestStarted = DateTimeOffset.UtcNow;
        }
        finally
        {
            requestLock.Release();
        }
    }

    private record NominatimResult([property: JsonPropertyName("lat")] string Latitude, [property: JsonPropertyName("lon")] string Longitude);
}