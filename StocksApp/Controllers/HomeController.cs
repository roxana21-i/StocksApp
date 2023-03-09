using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;

        public HomeController(FinnhubService finnhubService, IOptions<TradingOptions> options, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _tradingOptions = options;
            _configuration = configuration;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            Dictionary<string, object>? responseDictionaryStock = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            Dictionary<string, object>? responseDictionaryProfile = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionaryStock["c"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionaryStock["l"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionaryStock["h"].ToString()),
                OpenPrice = Convert.ToDouble(responseDictionaryStock["o"].ToString())
            };

            ViewBag.CompanyName = responseDictionaryProfile["name"].ToString();
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            ViewBag.CurrentTime = DateTime.Now.ToString("h:mm:ss tt");

            return View(stock);
        }
    }
}
