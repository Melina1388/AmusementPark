using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces
{

    /// قرارداد دسترسی به اطلاعات سبد خرید ثبت‌شده در پایگاه داده.
    /// این Interface هیچ وابستگی به Access یا MVC ندارد.

    public interface IShopBasketRepository
    {
        void ReplacePlayerBasket(
            int playerId,
            IEnumerable<ShopBasket> items);

        void ClearPlayerBasket(
            int playerId);
    }
}