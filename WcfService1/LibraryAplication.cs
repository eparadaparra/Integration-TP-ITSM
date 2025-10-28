using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WcfService1
{
    public class LibraryAplication
    {
        public static async Task<string> CreateTask(int assignmentID)
        {
            string uri = "";
            uri = (assignmentID > 730000 && assignmentID < 1500000) ? "https://selfservice.execon.mx" : "https://itsm.execon.mx";
            string taskID = assignmentID.ToString().Trim();
            //Console.WriteLine(uri);
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                httpClient.BaseAddress = new Uri(uri);
                HttpResponseMessage response = await httpClient.PostAsync($"/ScheduledTaskITSM/TaskToTp/{taskID}", null);

                if (response.IsSuccessStatusCode)
                {
                    return $"Tarea {taskID} Creada Exitosamente";
                }
                else
                {
                    return $"Tarea {taskID}: Error al consumir la API externa";
                }
            }
            catch (Exception ex)
            {
                List<string> parameters = new List<string>();
                parameters.Add(assignmentID.ToString());
                //CustomException.LogException(ex, "CreateTask", parameters);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}