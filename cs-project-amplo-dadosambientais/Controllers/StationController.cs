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

        public StationController(IStationInterface stationInterface)
        {
            _stationInterface = stationInterface;
        }

        [HttpPost]
        public ActionResult CreateStation(CreateStationDTO dto)
        {
            var response = _stationInterface.CreateStation(dto);
            if(response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public ActionResult<ServiceResponseModel<List<ReadStationDTO>>> ReadStations()
        {
            var response = _stationInterface.ReadStations();
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<ServiceResponseModel<ReadStationDTO>> ReadStationById(Guid Id)
        {
            var response = _stationInterface.ReadStationById(Id);
            if(response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        public ActionResult UpdateStation(Guid Id, UpdateStationDTO dto)
        {
            var response = _stationInterface.UpdateStation(Id, dto);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }

        [HttpDelete]
        public ActionResult DeleteStation(Guid Id)
        {
            var response = _stationInterface.DeleteStation(Id);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }
    }
}
