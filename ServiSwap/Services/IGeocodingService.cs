namespace ServiSwap.Services;

public interface IGeocodingService
{
    Task<GeoCoordinates?> GeocodeAddressAsync(string streetNumber, string streetName, string city, CancellationToken cancellationToken = default);
}