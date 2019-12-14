using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace MarcusTurewicz.Chat
{
    public static class Negotiate
    {
      [FunctionName("negotiate")]
      public static SignalRConnectionInfo GetSignalRInfo(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
          [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
      {
          return connectionInfo;
      }
    }
}
