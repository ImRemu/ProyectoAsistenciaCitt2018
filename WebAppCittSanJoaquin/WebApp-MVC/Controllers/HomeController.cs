using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_MVC.Models;

namespace WebApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        satcEntities dtb = new satcEntities();

        public ActionResult Index()
        {
            //ViewBag.ModalOn = "puedo poner lo que sea aca";
            //ViewBag.ModalMessage = "Mensaje de modal ejemplo";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Login";
            return View();
        }

        public ActionResult ListaTalleres()
        {
            List<ModeloTalleres> Model = new List<ModeloTalleres>();
            var lista = (from p in dtb.profesor join t in dtb.taller on p.id_profesor equals t.profesor_id_profesor
                         join h in dtb.horario on t.id_taller equals h.taller_id_taller
                         select new
                         {
                             uNombre = p.nombre,
                             uAps = p.apellidos,
                             idTaller = t.id_taller,
                             tNombre = t.nombre,
                             tDesc = t.descripcion,
                             hInicio = h.hora_inicio,
                             hTerm = h.hora_termino

                         }).ToList();
            foreach(var item in lista)
            {
                Model.Add(new ModeloTalleres()
                {
                    nombre = item.uNombre+" "+item.uAps,
                    id_taller = item.idTaller,
                    nombreTaller = item.tNombre,
                    descripcion = item.tDesc,
                    hora_inicio = item.hInicio,
                    hora_termino = item.hTerm
                });
            }

            ViewBag.Lista = Model;
            if (ViewBag.Lista != null)
            {
                return View("ListaTalleres");
            }
            else
            {
                //retorna a talleres vacios
                return View("Errores/LTaV");
            }
            
        }
    }
}