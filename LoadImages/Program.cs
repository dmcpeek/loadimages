using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;



namespace LoadImages
{
    internal class Program
    {
        private static object repo;
        private static object instance;

        static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);
            string connectionString = "your_connection_string"; // Replace with your MySQL connection string

            // Read the image file as a byte array
            byte[] imageBytes = File.ReadAllBytes("path_to_image_file");

            // Establish the database connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a parameterized SQL query to insert the image into the database
                string sql = "INSERT INTO images (image_data) VALUES (@ImageData)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Add the image byte array as a parameter
                    command.Parameters.AddWithValue("@ImageData", imageBytes);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
        }
    }
}