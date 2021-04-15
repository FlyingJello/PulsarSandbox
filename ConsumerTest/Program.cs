using System;
using System.Threading;
using System.Threading.Tasks;
using DotPulsar;
using DotPulsar.Extensions;

namespace ConsumerTest
{
    class Program
    {
        static async Task Main()
        {
            await using var client = PulsarClient.Builder()
                .ServiceUrl(new Uri("pulsar://pulsar:6650"))
                .Build();

            await using var consumer = client.NewConsumer(Schema.String)
                .StateChangedHandler(HandleStateChange)
                .SubscriptionName("test-sub")
                .SubscriptionType(SubscriptionType.Shared)
                .Topic("persistent://public/testevents/packagedelivery")
                .Create();

            await foreach (var message in consumer.Messages())
            {
                Console.WriteLine(message.Value());
                await consumer.Acknowledge(message);
            }
        }

        private static void HandleStateChange(ConsumerStateChanged stateChanged, CancellationToken cancellationToken)
        {
            Console.WriteLine(stateChanged.ConsumerState.ToString());
        }
    }
}
