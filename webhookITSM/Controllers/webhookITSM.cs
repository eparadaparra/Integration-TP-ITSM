using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webhookITSM.Services;

namespace webhookITSM.Controllers
{
    [EnableCors("RulesCors")]
    [Route("/")]
    [ApiController]
    public class webhookITSMController : ControllerBase
    {
        private readonly IServices _services;
        public webhookITSMController(IServices services)
        {
            _services = services;
        }

        [EnableCors("RulesCors")]

        #region Webhook ITSM
        [HttpPost]
        [Route("webhookITSM")]
        public async Task<IActionResult> ReceiveResponse()
        {
            bool response = false;
            string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject(requestBody);

            try
            {
                response = await _services.WHTaskITSM(data);

                if (response)
                {
                    return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = $"Tarea Actualizada" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = $"Falló la actualización" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "fail", message = ex.Message });
            }
        }
        #endregion
    }
}