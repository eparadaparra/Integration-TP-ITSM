namespace tasksAction.Models
{
    public partial class TasksSearchM
    {
        public string? IdTarea { get; set; }
        public string? ParentId { get; set; }
        public string? TipoTarea { get; set; }
        public string? FirebaseId { get; set; }
        public string? StatusIvanti { get; set; }
        public string? SubStatus { get; set; }
        public string? Owner { get; set; }
        public string? Prioridad { get; set; }
        public string? ManagerTeam { get; set; }
        public string? Team { get; set; }
        public string? Zona { get; set; } //
        public string? CustId { get; set; }
        public string? IdSitio { get; set; }
        public string? FechaCreacion { get; set; }
        public string? FechaAsignacion { get; set; }
        public string? FechaRequerida { get; set; }
        public string? FechaTraslado { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public string? FechaCompletada { get; set; }
        public string? FechaResolucion { get; set; }
        public string? ModificadoPor { get; set; }
        public string? UltimaModificacion { get; set; }
        public string? RecId { get; set; }
    }
}
