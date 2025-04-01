using Microsoft.AspNetCore.Mvc;
using OnionDlx.SolPwr.Services;

namespace OnionDlx.SolPwr.Application.Controllers
{
    [Route("tools")]
    public class ToolsController : Controller
    {
        readonly IPlantManagementService _service;


        [HttpPost]
        [Route("SeedPlants")]
        public async Task<IActionResult> SeedPlants([FromQuery(Name = "quarters")] int quartersBehind)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.SeedPlantsAsync(quartersBehind);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        public ToolsController(IPlantManagementService service)
        {
            _service = service;
        }
    }
}
