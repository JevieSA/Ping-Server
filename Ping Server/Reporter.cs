using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ping_Server
{
    class Reporter
    {
        public async Task sendReport(string name)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("jeblight", "");

            MailMessage mail = new MailMessage("noreply@jeblight.co.za", "jblight@hatfieldcs.co.za",
                "Server Offline", "The following server is offline: " + name);
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            await client.SendMailAsync(mail);
        }

    }
}
