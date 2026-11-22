using System.Runtime.CompilerServices;
using ServiSwap.Exceptions;

namespace ServiSwap.Models;

public class Service
{
    public Service(string serviceName, string serviceDescription, DateOnly dateOfPublication, Category category, Person provider)
    {
        ServiceName = serviceName;
        ServiceDescription = serviceDescription;
        DateOfPublication = dateOfPublication;
        Category = category;
        Provider = provider;
        Provider.AddService(this);
        requestedServices = [];
    }

    public int ServiceId
    {
        get;
        set
        {
            if (field != 0) throw new TechnicalException("Service ID is already set.");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            field = value;
        }
    }

    public string ServiceName
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (value.Length > 50)
            {
                throw new BusinessException(ErrorCode.InvalidServiceName, "Service name cannot exceed 50 characters.");
            } 

            field = value;
        }
    }

    public string ServiceDescription
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (value.Length > 1000) 
            {
                throw new BusinessException(ErrorCode.InvalidServiceDescription, "Service description cannot exceed 1000 characters.");
            }

            field = value;
        }
    }

    public DateOnly DateOfPublication { get; set; }

    public Category Category { get; private set; }

    public Person Provider { get; private set { ArgumentNullException.ThrowIfNull(value); field = value; } }

    private readonly List<RequestedService> requestedServices;

    public IReadOnlyList<RequestedService> RequestedServices => requestedServices;

    public void AddRequestedService(RequestedService requestedService)
    {
        ArgumentNullException.ThrowIfNull(requestedService);

        if (requestedService.Service != this) throw new TechnicalException("Requested service does not reference this service.");

        if (requestedServices.Contains(requestedService))
        {
            throw new TechnicalException("Requested service is already associated with this service.");
        } 

        requestedServices.Add(requestedService);
    }

    public bool Equals(Service? otherService)
    {
        if (otherService is null) return false;

        if (ServiceId == 0 || otherService.ServiceId == 0) return ReferenceEquals(this, otherService);

        return ServiceId == otherService.ServiceId;
    }

    public override bool Equals(object? obj) => Equals(obj as Service);

    public override int GetHashCode() => ServiceId == 0 ? RuntimeHelpers.GetHashCode(this) : ServiceId.GetHashCode();
}