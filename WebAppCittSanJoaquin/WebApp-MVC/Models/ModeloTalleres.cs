using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{   //modelo de mostreo en la vista talleres
    public class ModeloTalleres
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int id_taller { get; set; }
        public string nombreTaller { get; set; }
        public string descripcion { get; set; }
        public System.TimeSpan hora_inicio { get; set; }
        public System.TimeSpan hora_termino { get; set; }
    }
}