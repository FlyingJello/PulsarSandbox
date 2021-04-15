using System.Threading.Tasks;
using DotPulsar;

namespace PulsarConnector.Services
{
    public interface IProducerService
    {
        Task<MessageId> ProduceAsync(string topic, string payload);
    }
}
