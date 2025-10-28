using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using tasksAction.Conn;
using tasksAction.Models;

namespace tasksAction.Data
{
    public class UpTaskITSM
    {
        Connection cn = new Connection();

        #region UpTaskExecon SP_TrackPoint_UpTaskIvanti
        public async Task UpTask([FromBody] TPActivity objeto, string server) //[FromBody] 
        {
            //Console.WriteLine(JsonConvert.SerializeObject(objeto) + "\n");

            SqlConnection sql = new SqlConnection();
            string spName = "";

            if (server == "XCNIVANTIP")
            {
                spName = "EXsp_TrackPoint_UpTaskIvanti";
                sql = new SqlConnection(cn.SqlCommITSMPRO());
            }
            else
            {
                spName = "SP_TrackPoint_UpTaskIvanti";
                sql = new SqlConnection(cn.SqlComm());
            }

            //switch (server)
            //{
            //    case "CARSA-HEAT2015":
            //        spName = "SP_TrackPoint_UpTaskIvanti";
            //        sql = new SqlConnection(cn.SqlComm());
            //    break;
            //    case "XCNIVANTIP":
            //        spName = "EXsp_TrackPoint_UpTaskIvanti";
            //        sql = new SqlConnection(cn.SqlCommITSMPRO());
            //    break;
            //}

            //using (var sql = new SqlConnection(cn.SqlComm()))
            using ( sql )
            {
                //using (var cmd = new SqlCommand("SP_TrackPoint_UpTaskIvanti", sql))
                using (var cmd = new SqlCommand( spName, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdEvento",    Convert.ToString(objeto.data.status is null ? DBNull.Value : objeto.data.status));
                    cmd.Parameters.AddWithValue("@IdSitio",     Convert.ToString(objeto.data.preload.FirstOrDefault().frmIdSitio is null ? DBNull.Value : objeto.data.preload.FirstOrDefault().frmIdSitio));
                    cmd.Parameters.AddWithValue("@RecIdTask",   Convert.ToString(objeto.data.preload.FirstOrDefault().frmRecIdTask is null ? DBNull.Value : objeto.data.preload.FirstOrDefault().frmRecIdTask));
                    cmd.Parameters.AddWithValue("@IdTaskTP",    objeto.data.firebase_id != null ? Convert.ToString(objeto.data.firebase_id is null ? DBNull.Value : objeto.data.firebase_id) : null);
                    cmd.Parameters.AddWithValue("@StatusName",  objeto.data.statusInfo != null ? Convert.ToString(objeto.data.statusInfo.txt is null ? DBNull.Value : objeto.data.statusInfo.txt) : null);
                    cmd.Parameters.AddWithValue("@cooLat", (objeto.data.elements is null || objeto.data.elements.Count == 0)
                        ? null
                        : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a sitio".ToUpper()).FirstOrDefault().info is not null
                            ? Convert.ToString(
                                objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a sitio".ToUpper()).FirstOrDefault().info.latitude == ""
                                    ? DBNull.Value
                                    : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a sitio".ToUpper()).FirstOrDefault().info.latitude
                                )
                            : null
                    );
                    cmd.Parameters.AddWithValue("@cooLong", (objeto.data.elements is null || objeto.data.elements.Count == 0)
                        ? null
                        : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info is not null
                            ? Convert.ToString(
                                objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info.longitude == ""
                                    ? DBNull.Value
                                    : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info.longitude
                                )
                            : null
                    );
                    cmd.Parameters.AddWithValue("@FechaInicio", objeto.data.start_date != "" 
                        ? objeto.data.start_date == ""
                            ? DBNull.Value 
                            : String.Concat(Convert.ToString(objeto.data.start_date))
                        : null
                    );
                    cmd.Parameters.AddWithValue("@FechaLlegadaSitio", (objeto.data.elements is null || objeto.data.elements.Count == 0)
                        ? null
                        : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info is not null
                            ? Convert.ToString(
                                objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info.check_date is null
                                    ? DBNull.Value
                                    : objeto.data.elements.Where(item => item.title.ToUpper() == "Llegada a Sitio".ToUpper()).FirstOrDefault().info.check_date
                                )
                            : null
                    );
                    cmd.Parameters.AddWithValue("@FechaFin", objeto.data.end_date != "" 
                        ? objeto.data.end_date == "" 
                            ? DBNull.Value 
                            : String.Concat(Convert.ToString(objeto.data.end_date)) 
                        : null
                    );
                    cmd.Parameters.AddWithValue("@UserTask", 
                        (objeto.data.scheduled_user_email is not null)
                            ? new MailAddress( Convert.ToString( objeto.data.scheduled_user_email ) ).User 
                            : null
                    );
                    cmd.Parameters.AddWithValue("@FechaProgramada", objeto.data.scheduled_date_programming != null 
                        ? String.Concat(Convert.ToString(objeto.data.scheduled_date_programming), ' ', Convert.ToString(objeto.data.scheduled_hour_since)) 
                        : null
                    );
                    cmd.Parameters.AddWithValue("@DetailTask", objeto.data.scheduled_instructions != null ? Convert.ToString(objeto.data.scheduled_instructions is null ? DBNull.Value : objeto.data.scheduled_instructions) : null);
                    await cmd.ExecuteNonQueryAsync();
                    await sql.CloseAsync();
                }
                await sql.CloseAsync();
            }
        }
        #endregion
    }
}