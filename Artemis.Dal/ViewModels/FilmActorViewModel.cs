
namespace Artemis.Dal.ViewModels
{
    public class FilmActorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Film")]
        public int? FilmId { get; set; }

        [Required(ErrorMessage = "The actor is required")]
        [Display(Name = "Actor")]
        public int ActorId { get; set; }

        [Display(Name = "Role")]
        [StringLength(50, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        public string? Role { get; set; }

        [Display(Name = "Notes")]
        [StringLength(255, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        public string? Notes { get; set; }

        public virtual Actor? Actor { get; set; }

        public virtual Film? Film { get; set; }
    }

    public class FilmActorsViewModel
    {
        public int Id { get; set; }

        public int FilmId { get; set; }

        [Display(Name = "Name")]
        public string? ActorName { get; set; }

        [Display(Name = "Role")]
        public string? Role { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        public byte[]? Photo { get; set; }

        public string? ActorImage { get; set; }
    }
}
