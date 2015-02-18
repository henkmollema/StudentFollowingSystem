using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentFollowingSystem.Data
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetOpenConnection()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SVS"].ConnectionString);
            con.Open();
            return con;
        }
    }
}
