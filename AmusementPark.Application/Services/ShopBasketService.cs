
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
                throw new ArgumentException(
                    "Quantity must be greater than zero.",
                    nameof(item.Quantity));
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

        public void SetQuantity(
    int gameId,
    int quantity)
        {
            List<ShopBasketItemDto> basket =
                _basketStore.GetItems();

            ShopBasketItemDto? item =
                basket.FirstOrDefault(
                    x => x.GameID == gameId);

            // اگر تعداد صفر یا کمتر باشد،
            // بازی باید کاملاً از سبد حذف شود.
            if (quantity <= 0)
            {
                if (item != null)
                {
                    basket.Remove(item);
                    _basketStore.SaveItems(basket);
                }

                return;
            }

            // اگر بازی قبلاً در سبد وجود دارد،
            // فقط تعداد آن را تنظیم کن.
            if (item != null)
            {
                item.Quantity = quantity;

                _basketStore.SaveItems(basket);

                return;
            }

            // اگر بازی هنوز در سبد نیست،
            // چیزی برای اضافه کردن نداریم؛
            // Controller در این حالت باید اطلاعات بازی
            // را ساخته و AddItem را صدا بزند.
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
                x => x.TotalGamePrice);
        }


        public void Clear()
        {
            _basketStore.Clear();
        }
    }
}