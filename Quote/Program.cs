using System;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
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
    }
};