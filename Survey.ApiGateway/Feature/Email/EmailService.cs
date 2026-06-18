using System.Net.Mail;

namespace Survey.ApiGateway.Feature.Email
{
    public class EmailService
    {
        public void CreatePasswordResetEmail(string toEmail)
        {
            string fromEmail = "noreply@schulfunk-bkbecku.de";

            MailMessage mailMessage = new MailMessage(fromEmail,toEmail);
            mailMessage.Subject = "Passwort zurücksetzen";
            mailMessage.Body = "Hier ist der Link zum Zurücksetzen Ihres Passworts: https://schulfunk-bkbecku.de/reset-password?email=" + toEmail;
            SmtpClient smtpClient = new SmtpClient("127.0.0.0");

            smtpClient.UseDefaultCredentials = true;

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Fehler beim Senden der E-Mail: {ex.Message}");
            }
        }
    }
}
