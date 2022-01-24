using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Dotes.Web.Auth;
using Ext.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Templates.Web.Auth;
using Templates.Web.Helpers;
using Templates.Web.Models;

namespace Templates.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IDataProtector _dataProtector;
        private readonly IConfiguration _configuration;

        public AuthController(
            IConfiguration configuration,
            IJwtFactory jwtFactory,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _dataProtector = dataProtectionProvider.CreateProtector(JwtIssuerOptions.DataProtectorPurpose);
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel credentials)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await GetClaimsIdentity(credentials.Login, credentials.Password);

            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    var jwt = await Tokens.GenerateJwt(result.identity, _jwtFactory, credentials.Login, _jwtOptions,
                        new JsonSerializerSettings { Formatting = Formatting.Indented });
                    return new OkObjectResult(jwt);
                default:
                    return new ObjectResult("User does not have permission") {StatusCode = 403};
            }
        }


        private async Task<(ClaimsIdentity identity, HttpStatusCode statusCode)> GetClaimsIdentity(string login, string password)
        {
            var expectedLogin = _configuration["AuthInfo:Login"];
            var expectedPassword = _configuration["AuthInfo:Password"];

            if (login != expectedLogin || password != expectedPassword)
            {
                return (null, HttpStatusCode.Forbidden);
            }

            HttpContext.Session.Set(SessionHelper.AuthSessionKey, new LoginModel(login, password));
            HttpContext.Response.Cookies.Append("login", _dataProtector.Protect(login), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            HttpContext.Response.Cookies.Append("password", _dataProtector.Protect(password), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return (await Task.FromResult(_jwtFactory.GenerateClaimsIdentityWithCamundaAccess(login)), HttpStatusCode.OK);
        }
    }
}