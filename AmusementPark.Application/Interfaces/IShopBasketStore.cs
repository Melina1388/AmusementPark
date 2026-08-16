using AmusementPark.Application.DTOs;

namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// مسئولیت ذخیره و بازیابی سبد خرید.
    ///
    /// Application فقط این قرارداد را می‌شناسد
    /// و نمی‌داند اطلاعات کجا ذخیره می‌شوند.
    /// </summary>
    public interface IShopBasketStore
    {
        List<ShopBasketItemDto> GetItems();

        void SaveItems(
            List<ShopBasketItemDto> items);

        void Clear();
    }
}