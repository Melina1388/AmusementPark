using AmusementPark.Domain.Entities;

namespace AmusementPark.Web.ViewModels
{
    public class AmusementGroupViewModel
    {
        public string AmusementName { get; set; } = "";

        public List<GameViewModel> Games { get; set; }
            = new();
    }
}