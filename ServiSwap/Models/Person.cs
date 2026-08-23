using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ServiSwap.Exceptions;

namespace ServiSwap.Models;

public partial class Person
{
    public Person(string username, string password, string lastName, string firstName, string email, string streetNumber, 
        string streetName, string city, double latitude = 1f, double longitude = 1f)
    {
        Username = username;
        Password = password;
        LastName = lastName;
        FirstName = firstName;
        Email = email;
        StreetNumber = streetNumber;
        StreetName = streetName;
        City = city;
        Latitude = latitude; // TODO: implement GeoCodage in CreateUser() method in PersonService later.
        Longitude = longitude; // TODO: implement GeoCodage in CreateUser() method in PersonService later.
        services = [];
        servicesProvider = [];
        servicesRequester = [];
    }

    public int PersonId
    {
        get;
        set
        {
            if(field != 0) throw new TechnicalException("Person ID is already set.");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            field = value;
        }
    }

    public string Username
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 7 || value.Length > 20) 
            {
                throw new BusinessException(ErrorCode.InvalidUsername, "Person username should be between 7 and 20 characters.");
            }

            if(!UsernameRegex().IsMatch(value))
            {
                throw new BusinessException(
                    ErrorCode.InvalidUsernameFormat,
                    "Person username should contain only letters and numbers.");
            }

            field = value;
        }
    }

    public string Password
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(!UppercaseRegex().IsMatch(value) || !DigitRegex().IsMatch(value) || !SpecialCharacterRegex().IsMatch(value))
            {
                throw new BusinessException(ErrorCode.InvalidPasswordFormat,
                    "Password should contain at least one uppercase letter, one digit and one special character.");
            }

            field = value;
        }
    }

    public string LastName { get; set { ArgumentException.ThrowIfNullOrWhiteSpace(value); field = value; } }

    public string FirstName { get; set { ArgumentException.ThrowIfNullOrWhiteSpace(value); field = value; } }

    public string Email
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 7 || value.Length > 50) 
            {
                throw new BusinessException(ErrorCode.InvalidEmail, "Person email should be between 7 and 50 characters.");
            }

            if(!EmailRegex().IsMatch(value)) throw new BusinessException(ErrorCode.InvalidEmailFormat,"Person email has an invalid format.");

            field = value;
        }
    }

    public string StreetNumber
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 10) 
            {
                throw new BusinessException(ErrorCode.InvalidStreetNumber, "Street number should be between 1 and 10 characters.");
            }

            field = value;
        }
    }

    public string StreetName
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 50)
            {
                throw new BusinessException(ErrorCode.InvalidStreetName, "Street name should be between 1 and 50 characters.");
            } 

            field = value;
        }
    }

    public string City
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 50)
            {
                throw new BusinessException(ErrorCode.InvalidCity, "City name should be between 1 and 50 characters.");
            } 

            field = value;
        }
    }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public int CreditHours { get; set { ArgumentOutOfRangeException.ThrowIfNegative(value); field = value; } }

    private readonly List<Service> services;

    public IReadOnlyList<Service> Services => services;

    public void AddService(Service service)
    {
        ArgumentNullException.ThrowIfNull(service);

        if(service.Provider != this) throw new TechnicalException("Service provider does not match this person.");

        if(services.Contains(service)) throw new TechnicalException("Service is already associated with this person.");

        services.Add(service);
    }

    private readonly List<RequestedService> servicesProvider;

    public IReadOnlyList<RequestedService> ServicesProvider =>  servicesProvider;

    public void AddRequestedServiceAsProvider(RequestedService requestedService)
    {
        ArgumentNullException.ThrowIfNull(requestedService);

        if(requestedService.Service.Provider != this) throw new TechnicalException("Requested service provider does not match this person.");

        if(servicesProvider.Contains(requestedService)) throw new TechnicalException("Requested Service is already associated with this person.");

        servicesProvider.Add(requestedService);
    }

    private readonly List<RequestedService> servicesRequester;

    public IReadOnlyList<RequestedService> ServicesRequester =>  servicesRequester;

    public void AddRequestedServiceAsRequester(RequestedService requestedService)
    {
        ArgumentNullException.ThrowIfNull(requestedService);

        if(requestedService.Requester != this) throw new TechnicalException("Requested service requester does not match this person.");

        if(servicesRequester.Contains(requestedService)) throw new TechnicalException("Requested Service is already associated with this person.");

        servicesRequester.Add(requestedService);
    }

    public bool Equals(Person? otherPerson)
    {
        if(otherPerson is null) return false;

        if(PersonId == 0 || otherPerson.PersonId == 0) return ReferenceEquals(this, otherPerson);

        return PersonId == otherPerson.PersonId;
    }

    public override bool Equals(object? obj) => Equals(obj as Person);

    public override int GetHashCode() => PersonId == 0 ? RuntimeHelpers.GetHashCode(this) : PersonId.GetHashCode();

    [GeneratedRegex(@"^[a-zA-Z0-9]+$")]
    private static partial Regex UsernameRegex();
    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercaseRegex();
    [GeneratedRegex(@"\d")]
    private static partial Regex DigitRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex SpecialCharacterRegex();
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}