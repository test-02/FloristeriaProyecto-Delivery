using System;
using System.Collections.Generic;
using System.Text;

namespace FloristeriaProyecto.Modelo
{
    public class Categoria
    {
        public string idcategoria { get; set; }
        public string imagen { get; set; }
        public string nombre { get; set; }
        public Dictionary<string, Producto> productos { get; set; }
    }
}
