using MySqlConnector;
using ServiSwap.DAL.Interfaces;
using ServiSwap.Models;

namespace ServiSwap.DAL
{
    public class PersonDAL(string connectionString) : IPersonDAL
    {
        private readonly string connectionString = connectionString;

        public void CreatePerson(Person person)
        {
            using MySqlConnection connection = new(connectionString);

            connection.Open();

            using MySqlTransaction transaction = connection.BeginTransaction();

            try
            {
                string addressQuery = """
                    INSERT INTO Addresses (streetNumber, streetName, city) 
                    VALUES (@streetNumber, @streetName, @city)
                """;

                using MySqlCommand addressCommand = new(addressQuery, connection, transaction);

                addressCommand.Parameters.AddWithValue("@streetNumber", person.StreetNumber);
                addressCommand.Parameters.AddWithValue("@streetName", person.StreetName);
                addressCommand.Parameters.AddWithValue("@city", person.City);

                addressCommand.ExecuteNonQuery();

                int addressId = (int)addressCommand.LastInsertedId;

                string personQuery = """
                    INSERT INTO People (username, password, lastName, firstName, email, creditHours, latitude, longitude, addressId) 
                    VALUES (@username, @password, @lastName, @firstName, @email, @creditHours, @latitude, @longitude, @addressId)
                """;

                using MySqlCommand personCommand = new(personQuery, connection, transaction);

                personCommand.Parameters.AddWithValue("@username", person.Username);
                personCommand.Parameters.AddWithValue("@password", person.Password);
                personCommand.Parameters.AddWithValue("@lastName", person.LastName);
                personCommand.Parameters.AddWithValue("@firstName", person.FirstName);
                personCommand.Parameters.AddWithValue("@email", person.Email);
                personCommand.Parameters.AddWithValue("@creditHours", person.CreditHours);
                personCommand.Parameters.AddWithValue("@latitude", person.Latitude);
                personCommand.Parameters.AddWithValue("@longitude", person.Longitude);
                personCommand.Parameters.AddWithValue("@addressId", addressId);

                personCommand.ExecuteNonQuery();

                transaction.Commit();

                person.PersonId = (int)personCommand.LastInsertedId;
            } catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public bool Exists(string username, string email)
        {
            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT EXISTS (
                    SELECT 1 
                    FROM People 
                    WHERE username = @username OR email = @email)
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@email", email);

            return Convert.ToBoolean(command.ExecuteScalar());
        }

        public Person? FindByCredentials(string username, string password)
        {
            Person? person = null;

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT personId, lastName, firstName, email, creditHours, latitude, longitude, streetNumber, streetName, city 
                FROM People INNER JOIN Addresses ON People.addressId = Addresses.addressId 
                WHERE username = @username AND password = @password
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                person = new(
                    username,
                    password,
                    reader.GetString("lastName"),
                    reader.GetString("firstName"),
                    reader.GetString("email"),
                    reader.GetString("streetNumber"),
                    reader.GetString("streetName"),
                    reader.GetString("city"),
                    reader.GetDouble("latitude"),
                    reader.GetDouble("longitude")
                ) { PersonId = reader.GetInt32("personId") };
            }

            return person;
        }

        public Person? FindById(int personId)
        {
            Person? person = null;

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT username, password, lastName, firstName, email, creditHours, latitude, longitude, streetNumber, streetName, city 
                FROM People INNER JOIN Addresses ON People.addressId = Addresses.addressId 
                WHERE personId = @personId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@personId", personId);

            using MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                person = new(
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetString("lastName"),
                    reader.GetString("firstName"),
                    reader.GetString("email"),
                    reader.GetString("streetNumber"),
                    reader.GetString("streetName"),
                    reader.GetString("city"),
                    reader.GetDouble("latitude"),
                    reader.GetDouble("longitude")
                ) { PersonId = personId };
            }

            return person;
        }
    }
}