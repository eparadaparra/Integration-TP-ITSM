using ScheduledTaskITSM.Models;

namespace ScheduledTaskITSM.Services
{
    public interface IServices
    {
        Task<Object> GetActivityTP(string firebase_id);


        Task<TaskITSM> GetTaskITSM(string assignmentId, string server);
        Task<bool> UpTaskITSM(TPActivity objeto);
        Task<CustomerExecon> GetCustomerITSM(string custId, string server);
        Task<ClientId> GetCustomerTP(string custId, string server);
        Task<ClientId> InsCustomerTP(string custId, string server);
        Task<TPActivity> PostActivityTP(TaskITSM objeto, string server);
        Task<TpActivityComplement> PostAditionalActivity(TpActivityComplement objeto);

    }
}