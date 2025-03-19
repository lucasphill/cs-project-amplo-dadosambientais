using cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;
using cs_project_amplo_dadosambientais.Models;
using cs_project_amplo_dadosambientais.Services.AirQuality;
using Microsoft.AspNetCore.Mvc;

namespace cs_project_amplo_dadosambientais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityInterface _airQualityInterface;
        private readonly ILogger<AirQualityController> _logger;

        public AirQualityController(IAirQualityInterface airQualityInterface, ILogger<AirQualityController> logger)
        {
            _airQualityInterface = airQualityInterface;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<ServiceResponseModel<AirQualityModel>> CreateAirQuality(CreateAirQualityDTO dto)
        {
            var response = _airQualityInterface.CreateAirQuality(dto);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Status} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Status} - {response.Message}");
            return BadRequest(response);
        }

        [HttpGet]
        public ActionResult<ServiceResponseModel<List<ReadAirQualityDTO>>> ReadAirQuality()
        {
            var response = _airQualityInterface.ReadAirQuality();
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Status} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Status} - {response.Message}");
            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<ServiceResponseModel<ReadAirQualityDTO>> ReadAirQualityById(Guid Id)
        {
            var response = _airQualityInterface.ReadAirQualityById(Id);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Status} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
            return BadRequest(response);
        }

        [HttpGet("ByStationId/{StationId}")]
        public ActionResult<ServiceResponseModel<List<ReadAirQualityDTO>>> ReadAirQualityByStationId(Guid StationId)
        {
            var response = _airQualityInterface.ReadAirQualityByStationId(StationId);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Status} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message}");
            return BadRequest(response);
        }

        [HttpPut]
        public ActionResult<ServiceResponseModel<AirQualityModel>> UpdateAirQuality(Guid Id, UpdateAirQualityDTO dto)
        {
            var response = _airQualityInterface.UpdateAirQuality(Id, dto);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
                return NoContent();
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
            return BadRequest(response);
        }

        [HttpDelete]
        public ActionResult<ServiceResponseModel<AirQualityModel>> DeleteAirQuality(Guid Id)
        {
            var response = _airQualityInterface.DeleteAirQuality(Id);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
                return NoContent();
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
            return BadRequest(response);
        }
    }
}