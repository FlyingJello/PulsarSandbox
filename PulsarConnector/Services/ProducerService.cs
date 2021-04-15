using System;
using System.Threading;
using System.Threading.Tasks;
using DotPulsar;
using DotPulsar.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PulsarConnector.Configuration;

namespace PulsarConnector.Services
{
    public class ProducerService : IProducerService
    {
        private readonly PulsarConfig _pulsarConfig;
        private readonly ILogger<ProducerService> _logger;

        public ProducerService(IOptions<PulsarConfig> pulsarConfig, ILogger<ProducerService> logger)
        {
            _logger = logger;
            _pulsarConfig = pulsarConfig.Value;
        }

        public async Task<MessageId> ProduceAsync(string topic, string payload)
        {
            await using var client = PulsarClient.Builder()
                .ServiceUrl(new Uri(_pulsarConfig.ServiceUrl))
                .Build();

            await using var producer = client.NewProducer(Schema.String)
                .StateChangedHandler(HandleStateChange)
                .Topic(topic)
                .Create();

            return await producer.Send(payload);
        }

        private void HandleStateChange(ProducerStateChanged stateChanged, CancellationToken cancellationToke)
        {
            _logger.LogInformation(stateChanged.ProducerState.ToString());
        }
    }
}
