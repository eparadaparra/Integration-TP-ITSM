using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using tasksAction.Models;
using static System.Net.Mime.MediaTypeNames;

namespace tasksAction.Conn
{
    public class CustomException
    {
        private readonly string logPathString = string.Empty;
        private readonly string logPathStringError = string.Empty;
        private readonly string logPathStringEvent = string.Empty;
        public CustomException() {
            var conexion = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            logPathString = conexion.GetSection("Settings:logPath").Value;
            logPathString = Path.Combine(Directory.GetCurrentDirectory(), logPathString);//"wwwroot") + "\\" + logPathString; 
            logPathStringError = Path.Combine(Directory.GetCurrentDirectory(), String.Concat(logPathString, "\\00 ErrorLog.txt"));//"wwwroot") + "\\" + logPathString; 
            logPathStringEvent = Path.Combine(Directory.GetCurrentDirectory(), String.Concat(logPathString, "\\00 EventLog.txt"));//"wwwroot") + "\\" + logPathString; 
        }
        public string LogPath() => logPathString;
        public string LogPathError() => logPathStringError;
        public string LogPathEvent() => logPathStringEvent;


    }

    public class Logs {

        public void LogException(Exception exc, string source, List<string> parameters)
        {
            string logFile = new CustomException().LogPathError();

            StringBuilder sb = new StringBuilder();

            StreamWriter sw = new StreamWriter(logFile, true);

            sb.Append('*', 10).Append(" ").Append(DateTime.Now.ToString()).Append(" ").Append('*', 10);
            sb.AppendLine();

            if (exc.InnerException != null)
            {
                sb.AppendLine("Inner Exception Type: " + exc.InnerException.GetType().ToString());
                sb.AppendLine("Inner Exception: " + exc.InnerException.Message);
                sb.AppendLine("Inner Source: " + exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sb.AppendLine("Inner Stack Trace: ");
                    sb.AppendLine(exc.InnerException.StackTrace);
                }
            }
            sb.AppendLine("Exception Type: " + exc.GetType().ToString());
            sb.AppendLine("Exception: " + exc.Message);
            sb.AppendLine("Source: " + source);

            foreach (string p in parameters)
            {
                sb.AppendLine("Parameter: " + p);
            }

            sb.AppendLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sb.AppendLine(exc.StackTrace);
            }

            sw.WriteLine(sb.ToString());
            sw.Close();
        }
        
        #region Escribe en Log de Eventos en cada cambio de estatus
        public void WriteLog(string strComments, List<string>? parameters, string? idEvento = "")
        {
            string logFile = new CustomException().LogPathEvent();
            string state = (idEvento == "6") ? "Deleted:" : "Updated:";

            //Console.WriteLine(logFile);
            StringBuilder sb = new StringBuilder();

            sb.Append('*', 30).Append(" ").Append(DateTime.Now.ToString()).Append(" ").Append('*', 30);
            sb.AppendLine();
            sb.AppendLine(strComments);
            sb.AppendLine(state);
            foreach (string p in parameters)
            {
                sb.AppendLine("\t" + p);
            }

            using (StreamWriter sw = new StreamWriter(logFile, true)){
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
        }
        #endregion

        #region Asigna Nombre de Estatus basado en su Id de Estatus
        public static string SetStatusJson(string statusId, string statusName = "")
        {
            string status = "";

            switch (statusId)
            {
                case "1":
                    status = statusName.IsNullOrEmpty() ? statusName = "Programada" : statusName;
                    break;
                case "2":
                    status = "Abierta";
                    break;
                case "3":
                    status = "Cerrada";
                    break;
                case "4":
                    status = "Archivada";
                    break;
                case "5":
                    status = "Vencida";
                    break;
                case "6":
                    status = "Eliminada";
                    break;
            }

            return status;
        }
        #endregion

        #region Crea lista para escribir el Log de Eventos
        public static List<string> SetUpParameters( bool isCC, string? idEvento, string? nombreOwner, string? tipoTarea, string? statusName, string? recIdTask, string? idTaskTP, string? cooLat, string? cooLong, string? fechaLlegadaSitio, string? subStatusTask, string? comentariosCierre, string? codigoCierre, string? quienProporcionoCierre, string? fechaInicio, string? fechaFin, string? fechaProgramada, string? detailTask, string? deleteBy) 
        {    
            List<string> lstUp = new List<string>();
            switch (idEvento)
            {
                case "1":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    lstUp.Add($"RecId: {recIdTask}");
                    lstUp.Add($"TipoTarea: {tipoTarea}");

                    if (statusName == "Abierta")
                    {
                        lstUp.Add($"UsuarioAsignado: {nombreOwner}");
                        lstUp.Add($"FechaProgramada: {fechaProgramada}");
                        lstUp.Add($"Detalle de Tares: {detailTask}");
                    }
                    break;
                case "2":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    if (recIdTask != "") { lstUp.Add($"RecId: {recIdTask}"); };
                    lstUp.Add($"TipoTarea: {tipoTarea}");
                    lstUp.Add($"UsuarioAsignado: {nombreOwner}");
                    if (fechaProgramada != "") { lstUp.Add($"FechaProgramada: {fechaProgramada}"); };
                    lstUp.Add($"FechaTraslado: {fechaInicio}");
                    break;
                case "3":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    if (recIdTask != "") { lstUp.Add($"RecId: {recIdTask}"); };
                    lstUp.Add($"TipoTarea: {tipoTarea}");
                    lstUp.Add($"UsuarioAsignado: {nombreOwner}");
                    if (fechaProgramada != "") { lstUp.Add($"FechaProgramada: {fechaProgramada}"); };
                    lstUp.Add($"FechaTraslado: {fechaInicio}");
                    if (fechaLlegadaSitio != "") { lstUp.Add($"FechaInicio: {fechaLlegadaSitio}"); };
                    lstUp.Add($"FechaFin: {fechaFin}");
                    lstUp.Add($"LatitudSitio: {cooLat}");
                    lstUp.Add($"LongitudSitio: {cooLong}");
                    if (isCC) { lstUp.Add($"CodigoCierre: {codigoCierre}"); };
                    if (isCC) { lstUp.Add($"AgenteCC: {quienProporcionoCierre}"); };
                    if (subStatusTask != "") { lstUp.Add($"Resolucion: {subStatusTask}"); };
                    if (comentariosCierre != "") { lstUp.Add($"ComentariosCC: {comentariosCierre}"); };
                    break;
                case "4":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    lstUp.Add($"RecId: {recIdTask}");
                    lstUp.Add($"TipoTarea: {tipoTarea}");
                    if (nombreOwner == "") { lstUp.Add($"UsuarioAsignado: {nombreOwner}"); };
                    lstUp.Add($"FechaProgramada: {fechaProgramada}");
                    lstUp.Add($"FechaTraslado: {fechaInicio}");
                    lstUp.Add($"FechaInicio: {fechaLlegadaSitio}");
                    lstUp.Add($"FechaFin: {fechaFin}");
                    if (isCC) { lstUp.Add($"CodigoCierre: {codigoCierre}"); };
                    if (isCC) { lstUp.Add($"AgenteCC: {quienProporcionoCierre}"); };
                    lstUp.Add($"Resolucion: {subStatusTask}");
                    lstUp.Add($"ComentariosCC: {comentariosCierre}");
                    break;
                case "5":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    lstUp.Add($"RecId: {recIdTask}");
                    lstUp.Add($"TipoTarea: {tipoTarea}");
                    if (nombreOwner == "") { lstUp.Add($"UsuarioAsignado: {nombreOwner}"); };
                    lstUp.Add($"FechaProgramada: {fechaProgramada}");
                    lstUp.Add($"FechaTraslado: {fechaInicio}");
                    lstUp.Add($"FechaInicio: {fechaLlegadaSitio}");
                    lstUp.Add($"FechaFin: {fechaFin}");
                    if (isCC) { lstUp.Add($"CodigoCierre: {codigoCierre}"); };
                    if (isCC) { lstUp.Add($"AgenteCC: {quienProporcionoCierre}"); };
                    lstUp.Add($"Resolucion: {subStatusTask}");
                    lstUp.Add($"ComentariosCC: {comentariosCierre}");
                    break;
                case "6":
                    lstUp.Add($"FirebaseId: {idTaskTP}");
                    lstUp.Add($"RecId: {recIdTask}");
                    lstUp.Add($"TipoTarea: {tipoTarea}");
                    lstUp.Add("");
                    lstUp.Add($"Eliminada por: {deleteBy}");
                    break;
            }
            return lstUp;
        }
        #endregion

    }

