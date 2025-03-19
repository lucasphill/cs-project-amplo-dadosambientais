using cs_project_amplo_dadosambientais.Data;
using cs_project_amplo_dadosambientais.Data.DTO.Station;
using cs_project_amplo_dadosambientais.Models;

namespace cs_project_amplo_dadosambientais.Services.Station
{
    public class StationService : IStationInterface
    {
        private AppDbContext _context;

        public StationService(AppDbContext context)
        {
            _context = context;
        }

        public ServiceResponseModel<StationModel> CreateStation(CreateStationDTO dto)
        {
            /// <summary>
            /// Receive Data in CreateStationDTO format and insert in _context.Station
            /// Return Response Model with message
            /// Does not return inserted data
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<StationModel>();
            try
            {
                // Transform DTO to model
                var data = new StationModel
                {
                    Name = dto.Name,
                    Obs = dto.Obs
                };

                _context.Station.Add(data);
                _context.SaveChanges();

                serviceResponse.Message = "Inserted"; // Set response model message
                serviceResponse.Data = null;

                return serviceResponse;

            }
            catch(Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
        public ServiceResponseModel<List<StationModel>> ReadStations()
        {
            /// <summary>
            /// Return a list of ALL Stations in database
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<List<StationModel>>();
            try
            {
                // Select all stations from database
                var data = _context.Station.ToList();
                serviceResponse.Message = "Ok"; // Set response model message
                serviceResponse.Data = data; // Set response model data

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
        public ServiceResponseModel<ReadStationDTO> ReadStationById(Guid Id)
        {
            /// <summary>
            /// Return a station information from GUID
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<ReadStationDTO>();
            try
            {
                var data = _context.Station.FirstOrDefault(station => station.Id == Id);
                if (data is null) // verify station id exists and return erro message
                {
                    serviceResponse.Message = "Station not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                var station = new ReadStationDTO // Set ReadStationDTO format StationModel to not expose original model
                {
                    Id = data.Id,
                    Name = data.Name,
                    Obs = data.Obs,
                    InsertTimeStamp = data.InsertTimeStamp
                };

                serviceResponse.Message = "Ok";
                serviceResponse.Data = station;

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
        public ServiceResponseModel<StationModel> UpdateStation(Guid Id, UpdateStationDTO dto)
        {
            /// <summary>
            /// Receive station GUID and new complete data
            /// Return response model with message
            /// Does not return updated data
            /// </summary>

            var serviceResponse = new ServiceResponseModel<StationModel>();
            try
            {
                var station = _context.Station.FirstOrDefault(station => station.Id == Id);
                if (station is null) // verify station id exists and return erro message
                {
                    serviceResponse.Message = "Station not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                station.Name = dto.Name; // Att station data with received dto data
                station.Obs = dto.Obs;

                _context.Station.Update(station);
                _context.SaveChanges();

                serviceResponse.Message = "Updated";

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
        public ServiceResponseModel<StationModel> DeleteStation(Guid Id)
        {
            /// <summary>
            /// Receive station GUID
            /// Return Response Model with message
            /// </summary>

            var serviceResponse = new ServiceResponseModel<StationModel>();
            try
            {
                var station = _context.Station.FirstOrDefault(station => station.Id == Id);
                if (station is null) // verify id exists
                {
                    serviceResponse.Message = "Station not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                _context.Station.Remove(station);
                _context.SaveChanges();

                serviceResponse.Message = "Deleted";

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
    }
}
