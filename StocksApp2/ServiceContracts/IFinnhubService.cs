﻿namespace StocksApp2.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string symbol);
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
