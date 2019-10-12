using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.IO;
using AssignmentFIT5032.Models;

namespace AssignmentFIT5032.Utils
{
    public class BulkEmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents, BulkEmailViewModel model, String fullPath)
        {
            //BigData
            string fileName = Path.GetFileName(model.AttachedFile.FileName);

            var bytes = System.IO.File.ReadAllBytes(fullPath);
            var file = Convert.ToBase64String(bytes);
            var client = new SendGridClient(API_KEY);

            var from = new EmailAddress("noreply@localhost.com", "FIT5032 Example Email User");
            //var to = new EmailAddress(toEmail, "");

            var to_emails = new List<EmailAddress>
            {
                new EmailAddress(toEmail, "Example User1"),
                new EmailAddress("adnancse007@gmail.com", "Example User2")
            };


            var msg = new SendGridMessage();
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            msg.SetFrom("noreply@localhost.com", "FIT5032 Example Email User");
            msg.PlainTextContent = plainTextContent;
            msg.Subject = subject;
            //var msg = MailHelper.CreateSingleEmail(, subject, plainTextContent, htmlContent);
            msg.AddAttachment(model.AttachedFile.FileName, file);
            msg.AddTos(to_emails);
            var response = client.SendEmailAsync(msg);
        }
    }
}