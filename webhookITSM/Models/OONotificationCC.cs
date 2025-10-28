namespace webhookITSM.Models
{
    public partial class OONotificationCc
    {
        public string flowUuid { get; set; }
        public string runName { get; set; }
        public string logLevel { get; set; }
        public InputsOO inputs { get; set; }
    }

    public partial class InputsOO
    {
        public string? noTarea { get; set; }
        public string? cliente { get; set; }
        public string? ism { get; set; }
        public string? fecha_cierre { get; set; }
        public string? codigoCierre { get; set; }
        public string? codigoCierreIvanti { get; set; }
        public string? agente { get; set; }
        public string? owner { get; set; }
        public string? parent { get; set; }
        public string? subject { get; set; }
    }
}
