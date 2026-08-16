using AmusementPark.Domain.Entities;

namespace AmusementPark.Web.ViewModels
{
    public class HomeViewModel
    {
        public List<AmusementGroupViewModel> Amusements { get; set; } = new();

        public List<Game> Games { get; set; } = new();

        // تعداد بلیت هر بازی که در سبد خرید وجود دارد
        public Dictionary<int, int> BasketQuantities { get; set; } = new();
    }
}