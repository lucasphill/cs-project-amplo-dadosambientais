using cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;
using cs_project_amplo_dadosambientais.Models;
using cs_project_amplo_dadosambientais.Services;
using Microsoft.AspNetCore.Mvc;

namespace cs_project_amplo_dadosambientais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        AirQualityService _airQualityService;

        public AirQualityController(AirQualityService airQualityService)
        {
            _airQualityService = airQualityService;
        }

        [HttpPost]
        public ActionResult CreateAirQuality(CreateAirQualityDTO dto)
        {
            var response = _airQualityService.CreateAirQuality(dto);
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public ActionResult<ServiceResponseModel<List<ReadAirQualityDTO>>> ReadAirQuality()
        {
            var response = _airQualityService.ReadAirQuality();
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<ServiceResponseModel<ReadAirQualityDTO>> ReadAirQualityById(Guid Id)
        {
            var response = _airQualityService.ReadAirQualityById(Id);
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("ByStationId/{StationId}")]
        public ActionResult<ServiceResponseModel<List<ReadAirQualityDTO>>> ReadAirQualityByStationId(Guid StationId)
        {
            var response = _airQualityService.ReadAirQualityByStationId(StationId);
            if (response.Status == true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        public ActionResult UpdateAirQuality(Guid Id, UpdateAirQualityDTO dto)
        {
            var response = _airQualityService.UpdateAirQuality(Id, dto);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }

        [HttpDelete]
        public ActionResult DeleteAirQuality(Guid Id)
        {
            var response = _airQualityService.DeleteAirQuality(Id);
            if (response.Status == true)
            {
                return NoContent();
            }

            return BadRequest(response);
        }
    }
}
