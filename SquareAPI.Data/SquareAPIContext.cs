using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SquareAPI.Data;
public class SquareAPIContext
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;
    public SquareAPIContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection");
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
