using System.Data;
using System.Data.SqlClient;
using tasksAction.Conn;
using tasksAction.Models;

namespace tasksAction.Data
{
    public class TaskData
    {

        #region GetTaskExecon SP_TrackPoint_SelTaskIvanti
        public static async Task<TaskITSM> GetTaskId(TaskITSM parametros, string server)
        {
            Connection cn = new Connection();
            TaskITSM taskModel = new TaskITSM();
            SqlConnection sql = new SqlConnection();
            string spName = "";

            if (server == "XCNIVANTIP")
            {
                spName = "EXsp_TrackPoint_SelTaskIvanti";
                sql = new SqlConnection(cn.SqlCommITSMPRO());
            }
            else
            {
                spName = "SP_TrackPoint_SelTaskIvanti";
                sql = new SqlConnection(cn.SqlComm());
            }

            //switch (server)
            //{
            //    case "CARSA-HEAT2015":
            //        spName = "SP_TrackPoint_SelTaskIvanti";
            //        sql = new SqlConnection(cn.SqlComm());
            //    break;
            //    case "XCNIVANTIP":
            //        spName = "EXsp_TrackPoint_SelTaskIvanti";
            //        sql = new SqlConnection(cn.SqlCommITSMPRO());
            //    break;
            //}

            try
            {
                taskModel = parametros;
                //using (var sql = new SqlConnection(cn.SqlComm()))
                using ( sql )
                {
                    //using (var cmd = new SqlCommand("SP_TrackPoint_SelTaskIvanti", sql))
                    using (var cmd = new SqlCommand(spName, sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@taskId", taskModel.frmAssignmentId);
                        using (var item = await cmd.ExecuteReaderAsync()) 
                        { 
                            while (await item.ReadAsync()) 
                            { 
                                taskModel.scheduled_type_event         = Convert.ToString(item["scheduled_type_event"]       is null ? DBNull.Value : item["scheduled_type_event"]);
                                taskModel.scheduled_name_event         = Convert.ToString(item["scheduled_name_event"]       is null ? DBNull.Value : item["scheduled_name_event"]);
                                taskModel.scheduled_client_uuid        = Convert.ToString(item["scheduled_client_uuid"]      is null ? DBNull.Value : item["scheduled_client_uuid"]);
                                taskModel.scheduled_periodicity        = Convert.ToString(item["scheduled_periodicity"]      is null ? DBNull.Value : item["scheduled_periodicity"]);
                                taskModel.id_user                      = Convert.ToString(item["id_user"]                    is null ? DBNull.Value : item["id_user"]);
                                taskModel.latitude                     = Convert.ToDouble(item["latitude"]                   );
                                taskModel.longitude                    = Convert.ToDouble(item["longitude"]                  );
                                taskModel.scheduled_address            = Convert.ToString(item["scheduled_address"]          is null ? DBNull.Value : item["scheduled_address"]);
                                taskModel.scheduled_date_programming   = Convert.ToString(item["scheduled_date_programming"] is null ? DBNull.Value : item["scheduled_date_programming"]);
                                taskModel.scheduled_hour_since         = Convert.ToString(item["scheduled_hour_since"]       is null ? DBNull.Value : item["scheduled_hour_since"]);
                                taskModel.scheduled_hour_limit         = Convert.ToString(item["scheduled_hour_limit"]       is null ? DBNull.Value : item["scheduled_hour_limit"]);
                                taskModel.scheduled_expiration_date    = Convert.ToInt32(item["scheduled_expiration_date"]);
                                taskModel.scheduled_instructions       = Convert.ToString(item["scheduled_instructions"]     is null ? DBNull.Value : item["scheduled_instructions"]);
                                taskModel.frmRecIdTask                 = Convert.ToString(item["frmRecIdTask"]               is null ? DBNull.Value : item["frmRecIdTask"]);
                                taskModel.frmParentNumber              = Convert.ToString(item["frmParentNumber"]            is null ? DBNull.Value : item["frmParentNumber"]);
                                taskModel.frmParentCategory            = Convert.ToString(item["frmParentCategory"]          is null ? DBNull.Value : item["frmParentCategory"]);
                                taskModel.frmIdSitio                   = Convert.ToString(item["frmIdSitio"]                 is null ? DBNull.Value : item["frmIdSitio"]);
                                taskModel.frmCustId                    = Convert.ToString(item["frmCustId"]                  is null ? DBNull.Value : item["frmCustId"]);
                                taskModel.frmCodigoCierre              = Convert.ToString(item["frmCodigoCierre"]            is null ? DBNull.Value : item["frmCodigoCierre"]);
                                taskModel.frmParentOwner               = Convert.ToString(item["frmParentOwner"]             is null ? DBNull.Value : item["frmParentOwner"]);
                                taskModel.frmServer                    = Convert.ToString(item["frmServer"]                  is null ? DBNull.Value : item["frmServer"]);

                                taskModel.scheduled_clasification_name    = Convert.ToString(item["scheduled_clasification_name"]    is null ? DBNull.Value : item["scheduled_clasification_name"]);
                                taskModel.scheduled_clasification         = Convert.ToString(item["scheduled_clasification"]         is null ? DBNull.Value : item["scheduled_clasification"]);
                                taskModel.scheduled_subclasification_name = Convert.ToString(item["scheduled_subclasification_name"] is null ? DBNull.Value : item["scheduled_subclasification_name"]);
                                taskModel.scheduled_subclasification      = Convert.ToString(item["scheduled_subclasification"]      is null ? DBNull.Value : item["scheduled_subclasification"]);
                            }
                        }
                        await sql.CloseAsync();
                    }
                }
                return taskModel; 
            } catch (Exception ex)
            {  
                Console.WriteLine(ex.Message);
                return taskModel; 
            }
        }
        #endregion
    }
}
