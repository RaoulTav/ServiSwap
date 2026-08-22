namespace ServiSwap.Models;

public class Person
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
    }

    public int PersonId
    {
        get;
        set
        {
            if(field != 0) throw new ArgumentException("Person ID is already set.");

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

            if(value.Length < 7 || value.Length > 20) throw new ArgumentException("Person username should be between 7 and 20 characters.");

            field = value;
        }
    }

    public string Password
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            // Not defining password length here because hash method will be implemented later.

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

            if(value.Length < 7 || value.Length > 50) throw new ArgumentException("Person email should be between 7 and 50 characters.");

            // TODO: Add Regexp exception for email format.

            field = value;
        }
    }

    public string StreetNumber
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 10) throw new ArgumentException("Street number should be between 1 and 10 characters.");

            field = value;
        }
    }

    public string StreetName
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 50) throw new ArgumentException("Street name should be between 1 and 50 characters.");

            field = value;
        }
    }

    public string City
    {
        get;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if(value.Length < 1 || value.Length > 50) throw new ArgumentException("Street name should be between 1 and 50 characters.");

            field = value;
        }
    }

    public double Latitude { get; private set { ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value); field = value; } }

    public double Longitude { get; private set { ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value); field = value; } }

    public int CreditHours { get; set { ArgumentOutOfRangeException.ThrowIfNegative(value); field = value; } }
}