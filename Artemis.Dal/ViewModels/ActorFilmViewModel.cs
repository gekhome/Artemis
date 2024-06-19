
namespace Artemis.Dal.ViewModels
{
    public class ActorFilmViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Actor")]
        public int? ActorId { get; set; }

        [Required(ErrorMessage = "{0} must have a value.")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "{0} must have a value.")]
        [StringLength(255, ErrorMessage = "{0} cannot have more than {1} characters.")]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "{0} must have a value.")]
        [StringLength(150, ErrorMessage = "{0} cannot have more than {1} characters.")]
        [Display(Name = "Role")]
        public string? Role { get; set; }

        [Display(Name = "Notes")]
        [StringLength(255, ErrorMessage = "{0} cannot have more than {1} characters.")]
        public string? Notes { get; set; }

        public virtual Actor? Actor { get; set; }
    }
}
