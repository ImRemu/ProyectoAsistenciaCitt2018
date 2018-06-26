using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class ModeloTalleresTomados
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public System.TimeSpan hora_inicio { get; set; }
        public System.TimeSpan hora_termino { get; set; }
        public string dia_semana { get; set; }
    }
}