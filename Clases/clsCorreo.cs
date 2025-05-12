using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace API_CAFETAL.Clases
{
    public class clsCorreo
    {
        public async Task<bool> EnviarCorreoVerificacionAsync(string correoDestino, string codigoVerificacion)
        {
            try
            {

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ElCafetal110@gmail.com", "vduk hnyz qnap yfcu"),
                    EnableSsl = true,
                };


                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ElCafetal110@gmail.com"),
                    Subject = "Verificación de Correo",
                    Body = $"Tu código de verificación es: {codigoVerificacion}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(correoDestino);


                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                return false;
            }
        }

    }
}