namespace AmusementPark.Domain.Entities
{
    public class ShopBasket
    {
        public int? PlayerID { get; set; }

        public int GameID { get; set; }

        public string? AmusementName { get; set; }

        public string? GameName { get; set; }

        public decimal GamePrice { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal TotalPrice =>
            GamePrice * Quantity;
    }
}