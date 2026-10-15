using MySqlConnector;
using ServiSwap.DAL.Interfaces;
using ServiSwap.Models;

namespace ServiSwap.DAL
{
    public class RequestedServiceDAL(string connectionString) : IRequestedServiceDAL
    {
        private readonly string connectionString = connectionString;

        public bool Complete(RequestedService requestedService)
        {
            throw new NotImplementedException();
        }

        public void CreateRequestedService(RequestedService requestedService)
        {
            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                INSERT INTO RequestedServices (dateOfRequest, state, requesterId, serviceId) 
                VALUES (@dateOfRequest, @state, @requesterId, @serviceId)
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@dateOfRequest", requestedService.DateOfRequest);
            command.Parameters.AddWithValue("@state", (int)requestedService.State);
            command.Parameters.AddWithValue("@requesterId", requestedService.Requester.PersonId);
            command.Parameters.AddWithValue("@serviceId", requestedService.Service.ServiceId);

            command.ExecuteNonQuery();

            requestedService.RequestedServiceId = (int)command.LastInsertedId;
        }

        public RequestedService? FindById(int requestedServiceId)
        {
            RequestedService? requestedService = null;

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT rs.dateOfRequest, rs.state, 
                    r.personId AS rId, r.username AS rUsername, r.password AS rPassword, r.lastName AS rLastName, r.firstName AS rFirstName, 
                        r.email AS rEmail, r.creditHours AS rCreditHours, r.latitude AS rLatitude, r.longitude AS rLongitude, 
                    ra.streetNumber AS raStreetNumber, ra.streetName AS raStreetName, ra.city AS raCity, 
                    s.serviceId, s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName, 
                    p.personId AS pId, p.username AS pUsername, p.password AS pPassword, p.lastName AS pLastName, p.firstName AS pFirstName, 
                        p.email AS pEmail, p.creditHours AS pCreditHours, p.latitude AS pLatitude, p.longitude AS pLongitude, 
                    pa.streetNumber AS paStreetNumber, pa.streetName AS paStreetName, pa.city AS paCity 
                FROM RequestedServices rs 
                    INNER JOIN People r INNER JOIN Addresses ra ON r.addressId = ra.addressId ON rs.requesterId = r.personId 
                    INNER JOIN Services s 
                        INNER JOIN Categories c ON s.categoryId = c.categoryId 
                        INNER JOIN People p INNER JOIN Addresses pa ON p.addressId = pa.addressId ON s.providerId = p.personId 
                    ON rs.serviceId = s.serviceId
                WHERE reqServiceId = @requestedServiceId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@requestedServiceId", requestedServiceId);

            MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                Category category = new(reader.GetInt32("categoryId"), reader.GetString("categoryName"));

                Person provider = provider = new(
                    reader.GetString("pUsername"),
                    reader.GetString("pPassword"),
                    reader.GetString("pLastName"),
                    reader.GetString("pFirstName"),
                    reader.GetString("pEmail"),
                    reader.GetInt32("pCreditHours"),
                    reader.GetString("paStreetNumber"),
                    reader.GetString("paStreetName"),
                    reader.GetString("paCity"),
                    reader.GetDouble("pLatitude"),
                    reader.GetDouble("pLongitude")
                ) { PersonId = reader.GetInt32("pId") };

                Service service = new(
                    reader.GetString("serviceName"),
                    reader.GetString("serviceDescription"),
                    reader.GetDateOnly("dateOfPublication"),
                    category,
                    provider
                ) { ServiceId = reader.GetInt32("serviceId") };

                Person requester = new(
                    reader.GetString("rUsername"),
                    reader.GetString("rPassword"),
                    reader.GetString("rLastName"),
                    reader.GetString("rFirstName"),
                    reader.GetString("rEmail"),
                    reader.GetInt32("rCreditHours"),
                    reader.GetString("raStreetNumber"),
                    reader.GetString("raStreetName"),
                    reader.GetString("raCity"),
                    reader.GetDouble("rLatitude"),
                    reader.GetDouble("rLongitude")
                ) { PersonId = reader.GetInt32("rId") };

                requestedService = new(
                    reader.GetDateOnly("dateOfRequest"),
                    (RequestedServiceState)reader.GetInt32("state"),
                    requester,
                    service
                ) { RequestedServiceId = requestedServiceId };
            }

            return requestedService;
        }

        public void FindAllByRequester(Person requester)
        {
            Dictionary<int, Category> categories = [];
            Dictionary<int, Person> people = [];

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT rs.reqServiceId, rs.dateOfRequest, rs.state, 
                    s.serviceId, s.serviceName, s.serviceDescription, s.dateOfPublication, 
                    c.categoryId, c.categoryName, 
                    p.personId AS pId, p.username AS pUsername, p.password AS pPassword, p.lastName AS pLastName, p.firstName AS pFirstName, 
                        p.email AS pEmail, p.creditHours AS pCreditHours, p.latitude AS pLatitude, p.longitude AS pLongitude, 
                    pa.streetNumber AS paStreetNumber, pa.streetName AS paStreetName, pa.city AS paCity 
                FROM RequestedServices rs INNER JOIN Services s 
                        INNER JOIN Categories c ON s.categoryId = c.categoryId 
                        INNER JOIN People p INNER JOIN Addresses pa ON p.addressId = pa.addressId ON s.providerId = p.personId 
                    ON rs.serviceId = s.serviceId 
                WHERE rs.requesterId = @personId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@personId", requester.PersonId);

            using MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                int categoryId = reader.GetInt32("categoryId");

                if(!categories.TryGetValue(categoryId, out Category? category))
                {
                    category = new(categoryId, reader.GetString("categoryName"));

                    categories.Add(categoryId, category);
                }

                int personId = reader.GetInt32("pId");

                if(!people.TryGetValue(personId, out Person? provider))
                {
                    provider = new(
                        reader.GetString("pUsername"),
                        reader.GetString("pPassword"),
                        reader.GetString("pLastName"),
                        reader.GetString("pFirstName"),
                        reader.GetString("pEmail"),
                        reader.GetInt32("pCreditHours"),
                        reader.GetString("paStreetNumber"),
                        reader.GetString("paStreetName"),
                        reader.GetString("paCity"),
                        reader.GetDouble("pLatitude"),
                        reader.GetDouble("pLongitude")
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
                
                _ = new RequestedService(
                    reader.GetDateOnly("dateOfRequest"),
                    (RequestedServiceState)reader.GetInt32("state"),
                    requester,
                    service
                ) { RequestedServiceId = reader.GetInt32("reqServiceId") };
            }
        }
    }
}