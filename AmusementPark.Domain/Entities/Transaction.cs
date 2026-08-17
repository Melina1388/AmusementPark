
namespace AmusementPark.Domain.Entities
{
    public class Transaction
    {
        public int? TransactionID { get; set; }
        public int? PlayerID { get; set; }
        public string? CardNum { get; set; }
        public string? CardHolderName { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? TrackingNum { get; set; }

      

    }
}
