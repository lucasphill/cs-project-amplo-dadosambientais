using cs_project_amplo_dadosambientais.Data;
using cs_project_amplo_dadosambientais.Models; // demonstration purposes
using cs_project_amplo_dadosambientais.Services;
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
builder.Services.AddScoped<AirQualityService>();

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
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Allow");

app.MapControllers();

app.Run();
