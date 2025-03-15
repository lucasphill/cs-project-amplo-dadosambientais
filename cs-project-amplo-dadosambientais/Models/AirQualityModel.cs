using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs_project_amplo_dadosambientais.Models
{
    public class AirQualityModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Column("Data", TypeName = "jsonb")]
        public string Data { get; set; } = "{}";

        public string? Obs { get; set; }

        public DateTime InsertTimeStamp { get; set; } = DateTime.UtcNow;

        [Required]
        public StationModel Station { get; set; }
    }
}
