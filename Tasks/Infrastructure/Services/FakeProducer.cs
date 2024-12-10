using Intaker.Domain.Common;
using Newtonsoft.Json;

namespace Intaker.Infrastructure.Services
{
    public class FakeProducer(ILogger<FakeProducer> logger) : IEventProducer
    {
        private readonly ILogger<FakeProducer> logger = logger;

        public void Send(object message)
        {
            string msgJson = JsonConvert.SerializeObject(message);

            logger.LogWarning($"FakeProducer: sending {msgJson} to AMQP");
        }
    }
}
