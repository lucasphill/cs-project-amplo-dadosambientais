using System.ComponentModel.DataAnnotations;

namespace cs_project_amplo_dadosambientais.Models;

public class StationModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Obs { get; set; }

    public DateTime InsertTimeStamp { get; set; } = DateTime.UtcNow;
}
