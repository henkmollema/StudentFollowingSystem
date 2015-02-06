using System.Data;
using System.Data.SqlClient;

namespace StudentFollowingSystem.Data
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetOpenConnection()
        {
            // todo: add connection string.
            var con = new SqlConnection();
            con.Open();
            return con;
        }
    }
}
