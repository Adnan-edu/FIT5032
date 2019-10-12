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

            var from = new EmailAddress("noreply@localhost.com", "FIT5032 Example Email User");
            string[] listOfToEmail = toEmail.Split(',');
            var toEmailsList = new List<EmailAddress>();
            for (int i = 0; i < listOfToEmail.Length; i++)
            {
                toEmailsList.Add(new EmailAddress(listOfToEmail[i], "Customer "+(i+1)));
            };

            var client = new SendGridClient(API_KEY);
            var msg = new SendGridMessage();
            var plainTextContent = contents;
            msg.SetFrom("noreply@localhost.com", "FIT5032 Example Email User");
            msg.PlainTextContent = plainTextContent;
            msg.Subject = subject;
            msg.AddAttachment(model.AttachedFile.FileName, file);
            msg.AddTos(toEmailsList);
            var response = client.SendEmailAsync(msg);
        }
    }
}