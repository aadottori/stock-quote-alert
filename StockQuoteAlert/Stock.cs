using System;
namespace stockQuoteAlert
{
    public class Stock
    {
        public string Name { get; set; }
        public double SellPrice { get; set; }
        public double BuyPrice { get; set; }

        public Stock(string name, double sellPrice, double buyPrice)
        {
            Name = name;
            SellPrice = sellPrice;
            BuyPrice = buyPrice;
        }
    }
}

