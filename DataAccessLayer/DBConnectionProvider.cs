using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DBConnectionProvider
    {
        static string connectionString = "Data Source=localhost;Initial Catalog=moviedb;Integrated Security=True";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
