namespace Artemis.Dal.Interfaces
{
    public interface IActorFilmService
    {
        void Create(ActorFilmViewModel data, int actorId);
        void CreateRange(IEnumerable<ActorFilmViewModel> data, int actorId);
        void Destroy(ActorFilmViewModel data);
        void DestroyRange(IEnumerable<ActorFilmViewModel> data);
        ActorFilmViewModel? GetById(int id);
        IEnumerable<ActorFilmViewModel> Read(int actorId);
        void Update(ActorFilmViewModel data, int actorId);
        void UpdateRange(IEnumerable<ActorFilmViewModel> data, int actorId);
    }
}