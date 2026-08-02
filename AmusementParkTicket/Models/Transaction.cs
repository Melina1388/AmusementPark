using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmusementPark.Models
{
    public class Transaction
    {
        public int? PlayerID { get; set; }
        public string? CardNum { get; set; }
        public decimal? TotalPrice { get; set; }

        public string? TrackingNum { get; set; }
    }
}
