using VentaWorker.Domain.Service.Events;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Threading;
using VentaWorker.Domain.Service.WebServices;
using MongoDB.Bson;
using Stocks.Api;
using System.Text.Json;
using System.Net.Mime;
using System.Text;
using VentaWorker.Domain.Models;
using System.Reflection;
using VentaWorker.Application.Common;
using Models = VentaWorker.Domain.Models;

namespace VentaWorker.Worker.Workers
{
    public class ActualizarStocksWorker : BackgroundService
    {
        private readonly IVentaService _ventaService;
        private readonly IConsumerFactory _consumerFactory;

        public ActualizarStocksWorker(IConsumerFactory consumerFactory, IVentaService ventaService)
        {
            _consumerFactory = consumerFactory;
            _ventaService = ventaService;
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory.GetConsumer();
            consumer.Subscribe("stocks");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                //Llamar al servicio para actualizar la información del producto
                //, la actualización deberia relizarse llamando una api del
                //microservicio de Ventas
                Producto? producto = JsonSerializer.Deserialize<Producto>(consumeResult.Value);

                _ventaService.ActualizarProducto(producto);

            }

            consumer.Close();

            return Task.CompletedTask;
        }

    }
}
