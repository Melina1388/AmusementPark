using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmusementPark.Models
{
    public class Game
    {
        public int? GameID { get; set; }
        public string? GameName { get; set; }
        public string? AmusementName { get; set; }
        public string? GamePrice { get; set; }
        public string? GameComment { get; set; }
        public string? GamePIc { get; set; }
    }
}
