using MassTransit;
using MassTransitMessages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitConsumer
{
    public class MassTransitTestConsumer : IConsumer<MassTransitTestMessage>
    {
        public string SomeValue { get; set; }

        public async Task Consume(ConsumeContext<MassTransitTestMessage> context)
        {
            Console.WriteLine($"[{DateTime.Now} - THREAD {Thread.CurrentThread.ManagedThreadId}] Got a message with body: {context.Message.Text}");

            await Task.Delay(10000);

            Console.WriteLine($"[{DateTime.Now} - THREAD {Thread.CurrentThread.ManagedThreadId}] Done processing message with body: {context.Message.Text}");
        }
    }
}
