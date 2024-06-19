
namespace Artemis.Dal.ViewModels
{
    public class ActorGalleryViewModel
    {
        public int GalleryId { get; set; }

        [Display(Name = "Actor")]
        public int? ActorId { get; set; }

        [Display(Name = "Photo")]
        public byte[]? Image { get; set; }

        [Display(Name = "Photo")]
        public string? GalleryImage { get; set; }

        public virtual Actor? Actor { get; set; }
    }
}
