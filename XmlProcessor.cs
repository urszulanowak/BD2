using System;
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
        if (!IsValidXml(content))
        {
            throw new InvalidOperationException("Zawartość nie jest poprawnym dokumentem XML.");
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "IF NOT EXISTS (SELECT 1 FROM XmlDocuments WHERE Name = @Name) " +
                           "INSERT INTO XmlDocuments (Name, Content) VALUES (@Name, @Content)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Content", content);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteXmlDocument(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string sql = "DELETE FROM XmlDocuments WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", name);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Dokument XML został pomyślnie usunięty.");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono dokumentu o podanej nazwie.");
                }
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

    public string SearchXmlDocuments(string xpath)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // Bezpośrednie wstawienie literału do zapytania SQL
            string query = $"SELECT Name, Content.query('{xpath}') as Result FROM XmlDocuments WHERE Content.exist('{xpath}') = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string result = "";
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string content = reader["Result"].ToString();
                        result += $"Name: {name}\nContent: {content}\n\n";
                    }
                    return result;
                }
            }
        }
    }

    public void UpdateXmlDocument(string name, string xpath, string newValue)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string checkQuery = $"SELECT Content.exist('{xpath}') as NodeExists FROM XmlDocuments WHERE Name = @Name";
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@Name", name);
                connection.Open();
                bool nodesExist = (bool)checkCommand.ExecuteScalar();

                if (!nodesExist)
                {
                    throw new InvalidOperationException("Węzły XPath nie istnieją w dokumencie XML.");
                }
            }

            string query = "UPDATE XmlDocuments SET Content.modify('replace value of (" + xpath + ")[1] with (\"" + newValue + "\")') WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Nie znaleziono dokumentu o podanej nazwie.");
                }
            }
        }
    }



    private bool IsValidXml(string xml)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}

