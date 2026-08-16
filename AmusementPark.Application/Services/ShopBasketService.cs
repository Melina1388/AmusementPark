
using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;

namespace AmusementPark.Application.Services
{
    /// <summary>
    /// سرویس مدیریت سبد خرید.
    ///
    /// این کلاس فقط منطق سبد خرید را مدیریت می‌کند
    /// و هیچ اطلاعی از Session، HTTP یا MVC ندارد.
    /// </summary>
    public class ShopBasketService : IShopBasketService
    {
        private readonly IShopBasketStore _basketStore;

        public ShopBasketService(
            IShopBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        public bool ContainsGame(
    int gameId)
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();

            return basket.Any(
                x => x.GameID == gameId);
        }
        public List<ShopBasketItemDto> GetBasket()
        {
            return _basketStore.GetItems();
        }


        public void AddItem(
            ShopBasketItemDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(
                    nameof(item));
            }

            if (item.GameID <= 0)
            {
                throw new ArgumentException(
                    "Game ID is invalid.");
            }

            if (item.GamePrice < 0)
            {
                throw new ArgumentException(
                    "Game price cannot be negative.");
            }

            if (item.Quantity <= 0)
            {
                item.Quantity = 1;
            }


            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();


            ShopBasketItemDto? existingItem =
                basket.FirstOrDefault(
                    x => x.GameID == item.GameID);


            if (existingItem != null)
            {
                existingItem.Quantity +=
                    item.Quantity;
            }
            else
            {
                basket.Add(item);
            }


            _basketStore.SaveItems(basket);
        }


        public void IncreaseQuantity(
            int gameId)
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();


            ShopBasketItemDto? item =
                basket.FirstOrDefault(
                    x => x.GameID == gameId);


            if (item == null)
            {
                return;
            }


            item.Quantity++;

            _basketStore.SaveItems(basket);
        }


        public void DecreaseQuantity(
            int gameId)
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();


            ShopBasketItemDto? item =
                basket.FirstOrDefault(
                    x => x.GameID == gameId);


            if (item == null)
            {
                return;
            }


            item.Quantity--;


            if (item.Quantity <= 0)
            {
                basket.Remove(item);
            }


            _basketStore.SaveItems(basket);
        }


        public void RemoveItem(
            int gameId)
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();


            basket.RemoveAll(
                x => x.GameID == gameId);


            _basketStore.SaveItems(basket);
        }


        public decimal GetTotal()
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();


            return basket.Sum(
                x => x.TotalPrice);
        }


        public void Clear()
        {
            _basketStore.Clear();
        }
    }
}