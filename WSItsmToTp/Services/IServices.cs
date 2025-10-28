using System.ServiceModel;

namespace WSItsmToTp.Services
{
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        Task<string> GetMessage(int assignmentID);
    }
}
