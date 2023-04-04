using System;
using System.Collections.Generic;
using System.Text;

namespace FloristeriaProyecto.Modelo
{
    public class Despacho
    {
        public string personaContacto { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string celular { get; set; }

        public double longitud { get; set; }
        public double latitud { get; set; }
    }
}
