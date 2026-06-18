using System.Net.Mail;

namespace Survey.ApiGateway.Feature.Email
{
    public class EmailService
    {
        public void CreatePasswordResetEmail(string toEmail)
        {
            // ACHTUNG: Hier fehlte am Ende das "m" bei bkbeckum.de!
            string fromEmail = "noreply@schulfunk-bkbeckum.de";

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.Subject = "Passwort zurücksetzen";

            mailMessage.Body = "Hier ist der Link zum Zurücksetzen Ihres Passworts: https://schulfunk-bkbeckum.de/reset-password?email=" + toEmail;

            SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25);

            smtpClient.UseDefaultCredentials = true;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("E-Mail erfolgreich an Postfix übergeben!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der E-Mail: {ex.Message}");
                throw;
            }
        }
    }
}
