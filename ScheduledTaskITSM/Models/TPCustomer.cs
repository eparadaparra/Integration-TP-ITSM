namespace ScheduledTaskITSM.Models
{
    public partial class TPCustomer
    {
        public string status { get; set; }
        public string message { get; set; }
        public DataCustomerTP data { get; set; }
    }
    public partial class DataCustomerTP
    {
        public DateTimeOffset date { get; set; }
        public string address { get; set; }
        public string mail { get; set; }
        public string telephone { get; set; }
        public string client_id { get; set; }
        public string contact { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public ModulesNotify modules_notify { get; set; }
        public string? user_uuid_id { get; set; }
    }

    public partial class ModulesNotify
    {
        public List<string> email { get; set; }
        public Notificacion notificacion { get; set; }
        public string filters_uid { get; set; }
        public string firebase_uid { get; set; }
        public long type_id { get; set; }
        public bool status { get; set; }
    }

    public partial class Notificacion
    {
        public bool init { get; set; }
        public bool close { get; set; }
        public bool scheduled { get; set; }
        public bool daybefore { get; set; }
        public bool send_ics { get; set; }
    }

    public class ClientId       // Moder¿lo respuesta de Creación de Cliente (sin client_id) y Modelo Solicitud Info Cliente TP (Solo client_id)
    {
        public string status { get; set; }
        public string message { get; set; }
        public string? id { get; set; }
        public string? client_id { get; set; }
    }
}
