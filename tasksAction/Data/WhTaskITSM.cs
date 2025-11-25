using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Mail;
using tasksAction.Conn;

namespace tasksAction.Data
{
    public class WhTaskITSM
    {
        Connection cn = new Connection();

        #region UpTaskExecon SP_TrackPoint_UpTaskIvanti
        public async Task UpTask(JToken data, string server)
        {

            SqlConnection sql = new SqlConnection();
            string spName = "";

            if (server == "XCNIVANTIP")
            {
                spName = "EXsp_TrackPoint_UpTaskIvanti";
                sql = new SqlConnection(cn.SqlCommITSMPRO());
            }
            else {
                spName = "SP_TrackPoint_UpTaskIvanti";
                sql = new SqlConnection(cn.SqlComm());
            }

            //switch (server)
            //{
            //    case "CARSA-HEAT2015":
            //        spName = "SP_TrackPoint_UpTaskIvanti";
            //        sql = new SqlConnection(cn.SqlComm());
            //        break;
            //    case "XCNIVANTIP":
            //        spName = "EXsp_TrackPoint_UpTaskIvanti";
            //        sql = new SqlConnection(cn.SqlCommITSMPRO());
            //        break;
            //}

            List<string> lstUp  = new List<string>();
            string assignmentId             = Convert.ToString(data["preload"]?[0]?["frmAssignmentId"]?.ToString() is null ? DBNull.Value : data["preload"]?[0]?["frmAssignmentId"]?.ToString());
            string nombreOwner              = Convert.ToString(data["user_name"]?.ToString()           is null ? DBNull.Value : data["user_name"].ToString() );
            string tipoTarea                = Convert.ToString(data["modules_config"]?["name"]?.ToString() is null ? DBNull.Value : data["modules_config"]["name"].ToString());
            string idEvento                 = Convert.ToString( data["status"]?.ToString()           is null ? DBNull.Value : data["status"].ToString() );
            string statusName               = Convert.ToString( data["statusInfo"]?.ToString()       is null ? DBNull.Value : data["statusInfo"]?["txt"]?.ToString() is null ? DBNull.Value : data["statusInfo"]?["txt"]?.ToString() );
            string recIdTask                = Convert.ToString( data["preload"]?[0]?["frmRecIdTask"]?.ToString() is null ? DBNull.Value : data["preload"]?[0]?["frmRecIdTask"]?.ToString() );
            string IdSitio                  = Convert.ToString( data["preload"]?[0]?["frmIdSitio"]?.ToString()   is null ? DBNull.Value : data["preload"]?[0]?["frmIdSitio"]?.ToString());
            string custId                   = Convert.ToString( data["preload"]?[0]?["frmcustId"]?.ToString()    is null ? DBNull.Value : data["preload"]?[0]?["frmcustId"]?.ToString());
            string idTaskTP                 = Convert.ToString( data["firebase_id"]?.ToString()      is null ? DBNull.Value : data["firebase_id"]?.ToString());
            string cooLat                   = DBNull.Value.ToString();
            string cooLong                  = DBNull.Value.ToString();
            string fechaLlegadaSitio        = DBNull.Value.ToString();
            string fechaLlegadaSitioUTC     = DBNull.Value.ToString();
            string subStatusTask            = DBNull.Value.ToString();
            string comentariosCierre        = DBNull.Value.ToString();
            string codigoCierre             = DBNull.Value.ToString();
            string quienProporcionoCierre   = DBNull.Value.ToString();
            bool isCC = false;
            if (data["elements"] is not null)
            {
                foreach (var element in data["elements"])
                {
                    if (element["title"].ToString().ToUpper() == "Llegada a sitio".ToUpper())
                    {
                        if (element["info"] is not null)
                        {
                            if (element["info"]?["latitude"]?.ToString().ToUpper() != "")
                            {
                                cooLat = Convert.ToString(element["info"]?["latitude"]?.ToString() is null ? DBNull.Value : element["info"]?["latitude"]?.ToString());
                            }
                            if (element["info"]?["longitude"]?.ToString().ToUpper() != "")
                            {
                                cooLong = Convert.ToString(element["info"]?["longitude"]?.ToString() is null ? DBNull.Value : element["info"]?["longitude"]?.ToString());
                            }
                            if (element["info"]?["check_date"].ToString() != "")
                            {
                                fechaLlegadaSitio = Convert.ToString(element["info"]?["check_date"]?.ToString() is null ? DBNull.Value : element["info"]?["check_date"]?.ToString());
                            }
                            if (element["info"]?["check_date_utc"] is not null)
                            {
                                fechaLlegadaSitioUTC = Convert.ToString(
                                    utcToDatetime.castUtcDatetime(Convert.ToInt32(element["info"]?["check_date_utc"]?["_seconds"]), Convert.ToInt32(element["info"]?["check_date_utc"]?["_nanoseconds"]))
                                );
                            }
                        }
                    }

                    if (element["title"].ToString().ToUpper() == "Cierre de actividad".ToUpper())
                    {
                        foreach (var item in element["items"])
                        {
                            if (item["title"].ToString().ToUpper() == "Resolución de tarea".ToUpper())
                            {
                                subStatusTask = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }

                            if (item["title"].ToString().ToUpper() == "Comentarios".ToUpper())
                            {
                                comentariosCierre = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                        }
                    }

                    if (element["title"].ToString().ToUpper() == "Código de cierre".ToUpper())
                    {
                        foreach (var item in element["items"])
                        {
                            if (item["title"].ToString().ToUpper() == "Resolución de tarea".ToUpper())
                            {
                                subStatusTask = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                            if (item["title"].ToString().ToUpper() == "Comentarios".ToUpper())
                            {
                                comentariosCierre = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                            if (item["title"].ToString().ToUpper() == "Código de cierre".ToUpper())
                            {
                                isCC = true;
                                codigoCierre = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                            if (item["title"].ToString().ToUpper() == "Nombre de quién proporcionó el código de cierre".ToUpper())
                            {
                                quienProporcionoCierre = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                        }
                    }
                }
            }
            string fechaInicio              = Convert.ToString( data["start_date"]?.ToString() is null ? DBNull.Value : data["start_date"]?.ToString() );
            string fechaInicioUTC           = Convert.ToString(
                data["start_date_utc"] is null
                    ? DBNull.Value
                    : utcToDatetime.castUtcDatetime(Convert.ToInt32(data["start_date_utc"]?["_seconds"]), Convert.ToInt32(data["start_date_utc"]?["_nanoseconds"]))
            );
            string fechaFin                 = Convert.ToString( data["end_date"]?.ToString()   is null ? DBNull.Value : data["end_date"]?.ToString() );
            string fechaFinUTC              = Convert.ToString(
                data["end_date_utc"] is null 
                    ? DBNull.Value 
                    : utcToDatetime.castUtcDatetime( Convert.ToInt32(data["end_date_utc"]?["_seconds"]), Convert.ToInt32(data["end_date_utc"]?["_nanoseconds"] ) )
            );

            string userTask                 = Convert.ToString( data["scheduled_user_email"]?.ToString() is null ? DBNull.Value : new MailAddress( data["scheduled_user_email"]?.ToString() ).User );
            string fechaProgramada          = Convert.ToString(data["scheduled_date_programming"]?.ToString() is null ? DBNull.Value : String.Concat(data["scheduled_date_programming"]?.ToString(), " ", data["scheduled_hour_limit"]?.ToString()));
            string fechaProgramadaUTC       = Convert.ToString(
                data["scheduled_programming_date"]?["_seconds"].ToString() is null 
                    ? DBNull.Value 
                    : utcToDatetime.castUtcDatetime( Convert.ToInt32(data["scheduled_programming_date"]?["_seconds"]), Convert.ToInt32(data["scheduled_programming_date"]?["_nanoseconds"] ) )
            );
            string detailTask               = Convert.ToString(data["scheduled_instructions"]?.ToString() is null ? DBNull.Value : data["scheduled_instructions"]?.ToString());
            string deleteBy                 = Convert.ToString(data["deletedBy"]?.ToString() is null ? DBNull.Value : new MailAddress(data["deletedBy"]?.ToString()).User);

            switch (idEvento)
            {
                case "1":
                    statusName = Logs.SetStatusJson(idEvento, statusName);
                    break;
                case "2":
                    statusName = Logs.SetStatusJson(idEvento);
                    break;
                case "3":
                    statusName = Logs.SetStatusJson(idEvento);
                    break;
                case "4":
                    statusName = Logs.SetStatusJson(idEvento);
                    break;
                case "5":
                    statusName = Logs.SetStatusJson(idEvento);
                    break;
                case "6":
                    statusName = Logs.SetStatusJson(idEvento);
                    break;
            }

            lstUp = Logs.SetUpParameters(isCC, idEvento, nombreOwner, tipoTarea, statusName, recIdTask, idTaskTP, cooLat, cooLong, fechaLlegadaSitio, subStatusTask, comentariosCierre, codigoCierre, quienProporcionoCierre, fechaInicio, fechaFin, fechaProgramada, detailTask, deleteBy);
            
            try 
            {
                //using (var sql = new SqlConnection(cn.SqlComm()))
                using ( sql )
                {
                    //using (var cmd = new SqlCommand("SP_TrackPoint_UpTaskIvanti", sql))
                    using (var cmd = new SqlCommand(spName, sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdEvento"              , idEvento);
                        cmd.Parameters.AddWithValue("@IdSitio"               , IdSitio);
                        cmd.Parameters.AddWithValue("@CustId"                , custId);
                        cmd.Parameters.AddWithValue("@StatusName"            , statusName);
                        cmd.Parameters.AddWithValue("@SubStatusTask"         , subStatusTask);
                        cmd.Parameters.AddWithValue("@RecIdTask"             , recIdTask);
                        cmd.Parameters.AddWithValue("@IdTaskTP"              , idTaskTP);
                        cmd.Parameters.AddWithValue("@UserTask"              , userTask);
                        cmd.Parameters.AddWithValue("@DetailTask"            , detailTask);
                        cmd.Parameters.AddWithValue("@cooLat"                , cooLat);
                        cmd.Parameters.AddWithValue("@cooLong"               , cooLong);
                        cmd.Parameters.AddWithValue("@FechaProgramada"       , fechaProgramada);
                        cmd.Parameters.AddWithValue("@FechaProgramadaUTC"    , fechaProgramadaUTC);
                        cmd.Parameters.AddWithValue("@FechaInicio"           , fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaInicioUTC"        , fechaInicioUTC);
                        cmd.Parameters.AddWithValue("@FechaLlegadaSitio"     , fechaLlegadaSitio);
                        cmd.Parameters.AddWithValue("@FechaLlegadaSitioUTC"  , fechaLlegadaSitioUTC);
                        cmd.Parameters.AddWithValue("@FechaFin"              , fechaFin);
                        cmd.Parameters.AddWithValue("@FechaFinUTC"           , fechaFinUTC);
                        cmd.Parameters.AddWithValue("@CodigoCierre"          , codigoCierre);
                        cmd.Parameters.AddWithValue("@QuienProporcionoCierre", quienProporcionoCierre);
                        cmd.Parameters.AddWithValue("@ComentariosCierre"     , comentariosCierre);
                        cmd.Parameters.AddWithValue("@UserDeleted"           , deleteBy);

                        await cmd.ExecuteNonQueryAsync();
                        await sql.CloseAsync();
                    }
                }
                string stateN = (idEvento == "6") ? "Eliminada" : "Actualizada";
                
                new Logs().WriteLog($"Comentarios Tarea {assignmentId}: 0{idEvento} {statusName} - {stateN} Satisfactoriamente", lstUp, idEvento);// + "\n\n" + JsonConvert.SerializeObject(upActivity, Formatting.Indented));

            } catch (Exception ex)
            {
                new Logs().WriteLog($"Exception Tarea {assignmentId}: 0{idEvento} {statusName} - Ocurrió un Problema al Actualizar Estatus\n\t - " + StatusCodes.Status500InternalServerError + " " + ex.Message, []);// + "\n\n" + JsonConvert.SerializeObject(json, Formatting.Indented));
            }
        }
        #endregion

        public async Task UpTrasladoRetorno(JToken data, string server)
        {
            SqlConnection sql = new SqlConnection();
            string spName = "EXsp_TrackPoint_TrasladoRegreso";
            
            sql = new SqlConnection(cn.SqlCommBDE());

            List<string> lstUp = new List<string>();
            string nombreOwner = Convert.ToString(data["user_name"]?.ToString() is null ? DBNull.Value : data["user_name"].ToString());
            string activityName = data["modules_config"]!["name"]!.ToString();
            string idEvento = Convert.ToString(data["status"]?.ToString() is null ? DBNull.Value : data["status"].ToString());

            string status     = Convert.ToString(data["statusInfo"]?.ToString() is null ? DBNull.Value : data["statusInfo"]?["txt"]?.ToString() is null ? DBNull.Value : data["statusInfo"]?["txt"]?.ToString());
            string firebaseId = Convert.ToString(data["firebase_id"]?.ToString() is null ? DBNull.Value : data["firebase_id"]?.ToString());
            string user       = Convert.ToString(data["scheduled_user_email"]?.ToString() is null ? DBNull.Value : new MailAddress(data["scheduled_user_email"]?.ToString()).User);
            
            string inicioFechaTraslado = Convert.ToString( data["start_date_utc"] is null
                ? DBNull.Value
                : utcToDatetime.castUtcDatetime(Convert.ToInt32(data["start_date_utc"]?["_seconds"]), Convert.ToInt32(data["start_date_utc"]?["_nanoseconds"]))
            );
            string inicioDireccion = Convert.ToString(data["address_checkIn"]?.ToString() is null ? DBNull.Value : data["address_checkIn"]?.ToString());
            string inicioLat       = Convert.ToString(data["latitude"]?.ToString() is null ? DBNull.Value : data["latitude"]?.ToString());
            string inicioLon       = Convert.ToString(data["longitude"]?.ToString() is null ? DBNull.Value : data["longitude"]?.ToString());
            string inicioGeoHash   = Convert.ToString(data["geolocation"]?["geohash"]?.ToString() is null ? DBNull.Value : data["geolocation"]?["geohash"]?.ToString());

            string finFechaTraslado = null;
            string finDireccion     = null;
            string finLat           = null;
            string finLon           = null;
            string finGeoHash       = null;

            DateTime fecha = DateTime.ParseExact(inicioFechaTraslado, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if (data["elements"] is not null)
            {
                foreach (var element in data["elements"])
                {
                    if (element["title"].ToString().ToUpper() == "Destino".ToUpper())
                    {
                        finFechaTraslado = Convert.ToString( element["last_modified_at_utc"] is null
                            ? DBNull.Value
                            : utcToDatetime.castUtcDatetime(Convert.ToInt32(element["last_modified_at_utc"]?["_seconds"]), Convert.ToInt32(element["last_modified_at_utc"]?["_nanoseconds"]))
                        );

                        if (element["info"] is not null)
                        {
                            finDireccion = Convert.ToString(element["info"]?["address"]?.ToString() is null ? DBNull.Value : element["info"]?["address"]?.ToString());
                            finLat       = Convert.ToString(element["info"]?["latitude"]?.ToString() is null ? DBNull.Value : element["info"]?["latitude"]?.ToString());
                            finLon       = Convert.ToString(element["info"]?["longitude"]?.ToString() is null ? DBNull.Value : element["info"]?["longitude"]?.ToString());
                            finGeoHash   = Convert.ToString(element["info"]?["geolocation"]?["geohash"]?.ToString() is null ? DBNull.Value : element["info"]?["geolocation"]?["geohash"]?.ToString());
                        }
                    }
                }
            }

            lstUp = Logs.SetUpParameters(false, idEvento, nombreOwner, activityName, status, "", firebaseId, finLat ?? "", finLon ?? "", "", "", "", "", "", inicioFechaTraslado, finFechaTraslado, "", "", "");

            try
            {
                using (sql)
                {
                    using (var cmd = new SqlCommand(spName, sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = (object)status ?? DBNull.Value;
                        cmd.Parameters.Add("@FirebaseId", SqlDbType.NVarChar, 50).Value = (object)firebaseId ?? DBNull.Value;
                        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = (object)user ?? DBNull.Value;

                        cmd.Parameters.Add("@InicioFechatraslado", SqlDbType.NVarChar, 50).Value = (object)inicioFechaTraslado ?? DBNull.Value;
                        cmd.Parameters.Add("@InicioDireccion", SqlDbType.NVarChar, -1).Value = (object)inicioDireccion ?? DBNull.Value;
                        cmd.Parameters.Add("@InicioLat", SqlDbType.NVarChar, 50).Value = (object)inicioLat ?? DBNull.Value;
                        cmd.Parameters.Add("@InicioLon", SqlDbType.NVarChar, 50).Value = (object)inicioLon ?? DBNull.Value;
                        cmd.Parameters.Add("@InicioGeoHash", SqlDbType.NVarChar, 50).Value = (object)inicioGeoHash ?? DBNull.Value;

                        cmd.Parameters.Add("@FinFechaTraslado", SqlDbType.NVarChar, 50).Value = (object)finFechaTraslado ?? DBNull.Value;
                        cmd.Parameters.Add("@FinDireccion", SqlDbType.NVarChar, -1).Value = (object)finDireccion ?? DBNull.Value;
                        cmd.Parameters.Add("@FinLat", SqlDbType.NVarChar, 50).Value = (object)finLat ?? DBNull.Value; // Valor por defecto para decimal
                        cmd.Parameters.Add("@FinLon", SqlDbType.NVarChar, 50).Value = (object)finLon ?? DBNull.Value; // Valor por defecto para decimal
                        cmd.Parameters.Add("@FinGeoHash", SqlDbType.NVarChar, 50).Value = (object)finGeoHash ?? DBNull.Value;

                        await cmd.ExecuteNonQueryAsync();
                        await sql.CloseAsync();
                    }
                }

                string stateN = (idEvento == "2") ? "Creada" : "Actualizada";

                new Logs().WriteLog($"Comentarios Tarea {fecha.ToString("yyMMdd")}_{user}: 0{idEvento} {status} - {stateN} Satisfactoriamente", lstUp, idEvento);

            }
            catch (Exception ex)
            {
                new Logs().WriteLog($"Exception Tarea {fecha.ToString("yyMMdd")}_{user}: 0{idEvento} {status} - Ocurrió un Problema al Actualizar Estatus\n\t - " + StatusCodes.Status500InternalServerError + " " + ex.Message, []);
            }


        }

    }
}