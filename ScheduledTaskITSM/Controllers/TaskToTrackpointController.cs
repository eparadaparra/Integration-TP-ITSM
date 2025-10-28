using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScheduledTaskITSM.Models;
using ScheduledTaskITSM.Services;

namespace ScheduledTaskITSM.Controllers
{
    [EnableCors("RulesCors")]

    [ApiController]
    public class TaskToTrackpointController : ControllerBase
    {
        private readonly IServices _services;
        private readonly string serverName_ = Environment.MachineName; //"XCNIVANTIP"; //
        public TaskToTrackpointController(IServices servicesAPI)
        {
            _services = servicesAPI;
        }

        [EnableCors("RulesCors")]

        #region Create Activity Trackpoint
        [HttpPost]
        [Route("TaskToTp/{assignmentId}")]
        public async Task<IActionResult> ProgrammingTaskToTP(string assignmentId)
        {
            try
            {
                //Obtiene Tarea desde API Execon GetTaskId
                TaskITSM taskSP = await _services.GetTaskITSM(assignmentId.Trim(), serverName_);

                if (taskSP.status == "fail")
                {
                    return StatusCode(StatusCodes.Status200OK, new { status = taskSP.status, message = taskSP.message, data = taskSP.data });
                }
                else
                {
                    ClientId clientId = await _services.GetCustomerTP(taskSP.data.scheduled_client_uuid, serverName_);

                    if (clientId.status == "fail")
                    {
                        return StatusCode(StatusCodes.Status200OK, new { status = clientId.status, message = clientId.message });
                    }
                    else
                    {
                        //TPActivity taskActivity = new TPActivity();
                        taskSP.data.scheduled_client_uuid = clientId.id;

                        TPActivity taskActivity = await _services.PostActivityTP(taskSP, serverName_);

                        return StatusCode(StatusCodes.Status200OK, new { status = taskActivity.status, message = taskActivity.message, data = taskActivity.data });

                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }

        }
        #endregion

        #region Obtiene datos de la Actividad
        [HttpPost]
        [Route("getJson/{firebaseId}")]
        public async Task<IActionResult> GetActivityTP(string firebaseId)
        {
            var objeto = await _services.GetActivityTP(firebaseId.Trim()) ;

            return StatusCode(StatusCodes.Status200OK, objeto.ToString() );
        }
        #endregion
    }
}