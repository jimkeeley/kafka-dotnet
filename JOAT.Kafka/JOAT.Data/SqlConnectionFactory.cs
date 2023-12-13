using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace JOAT.Data
{
    /// <summary>
    /// Creates SQL database connections.
    /// </summary>
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;
     
        public SqlConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Open()
        {
            var connection = new SqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }
    }
}
