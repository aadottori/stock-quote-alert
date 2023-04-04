using System;
namespace stockQuoteAlert
{
    public static class PriceComparer
    {
        public static string ComparePrice(double currentPrice, double sellPrice, double buyPrice)
        {
            return currentPrice switch
            {
                _ when currentPrice >= sellPrice => "Sell",
                _ when currentPrice <= buyPrice => "Buy",
                _ => "Wait"
            };
        }
    }
}

