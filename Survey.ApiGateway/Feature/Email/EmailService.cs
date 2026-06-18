using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.Feature.User;
using System.Net.Mail;

namespace Survey.ApiGateway.Feature.Email
{
    public class EmailService(SurveyDbContext context)
    {
        public async Task<bool> CreatePasswordResetEmail(string toEmail)
        {
            var verifyEmail = await context.Users.FirstOrDefaultAsync(u => u.Email == toEmail);
            if (verifyEmail == null) 
            { 
                return false;
            }

            string fromEmail = "noreply@schulfunk-bkbeckum.de";

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.Subject = "Passwort zurücksetzen";
            mailMessage.Body = "Hier ist der Link zum Zurücksetzen Ihres Passworts: https://schulfunk-bkbeckum.de/reset-password?email=" + toEmail;

            // 172.17.0.1 ist unter Linux IMMER die IP des echten Servers aus Docker heraus!
            using (SmtpClient smtpClient = new SmtpClient("172.19.0.1", 25))
            {
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("E-Mail erfolgreich gesendet!");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Senden: {ex.Message}");
                    throw;
                    return false;
                }
            }
        }
    }
}
