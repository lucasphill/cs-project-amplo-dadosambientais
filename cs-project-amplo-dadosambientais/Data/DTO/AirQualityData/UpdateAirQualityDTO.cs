using System.ComponentModel.DataAnnotations;

namespace cs_project_amplo_dadosambientais.Data.DTO.AirQualityData
{
    public class UpdateAirQualityDTO
    {
        public CreateAirQualityDataDTO? Data { get; set; }

        public string? Obs { get; set; }

        [Required]
        public Guid StationId { get; set; }
    }
}
