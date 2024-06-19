namespace Artemis.Dal.Interfaces
{
    public interface IActorGalleryService
    {
        void Create(ActorGalleryViewModel data, int actorId);
        void CreateRange(IEnumerable<ActorGalleryViewModel> data, int actorId);
        void Destroy(ActorGalleryViewModel data);
        void DestroyRange(IEnumerable<ActorGalleryViewModel> data);
        ActorGalleryViewModel? GetById(int id);
        IEnumerable<ActorGalleryViewModel> Read(int actorId);
        void Update(ActorGalleryViewModel data, int actorId);
        void UpdateRange(IEnumerable<ActorGalleryViewModel> data, int actorId);
    }
}