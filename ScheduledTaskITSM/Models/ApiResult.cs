namespace ScheduledTaskITSM.Models
{
    public class ApiResult
    {
        public TaskITSM taskITSM { get; set; }
        public List<TaskITSM> lstTaskITSM { get; set; }
        public CustomerExecon customerExecon { get; set; }
        public TPActivity tpActivity { get; set; }
        public TPCustomer tpCustomer { get; set; }
        public TpActivityComplement tpActivityComplement { get; set; }
        public ClientId clientIdTp { get; set; }
        public string customerId { get; set; }
        public string firebase_id { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
