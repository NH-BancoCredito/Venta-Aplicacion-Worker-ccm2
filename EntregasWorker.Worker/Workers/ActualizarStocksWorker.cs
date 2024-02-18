using VentaWorker.Domain.Service.Events;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VentaWorker.Domain.Models;
using VentaWorker.Application.CasosUso.AdministrarStocks.ActualizarStocks;
using Newtonsoft.Json;

namespace VentaWorker.Worker.Workers
{
    public class ActualizarStocksWorker : BackgroundService
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IServiceProvider _serviceProvider;

        public ActualizarStocksWorker(IConsumerFactory consumerFactory, IServiceProvider serviceProvider)
        {
            _consumerFactory = consumerFactory;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory.GetConsumer();
            consumer.Subscribe("stocks");

            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var consumeResult = consumer.Consume(cancellationToken);
                //Llamar al handler para actualizar la información de stocks
                ActualizarStocksRequest request = JsonConvert.DeserializeObject<ActualizarStocksRequest>(consumeResult.Value);

                await mediator.Send(request);
            }

            consumer.Close();

        }

    }
}
