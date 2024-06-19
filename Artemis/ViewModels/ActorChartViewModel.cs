namespace Artemis.ViewModels
{
    public class ActorRegionViewModel
    {
        public string? Region { get; set; }
        public decimal Percentage { get; set; }
        public bool Explode { get; set; }
        public string? Color { get; set; }

        public ActorRegionViewModel()
        { }

        public ActorRegionViewModel(string region, decimal percentage, string? color = null, bool explode = false)
        {
            Region = region;
            Percentage = percentage;
            Explode = explode;
            Color = color;
        }
    }

    public class ActorRatingViewModel
    {
        public string? Rating { get; set; }
        public decimal Percentage { get; set; }
        public bool Explode { get; set; }
        public string? Color { get; set; }

        public ActorRatingViewModel()
        { }

        public ActorRatingViewModel(string rating, decimal percentage, string? color = null, bool explode = false)
        {
            Rating = rating;
            Percentage = percentage;
            Explode = explode;
            Color = color;
        }
    }

    public class ActorGenderViewModel
    {
        public string? Gender { get; set; }
        public decimal Percentage { get; set; }
        public string? Color { get; set; }

        public ActorGenderViewModel()
        { }

        public ActorGenderViewModel(string gender, decimal percentage, string? color = null)
        {
            Gender = gender;
            Percentage = percentage;
            Color = color;
        }
    }

}
