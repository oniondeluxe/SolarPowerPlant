using Microsoft.AspNetCore.Mvc;
using OnionDlx.SolPwr.Dto;
using OnionDlx.SolPwr.Services;

namespace OnionDlx.SolPwr.Application.Controllers
{
    [ApiController]
    [Route("auth")]
    public class UserAuthController : Controller
    {
        readonly IUserAuthService _service;

        public UserAuthController(IUserAuthService service)
        {
            _service = service;
        }


        [HttpPost(Name = "RegisterAccount")]
        [Route("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] UserAccountRegistration dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.RegisterUserAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost(Name = "SignOn")]
        [Route("signon")]
        public async Task<IActionResult> SignonUser([FromBody] UserSignonRecord dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _service.SignonUserAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
