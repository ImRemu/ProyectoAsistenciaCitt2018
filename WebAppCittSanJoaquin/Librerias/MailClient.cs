using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Threading;


namespace Librerias
{
    public class MailClient
    {
        const string API_KEY = "**REMOVED**";
        const string MAIL_ENVIADOR_CORREO = "jorgito@talleres.citt.cl";
        const string NOMBRE_ENVIADOR_CORREO = "Bot Jorgito";
        const string NOMBRE_ALUMNO = "Alumno";

        const string DIRECTORIO_PLANTILLAS_CORREO = "contenidos-mail";
        const string CADENA_REEMPLAZABLE = "REEMPLAZAME";

        const string ASUNTO_CORREO_REGISTRO = "Registro talleres CITT";
        const string REGISTRO_PLANO = "registro-plano.txt";
        const string REGISTRO_HTML = "registro-optimizado.html";

        public MailClient()
        {

        }
        // revisa https://github.com/sendgrid/sendgrid-csharp#how-to-create-an-email


        private static async Task<bool> EnviarEmail
            (string email, string textoPlano, string textoHTML,
            string nombre = "Alumnito", string asunto = "")
        {
            try
            {
                var client = new SendGridClient(API_KEY);
                var enviador = new EmailAddress(MAIL_ENVIADOR_CORREO, NOMBRE_ENVIADOR_CORREO);
                var receptor = new EmailAddress(email, nombre);
                string asuntoCorreo = string.IsNullOrEmpty(asunto) ? ASUNTO_CORREO_REGISTRO : asunto;

                var correo = MailHelper.CreateSingleEmail(enviador, receptor, asuntoCorreo, textoPlano, textoHTML);
                var respuesta = await client.SendEmailAsync(correo);

                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Se envio un correo: Resultado: {respuesta.StatusCode.ToString()}");
                return true;
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.WriteLine($"[{DateTime.Now}] Error al enviar mensaje");
                return false;
            }
        }

        /// <summary>
        /// Envia el mensaje de confirmacion de registro al
        /// destinatario pasado por parametros
        /// </summary>
        /// <param name="destinatario">Una direccion de email</param>
        /// <param name="url">Link (base) que contendra el correo</param>
        ///  <param name="id">Parametro adicional URL en correo</param>
        /// <returns>Tuple que contiene informacion acerca si la operacion
        /// se concreto correctamente y ademas una url que
        /// identifica la operacion</returns>
        public static async Task<bool> EnviarMensajeRegistro(string destinatario, string url, string id)
        {
            var a = AppDomain.CurrentDomain.BaseDirectory;
            if (!a.Contains("bin" + System.IO.Path.DirectorySeparatorChar))
            {
                a += "bin" + System.IO.Path.DirectorySeparatorChar;
            }

            string textoPlano = System.IO.File.ReadAllText(
                    $"{a}" +
                    $"{DIRECTORIO_PLANTILLAS_CORREO}{System.IO.Path.DirectorySeparatorChar}" +
                    $"{REGISTRO_PLANO}");
            string textoHTML = System.IO.File.ReadAllText(
                $"{a}" +
                $"{DIRECTORIO_PLANTILLAS_CORREO}{System.IO.Path.DirectorySeparatorChar}" +
                $"{REGISTRO_HTML}");

            url += id;
            textoHTML = textoHTML.Replace(CADENA_REEMPLAZABLE, url);
            textoPlano += "\n" + url;

            return await EnviarEmail(destinatario,
                textoPlano, textoHTML, NOMBRE_ALUMNO, ASUNTO_CORREO_REGISTRO); 
        }

    }
}
