using MySqlConnector;
using ServiSwap.DAL.Interfaces;
using ServiSwap.Models;

namespace ServiSwap.DAL
{
    public class ServiceDAL(string connectionString) : IServiceDAL
    {
        private readonly string connectionString = connectionString;

        public void CreateService(Service service)
        {
            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                INSERT INTO Services (serviceName, serviceDescription, dateOfPublication, categoryId, providerId) 
                VALUES (@serviceName, @serviceDescription, @dateOfPublication, @categoryId, @providerId)
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@serviceName", service.ServiceName);
            command.Parameters.AddWithValue("@serviceDescription", service.ServiceDescription);
            command.Parameters.AddWithValue("@dateOfPublication", service.DateOfPublication);
            command.Parameters.AddWithValue("@categoryId", service.Category.CategoryId);
            command.Parameters.AddWithValue("@providerId", service.Provider.PersonId);

            command.ExecuteNonQuery();

            service.ServiceId = (int)command.LastInsertedId;
        }

        public List<Service> FindAll()
        {
            List<Service> services = [];
            Dictionary<int, Category> categories = [];
            Dictionary<int, Person> people = [];

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT s.serviceId, s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName, 
                    p.personId, p.username, p.password, p.lastName, p.firstName, p.email, p.creditHours, p.latitude, p.longitude, 
                    a.addressId, a.streetNumber, a.streetName, a.city 
                FROM Services s 
                    INNER JOIN Categories c ON s.categoryId = c.categoryId 
                    INNER JOIN People p INNER JOIN Addresses a ON p.addressId = a.addressId ON s.providerId = p.personId
            """;

            using MySqlCommand command = new(query, connection);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int categoryId = reader.GetInt32("categoryId");

                if (!categories.TryGetValue(categoryId, out Category? category))
                {
                    category = new(categoryId, reader.GetString("categoryName"));

                    categories.Add(categoryId, category);
                }

                int personId = reader.GetInt32("personId");

                if (!people.TryGetValue(personId, out Person? provider))
                {
                    provider = new(
                        reader.GetString("username"),
                        reader.GetString("password"),
                        reader.GetString("lastName"),
                        reader.GetString("firstName"),
                        reader.GetString("email"),
                        reader.GetInt32("creditHours"),
                        reader.GetString("streetNumber"),
                        reader.GetString("streetName"),
                        reader.GetString("city"),
                        reader.GetDouble("latitude"),
                        reader.GetDouble("longitude")
                    ) { PersonId = personId };

                    people.Add(personId, provider);
                }

                Service service = new(
                    reader.GetString("serviceName"),
                    reader.GetString("serviceDescription"),
                    reader.GetDateOnly("dateOfPublication"),
                    category,
                    provider
                ) { ServiceId = reader.GetInt32("serviceId") };

                services.Add(service);
            }

            return services;
        }

        public Service? FindById(int serviceId)
        {
            Service? service = null;

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName, 
                    p.personId, p.username, p.password, p.lastName, p.firstName, p.email, p.creditHours, p.latitude, p.longitude, 
                    a.addressId, a.streetNumber, a.streetName, a.city 
                FROM Services s 
                    INNER JOIN Categories c ON s.categoryId = c.categoryId 
                    INNER JOIN People p INNER JOIN Addresses a ON p.addressId = a.addressId ON s.providerId = p.personId 
                WHERE s.serviceId = @serviceId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@serviceId", serviceId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Category category = new(reader.GetInt32("categoryId"), reader.GetString("categoryName"));

                Person provider = provider = new(
                    reader.GetString("username"),
                    reader.GetString("password"),
                    reader.GetString("lastName"),
                    reader.GetString("firstName"),
                    reader.GetString("email"),
                    reader.GetInt32("creditHours"),
                    reader.GetString("streetNumber"),
                    reader.GetString("streetName"),
                    reader.GetString("city"),
                    reader.GetDouble("latitude"),
                    reader.GetDouble("longitude")
                ) { PersonId = reader.GetInt32("personId") };

                service = new(
                    reader.GetString("serviceName"),
                    reader.GetString("serviceDescription"),
                    reader.GetDateOnly("dateOfPublication"),
                    category,
                    provider
                ) { ServiceId = serviceId };
            }

            return service;
        }

        public List<Service> SearchByCategoryAndOrDistance(double? latitude = null, double? longitude = null, 
            int? categoryId = null, double? distance = null)
        {
            if (latitude.HasValue != longitude.HasValue)
            {
                throw new ArgumentException("Latitude and longitude must be provided together.");
            }

            if (distance.HasValue && !latitude.HasValue)
            {
                throw new ArgumentException("Coordinates are required when filtering by distance.", nameof(distance));
            }

            List<Service> services = [];
            Dictionary<int, Category> categories = [];
            Dictionary<int, Person> people = [];

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT s.serviceId, s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName, 
                    p.personId, p.username, p.password, p.lastName, p.firstName, p.email, p.creditHours, p.latitude, p.longitude, 
                    a.addressId, a.streetNumber, a.streetName, a.city 
                FROM Services s 
                    INNER JOIN Categories c ON s.categoryId = c.categoryId 
                    INNER JOIN People p INNER JOIN Addresses a ON p.addressId = a.addressId ON s.providerId = p.personId 
            """;

            if (categoryId.HasValue)
            {
                query += " WHERE c.categoryId = @categoryId ";
            }

            if (distance.HasValue)
            {
                if(categoryId.HasValue)
                {
                    query += """
                        AND 6371 * ACOS(COS(RADIANS(@latitude)) * COS(RADIANS(p.latitude)) * COS(RADIANS(p.longitude) - RADIANS(@longitude))
                            + SIN(RADIANS(@latitude)) * SIN(RADIANS(p.latitude))) <= @distance
                    """;
                } else
                {
                    query += """
                        WHERE 6371 * ACOS(COS(RADIANS(@latitude)) * COS(RADIANS(p.latitude)) * COS(RADIANS(p.longitude) - RADIANS(@longitude))
                            + SIN(RADIANS(@latitude)) * SIN(RADIANS(p.latitude))) <= @distance
                    """;
                }
            }

            using MySqlCommand command = new(query, connection);

            if (categoryId.HasValue)
            {
                command.Parameters.AddWithValue("@categoryId", categoryId);
            }

            if (distance.HasValue)
            {
                command.Parameters.AddWithValue("@latitude", latitude!.Value);
                command.Parameters.AddWithValue("@longitude", longitude!.Value);
                command.Parameters.AddWithValue("@distance", distance);
            }

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int catId = reader.GetInt32("categoryId");

                if (!categories.TryGetValue(catId, out Category? category))
                {
                    category = new(catId, reader.GetString("categoryName"));

                    categories.Add(catId, category);
                }

                int personId = reader.GetInt32("personId");

                if (!people.TryGetValue(personId, out Person? provider))
                {
                    provider = new(
                        reader.GetString("username"),
                        reader.GetString("password"),
                        reader.GetString("lastName"),
                        reader.GetString("firstName"),
                        reader.GetString("email"),
                        reader.GetInt32("creditHours"),
                        reader.GetString("streetNumber"),
                        reader.GetString("streetName"),
                        reader.GetString("city"),
                        reader.GetDouble("latitude"),
                        reader.GetDouble("longitude")
                    ) { PersonId = personId };

                    people.Add(personId, provider);
                }

                Service service = new(
                    reader.GetString("serviceName"),
                    reader.GetString("serviceDescription"),
                    reader.GetDateOnly("dateOfPublication"),
                    category,
                    provider
                ) { ServiceId = reader.GetInt32("serviceId") };

                services.Add(service);
            }

            return services;
        }

        public List<Service> FindAllByProvider(Person provider)
        {
            List<Service> services = [];
            Dictionary<int, Category> categories = [];

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT s.serviceId, s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName 
                FROM Services s 
                    INNER JOIN Categories c ON s.categoryId = c.categoryId 
                    INNER JOIN People p ON s.providerId = p.personId
                WHERE p.personId = @providerId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@providerId", provider.PersonId);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int categoryId = reader.GetInt32("categoryId");

                if (!categories.TryGetValue(categoryId, out Category? category))
                {
                    category = new(categoryId, reader.GetString("categoryName"));

                    categories.Add(categoryId, category);
                }

                Service service = new(
                    reader.GetString("serviceName"),
                    reader.GetString("serviceDescription"),
                    reader.GetDateOnly("dateOfPublication"),
                    category,
                    provider
                ) { ServiceId = reader.GetInt32("serviceId") };

                services.Add(service);
            }

            return services;
        } 
    }
}