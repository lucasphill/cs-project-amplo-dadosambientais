using cs_project_amplo_dadosambientais.Models;
using Microsoft.EntityFrameworkCore;

namespace cs_project_amplo_dadosambientais.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<StationModel> Station { get; set; }
    public DbSet<AirQualityModel> AirQuality { get; set; }
}
