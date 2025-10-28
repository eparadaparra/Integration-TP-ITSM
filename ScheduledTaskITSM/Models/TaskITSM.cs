using System.ComponentModel.DataAnnotations;

namespace ScheduledTaskITSM.Models
{
    public class TaskITSM
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public DataTask? data { get; set; }
    }
    public partial class DataTask 
    { 
        public string? scheduled_type_event { get; set; }
        public string? scheduled_name_event { get; set; }
        public string? scheduled_client_uuid { get; set; }
        public string? scheduled_periodicity { get; set; }
        public string? id_user { get; set; }
        [Required]
        public double latitude { get; set; }
        [Required]
        public double longitude { get; set; }
        public string? scheduled_address { get; set; }
        public string? scheduled_date_programming { get; set; }
        public string? scheduled_hour_since { get; set; }
        public string? scheduled_hour_limit { get; set; }
        [Required]
        public int? scheduled_expiration_date { get; set; }
        public string? scheduled_instructions { get; set; }
        public string? scheduled_clasification_name { get; set; }
        public string? scheduled_clasification { get; set; }
        public string? scheduled_subclasification_name { get; set; }
        public string? scheduled_subclasification { get; set; }

        public string? frmRecIdTask { get; set; }
        [Required]
        public string frmAssignmentId { get; set; }
        public string? frmParentNumber { get; set; }
        public string? frmParentCategory { get; set; }
        public string? frmIdSitio { get; set; }
        public string? frmCustId { get; set; }
        public string? frmCodigoCierre { get; set; }
        public string? frmParentOwner { get; set; }
        public string? frmServer { get; set; }
        //public string? frmIdZona { get; set; }

    }
}