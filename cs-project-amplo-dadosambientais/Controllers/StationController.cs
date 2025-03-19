using cs_project_amplo_dadosambientais.Data.DTO.Station;
using cs_project_amplo_dadosambientais.Models;
using cs_project_amplo_dadosambientais.Services.Station;
using Microsoft.AspNetCore.Mvc;

namespace cs_project_amplo_dadosambientais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationInterface _stationInterface;
        private readonly ILogger<StationController> _logger;

        public StationController(IStationInterface stationInterface, ILogger<StationController> logger)
        {
            _stationInterface = stationInterface;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult CreateStation(CreateStationDTO dto)
        {
            var response = _stationInterface.CreateStation(dto);
            if(response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message}");
            return BadRequest(response);
        }

        [HttpGet]
        public ActionResult<ServiceResponseModel<List<ReadStationDTO>>> ReadStations()
        {
            var response = _stationInterface.ReadStations();
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message}");
                return Ok(response);
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message}");
            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<ServiceResponseModel<ReadStationDTO>> ReadStationById(Guid Id)
        {
            var response = _stationInterface.ReadStationById(Id);
            if(response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
                return Ok(response);
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
            return BadRequest(response);
        }

        [HttpPut]
        public ActionResult UpdateStation(Guid Id, UpdateStationDTO dto)
        {
            var response = _stationInterface.UpdateStation(Id, dto);
            if (response.Status == true)
            {
                _logger.LogInformation($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
                return NoContent();
            }
            _logger.LogError($"{response.Timestamp.ToString()} - {response.Message} - {Id}");
            return BadRequest(response);
        }

        [HttpDelete]
        public ActionResult DeleteStation(Guid Id)
        {
            var response = _stationInterface.DeleteStation(Id);
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
