using System;
using System.Text.Json;

namespace stockQuoteAlert
{
    public interface IStockQuoteApi
    {
        Task<double> GetStockQuote(string stockSymbol);
    }


    public class YahooFinanceApi : IStockQuoteApi
    {
        public async Task<double> GetStockQuote(string stockSymbol)
        {
            string url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={stockSymbol}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                var json = JsonDocument.Parse(responseBody);
                double? currentPrice = json.RootElement
                                        .GetProperty("quoteResponse")
                                        .GetProperty("result")[0]
                                        .GetProperty("regularMarketPrice")
                                        .GetDouble();

                return currentPrice ?? throw new Exception("Price not found.");
            }
        }
    }
}
