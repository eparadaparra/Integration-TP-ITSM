using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using tasksAction.Conn;
using tasksAction.Data;
using tasksAction.Models;

namespace tasksAction.Controllers
{
    //[Route("api/")]
    [Authorize]
    [ApiController]
    public class CustomerExeconController : ControllerBase
    {
        //private readonly string serverName_ = Environment.MachineName;
        [EnableCors("RulesCors")]

        #region CONSULTA SP PARA TRAER INFORMACION DE CLIENTE
        [HttpGet]
        [Route("GetCustomer")]
        public async Task<IActionResult> GetCustomerId(string custId, string serverName)
        {
            try
            {
                
                CustomerExecon customerExecon = new CustomerExecon { client_IdCustomer = custId };
                customerExecon = await CustomerData.GetCustomerId(customerExecon, serverName);

                if (customerExecon.recId is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = "No retorno datos el SP_TrackPoint_SelAccountIvanti de la funcion GetCustomerId", data = customerExecon });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = "Successed", data = customerExecon }); 
                }
            } catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "fail", message = ex.Message, data = "" });
            }
        }
        #endregion
    }
}
