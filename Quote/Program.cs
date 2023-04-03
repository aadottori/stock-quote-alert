using System;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.Json;
using System.Threading;
//using Newtonsoft.Json;

class Program
{

    static  async Task Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Erro: informe o ativo a ser monitorado, o preço de referência para venda e o preço de referência para compra como parâmetros da linha de comando.");
            return;
        }
        string stockSymbol = $"{args[0]}.SA";
        double sellPrice = double.Parse(args[1]);
        double buyPrice = double.Parse(args[2]);

        // Ler as configurações do arquivo de configuração
        try
        {
            Configuration configuration = ReadConfiguration();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler arquivo de configuração: {ex.Message}");
            return;
        }

        Configuration config = ReadConfiguration();


        // Monitorar a cotação do ativo em loop até ser parado
        while (true)
        {
            //Verifica a cotação
            double currentPrice = GetStockQuote(stockSymbol);
            Console.WriteLine($"[{DateTime.Now}] {stockSymbol}: {currentPrice}");

            //Compara os preços e dispara ou não o email.
            string action = comparePrice(stockSymbol, currentPrice, sellPrice, buyPrice);

            if (action == "Sell")
            {
                SendEmail($"Alerta de venda - {stockSymbol}",
                          $"O preço de {currentPrice:C} está acima do valor de venda de {sellPrice:C}.",
                          config.EmailFrom,
                          config.EmailTo,
                          config.SmtpPort,
                          config.SmtpServer,
                          config.SmtpPassword);
            }
            else if (action == "Buy")
            {
                SendEmail($"Alerta de compra - {stockSymbol}",
                          $"O preço de {currentPrice:C} está abaixo do valor de compra de {buyPrice:C}.",
                          config.EmailFrom,
                          config.EmailTo,
                          config.SmtpPort,
                          config.SmtpServer,
                          config.SmtpPassword);
            };

            // Aguardar 5 segundos antes de verificar novamente a cotação
            Thread.Sleep(5000);
        }
    }

    static double GetStockQuote(string stockSymbol)
    {
        // Definir URL da API do Yahoo Finance para a cotação do ativo
        string url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={stockSymbol}";

        // Fazer requisição HTTP para obter a cotação
        string responseBody = null;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            responseBody = reader.ReadToEnd();
        }

        dynamic json = JsonSerializer.Deserialize<JsonElement>(responseBody);
        double currentPrice = json.GetProperty("quoteResponse")
                                  .GetProperty("result")[0]
                                  .GetProperty("regularMarketPrice")
                                  .GetDouble();

        return currentPrice;
    }




    static string comparePrice(string stockSymbol, double currentPrice, double sellPrice, double buyPrice)
    {
        if (currentPrice >= sellPrice)
        {
            return "Sell";
        }
        if (currentPrice <= buyPrice)
        {
            return "Buy";
        }
        else
        {
            return "Wait";
        }
    }


    static void SendEmail(string Subject,
                                 string Body,
                                 string EmailFrom,
                                 string EmailTo,
                                 int SmtpPort,
                                 string SmtpServer,
                                 string SmtpPassword)
    {
        MailAddress from = new MailAddress(EmailFrom);
        MailAddress to = new MailAddress(EmailTo);

        MailMessage message = new MailMessage(from, to);
        message.Subject = Subject;
        message.Body = Body;

        SmtpClient client = new SmtpClient(SmtpServer);

        // Include credentials if the server requires them.
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(EmailFrom, SmtpPassword);
        client.EnableSsl = true;

        try
        {
            client.Send(message);
            Console.WriteLine("Email enviado.");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Não foi possível criar a mensagem: {ex.ToString()}");
        }
    }


    public class Configuration
    {
        public string EmailTo { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpPassword { get; set; }
    }


    public static Configuration ReadConfiguration()
    {
        string jsonString = File.ReadAllText("config.json");
        Configuration config = System.Text.Json.JsonSerializer.Deserialize<Configuration>(jsonString);
        return config;
    }

}