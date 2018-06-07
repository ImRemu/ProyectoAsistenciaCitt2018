using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace Librerias
{
    public class MailClient
    {
        const string API_KEY = "***REMOVED***";
        public MailClient()
        {

        }

        // revisa https://github.com/sendgrid/sendgrid-csharp#how-to-create-an-email
        static async Task SendMail()
        {
            var client = new SendGrid.SendGridClient(API_KEY);
            var from = new EmailAddress("jorgito@citt.cl", "Jorjito");
            var subject = "Este es un mail de prueba";
            var to = new EmailAddress("j.riverat@alumnos.duoc.cl");
        }

        public static async Task EjemploAPI()
        {

            var apiKey = API_KEY;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jorge@jorge.com", "joje");
            var subject = "probando la jhuea";
            var to = new EmailAddress("jo.manriqueza@alumnos.duoc.cl");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>Toy probando la cosa AAAAAAAAA</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            System.Diagnostics.Trace.WriteLine(response.StatusCode.ToString());
        }

    }
}
