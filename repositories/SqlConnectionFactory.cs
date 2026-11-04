//Intentar la comunicacion con la base de datos
using Microsoft.Data.SqlClient;
using MicrosoftExtensions.Configuration;

namespace TodoApi.Repositories;

public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException(
                "Falta configurar ConnectionStrings:SqlServer.");
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}