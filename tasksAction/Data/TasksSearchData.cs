using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using tasksAction.Conn;
using tasksAction.Models;

namespace tasksAction.Data
{
    public class TasksSearchData
    {
        #region GetTasksResults SP_TrackPoint_SelTasksSearch
        public static async Task<List<TasksSearchM>> FnTasksSearch(List<string> parametros, string server)
        {
            Connection cn = new Connection();
            SqlConnection sql = new SqlConnection();
            string spName = "";

            if (server == "XCNIVANTIP")
            {
                spName = "EXsp_TrackPoint_SelTasksSearch";
                sql = new SqlConnection(cn.SqlCommITSMPRO());
            }
            else
            {
                spName = "SP_TrackPoint_SelTasksSearch";
                sql = new SqlConnection(cn.SqlComm());
            }

            //switch (server)
            //{
            //    case "CARSA-HEAT2015":
            //        spName = "SP_TrackPoint_SelTasksSearch";
            //        sql = new SqlConnection(cn.SqlComm());
            //        break;
            //    case "XCNIVANTIP":
            //        spName = "EXsp_TrackPoint_SelTasksSearch";
            //        sql = new SqlConnection(cn.SqlCommITSMPRO());
            //        break;
            //}

            List<TasksSearchM> list = new List<TasksSearchM>();
            try
            {
                //using (var sql = new SqlConnection(cn.SqlComm()))
                using ( sql )
                {
                    //using (var cmd = new SqlCommand("SP_TrackPoint_SelTasksSearch", sql))
                    using (var cmd = new SqlCommand( spName, sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AssignmentId",    parametros[0].IsNullOrEmpty() ? DBNull.Value : parametros[0].ToString());
                        cmd.Parameters.AddWithValue("@ParentId",        parametros[1].IsNullOrEmpty() ? DBNull.Value : parametros[1].ToString());
                        cmd.Parameters.AddWithValue("@Status",          parametros[2].IsNullOrEmpty() ? DBNull.Value : parametros[2].ToString());
                        cmd.Parameters.AddWithValue("@Owner",           parametros[3].IsNullOrEmpty() ? DBNull.Value : parametros[3].ToString());
                        cmd.Parameters.AddWithValue("@Team",            parametros[4].IsNullOrEmpty() ? DBNull.Value : parametros[4].ToString());
                        cmd.Parameters.AddWithValue("@TipoTarea",       parametros[5].IsNullOrEmpty() ? DBNull.Value : parametros[5].ToString());
                        cmd.Parameters.AddWithValue("@FechaInicio",     parametros[6].IsNullOrEmpty() ? DBNull.Value : parametros[6].ToString());
                        cmd.Parameters.AddWithValue("@FechaFin",        parametros[7].IsNullOrEmpty() ? DBNull.Value : parametros[7].ToString());
                        cmd.Parameters.AddWithValue("@FechaInicioRec",  parametros[8].IsNullOrEmpty() ? DBNull.Value : parametros[8].ToString());
                        cmd.Parameters.AddWithValue("@FechaFinRec",     parametros[9].IsNullOrEmpty() ? DBNull.Value : parametros[9].ToString());
                        cmd.Parameters.AddWithValue("@IsTrackpoint", Convert.ToInt32(parametros[10]));

                        using (var item = await cmd.ExecuteReaderAsync()) 
                        {
                            while (await item.ReadAsync())
                            {
                                list.Add( new TasksSearchM() {

                                    IdTarea             = Convert.ToString(item["IdTarea"]            is null ? DBNull.Value : item["IdTarea"]),
                                    ParentId            = Convert.ToString(item["ParentId"]           is null ? DBNull.Value : item["ParentId"]),
                                    TipoTarea           = Convert.ToString(item["TipoTarea"]          is null ? DBNull.Value : item["TipoTarea"]),
                                    FirebaseId          = Convert.ToString(item["FirebaseId"]         is null ? DBNull.Value : item["FirebaseId"]),
                                    StatusIvanti        = Convert.ToString(item["StatusIvanti"]       is null ? DBNull.Value : item["StatusIvanti"]),
                                    SubStatus           = Convert.ToString(item["SubStatus"]          is null ? DBNull.Value : item["SubStatus"]),
                                    Owner               = Convert.ToString(item["Owner"]              is null ? DBNull.Value : item["Owner"]),
                                    Prioridad           = Convert.ToString(item["Prioridad"]          is null ? DBNull.Value : item["Prioridad"]),
                                    ManagerTeam         = Convert.ToString(item["ManagerTeam"]        is null ? DBNull.Value : item["ManagerTeam"]),
                                    Team                = Convert.ToString(item["Team"]               is null ? DBNull.Value : item["Team"]),
                                    CustId              = Convert.ToString(item["CustID"]             is null ? DBNull.Value : item["CustID"]),
                                    IdSitio             = Convert.ToString(item["IdSitio"]            is null ? DBNull.Value : item["IdSitio"]),
                                    FechaCreacion       = Convert.ToString(item["FechaCreacion"]      is null ? DBNull.Value : item["FechaCreacion"]),
                                    FechaAsignacion     = Convert.ToString(item["FechaAsignacion"]    is null ? DBNull.Value : item["FechaAsignacion"]),
                                    FechaRequerida      = Convert.ToString(item["FechaRequerida"]     is null ? DBNull.Value : item["FechaRequerida"]),
                                    FechaTraslado       = Convert.ToString(item["FechaTraslado"]      is null ? DBNull.Value : item["FechaTraslado"]),
                                    FechaInicio         = Convert.ToString(item["FechaInicio"]        is null ? DBNull.Value : item["FechaInicio"]),
                                    FechaFin            = Convert.ToString(item["FechaFin"]           is null ? DBNull.Value : item["FechaFin"]),
                                    FechaCompletada     = Convert.ToString(item["FechaCompletada"]    is null ? DBNull.Value : item["FechaCompletada"]),
                                    FechaResolucion     = Convert.ToString(item["FechaResolucion"]    is null ? DBNull.Value : item["FechaResolucion"]),
                                    ModificadoPor       = Convert.ToString(item["ModificadoPor"]      is null ? DBNull.Value : item["ModificadoPor"]),
                                    UltimaModificacion  = Convert.ToString(item["UltimaModificacion"] is null ? DBNull.Value : item["UltimaModificacion"]),
                                    RecId               = Convert.ToString(item["RecId"]              is null ? DBNull.Value : item["RecId"])
                                });
                            }
                        }
                    }
                    await sql.CloseAsync();
                }
                return list; 
            } catch (Exception ex)
            {  
                return list; 
            }
        }
        #endregion
    }
}
