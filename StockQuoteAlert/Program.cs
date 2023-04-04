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
            ConfigurationReader configurationReader = new ConfigurationReader();
            Configuration config = configurationReader.ReadConfiguration();

            IStockQuoteApi stockQuoteApi = new YahooFinanceApi();

            List<Task> monitoringTasks = new List<Task>();

            if (args.Length < 3 || args.Length%3 != 0)
            {
                Console.WriteLine("As ações devem ser informadas como por exemplo: PETR4 22.56 22.40 ABEV3 14.50 14.30");
                return;
            }

            for (int i=0; i<args.Length/3; i++)
            {
                string stockSymbol = args[3*i];
                double sellPrice = double.Parse(args[3*i+1]);
                double buyPrice = double.Parse(args[3*i+2]);
                Stock stock = new Stock(stockSymbol, sellPrice, buyPrice);

                StockMonitor stockMonitor = new StockMonitor(stockSymbol, sellPrice, buyPrice, config, stockQuoteApi);
                monitoringTasks.Add(stockMonitor.StartMonitoring());

            }
            await Task.WhenAll(monitoringTasks);
        }
    }
}
