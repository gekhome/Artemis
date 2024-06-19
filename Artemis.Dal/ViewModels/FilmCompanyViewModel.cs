
namespace Artemis.Dal.ViewModels
{
    public class FilmCompanyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Film")]
        public int? FilmId { get; set; }

        [Required(ErrorMessage = "The production company is required")]
        [Display(Name = "Production company")]
        public int StudioId { get; set; }

        public virtual Film? Film { get; set; }

        public virtual MovieStudio? Studio { get; set; }
    }
}
