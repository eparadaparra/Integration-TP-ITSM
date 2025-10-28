using Microsoft.AspNetCore.Mvc;

using AuthToken.Models;
using AuthToken.Custom;
using Microsoft.AspNetCore.Authorization;

namespace AuthToken.Controllers
{
    [ApiController]
    [AllowAnonymous]
    //[Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Utilities _utilities;

        public AuthController(Utilities utilities)
        {
            _utilities = utilities;
        }

        [HttpPost]
        [Route("AuthTokenExecon")]
        public IActionResult Validar([FromBody] AuthUsr request)
        {

            if (request.email == "api.connection@execon.mx" && request.pass == "FA6377E0B847FC1259FFFA1B6F824DE21FA5E163D399D01803D6E5E11534B077")
            {
                AuthResult authResult = _utilities.triggerJWT(request); 

                return StatusCode(StatusCodes.Status200OK, authResult );
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
    }
}
