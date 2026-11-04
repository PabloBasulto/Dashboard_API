
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Repositories;

public class QuerieRepository {
    private readonly IConfiguration _configuration;
    private readonly SqlConnectionFactory _connectionFactory;


    //Constructor querie repository

    public QuerieRepository(IConfiguration configuration, SqlConnectionFactory connectionFactory){

        _configuration = configuration;
        _connectionFactory = connectionFactory;

    }

    //Metodo de acceso al Querie en appsettings.json
    private string ObtenerQuery(string nombreQuery)
    {
        return _configuration[$"Queries:{nombreQuery}"]
            ?? throw new InvalidOperationException(
                $"No se encontró la consulta: {nombreQuery}");
    }


    //Ejecucion de la consulta y obtener las filas

     private async Task<List<Dictionary<string, object?>>> EjecutarQueryAsync(string nombreQuery)
    {
        string sql = ObtenerQuery(nombreQuery); 

        await using var connection =  _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);

        await using var reader =
            await command.ExecuteReaderAsync();

        var resultados = new List<Dictionary<string, object?>>();

        while (await reader.ReadAsync())
        {
            var fila = new Dictionary<string, object?>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                fila[reader.GetName(i)] =
                    await reader.IsDBNullAsync(i)
                        ? null
                        : reader.GetValue(i);
            }

            resultados.Add(fila);
        }

        return resultados;
    }

    // 1. Tiempo de atención mensual
    public Task<List<Dictionary<string, object?>>>
        ObtenerTiempoAtencionMensualAsync()
    {
        return EjecutarQueryAsync("TiempoAtencionMensual");
    }

    // 2. Promedio de atención mensual
    public Task<List<Dictionary<string, object?>>>
        ObtenerPromedioAtencionMensualAsync()
    {
        return EjecutarQueryAsync("PromedioAtencionMensual");
    }

    // 3. Eventos recientes
    public Task<List<Dictionary<string, object?>>>
        ObtenerEventosRecientesAsync()
    {
        return EjecutarQueryAsync("EventosRecientes");
    }

    // 4. Detección de ráfagas por zona
    public Task<List<Dictionary<string, object?>>>
        ObtenerDeteccionRafagasZonaAsync()
    {
        return EjecutarQueryAsync("DeteccionRafagasZona");
    }

}
