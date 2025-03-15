using System.ComponentModel.DataAnnotations;

namespace cs_project_amplo_dadosambientais.Data.DTO.Station
{
    public class CreateStationDTO
    {
        [Required]
        public string Name { get; set; }

        public string? Obs { get; set; }
    }
}
