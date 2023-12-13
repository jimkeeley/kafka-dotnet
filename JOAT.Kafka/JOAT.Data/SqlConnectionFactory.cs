
namespace JOAT.Data;

/// <summary>
/// Creates SQL database connections.
/// </summary>
public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string connectionString = connectionString;

    public IDbConnection Open()
    {
        var connection = new SqlConnection(this.connectionString);
        connection.Open();
        return connection;
    }
}