using Newtonsoft.Json;  //Convertir clases en JSon
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using webhookITSM.Models;

namespace webhookITSM.Services
{
    public class Services : IServices
    {

        #region Declaración de Variable
        private static string _baseUrlExecon;
        private static string _email;
        private static string _pass;
        private static string _tokenExecon;
        
        private static string _flwTokenOO;
        private static string _logLevelOO;
        private static string _baseUrlOO;
        private static string _runNameOO;
        private static string _authOO;
        #endregion

        #region Asignación de Valiables
        public Services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            //Declara variables para llamado de API WHTaskITSM
            _baseUrlExecon = builder.GetSection("Settings:baseUrl").Value;
            _email         = builder.GetSection("Settings:email").Value;
            _pass          = builder.GetSection("Settings:pass").Value;
            
            //Declara variables para llamado de API en OO para envío de notificaciones
            _baseUrlOO = builder.GetSection("OOnotifications:url").Value;
            _authOO        = builder.GetSection("OOnotifications:authBasic").Value;
            _flwTokenOO    = builder.GetSection("OOnotifications:flowUuid").Value;
            _runNameOO     = builder.GetSection("OOnotifications:runName").Value;
            _logLevelOO    = builder.GetSection("OOnotifications:logLevel").Value;
        }
        #endregion

        #region Autenticación de API's
        public async Task AuthExecon()
        {
            // Crea Conexion 
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrlExecon);
            
            var auth = new AuthUsr() { email = _email, password = _pass };
            var contentJson = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            
            var resp = await client.PostAsync("/apiTaskTP/login", contentJson);
            var respJson = await resp.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AuthResult>(respJson);
            _tokenExecon = result.token;
        }
        #endregion

        #region Llamado de API Webhook (realiza Update de Tarea en ITSM)
        public async Task<bool> WHTaskITSM(Object objeto)
        {
            bool status = false;
            var body = objeto.ToString();
            JObject json = JObject.Parse(body);
            JToken data = json["data"];
            Console.WriteLine(JsonConvert.SerializeObject(objeto, Formatting.Indented));

            if (data["statusInfo"]?["txt"]?.ToString() == "Cerrada") { await NotificationOO(data); }

            await AuthExecon();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrlExecon);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenExecon);
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/apiTaskTP/WHTaskITSM", content);

                //var respJson = await response.Content.ReadAsStringAsync();
                //var respActivity = JsonConvert.DeserializeObject(respJson);

                return response.IsSuccessStatusCode ? true : false;

            }
            catch (Exception ex)
            {
                return status;
            }
        }
        #endregion

        #region Llamado de API de Envío de Notificación de Códido de Cierre Incorrecto
        public async Task<bool> NotificationOO(JToken data)
        {
            bool status = false;
            OONotificationCc ooNot = new OONotificationCc();
            InputsOO inputs = new InputsOO();
            bool isCC = false;

            #region Asigna datos a variables de Inputs  de envio de correo
            string assignmentId  = data["preload"]?[0]?["frmAssignmentId"]?.ToString();
            string cliente       = data["customer_name"]?.ToString();
            string ism           = data["user_name"]?.ToString();
            string fechaCierre   = data["end_date"]?.ToString();
            string codigoCierre  = "";
            string agente        = "";
            string subStatusTask = "";
            #region Asigna información de Código de Cierre de Tarea
            foreach (var element in data["elements"])
                {
                    if (element["title"].ToString().ToUpper() == "Código de cierre".ToUpper())
                    {
                        foreach (var item in element["items"])
                    {
                            if (item["title"].ToString().ToUpper() == "Código de cierre".ToUpper())
                            {
                                codigoCierre = item["value"].ToString().ToUpper();
                                isCC = true;
                            }
                            if (item["title"].ToString().ToUpper() == "Nombre de quién proporcionó el código de cierre".ToUpper())
                            {
                                agente = inputs.agente = item["value"].ToString();
                            }
                            if (item["title"].ToString().ToUpper() == "Resolución de tarea".ToUpper())
                            {
                                subStatusTask = Convert.ToString(item["value"].ToString() is null ? DBNull.Value : item["value"].ToString());
                            }
                        }
                    }
                }
            #endregion
            string codigoCierreIvanti = data["preload"]?[0]?["frmCodigoCierre"]?.ToString().ToUpper();
            string parent             = data["preload"]?[0]?["frmParentCategory"]?.ToString();
            string owner              = data["preload"]?[0]?["frmParentOwner"]?.ToString();
            string subject            = String.Concat("Discrepancia en código de cierre Zona ", data["classification_category_name"]?.ToString(), " - # Tarea ", assignmentId);

            if (!isCC) { return status; } //Si Actividad no requere Codigo de cierre sale del envio de correo
            #endregion

            if ((codigoCierre != codigoCierreIvanti) && (codigoCierre != "" || codigoCierre is not null))
            {
                ooNot.flowUuid = _flwTokenOO;
                ooNot.runName  = _runNameOO;
                ooNot.logLevel = _logLevelOO;
                    inputs.noTarea      = assignmentId;
                    inputs.cliente      = cliente;
                    inputs.ism          = ism;
                    inputs.fecha_cierre = fechaCierre;
                    inputs.codigoCierre = codigoCierre;
                    inputs.agente       = agente;
                    inputs.codigoCierreIvanti = codigoCierreIvanti;
                    inputs.parent       = parent;
                    inputs.owner        = owner;
                    inputs.subject      = subject;
                ooNot.inputs = inputs;
            
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_baseUrlOO);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _authOO);

                try
                {
                    if (subStatusTask != "Cancelada")
                    {
                        var content  = new StringContent(JsonConvert.SerializeObject(ooNot), Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync("/oo/rest/v2/executions", content);

                        var respJson = await response.Content.ReadAsStringAsync();
                        var respActivity = JsonConvert.DeserializeObject(respJson);
                    }    
                    
                    return true;
                }
                catch (Exception ex)
                {
                    return status;
                }
            } 
            else 
            { 
                return status; 
            }
        }
        #endregion
    
    }
}
