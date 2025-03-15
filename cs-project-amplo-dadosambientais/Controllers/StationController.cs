using cs_project_amplo_dadosambientais.Data.DTO.Station;
using cs_project_amplo_dadosambientais.Models;
using cs_project_amplo_dadosambientais.Services;
using Microsoft.AspNetCore.Mvc;

namespace cs_project_amplo_dadosambientais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private StationService _stationService;

        public StationController(StationService stationService)
        {
            _stationService = stationService;
        }

        [HttpPost]
        public ActionResult CreateStation(CreateStationDTO dto)
        {
            var response = _stationService.CreateStation(dto);
            if(response.Status == true)
            {
                return Created();
            }

            return BadRequest(response);
        }

        [HttpGet]
        public ActionResult<ServiceResponseModel<List<ReadStationDTO>>> ReadStations()
        {
            var response = _stationService.ReadStations();
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<ServiceResponseModel<ReadStationDTO>> ReadStationById(Guid Id)
        {
            var response = _stationService.ReadStationById(Id);
            if(response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        public ActionResult UpdateStation(Guid Id, UpdateStationDTO dto)
        {
            var response = _stationService.UpdateStation(Id, dto);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }

        [HttpDelete]
        public ActionResult DeleteStation(Guid Id)
        {
            var response = _stationService.DeleteStation(Id);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }
    }
}
