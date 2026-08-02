using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmusementPark.Models
{
    public class ShopBacket
    {
        public int? PlayerID { get; set; }

        public string? AmusementName { get; set; }
        public string? GameName { get; set; }
        public decimal? GamePrice { get; set; }
    }
}
