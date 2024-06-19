using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.ViewModels
{
    public class ActorViewModel
    {
        public int ActorId { get; set; }

        [Required(ErrorMessage = "The name of the actor is required.")]
        [StringLength(255, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        [Display(Name = "Full name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "The date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime? BirthDate { get; set; }

        [StringLength(255, ErrorMessage = "{0} cannot have more than {1} characters.")]
        [Display(Name = "Birthplace ")]
        public string? BirthPlace { get; set; }

        [Display(Name = "Country")]
        public int? Country { get; set; }

        [Display(Name = "Country")]
        public string? CountryName { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "The gender of the actor is required.")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Display(Name = "Height (m)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [HeightRange(1.0, 3.0)]
        public double? Height { get; set; }

        [StringLength(255, ErrorMessage = "{0} cannot have more than {1} characters.")]
        [Display(Name = "Occupation(s)")]
        public string? Occupations { get; set; }

        [Display(Name = "Rating")]
        public double? Rating { get; set; }

        [Display(Name = "Bio summary")]
        public string? BioSummary { get; set; }

        public byte[]? Photo { get; set; }

        [Display(Name = "Photo")]
        public string? PhotoImage { get; set; }

        public virtual IEnumerable<ActorGallery> ActorGallery { get; set; } = new List<ActorGallery>();

        public virtual IEnumerable<FilmActor> FilmActor { get; set; } = new List<FilmActor>();
    }

    public class ActorInfoViewModel
    {
        public int ActorId { get; set; }

        [Display(Name = "Full name")]
        public string? FullName { get; set; }

        [Display(Name = "Birthplace")]
        public string? BirthPlace { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Born")]
        public string? Dob { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Display(Name = "Height")]
        public string? Height { get; set; }

        [Display(Name = "Occupation(s)")]
        public string? Occupations { get; set; }

        [Display(Name = "Rating")]
        public string? Rating { get; set; }

        public byte[]? Photo { get; set; }

        [Display(Name = "Photo")]
        public string? PhotoImage { get; set; }

        [Display(Name = "Bio summary")]
        public string? BioSummary { get; set; }
    }
}
