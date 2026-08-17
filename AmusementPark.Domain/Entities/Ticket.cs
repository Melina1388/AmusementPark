

namespace AmusementPark.Domain.Entities
{
    public class Ticket
    {
        public int? TicketID { get; set; }
        public int? PlayerID { get; set; }
        public int? GameID { get; set; }
        public int? TransactionID { get; set; }
        public string? IsUsed { get; set; }



    }
}
