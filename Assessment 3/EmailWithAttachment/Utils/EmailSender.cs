using EmailWithAttachment.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EmailWithAttachment.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(SendEmailViewModel model, String fullPath)
        {
            string fileName = Path.GetFileName(model.AttachedFile.FileName);

            var bytes = System.IO.File.ReadAllBytes(fullPath);
            var file = Convert.ToBase64String(bytes);
            var client = new SendGridClient(API_KEY);

            var from = new EmailAddress("noreply@localhost.com", "FIT5032 Example Email User");
            //var to = new EmailAddress(toEmail, "");

            var to_emails = new List<EmailAddress>
            {
                new EmailAddress(model.ToEmail, "Example User1")
            };


            var msg = new SendGridMessage();
            msg.SetFrom("noreply@localhost.com", "FIT5032 Assessment3");
            msg.PlainTextContent = model.Contents;
            msg.Subject = model.Subject;
            msg.AddAttachment(model.AttachedFile.FileName, file);
            msg.AddTos(to_emails);
            var response = client.SendEmailAsync(msg);
        }
    }
}