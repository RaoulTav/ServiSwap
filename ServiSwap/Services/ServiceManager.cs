using ServiSwap.DAL.Interfaces;
using ServiSwap.Exceptions;
using ServiSwap.Models;

namespace ServiSwap.Services;

public class ServiceManager(IPersonDAL personDAL, IServiceDAL serviceDAL, IRequestedServiceDAL requestedServiceDAL)
{
    private readonly IPersonDAL personDAL = personDAL;
    private readonly IServiceDAL serviceDAL = serviceDAL;
    private readonly IRequestedServiceDAL requestedServiceDAL = requestedServiceDAL;

    public List<Service> SearchServices(int? categoryId, int? personId, double? distance)
    {
        double? latitude = null;
        double? longitude = null;
        double? effectiveDistance = distance;

        if (distance.HasValue && personId.HasValue)
        {
            Person person = personDAL.FindById(personId.Value) ?? throw new EntityNotFoundException("Person", personId.Value);

            latitude = person.Latitude;
            longitude = person.Longitude;
        } else
        {
            effectiveDistance = null;
        }

        return serviceDAL.SearchByCategoryAndOrDistance(latitude, longitude, categoryId, effectiveDistance);
    }

    public Service GetService(int serviceId) => serviceDAL.FindById(serviceId) ?? throw new EntityNotFoundException("Service", serviceId);

    public void ReportHours(int personId, int reqServiceId, int reportedHours)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(reportedHours);

        Person provider = personDAL.FindById(personId) ?? throw new EntityNotFoundException("Person", personId);

        RequestedService serviceProvided = requestedServiceDAL.FindById(reqServiceId)
            ?? throw new EntityNotFoundException("ReqService", reqServiceId);

        if (!serviceProvided.Provider.Equals(provider))
        {
            throw new BusinessException(ErrorCode.NotServiceProvider, "Only the provider of the requested service can report its hours.");
        }

        serviceProvided.ChangeState(RequestedServiceState.Accepted);
        serviceProvided.ChangeState(RequestedServiceState.Completed);
        serviceProvided.ReportedHours = reportedHours;
        
        requestedServiceDAL.ReportHours(serviceProvided);
    }
}