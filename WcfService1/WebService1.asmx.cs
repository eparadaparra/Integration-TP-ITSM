using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Services;

namespace WcfService1
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService1 : WebService
    {
        [WebMethod]
        public string CallExternalApi(int assignmentID)
        {
            //Console.WriteLine(CreateTask(assignmentID).GetAwaiter().GetResult());
            Console.WriteLine(assignmentID);
            return CreateTask(assignmentID).GetAwaiter().GetResult();
            //CreateTask(assignmentID).GetAwaiter().GetResult();
        }

        public async Task<string> CreateTask(int assignmentID)
        {
            return await LibraryAplication.CreateTask(assignmentID);
        }
    }
}
