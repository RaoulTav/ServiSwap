using ServiSwap.DAL.Interfaces;
using ServiSwap.Models;
using MySqlConnector;

namespace ServiSwap.DAL
{
    public class CategoryDAL(string connectionString) : ICategoryDAL
    {
        private readonly string connectionString = connectionString;

        public List<Category> FindAll()
        {
            List<Category> categories = [];

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT categoryId, categoryName 
                FROM Categories
            """;

            using MySqlCommand command = new(query, connection);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Category category = new(
                    reader.GetInt32("categoryId"),
                    reader.GetString("categoryName")
                );

                categories.Add(category);
            }

            return categories;
        }

        public Category? FindById(int categoryId)
        {
            Category? category = null;

            using MySqlConnection connection = new(connectionString);

            connection.Open();

            string query = """
                SELECT categoryName 
                FROM Categories 
                WHERE categoryId = @categoryId
            """;

            using MySqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@categoryId", categoryId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                category = new(
                    categoryId,
                    reader.GetString("categoryName")
                );
            }

            return category;
        }
    }
}