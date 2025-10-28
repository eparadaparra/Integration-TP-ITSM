using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasksAction.Custom;
using tasksAction.Models;

namespace tasksAction.Controllers
{
    //[Route("api/")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Utilities _utilities;
        public AuthController(IConfiguration config, Utilities utilities)
        {
            _utilities = utilities;
            _config = config;
        }

        [EnableCors("RulesCors")]

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AuthUsr objeto)
        {
            //string encryptPass = _utilities.EncryptSHA256(objeto.pass).ToUpper();
            if (_config["Settings:email"].ToString().ToUpper() == objeto.email.ToUpper() && _config["Settings:pass"].ToUpper() == objeto.password.ToUpper())//encryptPass)
            {
                AuthResult token = new AuthResult { token = _utilities.triggerJWT(objeto) };
                return StatusCode(StatusCodes.Status200OK, new { token.token }); 
            } else {
                return StatusCode(StatusCodes.Status401Unauthorized, new { status = StatusCodes.Status401Unauthorized, message = "Credenciales incorrectas", token = ""}); 
            }
        }
    }
}