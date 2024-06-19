
namespace Artemis.Dal.ViewModels
{
    public class FilmViewModel
    {
        public int FilmId { get; set; }

        [Required(ErrorMessage = "The title of the film is required.")]
        [StringLength(255, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The release date of the film is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Year")]
        public int? ReleaseYear { get; set; }

        [Display(Name = "Plot Summary")]
        public string? PlotSummary { get; set; }

        [Display(Name = "Running time (min)")]
        [Range(1, 270, ErrorMessage = "Value for {0} must be between {1} and {2} minutes")]
        public int? RunningTime { get; set; }

        [Display(Name = "Budget")]
        [DisplayFormat(DataFormatString = "$ {0:N0}")]
        [Range(1, 10_000_000_000, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public long? Budget { get; set; }

        [Display(Name = "Box office")]
        [Range(1, 10_000_000_000, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public long? BoxOffice { get; set; }

        [Display(Name = "Language")]
        public int? Language { get; set; }

        [Display(Name = "Director")]
        [StringLength(255, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        public string? Director { get; set; }

        [Display(Name = "Producer(s)")]
        [StringLength(255, ErrorMessage = "Value for {0} cannot have more than {1} characters.")]
        public string? Producer { get; set; }

        [Display(Name = "Trailer URL")]
        public string? TrailerLink { get; set; }

        [Display(Name = "Rating")]
        public double? Rating { get; set; }

        public byte[]? Poster { get; set; }

        [Display(Name = "Poster")]
        public string? PosterImage { get; set; }

        [Display(Name = "Genres")]
        public string? Genres { get; set; }

        [Display(Name = "Genres")]
        public int[]? SelectedGenres { get; set; }

        public virtual IEnumerable<FilmActor> FilmActor { get; set; } = new List<FilmActor>();

        public virtual IEnumerable<FilmCompany> FilmCompany { get; set; } = new List<FilmCompany>();

        public virtual IEnumerable<FilmCountry> FilmCountry { get; set; } = new List<FilmCountry>();
    }

    public class FilmInfoViewModel
    {
        public int FilmId { get; set; }

        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Release date")]
        public string? ReleaseDate { get; set; }

        [Display(Name = "Year")]
        public int? ReleaseYear { get; set; }

        [Display(Name = "Running time")]
        public string? RunningTime { get; set; }

        [Display(Name = "Budget")]
        public string? Budget { get; set; }

        [Display(Name = "Box office")]
        public string? BoxOffice { get; set; }

        [Display(Name = "Language")]
        public string? Language { get; set; }

        [Display(Name = "Director")]
        public string? Director { get; set; }

        [Display(Name = "Rating")]
        public string? Rating { get; set; }

        [Display(Name = "Genres")]
        public string? Genres { get; set; }

        [Display(Name = "Trailer")]
        public string? TrailerLink { get; set; }

        [Display(Name = "Poster")]
        public byte[]? Poster { get; set; }

        [Display(Name = "Poster")]
        public string? PosterImage { get; set; }

        [Display(Name = "Plot summary")]
        public string? PlotSummary { get; set; }
    }
}
