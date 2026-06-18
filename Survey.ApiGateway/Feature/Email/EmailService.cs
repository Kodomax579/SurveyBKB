using System.Net.Mail;

namespace Survey.ApiGateway.Feature.Email
{
    public class EmailService
    {
        public void CreatePasswordResetEmail(string toEmail)
        {
            string fromEmail = "noreply@schulfunk-bkbeckum.de";

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.Subject = "Passwort zurücksetzen";
            mailMessage.Body = "Hier ist der Link zum Zurücksetzen Ihres Passworts: https://schulfunk-bkbeckum.de/reset-password?email=" + toEmail;

            // Explizit IP und Port 25 angeben
            using (SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25))
            {
                // WICHTIG für lokale Postfix-Instanzen ohne SSL auf Port 25:
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("E-Mail erfolgreich an Postfix übergeben!");
                }
                catch (Exception ex)
                {
                    // Schreibt den genauen Fehler in die Server-Konsole deiner App
                    Console.WriteLine($"[EmailService Fehler] -> {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"[Innerer Fehler] -> {ex.InnerException.Message}");
                    }
                    throw; // Wirft den Fehler weiter, damit der LoginController ihn fängt
                }
            }
        }
    }
}
