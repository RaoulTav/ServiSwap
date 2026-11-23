using ServiSwap.DAL.Interfaces;
using ServiSwap.Exceptions;
using ServiSwap.Models;

namespace ServiSwap.Services;

public class PersonService(
    IPersonDAL personDAL,
    IServiceDAL serviceDAL,
    IRequestedServiceDAL requestedServiceDAL,
    IGeocodingService geocodingService)
{
    private readonly IPersonDAL personDAL = personDAL;
    private readonly IServiceDAL serviceDAL = serviceDAL;
    private readonly IRequestedServiceDAL requestedServiceDAL = requestedServiceDAL;
    private readonly IGeocodingService geocodingService = geocodingService;

    public Person? Authenticate(string username, string password)
    {
        return personDAL.FindByCredentials(username, password);
    }

    public async Task<Person> CreateUser(string username, string password, string email, string lastname, string firstname,
        string streetnumber, string streetname, string city, CancellationToken cancellationToken = default)
    {
        if (personDAL.Exists(username, email))
        {
            throw new BusinessException(ErrorCode.UserAlreadyExist, "Another user is already using this username or email address.");
        }

        GeoCoordinates? coordinates = await geocodingService.GeocodeAddressAsync(streetnumber, streetname, city, cancellationToken) 
            ?? throw new BusinessException(ErrorCode.AddressNotFound, "The address could not be located.");

        Person person = new(
            username, 
            password, 
            lastname, 
            firstname, 
            email, 
            10, 
            streetnumber, 
            streetname, 
            city,
            coordinates.Latitude, 
            coordinates.Longitude
        );

        personDAL.CreatePerson(person);

        return person;
    }

    public void RequestService(int personId, int serviceId)
    {
        Person requester = personDAL.FindById(personId) ?? throw new EntityNotFoundException("Requester", personId);
    
        Service service = serviceDAL.FindById(serviceId) ?? throw new EntityNotFoundException("Service", serviceId);
    
        if (!requester.HasEnoughCredits())
        {
            throw new BusinessException(ErrorCode.NotEnoughCredits, "Requester doesn't have enough credits to request the service.");
        }
    
        if (requester.Equals(service.Provider))
        {
            throw new BusinessException(ErrorCode.RequesterCannotBeProvider, "A person cannot request their own service.");
        }
    
        requestedServiceDAL.FindAllByRequester(requester);

        if (requester.ServicesRequester.Any(rs => rs.Service.Equals(service)))
        {
            throw new BusinessException(ErrorCode.ServiceAlreadyRequested, "The service has already been requested by the person.");
        }
    
        RequestedService requestedService = new(
            DateOnly.FromDateTime(DateTime.Now),
            RequestedServiceState.Pending,
            requester,
            service);
    
        requestedServiceDAL.CreateRequestedService(requestedService);
    }
}