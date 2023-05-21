using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;


namespace LoadImages
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Server=localhost;Database=paphiopedilum;uid=root;Pwd=password"; // Replace with your MySQL connection string

            Console.Write("Enter the path to the image directory: ");
            string directoryPath = Console.ReadLine();

            Console.Write("Enter the image file name: ");
            string fileName = Console.ReadLine();

            // Combine the directory path and file name, handling the trailing backslash
            string imagePath = Path.Combine(directoryPath.TrimEnd('\\'), fileName);

            // Check if the file exists
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("File not found!");
                return;
            }

            // Read the image file as a byte array
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Establish the database connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a parameterized SQL query to insert the image into the database
                string sql = "INSERT INTO images (Image, FileName) VALUES (@Image, @FileName)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Add the image byte array and filename as parameters
                    command.Parameters.AddWithValue("@Image", imageBytes);
                    command.Parameters.AddWithValue("@FileName", fileName);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
        }
    }
}