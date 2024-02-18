using MediatR;
using VentaWorker.Application.Common;
using AutoMapper;
using Models = VentaWorker.Domain.Models;
using VentaWorker.Domain.Service.WebServices;

namespace VentaWorker.Application.CasosUso.AdministrarStocks.ActualizarStocks
{
    public class ActualizarStocksHandler : IRequestHandler<ActualizarStocksRequest, IResult>
    {
        private readonly IVentaService _ventaService;
        private readonly IMapper _mapper;
        public ActualizarStocksHandler(IVentaService ventaService, IMapper mapper)
        {
            _ventaService = ventaService;
            _mapper = mapper;
        }
        public async Task<IResult> Handle(ActualizarStocksRequest request, CancellationToken cancellationToken)
        {
            IResult response = null;

            try
            {
                //Aplicando el automapper para convertir el objeto Request a Producto
                var producto = _mapper.Map<Models.Producto>(request);

                /// Registrar entrega
                /// 
                var actualizar = await _ventaService.ActualizarProducto(producto);

                if (actualizar)
                    response = new SuccessResult();
                else
                    response = new FailureResult();

                return response;
            }
            catch (Exception ex)
            {
                return new FailureResult();
            }

        }

    }
}
