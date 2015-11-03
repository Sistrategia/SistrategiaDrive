using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace Sistrategia.Drive.Business
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) {
            return ConfigSendGridAsync(message);
            //return ConfigSendSMTPAsync(message);
        }

        private Task ConfigSendSMTPAsync(IdentityMessage message) {
            // string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
            // string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            var myMessage = new System.Net.Mail.MailMessage();
            myMessage.To.Add(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("from", "Full Name");
            myMessage.Subject = message.Subject;
            myMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(message.Body, null, System.Net.Mime.MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(message.Body, null, System.Net.Mime.MediaTypeNames.Text.Html));

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp", Convert.ToInt32(587)); // 587            
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("from", "Sistrategia1");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Port = 587;
            // return smtpClient.SendMailAsync(myMessage);
            smtpClient.Send(myMessage);
            return Task.FromResult(0);
        }

        private Task ConfigSendGridAsync(IdentityMessage message) {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("from", "Full Name");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            //var credentials = new System.Net.NetworkCredential(
            //    "user",
            //    "password"
            //    );

            //var transportWeb = new Web(credentials); // Cambiar por APIKEY
            var transportWeb = new Web("");

            if (transportWeb != null) {
                return transportWeb.DeliverAsync(myMessage);
            }
            else {
                return Task.FromResult(0);
            }
        }
    }
}
