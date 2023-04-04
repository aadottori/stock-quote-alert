using System;
using System.Net;
using System.Net.Mail;
using System.Text.Json;


namespace stockQuoteAlert
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Erro: por favor forneça o símbolo da ação para monitorar, o preço de referência de venda e o preço de referência de compra como argumentos de linha de comando.\n\n\n\n");
                return;
            }

            string stockSymbol = $"{args[0]}.SA";
            double sellPrice = double.Parse(args[1]);
            double buyPrice = double.Parse(args[2]);

            ConfigurationReader configurationReader = new ConfigurationReader();
            Configuration config = configurationReader.ReadConfiguration();

            IStockQuoteApi stockQuoteApi = new YahooFinanceApi();
            StockMonitor stockMonitor = new StockMonitor(stockSymbol, sellPrice, buyPrice, config, stockQuoteApi);
            await stockMonitor.StartMonitoring();
        }
    }
}
