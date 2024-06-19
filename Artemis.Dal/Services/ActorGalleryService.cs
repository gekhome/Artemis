namespace Artemis.Dal.Services
{
    public class ActorGalleryService : IActorGalleryService
    {
        private readonly ArtemisDbContext db;

        public ActorGalleryService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<ActorGalleryViewModel> Read(int actorId)
        {
            var data = (from d in db.ActorGallery
                        where d.ActorId == actorId
                        select new ActorGalleryViewModel
                        {
                            GalleryId = d.GalleryId,
                            ActorId = d.ActorId,
                            Image = d.Image,
                            GalleryImage = d.Image == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Image),
                        }).ToList();
            return data;
        }

        public void Create(ActorGalleryViewModel data, int actorId)
        {
            ActorGallery entity = new()
            {
                ActorId = actorId,
                Image = data.Image,
            };
            db.ActorGallery.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.GalleryId = entity.GalleryId;
        }

        public void CreateRange(IEnumerable<ActorGalleryViewModel> data, int actorId)
        {
            foreach (var entity in data)
            {
                Create(entity, actorId);
            }
        }

        public void Update(ActorGalleryViewModel data, int actorId)
        {
            ActorGallery entity = db.ActorGallery.Find(data.GalleryId)!;

            entity.ActorId = actorId;
            entity.Image = data.Image;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<ActorGalleryViewModel> data, int actorId)
        {
            foreach (var entity in data)
            {
                Update(entity, actorId);
            }
        }

        public void Destroy(ActorGalleryViewModel data)
        {
            ActorGallery entity = db.ActorGallery.Find(data.GalleryId)!;
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ActorGallery.Remove(entity);
                db.SaveChanges();
            }
        }

        public void DestroyRange(IEnumerable<ActorGalleryViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public ActorGalleryViewModel? GetById(int id)
        {
            var data = (from d in db.ActorGallery
                        where d.GalleryId == id
                        select new ActorGalleryViewModel
                        {
                            GalleryId = d.GalleryId,
                            ActorId = d.ActorId,
                            Image = d.Image,
                            GalleryImage = d.Image == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Image),
                        }).FirstOrDefault();

            return data ?? new ActorGalleryViewModel();
        }
    }
}
