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
        public ActionResult FormLogin(string txt_email, string txt_pass)
        {
            //verifica q la info del usuario no este vacia.
            if(!string.IsNullOrEmpty(txt_email) && !string.IsNullOrEmpty(txt_pass))
            {
                //busca al usuario segun su correo y password.
                if (dtb.alumno.FirstOrDefault(u => u.correo.Equals(txt_email) && u.password.Equals(txt_pass)) != null)
                {
                    //busca al usuario y lo selecciona.
                    var userE = from u in dtb.alumno
                                where u.correo.Equals(txt_email) & u.password.Equals(txt_pass)
                                select u;
                    
                    //redireccion hacia exito con el correo del usuario.
                    Session["user"] = new ModeloUsuario {
                        apellidos = userE.FirstOrDefault().apellido,
                        nombre = userE.FirstOrDefault().nombre,
                        correo = userE.FirstOrDefault().correo,
                        id_usuario = userE.FirstOrDefault().id_alumno,
                        password = userE.FirstOrDefault().password,
                        tipo_usuario = "a"
                    } ;

                    return RedirectToAction("Exito","Account");
                }
                else if(dtb.profesor.FirstOrDefault(u => u.correo.Equals(txt_email) && u.password.Equals(txt_pass)) != null)
                {
                    //busca al usuario y lo selecciona.
                    var userE = from u in dtb.profesor
                                where u.correo.Equals(txt_email) & u.password.Equals(txt_pass)
                                select u;

                    //redireccion hacia exito con el correo del usuario.
                    Session["user"] = new ModeloUsuario
                    {
                        apellidos = userE.FirstOrDefault().apellidos,
                        nombre = userE.FirstOrDefault().nombre,
                        correo = userE.FirstOrDefault().correo,
                        id_usuario = userE.FirstOrDefault().id_profesor,
                        password = userE.FirstOrDefault().password,
                        tipo_usuario = "p"
                    };

                    return RedirectToAction("exitoP", "Account");
                } 
                else if(dtb.administrador.FirstOrDefault(u => u.correo.Equals(txt_email) && u.password.Equals(txt_pass)) != null)
                {
                    //busca al usuario y lo selecciona.
                    var userE = from u in dtb.administrador
                                where u.correo.Equals(txt_email) & u.password.Equals(txt_pass)
                                select u;

                    //redireccion hacia exito con el correo del usuario.
                    Session["user"] = new ModeloUsuario
                    {
                        apellidos = userE.FirstOrDefault().apellidos,
                        nombre = userE.FirstOrDefault().nombre,
                        correo = userE.FirstOrDefault().correo,
                        id_usuario = userE.FirstOrDefault().id_admin,
                        password = userE.FirstOrDefault().password,
                        tipo_usuario = "A"
                    };

                    return RedirectToAction("exitoAdmin", "Account");
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

        public ActionResult redirectPerfil()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("Perfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("perfilP");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return RedirectToAction("perfilAdm");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult redirectExito()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("Exito");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("exitoP");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return RedirectToAction("exitoAdmin");
            }
            else
            {
                return View("Index");
            }
        }
#warning Notese que el parametro que se entrega tiene el mismo nombre que la clase anonima pasada por metodo RedirectToAction
        
        public ActionResult Exito()
        {
            //verifica que la session no sea nula.
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                //redirecciona a exito.
                return View("Exito");
            }
            else
            {
                return RedirectToAction("redirectExito");
            }
            
            
        }

        public ActionResult exitoAdmin()
        {
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return View("exitoAdmin");
            }
            else
            {
                return RedirectToAction("redirectExito");
            }

        }

        public ActionResult exitoP()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return View("exitoP");
            }
            else
            {
                return RedirectToAction("redirectExito");
            }

        }

        public ActionResult Recuperacion()
        {
            return View();
        }

        public ActionResult Registrarse(string txt_nombre, string txt_apell, string txt_email, string txt_pass )
        {
            log_acciones log = new log_acciones();
            //verifica si recibe nulos o vacios.
            if(!string.IsNullOrEmpty(txt_nombre) && !string.IsNullOrEmpty(txt_apell) &&
               !string.IsNullOrEmpty(txt_email) && !string.IsNullOrEmpty(txt_pass))
            {
                if(dtb.alumno.FirstOrDefault(u => u.correo.Equals(txt_email)) != null)
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
                    alumno userAl = new alumno();
                    userAl.nombre = txt_nombre;
                    userAl.apellido = txt_apell;
                    userAl.correo = txt_email;
                    userAl.password = txt_pass;
                    userAl.habilitado = 0;
                    
                    //se guardan los cambios en la base de datos.
                    dtb.alumno.Add(userAl);
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

        public ActionResult registrarP(string txt_nombre, string txt_apell, string txt_email, string txt_pass)
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                if (!string.IsNullOrEmpty(txt_nombre) && !string.IsNullOrEmpty(txt_apell) &&
              !string.IsNullOrEmpty(txt_email) && !string.IsNullOrEmpty(txt_pass))
                {
                    if (dtb.profesor.FirstOrDefault(u => u.correo.Equals(txt_email)) != null)
                    {
                        //en el caso de algun error manda un mensaje que se muestra en la vista registrarse.
                        TempData["error"] = "El email ya esta en uso";
                        ViewBag.Error = TempData["error"];
                        return View("registrarP");
                    }
                    else
                    {

                        //se le da la habilitacion, 0 = no habilitado, y el tipo usuario.
                        profesor userP = new profesor();
                        userP.nombre = txt_nombre;
                        userP.apellidos = txt_apell;
                        userP.correo = txt_email;
                        userP.password = txt_pass;
                        userP.habilitado = 0;

                        //se guardan los cambios en la base de datos.
                        dtb.profesor.Add(userP);
                        dtb.SaveChanges();
                        //se retorna el mensaje correspondiente.
                        TempData["creado"] = "El Usuario se ha registrado exitosamente.";
                        ViewBag.Registrado = TempData["creado"];
                        return View("registrarP");
                    }

                }
                return View();
            }
            else if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Perfil()
        {
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                //redireccion a perfil con la session q contiene al usuario.
                return View("Perfil", ((WebApp_MVC.Models.ModeloUsuario)Session["user"]) );
            }
            else
            {
                return RedirectToAction("redirectPerfil");
            }
        }

        public ActionResult perfilAdm()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                //redireccion a perfil con la session q contiene al usuario.
                return View("perfilAdm", ((WebApp_MVC.Models.ModeloUsuario)Session["user"]) );
            }
            else
            {
                return RedirectToAction("redirectPerfil");
            }
        }

        public ActionResult perfilP()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                //redireccion a perfil con la session q contiene al usuario.
                return View("perfilP", ((WebApp_MVC.Models.ModeloUsuario)Session["user"]) );
            }
            else
            {
                return RedirectToAction("redirectPerfil");
            }
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
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a") )
            {
                if (p1.Equals(p2))
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
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p") )
            {
                if (p1.Equals(p2))
                {
                    //en caso de que las constraseñas p1 y p2 sean igual se hara el cambio en la bdd.
                    var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                    //selecion del usuario que tiene inicida la sesion.
                    profesor usE = (from u in dtb.profesor
                                  where u.id_profesor == id
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
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                if (p1.Equals(p2))
                {
                    //en caso de que las constraseñas p1 y p2 sean igual se hara el cambio en la bdd.
                    var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                    //selecion del usuario que tiene inicida la sesion.
                    administrador usE = (from u in dtb.administrador
                                    where u.id_admin == id
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
            else
            {
                return View("Index");
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
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                //se saca la pass de la session para compararla
                string p2 = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).password;
                p2 = p2.Replace(" ", "");
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
            else if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                //se saca la pass de la session para compararla
                string p2 = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).password;
                p2 = p2.Replace(" ", "");
                if (p2.Equals(txt_pass))
                {
                    //seteo de id de usuario en session
                    var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;
                    //selecion del usuario que tiene inicida la sesion.
                    profesor usE = (from u in dtb.profesor
                                  where u.id_profesor == id
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
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                //se saca la pass de la session para compararla
                string p2 = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).password;
                p2 = p2.Replace(" ", "");
                if (p2.Equals(txt_pass))
                {
                    //seteo de id de usuario en session
                    var id = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;
                    //selecion del usuario que tiene inicida la sesion.
                    administrador usE = (from u in dtb.administrador
                                  where u.id_admin == id
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
            else
            {
                return View("Index");
            }
            
            
            
        }

        public ActionResult VerifTomaRamo( int id)
        {
            
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                int idTaller = id;
                return RedirectToAction("TomaRamo",new { id = idTaller});
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                TempData["tipoUser"] = "para tomar un taller debe ser un Alumno";
                ViewBag.Error = TempData["tipoUser"];
                return View("exitoP");
            }
            else if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                TempData["tipoUser"] = "para tomar un taller debe ser un Alumno";
                ViewBag.Error = TempData["tipoUser"];
                return View("exitoAdmin");
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
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                int idTaller = id;
                int userId = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                var verifTaller = (from dt in dtb.det_asist
                                   join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                                   where h.taller_id_taller == id && dt.alumno_id_alumno == userId
                                   select dt).FirstOrDefault();

                if (verifTaller == null)
                {
                    det_asist tTomado = new det_asist();
                    int idHorario = (from h in dtb.horario
                                     where h.taller_id_taller.Equals(idTaller)
                                     select h.id_horario).FirstOrDefault();
                    tTomado.alumno_id_alumno = userId;
                    tTomado.horario_id_horario = idHorario;
                    dtb.det_asist.Add(tTomado);
                    dtb.SaveChanges();

                    TempData["tomado"] = "Taller tomado con exito.";
                    ViewBag.Error = TempData["tomado"];
                    return View("../Home/TallerTomado");
                }
                else
                {
                    TempData["yaTomo"] = "Usted ya tiene tomado este taller.";
                    ViewBag.Error = TempData["yaTomo"];
                    return View("../Home/TallerTomado");
                }
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                TempData["tipoUser"] = "para tomar un taller debe ser un Alumno";
                ViewBag.Error = TempData["tipoUser"];
                return View("exitoP");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                TempData["tipoUser"] = "para tomar un taller debe ser un Alumno";
                ViewBag.Error = TempData["tipoUser"];
                return View("exitoAdmin");
            }
            else
            {
                TempData["LogNecesario"] = "Necesita conectarse para tomar el taller.";
                ViewBag.Error = TempData["LogNecesario"];
                return View("Index");
            }

            
        }

        public ActionResult TalleresTomados()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {


                int userId = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                List<ModeloTalleresTomados> Model = new List<ModeloTalleresTomados>();
                var lista = (from dt in dtb.det_asist
                             join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                             join t in dtb.taller on h.taller_id_taller equals t.id_taller
                             where dt.alumno_id_alumno == userId
                             select new
                             {
                                 nombre = t.nombre,
                                 descripcion = t.descripcion,
                                 horaInicio = h.hora_inicio,
                                 horaTermino = h.hora_termino,
                                 diaSemana = h.dia_semana

                             }).ToList();
                foreach (var item in lista)
                {
                    Model.Add(new ModeloTalleresTomados()
                    {
                        nombre = item.nombre,
                        descripcion = item.descripcion,
                        hora_inicio = item.horaInicio,
                        hora_termino = item.horaTermino,
                        dia_semana = item.diaSemana
                    });
                }

                ViewBag.Lista = Model;
                if (ViewBag.Lista != null)
                {
                    return View("TalleresTomados");
                }
                else
                {

                    return View("TalleresTomados");
                }
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }

            
        }

        public ActionResult talleresACargo()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                int userId = ((WebApp_MVC.Models.ModeloUsuario)(Session["user"])).id_usuario;

                List<ModeloTalleresTomados> Model = new List<ModeloTalleresTomados>();
                var lista = (from t in dtb.taller
                             join h in dtb.horario on t.id_taller equals h.taller_id_taller
                             where t.profesor_id_profesor == userId
                             select new
                             {
                                 nombre = t.nombre,
                                 descripcion = t.descripcion,
                                 horaInicio = h.hora_inicio,
                                 horaTermino = h.hora_termino,
                                 diaSemana = h.dia_semana

                             }).ToList();
                foreach (var item in lista)
                {
                    Model.Add(new ModeloTalleresTomados()
                    {
                        nombre = item.nombre,
                        descripcion = item.descripcion,
                        hora_inicio = item.horaInicio,
                        hora_termino = item.horaTermino,
                        dia_semana = item.diaSemana
                    });
                }

                ViewBag.Lista = Model;
                if (ViewBag.Lista != null)
                {
                    return View("TalleresTomados");
                }
                else
                {

                    return View("TalleresTomados");
                }
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }
        }
    }
}