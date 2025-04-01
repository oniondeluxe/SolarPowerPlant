using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Dto;
using OnionDlx.SolPwr.Services;

namespace OnionDlx.SolPwr.Application.Controllers
{
    [Authorize]
    [Route("api")]
    public class PlantController : Controller
    {
        readonly IPlantManagementService _service;

        public PlantController(IPlantManagementService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("GetAllPlants")]
        public async Task<IEnumerable<PowerPlantImmutable>> GetAllPlants()
        {
            var result = await _service.GetAllPlantsAsync();
            return result;
        }


        [HttpPost]
        [Route("CreatePlant")]
        public async Task<IActionResult> CreatePlant([FromBody] PowerPlant dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.CreatePlantAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPut]
        [Route("DeletePlant")]
        public async Task<IActionResult> DeletePlant([FromQuery(Name = "id")] Guid identity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.DeletePlantAsync(identity);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPut]
        [Route("UpdatePlant")]
        public async Task<IActionResult> UpdatePlant([FromQuery(Name = "id")] Guid identity, [FromBody] PowerPlant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.UpdatePlantAsync(identity, plant);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpGet]
        [Route("GetPowerData")]
        public async Task<IEnumerable<PlantPowerData>> GetPowerData([FromQuery(Name = "id")] Guid identity,
                                                                    [FromQuery(Name = "type")] int type,                    // History or forecast
                                                                    [FromQuery(Name = "resolution")] int resolution,        // 15 | 60 minutes
                                                                    [FromQuery(Name = "timespan")] int timeSpan,            // Number of units
                                                                    [FromQuery(Name = "timespancode")] string timeSpanCode) // Kind of units [m | h | d]
        {
            PowerDataTypes typeEnum = (PowerDataTypes)type;
            TimeResolution resol = TimeResolution.FifteenMinutes;
            if (resolution == 60)
            {
                resol = TimeResolution.SixtyMinutes;
            }
            else if (resolution != 15)
            {
                return Array.Empty<PlantPowerData>();
            }
            var code = TimeSpanCode.None;
            if (timeSpanCode == "m")
            {
                code = TimeSpanCode.Minutes;
            }
            else if (timeSpanCode == "h")
            {
                code = TimeSpanCode.Hours;
            }
            else if (timeSpanCode == "d")
            {
                code = TimeSpanCode.Days;
            }

            var result = await _service.GetPowerDataAsync(identity, typeEnum, resol, code, timeSpan);
            return result;
        }
    }
}
