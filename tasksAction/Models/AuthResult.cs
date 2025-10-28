namespace tasksAction.Models
{
    public class AuthResult
    {
        public string statusCode { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string uuid { get; set; }
        public string user { get; set; }
    }
}