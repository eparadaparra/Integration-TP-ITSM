namespace tasksAction.Conn
{
    public class Connection
    {
        private readonly string connectionStrig = string.Empty;
        private readonly string connectionStrigITSM_PRO = string.Empty;

        public Connection()
        {
            var conexion = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionStrig = conexion.GetSection("ConnectionStrings:connITSM").Value;
            
            var conexionITSM = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionStrigITSM_PRO = conexionITSM.GetSection("ConnectionStrings:connITSM-PRO").Value;
        }

        public string SqlComm() => connectionStrig;
        public string SqlCommITSMPRO() => connectionStrigITSM_PRO;
    }
}