
namespace Artemis.Dal.ViewModels;

public class CountryViewModel
{
    public int CountryId { get; set; }

    [Required(ErrorMessage = "{0} requires a value.")]
    [Display(Name = "Name")]
    [MaxLength(50, ErrorMessage = "{0} can have a max of {1} characters.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Capital")]
    [MaxLength(50, ErrorMessage = "{0} can have a max of {1} characters.")]
    public string? Capital { get; set; }

    [Display(Name = "Population")]
    [DisplayFormat(DataFormatString = "{0:N0}")]
    [Range(1, 10_000_000_000, ErrorMessage = "Value of {0} must be between {1} and {2}.")]
    public int? Population { get; set; }

    [Display(Name = "Region")]
    public int Region { get; set; }

    [Display(Name = "Flag")]
    public byte[]? Flag { get; set; }

    [Display(Name = "Flag")]
    public string? FlagImage { get; set; }
}

