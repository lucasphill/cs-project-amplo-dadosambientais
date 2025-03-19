using cs_project_amplo_dadosambientais.Data;
using cs_project_amplo_dadosambientais.Models; // demonstration purposes
using cs_project_amplo_dadosambientais.Services.AirQuality;
using cs_project_amplo_dadosambientais.Services.Station;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Postgres with Entity
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PsqlConnectionString")));

// Add services
builder.Services.AddScoped<IAirQualityInterface, AirQualityService>();
builder.Services.AddScoped<IStationInterface, StationService>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Escreve logs no console
    .WriteTo.File("logs/dadosambientais-.txt", rollingInterval: RollingInterval.Day) // Escreve logs em um arquivo com rotação diária
    .CreateLogger();
builder.Host.UseSerilog();


// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow", builder =>
    {
        builder.WithOrigins("*")  // Defina a origem permitida
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

{ // Grant database is created
    var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    dbContext!.Database.EnsureCreated();

    // Initial data (for demonstration purposes)
    if (!dbContext.Station.Any()) // Verifica se já existem dados
    {
        var station1 = new StationModel { Id = Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"), Name = "EMQAr-Sul 3 - Guanabara", Obs = "Obs1" };
        dbContext.Station.Add(station1);
        var station2 = new StationModel { Id = Guid.Parse("4bcda7a9-01c4-4724-a3d8-c07b6ffb8d29"), Name = "EMQAr-Sul 4 - Belo Horizonte", Obs = "Obs2" };
        dbContext.Station.Add(station2);
        var station3 = new StationModel { Id = Guid.Parse("50749e1e-56be-48ee-b24a-31142a33974d"), Name = "EMQAr-Sul 5 - Mae-Ba", Obs = "Obs3" };
        dbContext.Station.Add(station3);
        dbContext.SaveChanges();

        // INSERT INTO public."AirQuality"("Id", "Data", "Obs", "InsertTimeStamp", "StationId") VALUES(?, ?, ?, ?, ?);
        var sql = "INSERT INTO \"AirQuality\" (\"Id\", \"Data\", \"Obs\", \"InsertTimeStamp\", \"StationId\") VALUES(@p0, @p1, @p2, @p3, @p4)";
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("537d2377-ae9e-472b-aae9-14582c1aaf91"), JsonDocument.Parse("{}"), "Error Exemple", DateTime.UtcNow, Guid.Parse("4bcda7a9-01c4-4724-a3d8-c07b6ffb8d29"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.66, \"MonoxidoDeCarbono\": 257.49, \"MonoxidoDeNitrogenio\": 4.75, \"DioxidoDeNitrogenio\": 5.75, \"OxidosDeNitrogenio\": 10.5, \"ParticulasInalaveis2\": 7.62 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("925e52a0-fad6-41e5-b221-b164b671e524"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.13, \"MonoxidoDeCarbono\": 293.14, \"MonoxidoDeNitrogenio\": 4.81, \"DioxidoDeNitrogenio\": 8.62, \"OxidosDeNitrogenio\": 13.43, \"ParticulasInalaveis2\": 10.73 }"), "Flag", DateTime.UtcNow, Guid.Parse("4bcda7a9-01c4-4724-a3d8-c07b6ffb8d29"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("3d6af425-4ee4-415a-87f1-5cfc21cddee7"), JsonDocument.Parse("{ \"ParticulasTotaisEmSuspensao\": 24.06, \"ParticulasInalaveis10\": 24.81, \"DioxidoDeEnxofre\": 8.49, \"MonoxidoDeNitrogenio\": 1.63, \"DioxidoDeNitrogenio\": 7.52, \"OxidosDeNitrogenio\": 9.15, \"MonoxidoDeCarbono\": 1106.4, \"ParticulasInalaveis2\": 19.23, \"Ozonio\": 32.57 }"), null, DateTime.UtcNow, Guid.Parse("50749e1e-56be-48ee-b24a-31142a33974d"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("64db4d3c-65a9-4922-9f9c-77622ece95f5"), JsonDocument.Parse("{ \"Data\": \"Error\" }"), "Error Exemple", DateTime.UtcNow, Guid.Parse("50749e1e-56be-48ee-b24a-31142a33974d"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("b63c247d-c1ac-4833-8be9-47b8670b1f81"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.20, \"MonoxidoDeCarbono\": 208.65, \"MonoxidoDeNitrogenio\": 3.89, \"DioxidoDeNitrogenio\": 4.59, \"OxidosDeNitrogenio\": 8.48, \"ParticulasInalaveis2\": 3.59 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("60893ae1-2a2f-4ecb-bafa-5e1c89b2b581"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.13, \"MonoxidoDeCarbono\": 293.14, \"MonoxidoDeNitrogenio\": 4.81, \"DioxidoDeNitrogenio\": 8.62, \"OxidosDeNitrogenio\": 13.43, \"ParticulasInalaveis2\": 10.73 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("f4b78e41-1399-4fd1-8bf2-8e4951baaa08"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.09, \"MonoxidoDeCarbono\": 316.74, \"MonoxidoDeNitrogenio\": 4.39, \"DioxidoDeNitrogenio\": 7.21, \"OxidosDeNitrogenio\": 11.60, \"ParticulasInalaveis2\": 23.08 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("22946ea2-729d-4385-8093-bd0dd3678785"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.10, \"MonoxidoDeCarbono\": 289.26, \"MonoxidoDeNitrogenio\": 4.29, \"DioxidoDeNitrogenio\": 7.90, \"OxidosDeNitrogenio\": 12.19, \"ParticulasInalaveis2\": 23.63 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("d6b942c1-4a84-408d-8d6e-1898229ba49a"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.55, \"MonoxidoDeCarbono\": 268.60, \"MonoxidoDeNitrogenio\": 5.14, \"DioxidoDeNitrogenio\": 6.46, \"OxidosDeNitrogenio\": 11.60, \"ParticulasInalaveis2\": 12.14 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("09551641-d3b5-4062-b27e-b4a47959aeef"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.75, \"MonoxidoDeCarbono\": 217.42, \"MonoxidoDeNitrogenio\": 5.14, \"DioxidoDeNitrogenio\": 4.50, \"OxidosDeNitrogenio\": 9.64, \"ParticulasInalaveis2\": 5.72 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("c13a2598-f910-4c4b-90c4-c057a6b30e29"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.29, \"MonoxidoDeCarbono\": 232.53, \"MonoxidoDeNitrogenio\": 5.77, \"DioxidoDeNitrogenio\": 4.28, \"OxidosDeNitrogenio\": 10.05, \"ParticulasInalaveis2\": 10.72 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("3d7603d0-1849-4601-a27a-429d196f875d"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 7.00, \"MonoxidoDeCarbono\": 219.75, \"MonoxidoDeNitrogenio\": 4.07, \"DioxidoDeNitrogenio\": 3.43, \"OxidosDeNitrogenio\": 7.50, \"ParticulasInalaveis2\": 17.93 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("37591c98-7d91-4644-abd3-bf46b8bc9b5a"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.40, \"MonoxidoDeCarbono\": 243.64, \"MonoxidoDeNitrogenio\": 5.71, \"DioxidoDeNitrogenio\": 5.91, \"OxidosDeNitrogenio\": 11.62, \"ParticulasInalaveis2\": 17.62 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("3a9677ac-80dd-41fe-a539-7cf3ef61ad84"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 10.63, \"MonoxidoDeCarbono\": 276.57, \"MonoxidoDeNitrogenio\": 6.19, \"DioxidoDeNitrogenio\": 6.63, \"OxidosDeNitrogenio\": 12.82, \"ParticulasInalaveis2\": 23.65 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("fbbc4590-3504-47e2-a7ce-e00278cb2434"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.05, \"MonoxidoDeCarbono\": 267.52, \"MonoxidoDeNitrogenio\": 5.09, \"DioxidoDeNitrogenio\": 6.55, \"OxidosDeNitrogenio\": 11.64, \"ParticulasInalaveis2\": 7.81 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("6e6df1fd-f064-491d-87ce-9825c8b9b8b3"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.05, \"MonoxidoDeCarbono\": 316.24, \"MonoxidoDeNitrogenio\": 4.32, \"DioxidoDeNitrogenio\": 6.29, \"OxidosDeNitrogenio\": 10.61, \"ParticulasInalaveis2\": 0.28 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Allow");

app.MapControllers();

app.Run();



