using System.IdentityModel.Tokens.Jwt;
using HW6.Helpers;
using HW6.Models;
using Microsoft.AspNetCore.Mvc;

namespace HW6.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationControllers(SessionStorage sessionStorage) : ControllerBase
    {
        private const string TokenHeaderName = "Authorization";
        private const string SessionIdCookieName = "sid";

        private readonly SessionStorage _sessionStorage = sessionStorage;

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserCredentials credentials)
        {
            var user = AccountStorage.GetByUserName(credentials.Username);
            if (user == null)
            {
                return Unauthorized(new { errorText = "Invalid username or password." });
            }

            var session = _sessionStorage.AddSession(user);

            HttpContext.Response.Cookies.Append(SessionIdCookieName, session.ToString());

            return Ok($"Hello, {user.FirstName} {user.LastName}!");
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(SessionIdCookieName, out var sessionId))
            {
                return Unauthorized(new { errorText = "Should login first." });
            }

            if (!Guid.TryParse(sessionId, out var sessionGuid))
            {
                return BadRequest(new { errorText = "Session id cookie has wrong format." });
            }

            if (!_sessionStorage.TryGetSessionToken(sessionGuid, out var jwt))
            {
                return Unauthorized(new { errorText = "Your session is expired or was logout." });
            }

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            HttpContext.Response.Headers.Append(TokenHeaderName, $"Bearer {encodedJwt}");

            return Ok();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(SessionIdCookieName, out var sessionId))
            {
                return Unauthorized(new { errorText = "Should login first." });
            }

            if (!Guid.TryParse(sessionId, out var sessionGuid))
            {
                return BadRequest(new { errorText = "Session id cookie has wrong format." });
            }

            _sessionStorage.RemoveSessionToken(sessionGuid);

            HttpContext.Response.Cookies.Delete(SessionIdCookieName);

            return Ok();
        }
    }
}
