using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class ModeloTaller
    {
        public int id_taller { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int id_encargado { get; set; }
    }
}