using ScheduledTaskITSM.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ScheduledTaskITSM.Conn;

namespace ScheduledTaskITSM.Services
{
    public class Services : IServices
    {
        #region Declara Variables Privadas
        private static string _baseUrl;
        private static string _customerKey;
        private static string _autorization;
        private static string _email;
        private static string _pass;
        private static string _token;

        private static string _baseUrlExecon;
        private static string _emailExecon;
        private static string _passExecon;
        private static string _tokenExecon;
        #endregion

        #region Asingna valores a variables privadas
        public Services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _baseUrl      = builder.GetSection("ApiSettingsTrackPoint:baseUrl").Value;
            _customerKey  = builder.GetSection("ApiSettingsTrackPoint:customer_key").Value;
            _autorization = builder.GetSection("ApiSettingsTrackPoint:autorization").Value;
            _email        = builder.GetSection("ApiSettingsTrackPoint:email").Value;
            _pass         = builder.GetSection("ApiSettingsTrackPoint:pass").Value;



            _baseUrlExecon = builder.GetSection("ApiSettingsITSMExecon:baseUrl").Value;
            _emailExecon   = builder.GetSection("ApiSettingsITSMExecon:email").Value;
            _passExecon    = builder.GetSection("ApiSettingsITSMExecon:pass").Value;
        }
        #endregion

