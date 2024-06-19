
namespace Artemis.Dal.ViewModels
{
    public class FilmCountryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Film")]
        public int? FilmId { get; set; }

        [Required(ErrorMessage = "The country is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public virtual Country? Country { get; set; }

        public virtual Film? Film { get; set; }
    }
}
