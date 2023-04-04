using System;
using System.Net;
using System.Net.Mail;

namespace stockQuoteAlert
{
    public class EmailSender
    {
        private string _from;
        private string _to;
        private int _smtpPort;
        private string _smtpServer;
        private string _smtpPassword;

        public EmailSender(string from, string to, int smtpPort, string smtpServer, string smtpPassword)
        {
            _from = from;
            _to = to;
            _smtpPort = smtpPort;
            _smtpServer = smtpServer;
            _smtpPassword = smtpPassword;
        }

        public void Send(string subject, string body)
        {
            MailAddress from = new MailAddress(_from);
            MailAddress to = new MailAddress(_to);

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient(_smtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_from, _smtpPassword);
            client.EnableSsl = true;

            try
            {
                client.Send(message);
                Console.WriteLine($"Email enviado: {subject}.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível criar a mensagem: {ex.ToString()}");
            }
        }
    }
}

