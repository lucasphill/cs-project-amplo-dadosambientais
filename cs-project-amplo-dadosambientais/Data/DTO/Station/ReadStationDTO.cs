using System.ComponentModel.DataAnnotations;

namespace cs_project_amplo_dadosambientais.Data.DTO.Station
{
    public class ReadStationDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Obs { get; set; }

        public DateTime InsertTimeStamp { get; set; }
    }
}
