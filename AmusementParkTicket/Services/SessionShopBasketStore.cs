using System.Text.Json;
using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;

namespace AmusementPark.wb.Services
{
  
    /// پیاده‌سازی ذخیره سبد خرید با استفاده از ASP.NET Session.
    ///
    /// این کلاس در Presentation قرار دارد زیرا Session
    /// یک مفهوم مربوط به HTTP و Web است.
  
    public class SessionShopBasketStore
        : IShopBasketStore
    {
        private const string BasketSessionKey =
            "ShopBasket";

        private readonly IHttpContextAccessor
            _httpContextAccessor;


        public SessionShopBasketStore(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor =
                httpContextAccessor;
        }


        private ISession Session
        {
            get
            {
                HttpContext? httpContext =
                    _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    throw new InvalidOperationException(
                        "HTTP context is not available.");
                }

                return httpContext.Session;
            }
        }


        public List<ShopBasketItemDto> GetItems()
        {
            string? json =
                Session.GetString(
                    BasketSessionKey);


            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<ShopBasketItemDto>();
            }


            return JsonSerializer
                       .Deserialize<
                           List<ShopBasketItemDto>>(
                           json)
                   ?? new List<ShopBasketItemDto>();
        }


        public void SaveItems(
            List<ShopBasketItemDto> items)
        {
            string json =
                JsonSerializer.Serialize(items);


            Session.SetString(
                BasketSessionKey,
                json);
        }


        public void Clear()
        {
            Session.Remove(
                BasketSessionKey);
        }
    }
}