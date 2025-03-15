using cs_project_amplo_dadosambientais.Data;
using cs_project_amplo_dadosambientais.Models;
using cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace cs_project_amplo_dadosambientais.Services
{
    public class AirQualityService
    {
        private AppDbContext _context;

        public AirQualityService(AppDbContext context)
        {
            _context = context;
        }

        public ServiceResponseModel<StationModel> CreateAirQuality(CreateAirQualityDTO dto)
        {
            /// <summary>
            /// Receive Data in CreateAirQualityDTO format and insert in _context.AirQuality
            /// Return Response Model with message
            /// Does not return inserted data
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<StationModel>();
            try
            {
                var station = _context.Station.FirstOrDefault(station => station.Id == dto.StationId);
                if (station is null) // verify station id exists and return erro message
                {
                    serviceResponse.Message = "Station not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                // Transform DTO to model
                var data = new AirQualityModel
                {
                    Data = dto.Data is null ? "{}" : JsonSerializer.Serialize(dto.Data), // Transform received JSON to string and prevent it from null
                    Obs = dto.Obs,
                    Station = station
                };

                _context.AirQuality.Add(data);
                _context.SaveChanges();

                serviceResponse.Message = "Inserted"; // Set response model message

                return serviceResponse;

            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }
        public ServiceResponseModel<List<ReadAirQualityWStationDTO>> ReadAirQuality()
        {
            /// <summary>
            /// Return a list of ALL Air Quality Data with Station information in database
            /// Do not use for extense aplications. Instead, use ReadAirQualityByStationId()
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<List<ReadAirQualityWStationDTO>>();
            try
            {
                // Select all air quality data from database
                var data = _context.AirQuality
                    .Select(d => new ReadAirQualityWStationDTO // Set ReadAirQualityWStationDTO format from AirQualityModel to not expose original model
                    {
                        Id = d.Id,
                        Obs = d.Obs,
                        Data = JsonSerializer.Deserialize<Dictionary<string, object>>(d.Data, new JsonSerializerOptions { WriteIndented = true }), // Return idented JSON
                        Station = d.Station,
                        InsertTimeStamp = d.InsertTimeStamp
                    })
                    .ToList();

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

        public ServiceResponseModel<ReadAirQualityWStationDTO> ReadAirQualityById(Guid Id)
        {
            /// <summary>
            /// Return a Air Quality Data with Station information from inserted GUID
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<ReadAirQualityWStationDTO>();
            try
            {
                var data = _context.AirQuality.Include(s => s.Station).FirstOrDefault(aq => aq.Id == Id);
                if (data is null) // verify air quality data id exists and return erro message
                {
                    serviceResponse.Message = "Air quality data not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                var airQualityData = new ReadAirQualityWStationDTO // Set ReadAirQualityWStationDTO format from AirQualityModel to not expose original model
                {
                    Id = data.Id,
                    Obs = data.Obs,
                    Data = JsonSerializer.Deserialize<Dictionary<string, object>>(data.Data, new JsonSerializerOptions { WriteIndented = true }), // Return idented JSON
                    Station = data.Station,
                    InsertTimeStamp = data.InsertTimeStamp
                };

                serviceResponse.Message = "Ok";  // Set response model message
                serviceResponse.Data = airQualityData; // Set response model data

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Status = false;

                return serviceResponse;
            }
        }

        public ServiceResponseModel<List<ReadAirQualityDTO>> ReadAirQualityByStationId(Guid Id)
        {
            /// <summary>
            /// Return a list of Air Quality Data from given station GUID
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<List<ReadAirQualityDTO>>();
            try
            {
                var station = _context.Station.FirstOrDefault(station => station.Id == Id);
                if (station is null) // verify station id exists and return erro message
                {
                    serviceResponse.Message = "Station not found";
                    return serviceResponse;
                }

                // Select air quality data from database with given station GUID
                var data = _context.AirQuality // Show all data with station id only
                    .Where(data => data.Station.Id == Id)
                    .Select(data => new ReadAirQualityDTO
                    {
                        Id = data.Id,
                        Data = JsonSerializer.Deserialize<Dictionary<string, object>>(data.Data, new JsonSerializerOptions { WriteIndented = true }), // Return idented JSON
                        Obs = data.Obs,
                        InsertTimeStamp = data.InsertTimeStamp,
                    })
                    .ToList();

                if (data.Count == 0) // verify station have any data and return erro message
                {
                    serviceResponse.Message = "Station dont have any air quality data"; 
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

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

        public ServiceResponseModel<AirQualityModel> UpdateAirQuality(Guid Id, UpdateAirQualityDTO dto)
        {
            /// <summary>
            /// Receive air quality data GUId and new conmplete data
            /// Return Response Model with message
            /// Does not return updated data
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<AirQualityModel>();
            try
            {
                var airQualityData = _context.AirQuality.FirstOrDefault(aq => aq.Id == Id);
                if (airQualityData is null) // verify air quality data id exists and return erro message
                {
                    serviceResponse.Message = "Air quality data not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                var station = _context.Station.FirstOrDefault(station => station.Id == dto.StationId);
                if (station is null) // verify station id exists and return erro message
                {
                    serviceResponse.Message = "Station not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                // Transform DTO to model
                var data = new AirQualityModel 
                {
                    Data = dto.Data is null ? "{}" : JsonSerializer.Serialize(dto.Data),  // Transform received JSON to string and prevent it from null
                    Obs = dto.Obs,
                    Station = station
                };

                _context.AirQuality.Update(data);
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

        public ServiceResponseModel<AirQualityModel> DeleteAirQuality(Guid Id)
        {
            /// <summary>
            /// Receive air quality data GUID
            /// Return Response Model with message
            /// </summary>

            // Initialize response model object
            var serviceResponse = new ServiceResponseModel<AirQualityModel>();
            try
            {
                var airQualityData = _context.AirQuality.FirstOrDefault(aq => aq.Id == Id);
                if (airQualityData is null) // verify air quality data id exists and return erro message
                {
                    serviceResponse.Message = "Air quality data not found";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                _context.AirQuality.Remove(airQualityData);
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
