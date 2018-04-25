using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_MVC.Controllers
{
    public class AccountController : Controller
    {
        // El index se accesa cuando no se
        // tiene ningun parametro cuando se accede al controlador
        public ActionResult Index()
        {
            return View();
        }

        // Este metodo procesa el formulario
        // luego de eso, redirecciona a otra pagina
        #warning Notese que el parametro es de tipo FormModel
        [HttpPost]
        public ActionResult FormLogin(Models.FormModel formData)
        {
            // usamos los datos del form, en este caso, textBoxData de FormModel.
            return RedirectToAction("Exito", new { parametro = formData.TextBoxStringData });
        }

        // Este metodo recibe el parametro "parametro"
        // comprueba que no este vacio, y luego,
        // se lo pasa a la vista
#warning Notese que el parametro que se entrega tiene el mismo nombre que la clase anonima pasada por metodo RedirectToAction

        public ActionResult Exito(string parametro)
        {
            // Si no esta vacia, entonces pasamos el parametro a la vista
            if (!string.IsNullOrEmpty(parametro))
            {
                ViewBag.Resultado = parametro;
            }
            return View();
        }

        public ActionResult Recuperacion()
        {
            return View();
        }

        public ActionResult Registrarse()
        {
            return View();
        }
    }
}