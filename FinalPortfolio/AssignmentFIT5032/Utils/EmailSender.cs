using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.IO;

namespace AssignmentFIT5032.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "FIT5032 Assignment");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}