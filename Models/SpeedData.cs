using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speed.Models
{
    public class SpeedData
    {
        [JsonProperty("id_speed")]
        public string IdSpeed { get; set; }

        [JsonProperty("nombre_origen")]
        public string NombreOrigen { get; set; }

        [JsonProperty("ciudad_origen")]
        public string CiudadOrigen { get; set; }

        [JsonProperty("direccion_origen")]
        public string DireccionOrigen { get; set; }

        [JsonProperty("telefono_origen")]
        public string TelefonoOrigen { get; set; }

        [JsonProperty("referencia_origen")]
        public string ReferenciaOrigen { get; set; }

        [JsonProperty("nombre_destino")]
        public string NombreDestino { get; set; }

        [JsonProperty("ciudad_destino")]
        public string CiudadDestino { get; set; }

        [JsonProperty("direccion_destino")]
        public string DireccionDestino { get; set; }

        [JsonProperty("telefono_destino")]
        public string TelefonoDestino { get; set; }
        [JsonProperty("contiene")]
        public string Contiene { get; set; }
        [JsonProperty("guia")]
        public string Guia { get; set; }
        [JsonProperty("recaudo")]
        public string Recaudo  { get; set; }
        [JsonProperty("monto_factura")]
        public string MontoFactura{ get; set; }
    }

    }
