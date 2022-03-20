using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace RemskladMailer
{
    public class Mailer
    {
        public static async Task SendEmailWithCSVReportAsync(string message= "", string reportPath ="C:\\RemskladReports\\Report.csv",string subject ="")
        {
            MailAddress from = new MailAddress("remskladgreatsteve@gmail.com", "Уведомление от Remsklad");
            //MailAddress to = new MailAddress("alfimova.irina2015@yandex.ru"); 
            MailAddress to = new MailAddress("rewq95kso@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Остатки на складе(<4) :" + subject;
            m.Body = message;
            m.Attachments.Add(new Attachment(reportPath));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("remskladgreatsteve@gmail.com", "RemskladDesktop");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }

        public static async Task SendEmailWithHTMLReportAsync(string message = "", string reportPath = "C:\\RemskladReports\\Report.html", string subject = "")
        {
            MailAddress from = new MailAddress("remskladgreatsteve@gmail.com", "Уведомление от Remsklad");
            //MailAddress to = new MailAddress("alfimova.irina2015@yandex.ru");
            MailAddress to = new MailAddress("rewq95kso@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Остатки на складе(<4) :" + subject;
            m.Body = message;
            m.Attachments.Add(new Attachment(reportPath));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("remskladgreatsteve@gmail.com", "RemskladDesktop");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }


    }
}
