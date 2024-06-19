namespace Artemis.Dal.Services
{
    public class ActorFilmService : IActorFilmService
    {
        private readonly ArtemisDbContext db;

        public ActorFilmService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<ActorFilmViewModel> Read(int actorId)
        {
            var data = (from d in db.ActorFilms
                        where d.ActorId == actorId
                        orderby d.Year, d.Title
                        select new ActorFilmViewModel
                        {
                            Id = d.Id,
                            ActorId = d.ActorId,
                            Year = d.Year,
                            Title = d.Title,
                            Role = d.Role,
                            Notes = d.Notes,
                        }).ToList();
            return data;
        }

        public void Create(ActorFilmViewModel data, int actorId)
        {
            ActorFilms entity = new()
            {
                ActorId = actorId,
                Year = data.Year,
                Title = data.Title,
                Role = data.Role,
                Notes = data.Notes,
            };
            db.ActorFilms.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.Id = entity.Id;
        }

        public void CreateRange(IEnumerable<ActorFilmViewModel> data, int actorId)
        {
            foreach (var entity in data)
            {
                Create(entity, actorId);
            }
        }

        public void Update(ActorFilmViewModel data, int actorId)
        {
            ActorFilms entity = db.ActorFilms.Find(data.Id)!;

            entity.ActorId = actorId;
            entity.Year = data.Year;
            entity.Title = data.Title;
            entity.Role = data.Role;
            entity.Notes = data.Notes;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<ActorFilmViewModel> data, int actorId)
        {
            foreach (var entity in data)
            {
                Update(entity, actorId);
            }
        }

        public void Destroy(ActorFilmViewModel data)
        {
            ActorFilms entity = db.ActorFilms.Find(data.Id)!;
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ActorFilms.Remove(entity);
                db.SaveChanges();
            }
        }

        public void DestroyRange(IEnumerable<ActorFilmViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public ActorFilmViewModel? GetById(int id)
        {
            var data = (from d in db.ActorFilms
                        where d.ActorId == id
                        select new ActorFilmViewModel
                        {
                            Id = d.Id,
                            ActorId = d.ActorId,
                            Year = d.Year,
                            Title = d.Title,
                            Role = d.Role,
                            Notes = d.Notes,
                        }).FirstOrDefault();

            return data ?? new ActorFilmViewModel();
        }
    }
}
