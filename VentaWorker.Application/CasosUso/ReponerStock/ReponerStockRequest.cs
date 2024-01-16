using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Application.Common;

namespace Stocks.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ReponerStockRequest : IRequest<IResult>
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; } 
    }
}
