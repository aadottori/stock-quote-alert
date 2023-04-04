using System;
namespace stockQuoteAlert
{
	public class CurrencyFormatter
	{
        public static string FormatCurrency(double Price)
        {
            return Price.ToString("N2").Replace(".", ",");
        }
	}
}

