using System.Text;

namespace ScheduledTaskITSM.Conn
{
    public class CustomException
    {
        private readonly string logPathStringError = string.Empty;
        private readonly string logPathStringEvent = string.Empty;
        public CustomException() {
           logPathStringError = "C:\\inetpub\\wwwroot\\apiTaskTP\\App_Data\\00 ErrorLog.txt";//"wwwroot") + "\\" + logPathString; 
            logPathStringError = "C:\\inetpub\\wwwroot\\apiTaskTP\\App_Data\\00 EventLog.txt";//"wwwroot") + "\\" + logPathString; 
        }        
        public string LogPathError() => logPathStringError;
        public string LogPathEvent() => logPathStringEvent;
    }

    public class Logs {
        public void WriteLog(string strComments)
        {
            string logFile = new CustomException().LogPathEvent();
            //Console.WriteLine(logFile);
            StringBuilder sb = new StringBuilder();

            sb.Append('*', 50).Append(" ").Append(DateTime.Now.ToString()).Append(" ").Append('*', 50);
            sb.AppendLine();
            sb.AppendLine( strComments );

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine(sb.ToString());
            sw.Close();
        }

    }

}