using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_MVC.Models
{
    public class ModeloUsuario
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string password { get; set; }
        public byte habilitado { get; set; }
        public string tipo_usuario { get; set; }
    }
}