using ServiSwap.Exceptions;

namespace ServiSwap.Models;

public class RequestedService
{
    public RequestedService(DateOnly dateOfRequest, RequestedServiceState state)
    {
        DateOfRequest = dateOfRequest;
        State = state;
    }

    public int RequestedServiceId
    {
        get;
        set
        {
            if(field != 0) throw new TechnicalException("RequestedService ID is already set.");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            field = value;
        }
    }

    public DateOnly DateOfRequest { get; set; }

    public RequestedServiceState State { get; private set; }

    public void ChangeState(RequestedServiceState newState)
    {
        bool validTransition = (State, newState) switch
        {
            (RequestedServiceState.Pending, RequestedServiceState.Accepted) => true,
            (RequestedServiceState.Pending, RequestedServiceState.Rejected) => true,
            (RequestedServiceState.Accepted, RequestedServiceState.Completed) => true,
            (RequestedServiceState.Completed, RequestedServiceState.Validated) => true,

            _ => false
        };

        if (!validTransition) throw new BusinessException(ErrorCode.InvalidStateTransition, $"Cannot change state from {State} to {newState}.");

        State = newState;
    }
}