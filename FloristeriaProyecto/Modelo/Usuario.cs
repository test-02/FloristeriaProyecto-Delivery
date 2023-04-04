using System;
using System.Collections.Generic;
using System.Text;

namespace FloristeriaProyecto.Modelo
{
    public class Usuario
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string Clave { get; set; }
        public double longitud { get; set; }
        public double latitud { get; set; }


    }
}
