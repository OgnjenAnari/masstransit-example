using MassTransit;
using MassTransitMessages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassTransitProducer
{
    class Producer
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("eu-central-1", h => { });
                cfg.Message<MassTransitTestMessage>(m => m.SetEntityName("masstransit-test-topic"));
            });

            await busControl.StartAsync();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 20; i++)
            {
                tasks.Add(busControl.Publish(new MassTransitTestMessage($"Message number: {i + 1}")));
            }

            Task.WaitAll(tasks.ToArray());

            await busControl.StopAsync();
        }
    }
}
