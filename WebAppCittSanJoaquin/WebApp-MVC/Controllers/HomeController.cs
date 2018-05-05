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
            /*IList<WebApp_MVC.Models.ModeloTaller> listTalleres = new IList<Models.ModeloTaller>();*/
            var lista = (from l in dtb.taller
                         select l).ToList();
            ViewBag.Lista = lista;
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