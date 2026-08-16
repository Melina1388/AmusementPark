using AmusementPark.Application.DTOs;

namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// منطق کسب‌وکار مربوط به سبد خرید.
    /// </summary>
    public interface IShopBasketService
    {
        List<ShopBasketItemDto> GetBasket();

        void AddItem(
            ShopBasketItemDto item);

        bool ContainsGame(
            int gameId);

        void SetQuantity(
            int gameId,
            int quantity);

        void IncreaseQuantity(
            int gameId);

        void DecreaseQuantity(
            int gameId);

        void RemoveItem(
            int gameId);

        decimal GetTotal();

        void Clear();
    }
}