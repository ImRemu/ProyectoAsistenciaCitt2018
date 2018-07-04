using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp_MVC.Models;

namespace WebApp_MVC.Controllers
{
    public class MailController : Controller
    {

        public ActionResult Confirmar(string identificadorRegistro)
        {
            if (string.IsNullOrEmpty(identificadorRegistro))
            {
                // retornar a algun lado
                TempData["ModalMessage"] = "No puedes ingresar a esta pagina directamente! Te hemos redireccionado al Inicio";
                return RedirectToAction("Index", "Home");
            }
            try
            {
                Guid guid = Guid.Parse(identificadorRegistro);
                using (Models.SatcEntities entity = new SatcEntities())
                {
                    //var lista = entity.confirmacion.ToList();
                    //todo buscar un metodo mas elegante de hacer esta consulta
                    confirmacion confirmacion = entity.confirmacion.FirstOrDefault(
                        c => c.guid.Equals(guid));
                    alumno al = confirmacion.alumno.FirstOrDefault();
                    if (confirmacion == null || !confirmacion.habilitado)
                    {
                        throw new Exception("El enlace es invalido");
                    }
                    if (al == null)
                    {
                        throw new Exception("No existe un alumno con el correo establecido");
                    }

                    switch ((TipoConfirmacion)confirmacion.tipo)
                    {
                        case TipoConfirmacion.ConfirmacionMail:
                            al.habilitado = 1;
                            confirmacion.habilitado = false;
                            entity.SaveChanges();
                            TempData["ModalMessage"] = "Has confirmado tu cuenta correctamente. Ingresa tus credenciales para comenzar.";
                            return RedirectToAction("Index", "Account");
                        case TipoConfirmacion.Contrasena:
                            Session["usuario"] = new ModeloUsuario
                            {
                                apellidos = al.apellido,
                                nombre = al.nombre,
                                correo = al.correo,
                                id_usuario = al.id_alumno,
                                password = al.password,
                                tipo_usuario = "a"
                            };
                            return RedirectToAction("CambiarContrasena","Home");

                        case TipoConfirmacion.DesactivacionCuenta:
                            throw new NotImplementedException("Falta controlar las desactivaciones de cuentas");
                        default:
                            throw new Exception("Excepcion al recuperar el tipo de confirmacion de la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Ha ocurrido una excepcion.");
                System.Diagnostics.Trace.WriteLine(ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                ViewBag.ModalMessage = ex.Message;
                return View("Error");

            }

        }
        /// <summary>
        /// Crea un codigo de confirmacion o cambio de contraseña para la cuenta especificada
        /// </summary>
        /// <param name="txtMail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CrearMail(string txtMail, TipoConfirmacion tipo)
        {
            var telemetry = new TelemetryClient();
            try
            {
                Guid id = Guid.NewGuid();
                string url = "http://" + this.Request.Url.Authority + "/Confirmar/";
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Enviando mail con URL {url + id}");

                if (!FuncionesEmail.IsValidEmail(txtMail))
                {
                    throw new ArgumentException("Email");
                }

                using (Models.SatcEntities entity = new SatcEntities())
                {
                    alumno a = entity.alumno.FirstOrDefault(alumnoEnDb => alumnoEnDb.correo.Equals(txtMail));
                    if (a == null)
                    {
                        throw new ArgumentOutOfRangeException("No hay alumnos con ese correo registrados");
                    }
                    var c = new confirmacion
                    {
                        tipo = (int)tipo,
                        fecha = DateTime.Now,
                        guid = id,
                        alumno = new List<alumno>() { a },
                        habilitado = true,

                    };
                    entity.confirmacion.Add(c);
                    bool tarea = false;
                    Exception ex = null;
                    switch (tipo)
                    {

                        case TipoConfirmacion.NO_ESPECIFICADO:
                            ex = new Exception("Tipo de mail para enviar no especificado");
                            goto default;
                        case TipoConfirmacion.ConfirmacionMail:
                            tarea = await Librerias.MailClient.EnviarMensajeRegistro(txtMail, url, id.ToString());
                            break;
                        case TipoConfirmacion.DesactivacionCuenta:
                            ex = new NotImplementedException("tipo de mail 'desactivarCuenta' no implementado");
                            goto default;

                        case TipoConfirmacion.Contrasena:
                            tarea = await Librerias.MailClient.EnviarMensajeContrasena(txtMail, url, id.ToString());
                            break;
                        default:
                            telemetry.TrackException(ex, new Dictionary<string, string> { { "Controlador: ", "MailController" } });
                            throw ex;

                    }
                    if (!tarea)
                    {
                        throw new Exception("Error al enviar el mail al correo especificado");
                    }

                    entity.SaveChanges();
                }
                ViewBag.TipoMail = (int)tipo;
                return View("EstadoSolicitud");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Ha ocurrido una excepcion.");
                System.Diagnostics.Trace.WriteLine(ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                ViewBag.ModalMessage = ex.Message;

                telemetry.TrackException(ex, new Dictionary<string, string> { { "Controlador: ", "MailController" } });

                return View("Error");
                //throw ex;
            }
        }

        /// <summary>
        /// crea un codigo de confirmacion para el cambio de contraseña
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CrearMailCambio(string txtMail)
        {
            throw new NotImplementedException("Modulo cambio contraseña mail");
            if (!FuncionesEmail.IsValidEmail(txtMail))
            {
                throw new ArgumentException("Email");
            }
            using (Models.SatcEntities entity = new SatcEntities())
            {
                return null;
            }
        }
    }
    public enum TipoConfirmacion
    {
        NO_ESPECIFICADO = 0,
        ConfirmacionMail = 1,
        DesactivacionCuenta = 2,
        Contrasena = 3,
    }
    public static class FuncionesEmail
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}