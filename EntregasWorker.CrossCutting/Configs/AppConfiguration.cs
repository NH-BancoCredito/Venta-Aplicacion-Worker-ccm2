using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentaWorker.CrossCutting.Configs
{
    public class AppConfiguration
    {
        private readonly IConfiguration _configInfo;
        public AppConfiguration(IConfiguration configInfo) {
            _configInfo = configInfo;
        }

        public string UrlBaseServicioVentas
        {
            get
            {
                return _configInfo["url-base-servicio-ventas"];
            }
            private set { }
        }

        public string UrlKafkaServer
        {
            get
            {
                return _configInfo["url-kafka-server"];
            }
            private set { }
        }

    }
}
