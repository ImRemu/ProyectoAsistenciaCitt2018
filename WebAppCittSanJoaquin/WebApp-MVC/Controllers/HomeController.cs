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
            using (SatcEntities dtb = new SatcEntities())
            {
                List<ModeloTalleres> Model = new List<ModeloTalleres>();
                var lista = (from p in dtb.profesor
                             join t in dtb.taller on p.id_profesor equals t.profesor_id_profesor
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
                foreach (var item in lista)
                {
                    Model.Add(new ModeloTalleres()
                    {
                        nombre = item.uNombre + " " + item.uAps,
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
        [HttpPost]
        public ActionResult CambiarContrasena(string p1, string p2)
        {
            if (!p1.Equals(p2))
            {
                ViewBag.Error = "Las contraseñas ingresadas no son iguales. Reintenta.";
                return View();
            }
            try
            {
                using (SatcEntities ef = new SatcEntities())
                {
                    ModeloUsuario usuario = (ModeloUsuario)Session["usuario"];
                    if (usuario == null)
                    {
                        throw new Exception("La sesion del usuario no existe, ergo, no se puede acceder a ella");
                    }
                    var us = ef.alumno.FirstOrDefault(a => a.id_alumno == usuario.id_usuario);
                    us.password = p1.Trim();
                    ef.SaveChanges();
                    TempData["ModalMessage"] = "Se ha realizado el cambio de contraseña correctamente! " +
                        "Inicia sesion con tu nueva contraseña para probarla.";
                    Session["usuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Ha ocurrido un error en HomeController.");
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                //ViewBag.ModalMessage("Ha ocurrido un error al procesar la solicitud. Lamentamos la molestia");
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult CambiarContrasena()
        {
            if (Session["usuario"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
    }
}