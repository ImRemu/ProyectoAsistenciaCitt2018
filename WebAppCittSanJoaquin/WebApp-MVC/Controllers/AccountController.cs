using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_MVC.Models;

namespace WebApp_MVC.Controllers
{
    public class AccountController : Controller
    {
        satcEntities dtb = new satcEntities();

        // El index se accesa cuando no se
        // tiene ningun parametro cuando se accede al controlador
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormLogin(usuario user)
        {
            //verifica q la info del usuario no este vacia.
            if(!string.IsNullOrEmpty(user.correo) && !string.IsNullOrEmpty(user.password))
            {
                //busca al usuario segun su correo y password.
                if (dtb.usuario.FirstOrDefault(u => u.correo.Equals(user.correo) && u.password.Equals(user.password)) != null)
                {
                    //busca al usuario y lo selecciona.
                    var userE = from u in dtb.usuario
                                where u.correo.Equals(user.correo) & u.password.Equals(user.password)
                                select u;
                    
                    //redireccion hacia exito con el correo del usuario.
                    Session["user"] = new ModeloUsuario {
                        apellidos = userE.FirstOrDefault().apellidos,
                        nombre = userE.FirstOrDefault().nombre,
                        correo = userE.FirstOrDefault().correo,
                        id_usuario = userE.FirstOrDefault().id_usuario
                    } ;

                    return RedirectToAction("Exito","Account");
                }
                else
                {
                    //en caso de error del login te devuelve a la pagina y muestra el error.
                    TempData["Error"] = "Email o constraseña incorrectos";
                    ViewBag.Error = TempData["Error"];
                    ViewBag.ModalMessage = TempData["Error"];
                    return View("Index");
                }
            }
            return RedirectToAction("Exito");
        }


#warning Notese que el parametro que se entrega tiene el mismo nombre que la clase anonima pasada por metodo RedirectToAction
        
        public ActionResult Exito(string email)
        {
            //verifica que la session no sea nula.
            if (Session["user"] != null)
            {
                //envia el correo hacia la vista como un viewbag
                ViewBag.Correo = email;
                //redirecciona a exito.
                return View("Exito");
            }
            
            return View("Index");
        }

        public ActionResult Recuperacion()
        {
            return View();
        }

        public ActionResult Registrarse(usuario user)
        {
            log_acciones log = new log_acciones();
            //verifica si recibe nulos o vacios.
            if(!string.IsNullOrEmpty(user.nombre) && !string.IsNullOrEmpty(user.apellidos) &&
               !string.IsNullOrEmpty(user.correo) && !string.IsNullOrEmpty(user.password))
            {
                if(dtb.usuario.FirstOrDefault(u => u.correo.Equals(user.correo)) != null)
                {
                    //en el caso de algun error manda un mensaje que se muestra en la vista registrarse.
                    TempData["error"] = "El email ya esta en uso";
                    ViewBag.Error = TempData["error"];
                    return View("Registrarse");
                }
                else
                {
                    //se genera un log de accion
                    log.fecha = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yy hh:mm:ss"));
                    log.accion = "Creacion de Usuario Alumno";
                    log.nombre_ejecutor = "Usuario Nuevo";
                    log.id_ejecutor = 0;
                    //se le da la habilitacion, 0 = no habilitado, y el tipo usuario.
                    user.habilitado = 0;
                    user.tipo_usuario = "a";
                    //se guardan los cambios en la base de datos.
                    dtb.usuario.Add(user);
                    dtb.log_acciones.Add(log);
                    dtb.SaveChanges();
                    //se retorna el mensaje correspondiente.
                    TempData["creado"] = "El Usuario se ha registrado exitosamente.";
                    ViewBag.Registrado = TempData["creado"];
                    return View("Registrado");
                }

            }
            return View();
        }

        public ActionResult Perfil()
        {
            if(Session["user"] != null)
            {
                //redireccion a perfil con la session q contiene al usuario.
                return View("Perfil", ((WebApp_MVC.Models.ModeloUsuario)Session["user"]));
            }
            return View("Index");
        }

        public ActionResult LogOut()
        {
            //desloguea al usuario conectado, vuelve la session nula.
            Session["user"] = null;
            return View("Index");
        }

        public ActionResult CambioPass()
        {
            if(Session["user"] != null)
            {
                //confirmacion de session.
                return View("CambioPass", ((WebApp_MVC.Models.ModeloUsuario)Session["user"]));
            }
            return View("Index");
        }

        public ActionResult CPassConfirmacion(string p1, string p2)
        {
            if(p1.Equals(p2))
            {
                //en caso de que las constraseñas p1 y p2 sean igual se hara el cambio en la bdd.
                var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                //selecion del usuario que tiene inicida la sesion.
                usuario usE = (from u in dtb.usuario
                               where u.id_usuario == id
                               select u).FirstOrDefault();
                //se le cambia el password por al usuario encontrado de la bdd por el pass que entrega la vista.
                usE.password = p1;
                dtb.SaveChanges();

                ViewBag.Hecho = "Cambio de contraseña hecho correctamente.";
                return View("CambioPass");
            }
            else
            {
                //en caso de que las contraseñas p1 y p2 de los text no sea igual tirara error.
                TempData["error"] = "las contraseñas deben ser iguales";
                ViewBag.Error = TempData["error"];
                return View("CambioPass");
            }
            
        }
    }
}