using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Templates.Web.Models;

namespace Templates.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var expectedLogin = _configuration["AuthInfo:Login"];
            var expectedPassword = _configuration["AuthInfo:Password"];

            if (model.Login != expectedLogin || model.Password != expectedPassword)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok();
        }
    }
}