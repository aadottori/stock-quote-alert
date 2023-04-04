# stockQuoteAlert
O stockQuoteAlert é um programa de linha de comando em C# que monitora o preço de uma ação em tempo real e notifica o usuário por e-mail quando o preço da ação atinge um valor de referência de venda ou compra.

Para executar o programa, dentro da pasta StockQuoteAlert deve-se utilizar o comando:
```
dotnet build
dotnet run STOCK_SYMBOL SELL_PRICE BUY_PRICE
```


Podem ser monitorados um ou mais ativos:
```
dotnet run PETR4 22.50 22.40
dotnet run PETR4 22.50 22.40 ABEV3 14.45 14.30
```


