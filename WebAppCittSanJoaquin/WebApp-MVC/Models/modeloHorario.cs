using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class modeloHorario
    {
        public int id_horario { get; set; }
        public System.TimeSpan hora_inicio { get; set; }
        public System.TimeSpan hora_termino { get; set; }
        public string dia_semana { get; set; }
        public int cupo { get; set; }
        public int taller_id_taller { get; set; }
    }
}