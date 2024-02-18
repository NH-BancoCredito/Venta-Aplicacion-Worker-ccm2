  using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = VentaWorker.Domain.Models;

namespace VentaWorker.Application.CasosUso.AdministrarStocks.ActualizarStocks
{
    public class ActualizarStocksMapper: Profile
    {
        public ActualizarStocksMapper() {
            CreateMap<ActualizarStocksRequest, Models.Producto>();

        }
    }
}