    public class utcToDatetime
    {
        public static string castUtcDatetime(long seconds = 0, long nanoseconds = 0)
        {
            // Datos de la línea JSON
            //long seconds = 1727137428;
            //int nanoseconds = 557000000;

            // Convertir los segundos en un DateTime a partir del 1 de enero de 1970 (Unix Epoch)
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

            // Agregar los nanosegundos como una fracción de segundo
            dateTime = dateTime.AddTicks(nanoseconds / 100); // 1 tick = 100 nanosegundos

            // Convertir a string (puedes cambiar el formato según lo que necesites)
            string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Imprimir la fecha formateada
            Console.WriteLine(formattedDate);
            return formattedDate;
        }
    }

    public class FileService
    {
        #region CADA RESPUESTA WEBHOOK CREA .JSON
        public void CreaJsonWH(JToken json)
        {
            JToken data = json["data"];
            DateTime fecha = (data["modules_config"]?["name"]?.ToString() == "Traslado regreso")
                ? DateTime.ParseExact(data["start_date"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                : DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            string user = new MailAddress(Convert.ToString(data["scheduled_user_email"].ToString())).User;

            string nameFile = (data["modules_config"]?["name"]?.ToString() == "Traslado regreso")
                ? String.Concat(fecha.ToString("yyMMddHHmm"), "_", user, " - 0", data["status"]?.ToString(), " ", Logs.SetStatusJson(data["status"]?.ToString(), data["statusInfo"]?["txt"]?.ToString()))
                : String.Concat(data["preload"]?[0]?["frmAssignmentId"]?.ToString(), " - 0", data["status"]?.ToString(), " ", Logs.SetStatusJson(data["status"]?.ToString(), data["statusInfo"]?["txt"]?.ToString()) );

            // MANDA A LLAMAR FUNCION QUE CREA JSON
            SaveJson(JsonConvert.SerializeObject(json, Formatting.Indented), nameFile);
        }
        #endregion

        #region GUARDA JSON CREAO
        public void SaveJson(string content, string nameFile) {
            //Obtiene ubicación para guardar JSON
            string folderPath = new CustomException().LogPath();
            
            //Si no existe ubicación la crea
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

            //Crea ruta completa del archivo JSON
            string filePath = Path.Combine(folderPath, nameFile + ".json");

            //Crea archivo JSON
            File.WriteAllText(filePath, content);
        }
        #endregion
    }
}