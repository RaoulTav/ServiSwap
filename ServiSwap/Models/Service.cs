using ServiSwap.Exceptions;

namespace ServiSwap.Models;

public class Service
{
    public Service(string serviceName, string serviceDescription, DateOnly dateOfPublication)
    {
        ServiceName = serviceName;
        ServiceDescription = serviceDescription;
        DateOfPublication = dateOfPublication;
    }

    public int ServiceId
    {
        get;
        set
        {
            if(field != 0) throw new TechnicalException("Service ID is already set.");

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

            if(value.Length > 50) throw new BusinessException(ErrorCode.InvalidServiceName, "Service name cannot exceed 50 characters.");

            field = value;
        }
    }

    public string ServiceDescription
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length > 1000) throw new BusinessException(ErrorCode.InvalidServiceDescription, "Service description cannot exceed 1000 characters.");

            field = value;
        }
    }

    public DateOnly DateOfPublication { get; set; }
}