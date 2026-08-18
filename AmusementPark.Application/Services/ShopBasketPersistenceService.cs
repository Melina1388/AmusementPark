using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    /// <summary>
    /// سرویس ثبت سبد خرید در پایگاه داده.
    ///
    /// این کلاس مسئول هماهنگ کردن اطلاعات سبد خرید
    /// با Repository مربوط به پایگاه داده است.
    /// </summary>
    public class ShopBasketPersistenceService
        : IShopBasketPersistenceService
    {
        private readonly IShopBasketRepository
            _shopBasketRepository;

        private readonly IGameService
            _gameService;

        public ShopBasketPersistenceService(
            IShopBasketRepository shopBasketRepository,
            IGameService gameService)
        {
            _shopBasketRepository =
                shopBasketRepository;

            _gameService =
                gameService;
        }

        public void SaveBasket(
            int playerId,
            IEnumerable<ShopBasketItemDto> items)
        {
            if (playerId <= 0)
            {
                throw new ArgumentException(
                    "Player ID is invalid.",
                    nameof(playerId));
            }

            if (items == null)
            {
                throw new ArgumentNullException(
                    nameof(items));
            }

            List<ShopBasket> basketItems = new();
            
            foreach (ShopBasketItemDto item in items)
            {
                if (item.GameID <= 0 ||
                    item.Quantity <= 0)
                {
                    continue;
                }

                // اطلاعات بازی دوباره از منبع اصلی خوانده می‌شود
                // تا قیمت یا نام موجود در Session مبنای نهایی نباشد.

                Game? game = _gameService.GetById(item.GameID);

                if (game == null)
                {
                    throw new InvalidOperationException(
                        $"Game with ID {item.GameID} was not found.");
                }

                decimal gamePrice =
                    game.GamePrice ?? 0;

                if (gamePrice <= 0)
                {
                    throw new InvalidOperationException(
                        $"Game with ID {item.GameID} has an invalid price.");
                }

                basketItems.Add(
                    new ShopBasket
                    {
                        PlayerID =
                            playerId,

                        GameID =
                            item.GameID,

                        AmusementName =
                            game.AmusementName,

                        GameName =
                            game.GameName,

                        GamePrice =
                            gamePrice,

                        Quantity =
                            item.Quantity
                    });
            }

            if (basketItems.Count == 0)
            {
                throw new InvalidOperationException(
                    "Shopping basket is empty.");
            }

            _shopBasketRepository
                .ReplacePlayerBasket(
                    playerId,
                    basketItems);
        }

        public void ClearBasket(
            int playerId)
        {
            if (playerId <= 0)
            {
                return;
            }

            _shopBasketRepository
                .ClearPlayerBasket(
                    playerId);
        }
    }
}
