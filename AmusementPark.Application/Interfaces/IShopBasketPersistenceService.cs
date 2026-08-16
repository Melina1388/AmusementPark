using AmusementPark.Application.DTOs;

namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// مسئول ثبت سبد خرید فعلی کاربر در پایگاه داده.
    /// </summary>
    public interface IShopBasketPersistenceService
    {
        void SaveBasket(
            int playerId,
            IEnumerable<ShopBasketItemDto> items);

        void ClearBasket(
            int playerId);
    }
}