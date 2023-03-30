using System;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json;

class Program
{
    static string emailRecipient;
    static string smtpServer;
    static int smtpPort;
    static string smtpUsername;
    static string smtpPassword;

    static void Main(string[] args)
    {
        string stockSymbol = args[0] + ".SA";
        double sellPrice = double.Parse(args[1]);
        double buyPrice = double.Parse(args[2]);

        // Ler as configurações do arquivo de configuração
        ReadConfiguration();

        // Monitorar a cotação do ativo em loop até ser parado
        while (true)
        {
            double currentPrice = GetStockQuote(stockSymbol);
            Console.WriteLine($"[{DateTime.Now}] {stockSymbol}: {currentPrice}");

            if (currentPrice >= sellPrice)
            {
                SendEmail($"Alerta de venda - {stockSymbol}",
                           $"O preço de {currentPrice:C} está acima do valor de venda de {sellPrice:C}.");
            }

            if (currentPrice <= buyPrice)
            {
                SendEmail($"Alerta de compra - {stockSymbol}", $"O preço de {currentPrice:C} está abaixo do valor de compra de {buyPrice:C}.");
            }

            // Aguardar 5 segundos antes de verificar novamente a cotação
            Thread.Sleep(5000);
        }
    }

    static void ReadConfiguration()
    {
        // Ler as configurações do arquivo de configuração
        string[] lines = File.ReadAllLines("config.txt");
        emailRecipient = lines[0];
        smtpServer = lines[1];
        smtpPort = int.Parse(lines[2]);
        smtpUsername = lines[3];
        smtpPassword = lines[4];
    }



    static double GetStockQuote(string stockSymbol)
    {
        // Definir URL da API do Yahoo Finance para a cotação do ativo
        string url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={stockSymbol}";

        // Fazer requisição HTTP para obter a cotação
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string responseBody = null;
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            responseBody = reader.ReadToEnd();
        }

        // Analisar a resposta JSON para obter a cotação atual
        dynamic json = JsonConvert.DeserializeObject(responseBody);
        double currentPrice = json["quoteResponse"]["result"][0]["regularMarketPrice"];


        return currentPrice;
    }


    static void SendEmail(string subject, string body)
    {
        // Configurar o cliente SMTP
        SmtpClient client = new SmtpClient(smtpServer, smtpPort);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

        // Criar a mensagem de e-mail
        MailMessage message = new MailMessage();
        message.From = new MailAddress(smtpUsername);
        message.To.Add(emailRecipient);
        message.Subject = subject;
        message.Body = body;

        // Enviar o e-mail
        client.Send(message);
        Console.WriteLine($"[{DateTime.Now}] E-mail enviado: {subject}");
    }
}