namespace AmusementPark.Domain.Entities
{
    public class Player
    {
        public int? PlayerID { get; set; }
        public string? PlayerName { get; set; }
        public string? PlayerMobile { get; set; }

        // false = کاربر عادی
        // true = مدیر
        public bool IsAdmin { get; set; }
    }
}
