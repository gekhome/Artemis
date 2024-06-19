namespace Artemis.Dal.Interfaces
{
    public interface IChartRepository
    {
        List<ActorGenderCount> GetActorGenders();
        List<ActorRatingCount> GetActorRatings();
        List<ActorCountRegions> GetActorRegions();
    }
}