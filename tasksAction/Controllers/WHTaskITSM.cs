using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using tasksAction.Conn;
using tasksAction.Data;

namespace tasksAction.Controllers
{
    [ApiController]
    public class WHTaskITSMController : ControllerBase
    {
        //private readonly string serverName_ = Environment.MachineName;
        [EnableCors("RulesCors")]

        #region Webhook ITSM
        [Route("WHTaskITSM")]
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ReceiveResponse() 
        {
            string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject(requestBody).ToString();
            JObject json = JObject.Parse(body);
            JToken data = json["data"];
            string serverName = Convert.ToString(data["preload"]?[0]?["frmServer"]?.ToString() is null ? DBNull.Value : data["preload"]?[0]?["frmServer"]?.ToString());

            new FileService().CreaJsonWH(json);
            
            try
            {
                WhTaskITSM fn = new WhTaskITSM();
                await fn.UpTask(data, serverName);

                return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = "Actualizada Satisfactoriamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "fail", message = ex.Message });
            }
        }
        #endregion
    }
}