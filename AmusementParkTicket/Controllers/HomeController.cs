using AmusementPark.Web.Services;
using AmusementPark.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AmusementPark.Domain.Entities;
using AmusementPark.Application.Interfaces;
public class HomeController : Controller
{
    private readonly APIService _apiService;

    private readonly IShopBasketService _shopBasketService;

    private readonly IGameImageService _gameImageService;
    public HomeController(
       APIService apiService,
       IShopBasketService shopBasketService,
       IGameImageService gameImageService)
    {
        _apiService = apiService;
        _shopBasketService = shopBasketService;
        _gameImageService = gameImageService;
    }
    [HttpGet]
    public async Task<IActionResult> Home(string? search)
    {
        List<Game> games = new();

        try
        {
            // ============================================
            // 1. دریافت بازی‌ها
            // ============================================

            if (string.IsNullOrWhiteSpace(search))
            {
                games = await _apiService.GetGamesAsync();
            }
            else
            {
                games = await _apiService.SearchGamesAsync(search);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("========== HOME ERROR ==========");
            Console.WriteLine(ex);
            Console.WriteLine("================================");

            throw;
        }

        // ============================================
        // 2. تبدیل Game به GameViewModel
        // ============================================

        var gameViewModels = games
            .Select(game =>
                new GameViewModel
                {
                    GameID = game.GameID,
                    GameName = game.GameName,
                    AmusementName = game.AmusementName,
                    GamePrice = game.GamePrice,
                    GameComment = game.GameComment,

                    GameImageUrl =
                        _gameImageService.GetImageUrl(
                            game.GamePic)
                })
            .ToList();

        // ============================================
        // 3. دریافت سبد خرید فعلی
        // ============================================

        var basket = _shopBasketService.GetBasket();

        var basketQuantities = basket
            .GroupBy(x => x.GameID)
            .ToDictionary(
                x => x.Key,
                x => x.Last().Quantity
            );

        // ============================================
        // 4. ساخت HomeViewModel
        // ============================================

        var model = new HomeViewModel
        {
            Games = gameViewModels,

            BasketQuantities = basketQuantities,

            Amusements = gameViewModels
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

        // ============================================
        // Debug
        // ============================================

        Console.WriteLine(
            $"Games Count: {games.Count}");

        Console.WriteLine(
            $"GameViewModels Count: {gameViewModels.Count}");

        Console.WriteLine(
            $"Amusements Count: {model.Amusements.Count}");

        Console.WriteLine(
            $"Basket Items Count: {basket.Count}");

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