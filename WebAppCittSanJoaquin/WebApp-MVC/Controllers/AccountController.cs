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
        SatcEntities dtb = new SatcEntities();

        // El index se accesa cuando no se
        // tiene ningun parametro cuando se acchgede al controlador
        public ActionResult Index()
        {
            if (TempData["ModalMessage"]!= null)
            {
                ViewBag.ModalMessage = TempData["ModalMessage"].ToString();
                TempData.Remove("ModalMessage");
            }

            if(Session["user"] != null)
            {
                return RedirectToAction("redirectExito");
            }
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

        public ActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Registrarse(string txt_nombre,
            string txt_apell, string txt_email, string txt_pass )
        {
            log_acciones log = new log_acciones();

            if (!FuncionesEmail.IsValidEmail(txt_email))
            {
                throw new Exception("Email invalido");
            }

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
                    alumno userAl = new alumno
                    {
                        nombre = txt_nombre,
                        apellido = txt_apell,
                        correo = txt_email,
                        password = txt_pass,
                        habilitado = 0
                    };
                    #region confirmacion email
                    Guid guid = Guid.NewGuid();

                    string url = "http://" + this.Request.Url.Authority + "/Confirmar/";
                    System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Enviando mail con URL {url + guid}");

                    confirmacion conf = new confirmacion
                    {
                        alumno = new List<alumno>() { userAl },
                        fecha = DateTime.Now,
                        guid = guid,
                        habilitado = true,
                        tipo = (int)TipoConfirmacion.ConfirmacionMail,
                    };
                    var tarea = await Librerias.MailClient.EnviarMensajeRegistro(txt_email, url, guid.ToString().ToUpper());

                    if (!tarea)
                    {
                        throw new Exception("Error al enviar el mail al correo especificado");
                    }

                    

                    #endregion


                    //se guardan los cambios en la base de datos.
                    dtb.alumno.Add(userAl);
                    dtb.confirmacion.Add(conf);

                    dtb.log_acciones.Add(log);
                    dtb.SaveChanges();
                    //se retorna el mensaje correspondiente.
                    TempData["creado"] = "El Usuario se ha registrado exitosamente.";
                    ViewBag.Registrado = TempData["creado"];
                    ViewBag.ModalMessage = "Verifica tu cuenta haciendo click en el vinculo enviado a tu correo electronico<br>" +
                        "¡Asegúrate de revisar la carpeta de Spam!";
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
                                 diaSemana = h.dia_semana,
                                 id = t.id_taller

                             }).ToList();
                foreach (var item in lista)
                {
                    Model.Add(new ModeloTalleresTomados()
                    {
                        nombre = item.nombre,
                        descripcion = item.descripcion,
                        hora_inicio = item.horaInicio,
                        hora_termino = item.horaTermino,
                        dia_semana = item.diaSemana,
                        id = item.id
                    });
                }

                ViewBag.Lista = Model;
                if (ViewBag.Lista != null)
                {
                    return View("talleresACargo");
                }
                else
                {

                    return View("talleresACargo");
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

        public ActionResult OperacionesTalleres()
        {
            if (Session["user"] != null && ((ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
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
                    return View("OperacionesTalleres");
                }
            }
            else if(Session["user"] != null && ((ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectExito");
            }
            else if (Session["user"] != null && ((ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectExito");
            }
            else
            {
                return View("Index");
            }



                return View();
            
        }

        public ActionResult eliminarTaller(int id)
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                var asistencias = (from a in dtb.asistencia
                                   join dt in dtb.det_asist on a.id_asistencia equals dt.asistencia_id_asistencia
                                   join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                                   join t in dtb.taller on h.taller_id_taller equals t.id_taller
                                   where t.id_taller == id
                                   select new
                                   {
                                       idAs = a.id_asistencia,
                                       idT = t.id_taller
                                   }).ToList();
                var dts = (from dt in dtb.det_asist
                           join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                           join t in dtb.taller on h.taller_id_taller equals t.id_taller
                           where t.id_taller == id
                           select new
                           {
                               idDts = dt.id_detalle
                           }).ToList();

                if (asistencias.Count() != 0)
                {
                    foreach (var asis in asistencias)
                    {

                        asistencia asist = (from ast in dtb.asistencia
                                            where ast.id_asistencia == asis.idAs
                                            select ast).FirstOrDefault();

                        taller tall = (from t in dtb.taller
                                       where t.id_taller == id
                                       select t).FirstOrDefault();

                        horario hr = (from h in dtb.horario
                                      where h.taller_id_taller == tall.id_taller
                                      select h).FirstOrDefault();

                        det_asist dt = (from dta in dtb.det_asist
                                        where dta.horario_id_horario == hr.id_horario
                                        select dta).FirstOrDefault();


                        dtb.det_asist.Remove(dt);
                        dtb.asistencia.Remove(asist);
                        dtb.horario.Remove(hr);
                        dtb.taller.Remove(tall);
                        dtb.SaveChanges();
                    }

                }
                else if (dts.Count() != 0)
                {


                    taller tall = (from t in dtb.taller
                                   where t.id_taller == id
                                   select t).FirstOrDefault();

                    horario hr = (from h in dtb.horario
                                  where h.taller_id_taller == tall.id_taller
                                  select h).FirstOrDefault();

                    det_asist dt = (from dta in dtb.det_asist
                                    where dta.horario_id_horario == hr.id_horario
                                    select dta).FirstOrDefault();

                    dtb.det_asist.Remove(dt);
                    dtb.horario.Remove(hr);
                    dtb.taller.Remove(tall);
                    dtb.SaveChanges();

                    return RedirectToAction("OperacionesTalleres");
                }
                else
                {
                    taller tall = (from t in dtb.taller
                                   where t.id_taller == id
                                   select t).FirstOrDefault();

                    horario hr = (from h in dtb.horario
                                  where h.taller_id_taller == tall.id_taller
                                  select h).FirstOrDefault();

                    dtb.horario.Remove(hr);
                    dtb.taller.Remove(tall);
                    dtb.SaveChanges();

                    return RedirectToAction("OperacionesTalleres");
                }
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }

            return RedirectToAction("OperacionesTalleres");
        }

        public ActionResult crearTaller()
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                List<modeloProfesores> Model = new List<modeloProfesores>();

                var lista = (from p in dtb.profesor
                             select p).ToList();

                foreach (var item in lista)
                {
                    Model.Add(new modeloProfesores()
                    {
                        id_profesor = item.id_profesor,
                        nombre = item.nombre + " " + item.apellidos
                    });
                }

                ViewBag.lista = Model;
                return View();
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }

            
        }

        public ActionResult creacionTaller(string txt_nombreTaller, string txt_descripcion, int? opt_encargado, TimeSpan? tm_inicio,
                                            TimeSpan? tm_termino, string opt_diaSemana, int? txt_cupos)
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A")
                && opt_encargado != null && txt_cupos != null)
            {
                taller nTaller = new taller();
                horario nHora = new horario();

                nTaller.nombre = txt_nombreTaller;
                nTaller.descripcion = txt_descripcion;
                nTaller.profesor_id_profesor = (int)opt_encargado;
                dtb.taller.Add(nTaller);

                nHora.hora_inicio = (TimeSpan)tm_inicio;
                nHora.hora_termino = (TimeSpan)tm_termino;
                nHora.dia_semana = opt_diaSemana;
                nHora.cupo = (int)txt_cupos;
                nHora.taller_id_taller = nTaller.id_taller;

                dtb.horario.Add(nHora);
                dtb.SaveChanges();

                ViewBag.exito = "taller creado exitosamente.";
                return RedirectToAction("OperacionesTalleres");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }

        }

        public ActionResult modTaller(int? id)
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A") && id != null)
            {
                List<ModeloTaller> list = new List<ModeloTaller>();
                var eTall = (from t in dtb.taller
                                            where t.id_taller == id
                                            select t).ToList();

                List<modeloProfesores> profesores = new List<modeloProfesores>();

                var lista = (from p in dtb.profesor
                             select p).ToList();

                foreach (var item in lista)
                {
                    profesores.Add(new modeloProfesores()
                    {
                        id_profesor = item.id_profesor,
                        nombre = item.nombre + " " + item.apellidos
                    });
                }

                

                foreach (var i in eTall)
                {
                    list.Add(new ModeloTaller()
                    {
                        id_taller = i.id_taller,
                        descripcion = i.descripcion,
                        nombre = i.nombre,
                        id_encargado = i.profesor_id_profesor
                    });
                }

                ViewBag.lP = profesores;
                ViewBag.lTall = list;

                return View();

            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }
            return View();
        }

        public ActionResult modificTaller(int? txt_idT, string txt_nombre, string txt_descripcion, int? opt_encargado)
        {
            if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A")
                && txt_idT != null && opt_encargado != null)
            {
                taller eTaller = (from t in dtb.taller
                                  where t.id_taller == txt_idT
                                  select t).FirstOrDefault();

                eTaller.nombre = txt_nombre;
                eTaller.descripcion = txt_descripcion;
                eTaller.profesor_id_profesor = (int)opt_encargado;

                dtb.SaveChanges();
                return RedirectToAction("OperacionesTalleres");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if (Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return View("Index");
            }
            return View();
        }

        public ActionResult tomarAsistencia(int? id)
        {
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p"))
            {
                List<ModeloUsuario> model = new List<ModeloUsuario>();
                List<ModeloTaller> mTall = new List<ModeloTaller>();
                var tall = (from t in dtb.taller
                                     where t.id_taller == id
                                     select t).ToList();

                foreach (var item in tall)
                {
                    mTall.Add(new ModeloTaller()
                    {
                        id_taller = item.id_taller,
                        nombre = item.nombre,
                        descripcion = item.descripcion

                    });
                }

                ViewBag.iTall = mTall;

                var lAl = (from a in dtb.alumno
                           join dt in dtb.det_asist on a.id_alumno equals dt.alumno_id_alumno
                           join h in dtb.horario on dt.horario_id_horario equals h.id_horario
                           join t in dtb.taller on h.taller_id_taller equals t.id_taller
                           where t.id_taller == id
                           select new
                           {
                               id = a.id_alumno,
                               nombre = a.nombre + " " + a.apellido
                           }).ToList();

                foreach (var item in lAl)
                {
                    model.Add(new ModeloUsuario()
                    {
                        id_usuario = item.id,
                        nombre = item.nombre
                    });

                }

                ViewBag.listaAl = model;
                return View("tomarAsistencia");
            }
            else if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("a"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("A"))
            {
                return RedirectToAction("redirectPerfil");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
            return View();
        }

        public ActionResult tomaAsistencia(List<int> chk_asist, int? hddn_id)
        {
            if(Session["user"] != null && ((WebApp_MVC.Models.ModeloUsuario)Session["user"]).tipo_usuario.Equals("p")
                && hddn_id != null)
            {
                foreach (int x in chk_asist)
                {
                    det_asist dtAs = new det_asist();
                    asistencia asist = new asistencia();

                    asist.fecha = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yy hh:mm:ss"));
                    dtb.asistencia.Add(asist);

                    int idHr = (from h in dtb.horario
                                where h.taller_id_taller == hddn_id
                                select h.id_horario).FirstOrDefault();

                    dtAs.alumno_id_alumno = x;
                    dtAs.asistencia_id_asistencia = asist.id_asistencia;
                    dtAs.horario_id_horario = idHr;
                    dtb.det_asist.Add(dtAs);
                    dtb.SaveChanges();
                }
                return RedirectToAction("talleresACargo");
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

        public ActionResult OperacionesHorarios()
        {
            List<modeloHorario> model = new List<modeloHorario>();

            var lH = (from h in dtb.horario
                      select h).ToList();

            foreach(var h in lH)
            {
                model.Add(new modeloHorario
                {
                    id_horario = h.id_horario,
                    cupo = h.cupo,
                    dia_semana = h.dia_semana,
                    hora_inicio = h.hora_inicio,
                    hora_termino = h.hora_termino,
                    taller_id_taller = h.taller_id_taller
                });
            }

            ViewBag.lHor = model;
            return View("OperacionesHorarios");
        }
    }
}