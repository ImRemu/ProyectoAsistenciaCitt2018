using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class joinTallUsHr
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int id_taller { get; set; }
        public string nombreTaller { get; set; }
        public string descripcion { get; set; }
        public System.DateTime hora_inicio { get; set; }
        public System.DateTime hora_termino { get; set; }
    }
}