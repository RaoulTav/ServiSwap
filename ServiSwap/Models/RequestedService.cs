using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ServiSwap.Exceptions;

namespace ServiSwap.Models;

public class RequestedService
{
    public RequestedService(DateOnly dateOfRequest, RequestedServiceState state, Person requester, Service service, int reportedHours = 0)
    {
        DateOfRequest = dateOfRequest;
        State = state;
        ReportedHours = reportedHours;
        Requester = requester;
        Service = service;
        Provider = service.Provider;

        if (Requester.Equals(Provider)) 
        {
            throw new BusinessException(ErrorCode.RequesterCannotBeProvider, "A person cannot request their own service.");
        }
        
        Requester.AddRequestedServiceAsRequester(this);
        Service.AddRequestedService(this);
        Provider.AddRequestedServiceAsProvider(this);
    }

    public int RequestedServiceId
    {
        get;
        set
        {
            if (field != 0) throw new TechnicalException("RequestedService ID is already set.");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            field = value;
        }
    }

    public DateOnly DateOfRequest { get; set; }

    public RequestedServiceState State { get; private set; }
    
    [Display(Name = "Heures prestées")]
    public int ReportedHours
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            field = CanReportHours(State) ? value : 0;
        }
    }

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

        if (!validTransition)
        {
            throw new BusinessException(ErrorCode.InvalidStateTransition, $"Cannot change state from {State} to {newState}.");
        } 

        State = newState;
        if (!CanReportHours(State))
        {
            ReportedHours = 0;
        }
    }

    private static bool CanReportHours(RequestedServiceState state)
        => state is RequestedServiceState.Completed or RequestedServiceState.Validated;

    public Person Requester { get; private set { ArgumentNullException.ThrowIfNull(value); field = value; } }

    public Person Provider { get; private set { ArgumentNullException.ThrowIfNull(value); field = value; } }

    public Service Service { get; private set { ArgumentNullException.ThrowIfNull(value); field = value; } }

    public bool Equals(RequestedService? otherRequestedService)
    {
        if (otherRequestedService is null) return false;

        if (RequestedServiceId == 0 || otherRequestedService.RequestedServiceId == 0)
        {
            return ReferenceEquals(this, otherRequestedService);
        } 

        return RequestedServiceId == otherRequestedService.RequestedServiceId;
    }

    public override bool Equals(object? obj) => Equals(obj as RequestedService);

    public override int GetHashCode() => RequestedServiceId == 0 ? RuntimeHelpers.GetHashCode(this) : RequestedServiceId.GetHashCode();
}