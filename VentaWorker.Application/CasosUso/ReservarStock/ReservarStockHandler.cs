using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Application.Common;
using Stocks.Domain.Repositories;

namespace Stocks.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ReservarStockHandler :
        IRequestHandler<ReservarStockRequest, IResult>
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        public ReservarStockHandler(IProductoRepository productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
       

        public async Task<IResult> Handle(ReservarStockRequest request, CancellationToken cancellationToken)
        {

            IResult response = null;

            try
            {               
                var producto = await _productoRepository.Consultar(request.IdProducto);
                producto.Stock -= request.Cantidad; //disminuir el stock
                var actualizar =  await _productoRepository.Modificar(producto);
                if (actualizar) return new SuccessResult();
                else return new FailureResult();
            }
            catch(Exception ex)
            {
                response = new FailureResult();
                return response;
            }
        }
    }
}
