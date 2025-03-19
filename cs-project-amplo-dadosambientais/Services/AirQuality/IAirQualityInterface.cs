using cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;
using cs_project_amplo_dadosambientais.Models;

namespace cs_project_amplo_dadosambientais.Services.AirQuality;

public interface IAirQualityInterface
{
    // Add air quality log, receive a DTO. Need data in dto format
    ServiceResponseModel<AirQualityModel> CreateAirQuality(CreateAirQualityDTO dto);

    // Return a list of all air quality logs of all stations in dto format. No needs
    ServiceResponseModel<List<ReadAirQualityWStationDTO>> ReadAirQuality();

    // Return a list of air quality logs from specific stations in dto format. Need Id from station
    ServiceResponseModel<List<ReadAirQualityDTO>> ReadAirQualityByStationId(Guid StationId);

    // Return a specific air quality log. Need Id from log
    ServiceResponseModel<ReadAirQualityWStationDTO> ReadAirQualityById(Guid Id);

    // Edit a specific air quality log. Need Id from log and new data in dto format
    ServiceResponseModel<AirQualityModel> UpdateAirQuality(Guid Id, UpdateAirQualityDTO dto);

    // Remove specific log. Need Id from log
    ServiceResponseModel<AirQualityModel> DeleteAirQuality(Guid Id);
}
