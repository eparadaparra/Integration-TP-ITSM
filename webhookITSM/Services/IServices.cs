using Newtonsoft.Json.Linq;
using webhookITSM.Models;

namespace webhookITSM.Services
{
    public interface IServices
    {
        Task<bool> WHTaskITSM(object objeto);

        Task<bool> NotificationOO(JToken objeto);
    }
}
