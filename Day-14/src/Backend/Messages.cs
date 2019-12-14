using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace MarcusTurewicz.Chat
{
    public static class Messages
    {
    [FunctionName("messages")]
    public static Task SendMessage(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post")] object message,
          [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
    {
      return signalRMessages.AddAsync(
          new SignalRMessage
          {
            Target = "newMessage",
            Arguments = new[] { message }
          });
    }
    }
}
