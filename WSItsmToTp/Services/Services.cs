
namespace WSItsmToTp.Services
{
    public class Services : IServices
    {
        public async Task<string> GetMessage(int assignmentID)
        {
            string taskID = assignmentID.ToString();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://selfservice.execon.mx");
                HttpResponseMessage response = await httpClient.PostAsync($"/ScheduledTaskITSM/TaskToTp/{taskID}", null);

                if (response.IsSuccessStatusCode)
                {
                    var respJson = await response.Content.ReadAsStringAsync();
                    return $"Tarea {taskID} Creada correctamente";
                }
                else
                {
                    return $"Tarea {taskID}: Error al consumir la API externa";
                }
            }
            catch (Exception ex)
            {
                return $"Tarea {taskID}: {ex.Message}";
            }


            //return Task.FromResult($"Hello, {name}");
        }
    }
}
