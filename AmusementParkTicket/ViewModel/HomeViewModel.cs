using AmusementPark.Domain.Entities;

namespace AmusementPark.Web.ViewModels
{
    public class HomeViewModel
    {
      
            public List<GameViewModel> Games { get; set; }
                = new();

            public Dictionary<int, int> BasketQuantities { get; set; }
                = new();

            public List<AmusementGroupViewModel> Amusements { get; set; }
               = new();
        
    }
}