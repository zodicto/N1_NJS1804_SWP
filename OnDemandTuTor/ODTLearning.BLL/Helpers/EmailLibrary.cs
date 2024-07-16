using System.Net;
using System.Net.Mail;

namespace ODTLearning.BLL.Helpers
{
    public class EmailLibrary
    {
        private static string email = "odtlearning@gmail.com";
        private static string password = "ugdo ohgv gual onet";

        public async Task<bool> SendMail(string name, string subject, string content, string toMail)
        {
            try
            {
                MailMessage message = new MailMessage();

                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(email, password);
                    smtp.Timeout = 20000;
                }

                var fromAddress = new MailAddress(email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
