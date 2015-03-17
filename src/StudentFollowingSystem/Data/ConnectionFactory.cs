using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentFollowingSystem.Data
{
    public static class ConnectionFactory
    {
        /// <summary>
        /// Gets an open connection to the database using the configured connection string.
        /// </summary>
        /// <returns>An open connection to the database.s</returns>
        public static IDbConnection GetOpenConnection()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SVS"].ConnectionString);
            con.Open();
            return con;
        }
    }
}
