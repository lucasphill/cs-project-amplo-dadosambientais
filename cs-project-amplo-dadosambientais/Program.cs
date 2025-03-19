using cs_project_amplo_dadosambientais.Data;
using cs_project_amplo_dadosambientais.Models; // demonstration purposes
using cs_project_amplo_dadosambientais.Services.AirQuality;
using cs_project_amplo_dadosambientais.Services.Station;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
builder.Services.AddScoped<StationService>();
builder.Services.AddScoped<IAirQualityInterface, AirQualityService>();

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
        dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("b63c247d-c1ac-4833-8be9-47b8670b1f81"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.20, \"MonoxidoDeCarbono\": 208.65, \"MonoxidoDeNitrogenio\": 3.89, \"DioxidoDeNitrogenio\": 4.59, \"OxidosDeNitrogenio\": 8.48, \"ParticulasInalaveis2\": 3.59 }"), "Flag", DateTime.UtcNow, Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));}
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Allow");

app.MapControllers();

app.Run();


/*
dbContext.Database.ExecuteSqlRaw(sql, Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"), JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.13, \"MonoxidoDeCarbono\": 293.14, \"MonoxidoDeNitrogenio\": 4.81, \"DioxidoDeNitrogenio\": 8.62, \"OxidosDeNitrogenio\": 13.43, \"ParticulasInalaveis2\": 10.73 }"),"Flag",DateTime.UtcNow,Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.09, \"MonoxidoDeCarbono\": 316.74, \"MonoxidoDeNitrogenio\": 4.39, \"DioxidoDeNitrogenio\": 7.21, \"OxidosDeNitrogenio\": 11.60, \"ParticulasInalaveis2\": 23.08 }"),"Flag",DateTime.UtcNow,Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.10, \"MonoxidoDeCarbono\": 289.26, \"MonoxidoDeNitrogenio\": 4.29, \"DioxidoDeNitrogenio\": 7.90, \"OxidosDeNitrogenio\": 12.19, \"ParticulasInalaveis2\": 23.63 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.55, \"MonoxidoDeCarbono\": 268.60, \"MonoxidoDeNitrogenio\": 5.14, \"DioxidoDeNitrogenio\": 6.46, \"OxidosDeNitrogenio\": 11.60, \"ParticulasInalaveis2\": 12.14 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.75, \"MonoxidoDeCarbono\": 217.42, \"MonoxidoDeNitrogenio\": 5.14, \"DioxidoDeNitrogenio\": 4.50, \"OxidosDeNitrogenio\": 9.64, \"ParticulasInalaveis2\": 5.72 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.29, \"MonoxidoDeCarbono\": 232.53, \"MonoxidoDeNitrogenio\": 5.77, \"DioxidoDeNitrogenio\": 4.28, \"OxidosDeNitrogenio\": 10.05, \"ParticulasInalaveis2\": 10.72 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 7.00, \"MonoxidoDeCarbono\": 219.75, \"MonoxidoDeNitrogenio\": 4.07, \"DioxidoDeNitrogenio\": 3.43, \"OxidosDeNitrogenio\": 7.50, \"ParticulasInalaveis2\": 17.93 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.40, \"MonoxidoDeCarbono\": 243.64, \"MonoxidoDeNitrogenio\": 5.71, \"DioxidoDeNitrogenio\": 5.91, \"OxidosDeNitrogenio\": 11.62, \"ParticulasInalaveis2\": 17.62 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 10.63, \"MonoxidoDeCarbono\": 276.57, \"MonoxidoDeNitrogenio\": 6.19, \"DioxidoDeNitrogenio\": 6.63, \"OxidosDeNitrogenio\": 12.82, \"ParticulasInalaveis2\": 23.65 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.05, \"MonoxidoDeCarbono\": 267.52, \"MonoxidoDeNitrogenio\": 5.09, \"DioxidoDeNitrogenio\": 6.55, \"OxidosDeNitrogenio\": 11.64, \"ParticulasInalaveis2\": 7.81 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.05, \"MonoxidoDeCarbono\": 316.24, \"MonoxidoDeNitrogenio\": 4.32, \"DioxidoDeNitrogenio\": 6.29, \"OxidosDeNitrogenio\": 10.61, \"ParticulasInalaveis2\": 0.28 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 11.39, \"MonoxidoDeCarbono\": 559.70, \"MonoxidoDeNitrogenio\": 4.45, \"DioxidoDeNitrogenio\": 8.79, \"OxidosDeNitrogenio\": 13.24, \"ParticulasInalaveis2\": 24.56 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.41, \"MonoxidoDeCarbono\": 395.64, \"MonoxidoDeNitrogenio\": 4.71, \"DioxidoDeNitrogenio\": 9.93, \"OxidosDeNitrogenio\": 14.64, \"ParticulasInalaveis2\": 22.16 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.93, \"MonoxidoDeCarbono\": 504.04, \"MonoxidoDeNitrogenio\": 5.72, \"DioxidoDeNitrogenio\": 12.72, \"OxidosDeNitrogenio\": 18.45, \"ParticulasInalaveis2\": 27.04 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.89, \"MonoxidoDeCarbono\": 407.59, \"MonoxidoDeNitrogenio\": 3.45, \"DioxidoDeNitrogenio\": 4.67, \"OxidosDeNitrogenio\": 8.12, \"ParticulasInalaveis2\": 18.96 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 8.41, \"MonoxidoDeCarbono\": 414.52, \"MonoxidoDeNitrogenio\": 3.31, \"DioxidoDeNitrogenio\": 4.56, \"OxidosDeNitrogenio\": 7.87, \"ParticulasInalaveis2\": 17.54 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 7.91, \"MonoxidoDeCarbono\": 430.37, \"MonoxidoDeNitrogenio\": 4.53, \"DioxidoDeNitrogenio\": 8.07, \"OxidosDeNitrogenio\": 12.61, \"ParticulasInalaveis2\": 19.49 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 8.06, \"MonoxidoDeCarbono\": 434.22, \"MonoxidoDeNitrogenio\": 5.13, \"DioxidoDeNitrogenio\": 11.36, \"OxidosDeNitrogenio\": 16.49, \"ParticulasInalaveis2\": 18.67 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 6.34, \"MonoxidoDeCarbono\": 428.36, \"MonoxidoDeNitrogenio\": 4.11, \"DioxidoDeNitrogenio\": 9.37, \"OxidosDeNitrogenio\": 13.48, \"ParticulasInalaveis2\": 18.30 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.92, \"MonoxidoDeCarbono\": 483.57, \"MonoxidoDeNitrogenio\": 5.58, \"DioxidoDeNitrogenio\": 8.31, \"OxidosDeNitrogenio\": 13.89, \"ParticulasInalaveis2\": 24.08 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.57, \"MonoxidoDeCarbono\": 364.71, \"MonoxidoDeNitrogenio\": 4.12, \"DioxidoDeNitrogenio\": 5.03, \"OxidosDeNitrogenio\": 9.15, \"ParticulasInalaveis2\": 15.31 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.88, \"MonoxidoDeCarbono\": 325.09, \"MonoxidoDeNitrogenio\": 4.09, \"DioxidoDeNitrogenio\": 5.21, \"OxidosDeNitrogenio\": 9.30, \"ParticulasInalaveis2\": 5.55 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.52, \"MonoxidoDeCarbono\": 366.19, \"MonoxidoDeNitrogenio\": 6.07, \"DioxidoDeNitrogenio\": 5.74, \"OxidosDeNitrogenio\": 11.82, \"ParticulasInalaveis2\": 2.42 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.36, \"MonoxidoDeCarbono\": 237.82, \"MonoxidoDeNitrogenio\": 4.02, \"DioxidoDeNitrogenio\": 4.05, \"OxidosDeNitrogenio\": 8.07, \"ParticulasInalaveis2\": 0.90 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.65, \"MonoxidoDeCarbono\": 212.02, \"MonoxidoDeNitrogenio\": 3.44, \"DioxidoDeNitrogenio\": 2.55, \"OxidosDeNitrogenio\": 5.99, \"ParticulasInalaveis2\": 4.76 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.59, \"MonoxidoDeCarbono\": 214.07, \"MonoxidoDeNitrogenio\": 3.98, \"DioxidoDeNitrogenio\": 2.27, \"OxidosDeNitrogenio\": 6.25, \"ParticulasInalaveis2\": 6.55 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.64, \"MonoxidoDeCarbono\": 207.22, \"MonoxidoDeNitrogenio\": 4.07, \"DioxidoDeNitrogenio\": 2.84, \"OxidosDeNitrogenio\": 6.90, \"ParticulasInalaveis2\": 8.06 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.49, \"MonoxidoDeCarbono\": 179.27, \"MonoxidoDeNitrogenio\": 3.51, \"DioxidoDeNitrogenio\": 2.53, \"OxidosDeNitrogenio\": 6.05, \"ParticulasInalaveis2\": 7.30 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.04, \"MonoxidoDeCarbono\": 163.48, \"MonoxidoDeNitrogenio\": 3.46, \"DioxidoDeNitrogenio\": 2.16, \"OxidosDeNitrogenio\": 5.62, \"ParticulasInalaveis2\": 4.77 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 2.37, \"MonoxidoDeCarbono\": 204.60, \"MonoxidoDeNitrogenio\": 6.43, \"DioxidoDeNitrogenio\": 3.71, \"OxidosDeNitrogenio\": 10.14, \"ParticulasInalaveis2\": 7.28 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.75, \"MonoxidoDeCarbono\": 208.17, \"MonoxidoDeNitrogenio\": 6.38, \"DioxidoDeNitrogenio\": 3.68, \"OxidosDeNitrogenio\": 10.05, \"ParticulasInalaveis2\": 9.51 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.17, \"MonoxidoDeCarbono\": 180.74, \"MonoxidoDeNitrogenio\": 4.78, \"DioxidoDeNitrogenio\": 2.89, \"OxidosDeNitrogenio\": 7.67, \"ParticulasInalaveis2\": 8.38 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.19, \"MonoxidoDeCarbono\": 216.70, \"MonoxidoDeNitrogenio\": 6.47, \"DioxidoDeNitrogenio\": 4.72, \"OxidosDeNitrogenio\": 11.19, \"ParticulasInalaveis2\": 6.16 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.64, \"MonoxidoDeCarbono\": 190.13, \"MonoxidoDeNitrogenio\": 5.45, \"DioxidoDeNitrogenio\": 3.68, \"OxidosDeNitrogenio\": 9.13, \"ParticulasInalaveis2\": 7.78 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.91, \"MonoxidoDeCarbono\": 310.51, \"MonoxidoDeNitrogenio\": 4.97, \"DioxidoDeNitrogenio\": 3.79, \"OxidosDeNitrogenio\": 8.77, \"ParticulasInalaveis2\": 11.41 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 6.66, \"MonoxidoDeCarbono\": 342.84, \"MonoxidoDeNitrogenio\": 5.29, \"DioxidoDeNitrogenio\": 4.41, \"OxidosDeNitrogenio\": 9.70, \"ParticulasInalaveis2\": 12.43 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 6.30, \"MonoxidoDeCarbono\": 296.02, \"MonoxidoDeNitrogenio\": 3.96, \"DioxidoDeNitrogenio\": 4.03, \"OxidosDeNitrogenio\": 7.99, \"ParticulasInalaveis2\": 10.80 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 5.96, \"MonoxidoDeCarbono\": 274.71, \"MonoxidoDeNitrogenio\": 4.09, \"DioxidoDeNitrogenio\": 4.53, \"OxidosDeNitrogenio\": 8.62, \"ParticulasInalaveis2\": 12.04 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 6.32, \"MonoxidoDeCarbono\": 258.28, \"MonoxidoDeNitrogenio\": 5.11, \"DioxidoDeNitrogenio\": 4.21, \"OxidosDeNitrogenio\": 9.32, \"ParticulasInalaveis2\": 11.03 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 7.63, \"MonoxidoDeCarbono\": 237.48, \"MonoxidoDeNitrogenio\": 5.72, \"DioxidoDeNitrogenio\": 6.01, \"OxidosDeNitrogenio\": 11.72, \"ParticulasInalaveis2\": 12.73 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 35.64, \"MonoxidoDeCarbono\": 316.59, \"MonoxidoDeNitrogenio\": 18.25, \"DioxidoDeNitrogenio\": 14.02, \"OxidosDeNitrogenio\": 32.27, \"ParticulasInalaveis2\": 12.61 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 8.86, \"MonoxidoDeCarbono\": 263.87, \"MonoxidoDeNitrogenio\": 5.92, \"DioxidoDeNitrogenio\": 8.47, \"OxidosDeNitrogenio\": 14.39, \"ParticulasInalaveis2\": 0.17 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 8.07, \"MonoxidoDeCarbono\": 295.27, \"MonoxidoDeNitrogenio\": 7.17, \"DioxidoDeNitrogenio\": 10.97, \"OxidosDeNitrogenio\": 18.15, \"ParticulasInalaveis2\": 8.31 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 9.69, \"MonoxidoDeCarbono\": 289.77, \"MonoxidoDeNitrogenio\": 5.44, \"DioxidoDeNitrogenio\": 7.37, \"OxidosDeNitrogenio\": 12.81, \"ParticulasInalaveis2\": 10.56 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 4.76, \"MonoxidoDeCarbono\": 293.98, \"MonoxidoDeNitrogenio\": 4.69, \"DioxidoDeNitrogenio\": 5.36, \"OxidosDeNitrogenio\": 10.05, \"ParticulasInalaveis2\": 6.54 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.77, \"MonoxidoDeCarbono\": 282.12, \"MonoxidoDeNitrogenio\": 4.41, \"DioxidoDeNitrogenio\": 5.30, \"OxidosDeNitrogenio\": 9.70, \"ParticulasInalaveis2\": 10.91 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

dbContext.Database.ExecuteSqlRaw(sql,
    Guid.Parse("f74125ec-448d-44f6-a7c2-72a9fc77122e"),
    JsonDocument.Parse("{ \"DioxidoDeEnxofre\": 3.75, \"MonoxidoDeCarbono\": 290.22, \"MonoxidoDeNitrogenio\": 4.17, \"DioxidoDeNitrogenio\": 4.43, \"OxidosDeNitrogenio\": 8.60, \"ParticulasInalaveis2\": 8.54 }"),
    "Flag",
    DateTime.UtcNow,
    Guid.Parse("6c1b6204-05d4-4bd2-8437-d070a9225124"));

*/