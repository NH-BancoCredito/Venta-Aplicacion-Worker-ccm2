using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VentaWorker.Domain.Models;
using VentaWorker.Domain.Service.WebServices;

namespace VentaWorker.Infrastructure.Services.WebServices
{
    public class VentaService : IVentaService
    {
        private readonly HttpClient _httpClientVenta;
        public VentaService(HttpClient httpClientVenta) {
            _httpClientVenta = httpClientVenta;
            _httpClientVenta.DefaultRequestHeaders.ProxyAuthorization = null;
        }

        public async Task<bool> ActualizarProducto(Producto producto)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, "api/productos/actualizar");

            var entidadSerializada = JsonSerializer.Serialize(new
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                PrecioUnitario = producto.PrecioUnitario,
                IdCategoria = producto.IdCategoria
            });

            request.Content = new StringContent(entidadSerializada, Encoding.UTF8, MediaTypeNames.Application.Json );

            try
            {
                var response = await _httpClientVenta.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }


    
        }
    }
}
