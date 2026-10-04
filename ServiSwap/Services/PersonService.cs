using ServiSwap.DAL.Interfaces;
using ServiSwap.Exceptions;
using ServiSwap.Models;

namespace ServiSwap.Services;

public class PersonService(IPersonDAL personDAL, IServiceDAL serviceDAL, IRequestedServiceDAL requestedServiceDAL)
{
    private readonly IPersonDAL personDAL = personDAL;
    private readonly IServiceDAL serviceDAL = serviceDAL;
    private readonly IRequestedServiceDAL requestedServiceDAL = requestedServiceDAL;

    public void RequestService(int personId, int serviceId)
    {
        Person? requester = personDAL.FindById(personId);

        ArgumentNullException.ThrowIfNull(requester);

        if(!requester.HasEnoughCredits())
        {
            throw new BusinessException(ErrorCode.NotEnoughCredits, "Requester doesn't have enough credits to request the service.");
        }

        Service? service = serviceDAL.FindById(serviceId);

        ArgumentNullException.ThrowIfNull(service);

        if(requester.Equals(service.Provider))
        {
            throw new BusinessException(ErrorCode.RequesterCannotBeProvider, "A person cannot request their own service.");
        }

        RequestedService requestedService = new(
            DateOnly.FromDateTime(DateTime.Now), 
            RequestedServiceState.Pending,
            requester,
            service
        );

        requestedServiceDAL.CreateRequestedService(requestedService);
    }
}