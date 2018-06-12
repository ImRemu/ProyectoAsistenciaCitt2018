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
                return View("IngresarCorreo");
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
                    al.habilitado = 1;
                    confirmacion.habilitado = false;
                    entity.SaveChanges();
                    TempData["ModalMessage"] = "Has confirmado tu cuenta correctamente. Ingresa tus credenciales para comenzar.";
                    return RedirectToAction("Index", "Account");
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
        /// Crea un codigo de confirmacion para la cuenta especificada
        /// </summary>
        /// <param name="txtMail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CrearIdMail(string txtMail)
        {
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
                    entity.confirmacion.Add(new confirmacion
                    {
                        tipo = (int)TipoConfirmacion.ConfirmacionMail,
                        fecha = DateTime.Now,
                        guid = id,
                        alumno = new List<alumno>() { a },
                        habilitado = true,

                    });
                    var tarea = await Librerias.MailClient.EnviarMensajeRegistro(txtMail, url, id.ToString());

                    if (!tarea)
                    {
                        throw new Exception("Error al enviar el mail al correo especificado");
                    }

                    entity.SaveChanges();
                }
                return View("Confirmar");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Ha ocurrido una excepcion.");
                System.Diagnostics.Trace.WriteLine(ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                ViewBag.ModalMessage = ex.Message;
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