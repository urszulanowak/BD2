using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;

public class XmlProcessor : IXmlProcessor
{
    private readonly string _connectionString;

    public XmlProcessor()
    {
        _connectionString = "Server=DESKTOP-FMPII3S\\SQLEXPRESS;Database=BD2;Trusted_Connection=True;";
    }

    public void SaveXmlDocument(string name, string content)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Nazwa dokumentu nie może być pusta.");
        }

        // Sprawdzanie poprawności dokumentu XML
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
        }
        catch (XmlException ex)
        {
            throw new InvalidOperationException("Nieprawidłowy dokument XML: " + ex.Message);
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Sprawdzenie, czy dokument o tej nazwie już istnieje
            string checkQuery = "SELECT COUNT(*) FROM XmlDocuments WHERE Name = @Name";
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@Name", name);
                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    throw new InvalidOperationException("Dokument o podanej nazwie już istnieje.");
                }
            }

            string query = "INSERT INTO XmlDocuments (Name, Content) VALUES (@Name, @Content)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Content", content);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteXmlDocument(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Nazwa dokumentu nie może być pusta.");
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string sql = "DELETE FROM XmlDocuments WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", name);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Nie znaleziono dokumentu o podanej nazwie.");
                }

                Console.WriteLine("Dokument XML został pomyślnie usunięty.");
            }
        }
    }

    public List<string> GetXmlDocumentNames()
    {
        List<string> names = new List<string>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Name FROM XmlDocuments";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add(reader.GetString(0));
                    }
                }
            }
        }

        return names;
    }
}
