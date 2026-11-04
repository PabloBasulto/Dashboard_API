var builder = WebApplication.CreateBuilder(args);

// Agregar servicio y componentes a la 

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Agregar configuracion de instancia de la BD:
builder.Services.AddScoped<SqlConnectionFactory>();

builder.Services.AddScoped<QueriesRepository>();

var app = builder.Build();

// Configuracion del flujo de datos http
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
