using cs_project_amplo_dadosambientais.Data.DTO.Station;
using cs_project_amplo_dadosambientais.Models;

namespace cs_project_amplo_dadosambientais.Services.Station;

public interface IStationInterface
{
    public ServiceResponseModel<StationModel> CreateStation(CreateStationDTO dto);
    public ServiceResponseModel<List<StationModel>> ReadStations();
    public ServiceResponseModel<ReadStationDTO> ReadStationById(Guid Id);
    public ServiceResponseModel<StationModel> UpdateStation(Guid Id, UpdateStationDTO dto);
    public ServiceResponseModel<StationModel> DeleteStation(Guid Id);

}
