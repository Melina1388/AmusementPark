namespace AmusementPark.Application.DTOs
{
 
    /// اطلاعاتی که برای نمایش یک آیتم سبد خرید
    /// مورد نیاز Application است.

    public class ShopBasketItemDto
    {
        public int GameID { get; set; }

        public string? AmusementName { get; set; }

        public string? GameName { get; set; }

        public decimal GamePrice { get; set; }

        public int Quantity { get; set; }

 
        /// قیمت کل این آیتم.
   
        public decimal TotalGamePrice =>
            GamePrice * Quantity;
    }
}