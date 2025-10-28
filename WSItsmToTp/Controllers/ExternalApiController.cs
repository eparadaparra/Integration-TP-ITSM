using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WSItsmToTp.Services;

namespace WSItsmToTp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExternalApiController : ControllerBase
    {
        private readonly IServices _services;

        public ExternalApiController(IServices services)
        {
            _services = services;
        }

        [HttpPost("consume")]
        public async Task<string> ConsumeExternalApi(int assignmentID)
        {
            return await _services.GetMessage(assignmentID);
        }
    }
}
