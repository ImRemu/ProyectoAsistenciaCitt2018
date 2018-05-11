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
        public ActionResult FormLogin(ModeloUsuario user)
        {
            //verifica q la info del usuario no este vacia.
            if(!string.IsNullOrEmpty(user.correo) && !string.IsNullOrEmpty(user.password))
            {
                //busca al usuario segun su correo y password.
                if (dtb.alumno.FirstOrDefault(u => u.correo.Equals(user.correo) && u.password.Equals(user.password)) != null)
                {
                    //busca al usuario y lo selecciona.
                    var userE = from u in dtb.alumno
                                where u.correo.Equals(user.correo) & u.password.Equals(user.password)
                                select u;
                    
                    //redireccion hacia exito con el correo del usuario.
                    Session["user"] = new ModeloUsuario {
                        apellidos = userE.FirstOrDefault().apellido,
                        nombre = userE.FirstOrDefault().nombre,
                        correo = userE.FirstOrDefault().correo,
                        id_usuario = userE.FirstOrDefault().id_alumno,
                        password = userE.FirstOrDefault().password
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

        public ActionResult Registrarse(alumno user)
        {
            log_acciones log = new log_acciones();
            //verifica si recibe nulos o vacios.
            if(!string.IsNullOrEmpty(user.nombre) && !string.IsNullOrEmpty(user.apellido) &&
               !string.IsNullOrEmpty(user.correo) && !string.IsNullOrEmpty(user.password))
            {
                if(dtb.alumno.FirstOrDefault(u => u.correo.Equals(user.correo)) != null)
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
                    log.nombre_ejecucion = "Usuario Nuevo";
                    log.id_ejecutor = 0;
                    //se le da la habilitacion, 0 = no habilitado, y el tipo usuario.
                    user.habilitado = 0;
                    
                    //se guardan los cambios en la base de datos.
                    dtb.alumno.Add(user);
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
                alumno usE = (from u in dtb.alumno
                               where u.id_alumno == id
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

        public ActionResult DesactCuenta()
        {
            if(Session["user"] != null)
            {
                return View("DesactCuenta");
            }
            return View("Index");
        }
        //action que confirma la desactivacion de la cuenta.
        public ActionResult CuentaDesct(string txt_pass)
        {
            //se saca la pass de la session para compararla
            string p2 = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).password;
            p2 = p2.Replace(" ","");
            if (p2.Equals(txt_pass))
            {
                //seteo de id de usuario en session
                var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;
                //selecion del usuario que tiene inicida la sesion.
                alumno usE = (from u in dtb.alumno
                               where u.id_alumno == id
                               select u).FirstOrDefault();
                //se deshabilita el usuario
                usE.habilitado = 0;
                dtb.SaveChanges();
                //la session user se vuelve nula
                Session["user"] = null;
                //retorna al index de Account
                return View("Index");
            }
            else
            {
                //la contraseña no es la correcta, muestra error
                TempData["error"] = "La contraseña no es correcta.";
                ViewBag.Error = TempData["error"];
                //devuelve a la vista
                return View("DesactCuenta");
            }
            
            
        }

        public ActionResult VerifTomaRamo( int id)
        {
            
            if(Session["user"] != null)
            {
                int idTaller = id;
                return RedirectToAction("TomaRamo",new { id = idTaller});
            }
            else
            {
                TempData["LogNecesario"] = "Necesita conectarse para tomar el taller.";
                ViewBag.Error = TempData["LogNecesario"];
                return View("Index");
            }
        }

        public ActionResult TomaRamo(int id)
        {
            int idTaller = id;
            int userId = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

            var verifTaller = (from dt in dtb.det_asist
                               join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                               where h.taller_id_taller == id && dt.alumno_id_alumno == userId
                               select dt).FirstOrDefault();

            return View();
        }


    }
}