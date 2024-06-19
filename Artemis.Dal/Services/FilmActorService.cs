namespace Artemis.Dal.Services
{
    public class FilmActorService : IFilmActorService
    {
        private readonly ArtemisDbContext db;

        public FilmActorService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<FilmActorViewModel> Read(int filmId)
        {
            var data = (from d in db.FilmActor
                        where d.FilmId == filmId
                        orderby d.Actor.FullName
                        select new FilmActorViewModel
                        {
                            Id = d.Id,
                            FilmId = d.FilmId,
                            ActorId = d.ActorId,
                            Role = d.Role,
                            Notes = d.Notes,
                        }).ToList();
            return data;
        }

        public void Create(FilmActorViewModel data, int filmId)
        {
            FilmActor entity = new()
            {
                FilmId = filmId,
                ActorId = data.ActorId,
                Role = data.Role,
                Notes = data.Notes,
            };
            db.FilmActor.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.Id = entity.Id;
        }

        public void CreateRange(IEnumerable<FilmActorViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                if (!Exists(filmId, entity.ActorId))
                {
                    Create(entity, filmId);
                }
            }
        }

        public void Update(FilmActorViewModel data, int filmId)
        {
            FilmActor entity = db.FilmActor.Find(data.Id)!;

            entity.FilmId = filmId;
            entity.ActorId = data.ActorId;
            entity.Role = data.Role;
            entity.Notes = data.Notes;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<FilmActorViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                Update(entity, filmId);
            }
        }

        public void Destroy(FilmActorViewModel data)
        {
            FilmActor? entity = db.FilmActor.Find(data.Id);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.FilmActor.Remove(entity);
                db.SaveChanges();
            }
        }

        public void DestroyRange(IEnumerable<FilmActorViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public FilmActorViewModel? GetById(int id)
        {
            FilmActorViewModel? data = new();

            data = (from d in db.FilmActor
                    where d.Id == id
                    select new FilmActorViewModel
                    {
                        Id = d.Id,
                        FilmId = d.FilmId,
                        ActorId = d.ActorId,
                        Role = d.Role,
                        Notes = d.Notes,
                    }).FirstOrDefault();

            return data ?? new FilmActorViewModel();
        }

        private bool Exists(int? filmId, int? actorId)
        {
            bool exists = db.FilmActor.Where(c => c.FilmId == filmId && c.ActorId == actorId).Any();
            return exists;
        }
    }
}
