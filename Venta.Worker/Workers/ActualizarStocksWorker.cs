
using Stocks.Domain.Service.Events;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Threading;

namespace Venta.Worker.Workers
{
    public class ActualizarStocksWorker : BackgroundService
    {
        private readonly IConsumerFactory _consumerFactory;
        public ActualizarStocksWorker(IConsumerFactory consumerFactory) {
            _consumerFactory = consumerFactory;
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory.GetConsumer();
            consumer.Subscribe("stocks");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);

                // handle consumed message.
                
            }

            consumer.Close();

            return Task.CompletedTask;
        }
    }
}
