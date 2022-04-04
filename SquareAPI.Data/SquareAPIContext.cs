using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SquareAPI.Data;
/// <summary>
/// Context class to create Square DB connection.
/// </summary>
public class SquareAPIContext
{
    /// <summary>
    /// Connection string read from config.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance of Configuration to read config stored in json.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor to initialize local and readonly variables.
    /// </summary>
    /// <param name="configuration">Injected object of Configuration.</param>
    public SquareAPIContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection");
    }

    /// <summary>
    /// Create SQL DB connection.
    /// </summary>
    /// <returns>DBConnection instance</returns>
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
