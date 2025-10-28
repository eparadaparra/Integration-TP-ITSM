namespace ScheduledTaskITSM.Models
{

    public partial class TpActivityComplement
    {
        public string status { get; set; }
        public string message { get; set; }
        public MensajeComp? messageError { get; set; }
        public DataComplement data { get; set; }
    }

    public partial class MensajeComp 
    {
        public string? code { get; set; }
        public string? message { get; set; }
    }

    public partial class DataComplement
    {
        public string? firebase_id { get; set; }
        public List<Preload> data { get; set; }
    }

    public partial class DataFrm 
    {
        public string frmRecIdTask { get; set; }
        public string frmAssignmentId { get; set; }
        public string frmParentNumber { get; set; }
        public string frmParentCategory { get; set; }
        public string frmCodigoCierre { get; set; }
        public string frmParentOwner { get; set; }
        public string frmIdSitio { get; set; }
        public string frmCustId { get; set; }
        public string? frmServer { get; set; }
        //public string frmZona { get; set; }
    }
}
