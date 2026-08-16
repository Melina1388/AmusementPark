using AmusementPark.Web.Services;
using AmusementPark.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AmusementPark.Domain.Entities;
using AmusementPark.Application.Interfaces;
public class HomeController : Controller
{
    private readonly APIService _apiService;

    private readonly IShopBasketService _shopBasketService;
    public HomeController(
     APIService apiService,
     IShopBasketService shopBasketService)
    {
        _apiService = apiService;
        _shopBasketService = shopBasketService;
    }

    [HttpGet]
    public async Task<IActionResult> Home(string? search)
    {
        List<Game> games = new();

        try
        {
            // -----------------------------------------
            // دریافت بازی‌ها
            // -----------------------------------------
            
            if (string.IsNullOrWhiteSpace(search))
            {
                games =
                    await _apiService.GetGamesAsync();
            }
            else
            {
                games =
                    await _apiService.SearchGamesAsync(
                        search);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("========== HOME ERROR ==========");
            Console.WriteLine(ex);
            Console.WriteLine("================================");

            throw;
        }
        // -----------------------------------------
        // دریافت تعداد بلیت‌های موجود در سبد خرید
        // -----------------------------------------

        var basket =
            _shopBasketService.GetBasket();

        var basketQuantities =
            basket.ToDictionary(
                x => x.GameID,
                x => x.Quantity);

        // -----------------------------------------
        // گروه‌بندی بازی‌ها بر اساس شهربازی
        // -----------------------------------------

        var model = new HomeViewModel
        {
            Games = games,

            Amusements = games
                .Where(g =>
                    !string.IsNullOrWhiteSpace(
                        g.AmusementName))
                .GroupBy(g =>
                    g.AmusementName!.Trim())
                .Select(g =>
                    new AmusementGroupViewModel
                    {
                        AmusementName = g.Key,

                        Games = g.ToList()
                    })
                .ToList()
        };

        Console.WriteLine($"Games Count: {games.Count}");
        Console.WriteLine($"Amusements Count: {model.Amusements.Count}");
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(
    string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return Json(
                Array.Empty<SearchSuggestionViewModel>());
        }

        var games =
            await _apiService.SearchGamesAsync(
                search.Trim());

        var suggestions =
            games
                .Select(game =>
                    new SearchSuggestionViewModel
                    {
                        GameName = game.GameName,
                        AmusementName = game.AmusementName
                    })
                .ToList();

        return Json(suggestions);
    }
}