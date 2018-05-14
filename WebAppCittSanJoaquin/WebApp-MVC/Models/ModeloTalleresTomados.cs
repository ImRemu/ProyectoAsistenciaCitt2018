using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class ModeloTalleresTomados
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public System.DateTime hora_inicio { get; set; }
        public System.DateTime hora_termino { get; set; }
        public string dia_semana { get; set; }
    }
}