using Microsoft.AspNetCore.Http;
using ServiSwap.Models;

namespace ServiSwap.Extensions;

public static class PersonSessionExtensions
{
    private const string PersonIdKey = "PersonId";
    private const string UsernameKey = "Username";
    private const string CreditHoursKey = "CreditHours";

    public static int? GetPersonId(this ISession session)
    {
        int? personId = session.GetInt32(PersonIdKey);
        return personId is > 0 ? personId : null;
    }

    public static void SetAuthenticatedPerson(this ISession session, Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        session.SetInt32(PersonIdKey, person.PersonId);
        session.SetString(UsernameKey, person.Username);
        session.SetInt32(CreditHoursKey, person.CreditHours);
    }

    public static void ClearAuthenticatedPerson(this ISession session)
    {
        session.Remove(PersonIdKey);
        session.Remove(UsernameKey);
        session.Remove(CreditHoursKey);
    }
}
