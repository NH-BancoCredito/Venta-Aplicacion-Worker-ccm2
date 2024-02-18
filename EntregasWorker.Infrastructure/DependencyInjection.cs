using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using VentaWorker.Domain.Service.Events;
using VentaWorker.Infrastructure.Services.Events;
using VentaWorker.CrossCutting.Configs;
using System.Net;
using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.Configuration;
using VentaWorker.Domain.Service.WebServices;
using VentaWorker.Infrastructure.Services.WebServices;


namespace VentaWorker.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);

            var httpClientBuilder = services.AddHttpClient<IVentaService, VentaService>(
                options =>
                {
                    options.BaseAddress = new Uri(appConfiguration.UrlBaseServicioVentas);
                    //options.Timeout = TimeSpan.FromMilliseconds(5000);
                }
                ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetBulkHeadPolicy());



            services.AddProducer(configInfo);
            services.AddEventServices();
            services.AddConsumer(configInfo);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2,
                            retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                //Camino altenativo para llamar a otro servicio failure o publicar en una cola(topico kafka)
                Console.WriteLine(result);
            };


            Action onReset = null;


            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(c => !c.IsSuccessStatusCode)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                onBreak, onReset
                );


        }

        private static IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy()
        {
            return Policy.BulkheadAsync<HttpResponseMessage>(1000, int.MaxValue);
        }


        private static IServiceCollection AddProducer(this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);

            var config = new ProducerConfig
            {
                Acks = Acks.Leader,
                BootstrapServers = appConfiguration.UrlKafkaServer,
                ClientId = Dns.GetHostName(),
            };

            services.AddSingleton<IPublisherFactory>(sp => new PublisherFactory(config));
            return services;
        }

        private static IServiceCollection AddConsumer(this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);

            var config = new ConsumerConfig
            {
                BootstrapServers = appConfiguration.UrlKafkaServer,
                GroupId = "stocks-actualizar-stocks",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            services.AddSingleton<IConsumerFactory>(sp => new ConsumerFactory(config));
            return services;
        }

        private static void AddEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, EventSender>();
        }
    }
}
