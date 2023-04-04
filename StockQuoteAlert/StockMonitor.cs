using System;
using stockQuoteAlert;

namespace stockQuoteAlert
{
    public class StockMonitor
    {
        private readonly string _stockSymbol;
        private readonly double _sellPrice;
        private readonly double _buyPrice;
        private readonly Configuration _config;
        private readonly IStockQuoteApi _stockQuoteApi;


        public StockMonitor(string stockSymbol, double sellPrice, double buyPrice, Configuration config, IStockQuoteApi stockQuoteApi)
        {
            _stockSymbol = stockSymbol;
            _sellPrice = sellPrice;
            _buyPrice = buyPrice;
            _config = config;
            _stockQuoteApi = stockQuoteApi;
        }

        public async Task StartMonitoring()
        {
            while (true)
            {
                double currentPrice = await _stockQuoteApi.GetStockQuote($"{_stockSymbol}.SA");
                Console.WriteLine($"[{DateTime.Now}] {_stockSymbol}: {currentPrice}");

                string action = PriceComparer.ComparePrice(currentPrice, _sellPrice, _buyPrice);

                EmailSender sender = new EmailSender(_config.emailFrom, _config.emailTo, _config.smtpPort, _config.smtpServer, _config.smtpPassword);

                if (action == "Sell")
                {
                    sender.Send($"Alerta de venda - {_stockSymbol}",
                                     $"O preço de R${CurrencyFormatter.FormatCurrency(currentPrice)} está acima do preço de venda de referência R${CurrencyFormatter.FormatCurrency(_sellPrice)}.");
                }
                else if (action == "Buy")
                {
                    sender.Send($"Alerta de compra - {_stockSymbol}",
                                          $"O preço de R${CurrencyFormatter.FormatCurrency(currentPrice)} está abaixo do preço de compra de referência R${CurrencyFormatter.FormatCurrency(_buyPrice)}.");
                }

                await Task.Delay(5000);
            }
        }
    }
}

