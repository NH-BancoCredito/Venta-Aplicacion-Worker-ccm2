using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentaWorker.Application.CasosUso.AdministrarEntregas.RegistrarEntrega;
using VentaWorker.Domain.Repositories;
using NSubstitute;
using System.Threading;
using VentaWorker.Application.Common;
using NSubstitute.ExceptionExtensions;
using static VentaWorker.Application.CasosUso.AdministrarEntregas.RegistrarEntrega.ActualizarStocksHandler;

namespace VentaWorker.Test.Aplication.Testes
{


    public class AdministrarEntregasTests
    {
        private readonly IEntregaRepository _entregaRepository;
        private readonly IMapper _mapper;
        private readonly ActualizarStocksHandler _registrarEntregaHandler;

        public AdministrarEntregasTests()
        {
            _entregaRepository = Substitute.For<IEntregaRepository>();
            _mapper = Substitute.For<IMapper>();
            _registrarEntregaHandler = Substitute.For<ActualizarStocksHandler>(_entregaRepository, _mapper);

        }

        [Fact]
        public async Task RegistrarEntregaOK()
        {
            var request = new ActualizarStocksRequest() { IdVenta = 123 };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //Escenarios
            _entregaRepository.Adicionar(default).ReturnsForAnyArgs(true);

            cts.Cancel();
            var retorno = await _registrarEntregaHandler.Handle(request, cancellationToken);

            /// Assert
            Assert.True(retorno.HasSucceeded);

        }

        [Fact]
        public async Task RegistrarEntregaError()
        {
            var request = new ActualizarStocksRequest() { IdVenta = 123 };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //Escenarios
            _entregaRepository.Adicionar(default).ReturnsForAnyArgs(false);

            cts.Cancel();
            var retorno = await _registrarEntregaHandler.Handle(request, cancellationToken);

            /// Assert
            Assert.False(retorno.HasSucceeded);

        }

        [Fact]
        public async Task RegistrarEntregaException()
        {

            var request = new ActualizarStocksRequest() { IdVenta = 123 };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //Escenarios
            _entregaRepository.Adicionar(default).ThrowsForAnyArgs<Exception>();

            cts.Cancel();
            /// Assert
            Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await _registrarEntregaHandler.Handle(request, cancellationToken);
            });


        }


    }
}
