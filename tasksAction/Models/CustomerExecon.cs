namespace tasksAction.Models
{
    public partial class CustomerExecon
    {
        public string? recId { get; set; }
        public string? uuid { get; set; }
        public string? nameCustomer { get; set; }
        public string? mailAdminTP { get; set; }
        public string? uuidAdminTP { get; set; }
        public string? contactCustomer { get; set; }
        public string? addressCustomer { get; set; }
        public string? telephoneCustomer { get; set; }
        public string? client_IdCustomer { get; set; }
        public string? email { get; set; }
        public bool init { get; set; }
        public bool close { get; set; }
        public bool scheduled { get; set; }
        public bool daybefore { get; set; }
        public bool send_ics { get; set; }
    }
}
