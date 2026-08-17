namespace AmusementPark.wb.ViewModels
{
    public class TicketViewModel
    {
        public int TicketID { get; set; }

        public int GameID { get; set; }

        public int TransactionID { get; set; }

        public string GameName { get; set; } = "";

        public string AmusementName { get; set; } = "";

        public decimal GamePrice { get; set; }

        public string TrackingNum { get; set; } = "";

        public string IsUsed { get; set; } = "";
    }
}