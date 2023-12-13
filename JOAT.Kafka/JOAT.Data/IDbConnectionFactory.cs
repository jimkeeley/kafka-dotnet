namespace JOAT.Data;

/// <summary>
/// Interface for classes that create database connections.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Returns an open database connection.
    /// </summary>
    /// <returns>
    /// An open <see cref="IDbConnection"/> instance.
    /// </returns>
    IDbConnection Open();
}

