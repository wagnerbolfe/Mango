using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private const string connectionString = "Endpoint=sb://wbmangoweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pVepH6F7IyZA/BRknt1Wspc3nFdO8FR0F+ASbESDw2Y=";

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