        #region (API) ObtieneTokenTP
        public async Task AuthTp()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _autorization);
            client.DefaultRequestHeaders.Add("api-customer-key", _customerKey);

            var auth = new Auth() { email = _email, password = _pass };
            var authJson = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");

            var resp = await client.PostAsync("/apiScheduledAuthToken", authJson);
            var respJson = await resp.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<AuthResult>(respJson);
            _token = res.token;
        }
        #endregion

        #region (API) ObtieneTokenExecon
        public async Task AuthExecon()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_baseUrlExecon); //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_autorizationExecon}");
                //client.DefaultRequestHeaders.Add("content-type", "application/json");

                Auth authExecon = new Auth() { email = _emailExecon, password = _passExecon };
                var contentJson = new StringContent(JsonConvert.SerializeObject(authExecon, Formatting.Indented), Encoding.UTF8, "application/json");
                
                var resp = await client.PostAsync("/apiTaskTP/login", contentJson);
                var respJson = await resp.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<AuthResult>(respJson);
                _tokenExecon = res.token;
            }catch (Exception ex)
            {
                new Logs().WriteLog($"Exception AuthExecon: " + ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
        
        #region GetCustomerTP (API.tp_ApigetCustomerbyid)
        public async Task<ClientId> GetCustomerTP(string custId, string server)
        {
            ClientId client_id = new ClientId { client_id = custId }; 

            await AuthTp();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            httpClient.DefaultRequestHeaders.Add("api-customer-key", _customerKey);

            try
            {
                // Ejecuta
                var content = new StringContent(JsonConvert.SerializeObject(client_id), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/ApigetCustomerbyid", content);

                var respJson = await response.Content.ReadAsStringAsync();
                var respCustomer = JsonConvert.DeserializeObject<TPCustomer>(respJson);

                if (respCustomer.data.modules_notify is null || respCustomer.data.status != "A")
                {
                    // Si API NO encuentra el Cliente en TP se Crea usuario mendiante API
                    client_id = await InsCustomerTP(custId, server);

                    new Logs().WriteLog($"Comentarios GetCustomerTP {custId}: Se crea Nuevo Cliente en TP" + "\n\n" + JsonConvert.SerializeObject(client_id, Formatting.Indented));
                    return client_id;
                }
                else
                {
                    // Si API Encuentra el ID del Cliente en TP se guarda en modelo CLientId
                    client_id.status    = respCustomer.status;
                    client_id.message   = respCustomer.message;
                    client_id.client_id = respCustomer.data.modules_notify.filters_uid;
                    client_id.id        = respCustomer.data.modules_notify.filters_uid;

                    return client_id;
                }
            }
            catch (Exception ex)
            {
                client_id.status    = StatusCodes.Status500InternalServerError.ToString();
                client_id.message   = ex.Message;

                new Logs().WriteLog($"Exception GetCustomerTP {custId}: " + ex.Message + "\n\n" + JsonConvert.SerializeObject(client_id, Formatting.Indented));
                return client_id;
            }
        }
        #endregion

        #region InsCustomerTP (API.TP_ApicreateCustomer)
        public async Task<ClientId> InsCustomerTP(string custId, string server)
        {
            // OBTIENE LOS DATOS DE CUSTOMER DE EXECON
            CustomerExecon dataCustomerExecon = await GetCustomerITSM(custId, server);
            ClientId newCustomerTP = new ClientId();

            if (dataCustomerExecon.data is not null) 
            {
                Notificacion notificacionCustTP = new Notificacion { 
                    init      = dataCustomerExecon.data.init,
                    close     = dataCustomerExecon.data.close,
                    scheduled = dataCustomerExecon.data.scheduled,
                    daybefore = dataCustomerExecon.data.daybefore,
                    send_ics  = dataCustomerExecon.data.send_ics
                };

                ModulesNotify modNotify = new ModulesNotify{
                    email = [dataCustomerExecon.data.email],
                    notificacion = notificacionCustTP
                };

                DataCustomerTP customerTP = new DataCustomerTP{
                    name           = dataCustomerExecon.data.nameCustomer,
                    mail           = dataCustomerExecon.data.mailAdminTP,
                    user_uuid_id   = dataCustomerExecon.data.uuidAdminTP,
                    contact        = dataCustomerExecon.data.contactCustomer,
                    address        = dataCustomerExecon.data.addressCustomer,
                    telephone      = dataCustomerExecon.data.telephoneCustomer,
                    client_id      = dataCustomerExecon.data.client_IdCustomer,
                    modules_notify = modNotify
                };
                
                TPCustomer newCustTp = new TPCustomer { data = customerTP };

                await AuthTp();

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                httpClient.DefaultRequestHeaders.Add("api-customer-key", _customerKey);

                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(newCustTp), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("/ApicreateCustomer", content);

                    var respJson = await response.Content.ReadAsStringAsync();
                    var respCustomer = JsonConvert.DeserializeObject<ClientId>(respJson);
                    //var respCustomer = result.clientIdTp;
                    //Console.WriteLine(JsonConvert.SerializeObject(respCustomer) + "\n");
                    if (respCustomer.id != "")
                    {
                        //Si API Crea correctamente el Cliente en TP se guarda en modelo CLientId
                        respCustomer.client_id = respCustomer.id;
                        return respCustomer;
                    }
                    else
                    {
                        //Si API NO crea el Cliente en TP guarda información en modelo CLientId
                        respCustomer.message = String.Concat($"CustId: {custId} - ", respCustomer.message);

                        new Logs().WriteLog($"Comentarios InsCustomerTP {custId}: No pudo ser creado el cliente en Trackpoint" + "\n\n" + JsonConvert.SerializeObject(respCustomer, Formatting.Indented));
                        return respCustomer;
                    }
                } catch (Exception ex)
                {
                    newCustomerTP.status = "fail";
                    newCustomerTP.message = String.Concat($"CustId: {custId} - ", ex.Message);
                    
                    new Logs().WriteLog($"Exception InsCustomerTP {custId}: " + ex.Message + "\n\n" + JsonConvert.SerializeObject(newCustomerTP, Formatting.Indented));
                    return newCustomerTP;
                }
            } else{
                newCustomerTP.status = "fail";
                newCustomerTP.message = String.Concat($"CustId: {custId} - {dataCustomerExecon.message}");
            }

            return newCustomerTP;
        }
        #endregion

        #region Crea Actividad en TP con API apiScheduledProgrammingAdd
        public async Task<TPActivity> PostActivityTP(TaskITSM taskToSchedule, string server)
        {
            Console.WriteLine(JsonConvert.SerializeObject(taskToSchedule.data, Formatting.Indented));
            await AuthTp();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            httpClient.DefaultRequestHeaders.Add("api-customer-key", _customerKey);

            try
            {
                // MANDA A LLAMAR API apiScheduledProgrammingAdd DE TP PARA CARGAR ACTIVIDAD EN PLATAFORMA TP
                var content = new StringContent(JsonConvert.SerializeObject(taskToSchedule.data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/apiScheduledProgrammingAdd", content);
                
                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject<TPActivity>(respJson);  //RESPUESTA DE API

                if (respActivity.data is null)
                {
                    //SI NO CREA ACTIVIDAD EN TRACKPOINT REGRESA MENSAJE
                    respActivity.status = respActivity.statusCode;
                    respActivity.message = String.Concat($"Tarea {taskToSchedule.data.frmAssignmentId} - ", respActivity.messageError.code, " || ", respActivity.messageError.message);

                    new Logs().WriteLog($"Comentarios PostActivityTP {taskToSchedule.data.frmAssignmentId}: No pudo ser creada la tarea en Trackpoint - " + respActivity.messageError.code + "\n\n" + JsonConvert.SerializeObject(respActivity, Formatting.Indented));
                    return respActivity;
                }
                else
                {
                    respActivity.status = respActivity.statusCode;
                    respActivity.data.statusInfo = new StatusInfo { txt = "Programada", color = "" }; //null; //
                    respActivity.data.elements = null;
                    respActivity.data.scheduledInstructions = null;
                    respActivity.data.scheduled_date_programming = null;
                    respActivity.data.startDateSeparated = null;
                    respActivity.data.endDateSeparated = null;

                    //SI CREA ACTIVIDAD EN TRACKPOINT PROCEDE A CARGAR DATOS ADICIONALES A LA ACTVIDAD
                    Preload frm = new Preload();
                        frm.frmRecIdTask        = taskToSchedule.data.frmRecIdTask;
                        frm.frmAssignmentId     = taskToSchedule.data.frmAssignmentId;
                        frm.frmParentNumber     = taskToSchedule.data.frmParentNumber;
                        frm.frmParentCategory   = taskToSchedule.data.frmParentCategory;
                        frm.frmIdSitio          = taskToSchedule.data.frmIdSitio;
                        frm.frmCustId           = taskToSchedule.data.frmCustId;
                        frm.frmCodigoCierre     = taskToSchedule.data.frmCodigoCierre;
                        frm.frmParentOwner      = taskToSchedule.data.frmParentOwner;
                        frm.frmServer           = taskToSchedule.data.frmServer;
                        
                        //frm.frmIdZona           = taskToSchedule.frmIdZona;
                        List<Preload> lstFrom = new List<Preload>();
                        lstFrom.Add(frm);
                        
                    DataComplement dataComplent = new DataComplement{ 
                        firebase_id = respActivity.data.firebase_id,
                        data = lstFrom
                    };
                    TpActivityComplement respActivityAditional = new TpActivityComplement{ 
                        data = dataComplent, 
                        status = "", 
                        message = "", messageError = new MensajeComp() 
                    };

                    // MANDA A LLAMAR API DE TP PARA CARGAR DATOS ADICIONALES A LA ACTIVIDAD CREADA PREVIAMENTE TP
                    respActivityAditional = await PostAditionalActivity(respActivityAditional);

                    if (respActivityAditional.status == "fail")
                    { 
                        //ACTUALIZA MENSAJE DE ACTIVIDAD CREADA EN TRACKPOINT PERO SIN DATOS ADICIONALES
                        respActivity.message = String.Concat(respActivity.message, ", Pero no se cargaron los datos adicionales.");
                        respActivity.data.status = "1";
                        respActivity.data.statusInfo = new StatusInfo { txt = "", color = "" };

                        // MANDA A LLAMAR API EXECON PARA ACTUALIZAR TAREA AGREGANDO EL FIREBASE_ID DE LA ACTIVIDAD
                        await UpTaskITSM(respActivity);
                        return respActivity;
                    }
                    else
                    {
                        //SI AGREGA DE MANERA EXITOSA LOS DATOS ADICIONALES
                        respActivity.data.status                     = "1";
                        respActivity.data.preload                    = lstFrom;

                        // MANDA A LLAMAR API EXECON PARA ACTUALIZAR TAREA AGREGANDO EL FIREBASE_ID DE LA ACTIVIDAD
                        await UpTaskITSM(respActivity);
                        return respActivity;
                    }
                }
            } catch (Exception ex)
            {
                var createdActivity = new TPActivity();
                createdActivity.status = "fail";
                createdActivity.message = ex.Message;

                new Logs().WriteLog($"Exception PostActivityTP {taskToSchedule.data.frmAssignmentId}: " + ex.Message + "\n\n" + JsonConvert.SerializeObject(createdActivity, Formatting.Indented));
                return createdActivity;
            }
        }
        #endregion

        #region Agrega Parametros Adicionales a la Actividad en TP updateEventWebhook
        public async Task<TpActivityComplement> PostAditionalActivity(TpActivityComplement objeto)
        {
            // PREPARA LLAMADO DE API updateEventWebhook
            await AuthTp();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            httpClient.DefaultRequestHeaders.Add("api-customer-key", _customerKey);
            
            try
            {
                // MANDA A LLAMAR API updateEventWebhook DE TP PARA CARGAR DATOD ASICIONALES EN LA ACTIVIDAD YA CREADA EN TP
                var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/updateEventWebhook", content);

                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject<TpActivityComplement>(respJson);

                if (respActivity.status == "fail")
                {
                    //frmAditional.status = respActivity.status;
                    respActivity.message = String.Concat($"Tarea {objeto.data.data[0].frmAssignmentId} - ", respActivity.message);

                    new Logs().WriteLog($"Comentarios PostAditionalActivity {objeto.data.data[0].frmAssignmentId}: No se adjuntó la info adicional en Trackpoint - " + respActivity.message + "\n\n" + JsonConvert.SerializeObject(respActivity, Formatting.Indented));
                    return respActivity;
                }
                else {
                    // REGRESA LA RESPUESTA DE API updateEventWebhook 
                    return respActivity;
                } 
            } catch (Exception ex)
            {
                var frmAditional = new TpActivityComplement();
                frmAditional.status = "fail";
                frmAditional.message = String.Concat($"Tarea {objeto.data.data[0].frmAssignmentId} - ", ex.Message);

                new Logs().WriteLog($"Exception PostAditionalActivity {objeto.data.data[0].frmAssignmentId}: " + ex.Message + "\n\n" + JsonConvert.SerializeObject(frmAditional, Formatting.Indented));
                return frmAditional;
            }
        }
        #endregion
        
        #region Get ITSM's Task
        public async Task<TaskITSM> GetTaskITSM(string assignmentId, string server)
        {
            await AuthExecon();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrlExecon);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenExecon);

            try
            {
                var response = await httpClient.GetAsync($"/apiTaskTP/GetTaskITSM?serverName={server}&assignmentId={assignmentId}");
            
                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject<TaskITSM>(respJson);

                if (respActivity.status == "fail") {
                    new Logs().WriteLog($"Comentarios Tarea {assignmentId}: " + respActivity.message + "\n" + JsonConvert.SerializeObject(respActivity.data, Formatting.Indented));
                }

                return respActivity;

            }
            catch (Exception ex)
            {
                new Logs().WriteLog($"Exception Tarea {assignmentId}: " + StatusCodes.Status500InternalServerError + " " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Get ITSM's Customer
        public async Task<CustomerExecon> GetCustomerITSM(string custId, string server)
        {
            await AuthExecon();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrlExecon);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenExecon);

            try
            {
                var response = await httpClient.GetAsync($"/apiTaskTP/GetCustomer?serverName={server}&custId={custId}");
            
                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject<CustomerExecon>(respJson);

                if (respActivity.status == "fail") {
                    new Logs().WriteLog($"Comentarios Customer {custId}: No retorno datos el SP_TrackPoint_SelAccountIvanti de la funcion GetCustomerId \n\n" + JsonConvert.SerializeObject(respActivity.data, Formatting.Indented));
                }
                return respActivity;

            }
            catch (Exception ex)
            {
                new Logs().WriteLog($"Exception Customer {custId}: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Up ITSM's Task
        public async Task<bool> UpTaskITSM(TPActivity objeto)
        {
            bool status = false;
            
            await AuthExecon();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrlExecon);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenExecon);
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync("/apiTaskTP/UpTaskITSM", content);

                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject(respJson);
                
                return true;

            }
            catch (Exception ex)
            {
                return status;
            }
        }
        #endregion

        #region Obtiene Json de Actividad en Trackpoint
        public async Task<Object> GetActivityTP(string firebase_id)
        {
            try
            { 
                await AuthTp();

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _autorization);
                httpClient.DefaultRequestHeaders.Add("api-customer-key", _customerKey);

                // MANDA A LLAMAR API apiScheduledProgrammingAdd DE TP PARA CARGAR ACTIVIDAD EN PLATAFORMA TP
                var response = await httpClient.PostAsync($"/getDataEventWeb/{firebase_id}", null);

                var respJson = await response.Content.ReadAsStringAsync();
                var respActivity = JsonConvert.DeserializeObject(respJson);  //RESPUESTA DE API

                return respActivity;
            }
            catch(Exception ex)
            {
                return new { message = ex.Message};
            }

        }
        #endregion

    }
}
