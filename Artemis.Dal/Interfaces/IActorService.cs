namespace Artemis.Dal.Interfaces
{
    public interface IActorService
    {
        void Create(ActorViewModel data);
        void CreateRange(IEnumerable<ActorViewModel> data);
        void Destroy(ActorViewModel data);
        void DestroyRange(IEnumerable<ActorViewModel> data);
        ActorViewModel? GetById(int id);
        IEnumerable<ActorViewModel> Read();
        void Update(ActorViewModel data);
        void UpdateRange(IEnumerable<ActorViewModel> data);
        bool Exists(string? actorName);

    }
}