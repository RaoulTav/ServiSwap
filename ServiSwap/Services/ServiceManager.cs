using ServiSwap.DAL.Interfaces;
using ServiSwap.Exceptions;
using ServiSwap.Models;

namespace ServiSwap.Services;

public class ServiceManager(IPersonDAL personDAL, IServiceDAL serviceDAL)
{
    private readonly IPersonDAL personDAL = personDAL;
    private readonly IServiceDAL serviceDAL = serviceDAL;

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
}