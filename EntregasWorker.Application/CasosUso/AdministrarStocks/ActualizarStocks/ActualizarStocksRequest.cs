using MediatR;
using VentaWorker.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentaWorker.Domain.Models;
using MongoDB.Bson;

namespace VentaWorker.Application.CasosUso.AdministrarStocks.ActualizarStocks
{
    public class ActualizarStocksRequest: IRequest<IResult>
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int IdCategoria { get; set; }

    }

}
