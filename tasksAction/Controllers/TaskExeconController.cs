using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using tasksAction.Conn;
using tasksAction.Data;
using tasksAction.Models;

namespace tasksAction.Controllers
{

 //   [Authorize]
    [ApiController]
    public class TaskExeconController : ControllerBase
    {
        //private readonly string serverName_ = Environment.MachineName;  //"XCNIVANTIP"; //

        [EnableCors("RulesCors")]

        #region GetTaskByAssignmentId
        [HttpGet]
        [Route("GetTaskITSM")]
        public async Task<IActionResult> GetTaskId(string assignmentId, string serverName)
        {
            TaskITSM taskITSM = new TaskITSM { frmAssignmentId = assignmentId.Trim() };
            try
            {
                taskITSM = await TaskData.GetTaskId(taskITSM, serverName);
                if (taskITSM.frmRecIdTask == "" || taskITSM.scheduled_type_event == "" || taskITSM.scheduled_clasification_name == "" || taskITSM.id_user == "" || taskITSM.scheduled_date_programming == "" || taskITSM.scheduled_hour_since == "" || taskITSM.scheduled_client_uuid == "")
                {
                    #region Validación de campos Obligatorios
                    string requerido = "Datos incompletos del SP_TrackPoint_SelTaskIvanti - GetTaskId Campos: ";

                    if (taskITSM.frmRecIdTask == "")
                    {
                        requerido = String.Concat(requerido, "frmRecIdTask, ");
                    }

                    if (taskITSM.scheduled_type_event == "")
                    {
                        requerido = String.Concat(requerido, "scheduled_type_event, ");
                    }

                    if (taskITSM.scheduled_clasification_name == "")
                    {
                        requerido = String.Concat(requerido, "scheduled_clasification_name, ");
                    }

                    if (taskITSM.id_user == "")
                    {
                        requerido = String.Concat(requerido, "id_user, ");
                    }

                    if (taskITSM.scheduled_date_programming == "")
                    {
                        requerido = String.Concat(requerido, "scheduled_date_programming, ");
                    }

                    if (taskITSM.scheduled_hour_since == "")
                    {
                        requerido = String.Concat(requerido, "scheduled_hour_since, ");
                    }

                    if (taskITSM.scheduled_client_uuid == "")
                    {
                        requerido = String.Concat(requerido, "scheduled_client_uuid, ");
                    }
                    #endregion


                    return StatusCode(StatusCodes.Status200OK, new { Status = "fail", message = requerido, data = taskITSM });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { Status = StatusCodes.Status200OK, message = "Successed", data = taskITSM });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }
        }
        #endregion

        #region TasksSearch
        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> TasksSearch(string? assignmentId, string? parentId, string? status, string? owner, string? zona, string? tipoTareas, string? fechaInicio, string? fechaFin, string? fechaInicioRec, string? fechaFinRec, string isTrackpoint, string serverName)
        {
            List<string> parameters = new List<string>();
            List<TasksSearchM> lstResult = new List<TasksSearchM>();

            parameters.Add(assignmentId.IsNullOrEmpty() ? "" : assignmentId.ToString());
            parameters.Add(parentId.IsNullOrEmpty() ? "" : parentId.ToString());
            parameters.Add(status.IsNullOrEmpty() ? "" : status.ToString());
            parameters.Add(owner.IsNullOrEmpty() ? "" : owner.ToString());
            parameters.Add(zona.IsNullOrEmpty() ? "" : zona.ToString());
            parameters.Add(tipoTareas.IsNullOrEmpty() ? "" : tipoTareas.ToString());
            parameters.Add(fechaInicio.IsNullOrEmpty() ? "" : fechaInicio.ToString());
            parameters.Add(fechaFin.IsNullOrEmpty() ? "" : fechaFin.ToString());
            parameters.Add(fechaInicioRec.IsNullOrEmpty() ? "" : fechaInicioRec.ToString());
            parameters.Add(fechaFinRec.IsNullOrEmpty() ? "" : fechaFinRec.ToString());
            parameters.Add(isTrackpoint);

            try
            {
                lstResult = await TasksSearchData.FnTasksSearch(parameters, serverName);
                if (lstResult.Count == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new { Status = StatusCodes.Status200OK, message = "No Data", data = lstResult });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { Status = StatusCodes.Status200OK, message = "Successed", data = lstResult });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, message = ex.Message });
            }
        }
        #endregion

        #region UpTaskITSM
        [Route("UpTaskITSM")]
        [HttpPut]

        public async Task<IActionResult> UpdateTaskExecon([FromBody] TPActivity upActivity)
        {

            try
            {
                //System.Console.WriteLine("Webhook received: \n");
                //System.Console.WriteLine(JsonConvert.SerializeObject(upActivity, Formatting.Indented) + "\n");
                string serverName = upActivity.data.preload[0].frmServer;
                //ACTUALIZA TAREA 
                UpTaskITSM fn = new UpTaskITSM();
                await fn.UpTask(upActivity, serverName);

                //new FileService().CreaJson(upActivity);
                //new Logs().WriteLog($"Comentarios Tarea {upActivity.data.preload.FirstOrDefault().frmAssignmentId}: Actualizada Satisfactoriamente" + "\n\n" + JsonConvert.SerializeObject(upActivity, Formatting.Indented));
                return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, message = "Tarea actualizada" });
            }
            catch (Exception ex)
            {
                new Logs().WriteLog($"Exception Tarea {upActivity.data.preload.FirstOrDefault().frmAssignmentId}: " + StatusCodes.Status500InternalServerError + " " + ex.Message + "\n\n" + JsonConvert.SerializeObject(upActivity, Formatting.Indented), []);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "fail", message = ex.Message });
            }
        }
        #endregion

      

    }
}