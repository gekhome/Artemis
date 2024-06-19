namespace Artemis.Dal.Services
{
    public class ActorService : IActorService
    {
        private readonly ArtemisDbContext db;

        public ActorService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<ActorViewModel> Read()
        {
            var data = (from d in db.Actor
                        orderby d.FullName
                        select new ActorViewModel
                        {
                            ActorId = d.ActorId,
                            FullName = d.FullName,
                            BirthDate = d.BirthDate,
                            BirthPlace = d.BirthPlace,
                            Country = d.Country,
                            CountryName = d.Country != null ? db.Country.FirstOrDefault(c => c.CountryId == d.Country)!.Name : "Not available",
                            Age = d.Age,
                            Gender = d.Gender ?? 0,
                            Height = d.Height,
                            Rating = d.Rating,
                            Occupations = d.Occupations,
                            BioSummary = d.BioSummary,
                            Photo = d.Photo,
                            PhotoImage = d.Photo == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Photo),
                        }).ToList();
            return data;
        }

        public void Create(ActorViewModel data)
        {
            Actor entity = new()
            {
                FullName = data.FullName,
                BirthDate = data.BirthDate,
                BirthPlace = data.BirthPlace,
                Country = data.Country,
            };
            db.Actor.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.ActorId = entity.ActorId;
        }

        public void CreateRange(IEnumerable<ActorViewModel> data)
        {
            foreach (var entity in data)
            {
                Create(entity);
            }
        }

        public void Update(ActorViewModel data)
        {
            Actor entity = db.Actor.Find(data.ActorId)!;

            entity.FullName = data.FullName;
            entity.BirthDate = data.BirthDate;
            entity.BirthPlace = data.BirthPlace;
            entity.Country = data.Country;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<ActorViewModel> data)
        {
            foreach (var entity in data)
            {
                Update(entity);
            }
        }

        public void Destroy(ActorViewModel data)
        {
            Actor entity = db.Actor.Find(data.ActorId)!;
            if (entity != null)
            {
                if (CanDeleteActor(data.ActorId))
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Actor.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public void DestroyRange(IEnumerable<ActorViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public ActorViewModel? GetById(int id)
        {
            var data = (from d in db.Actor
                        where d.ActorId == id
                        select new ActorViewModel
                        {
                            ActorId = d.ActorId,
                            FullName = d.FullName,
                            BirthDate = d.BirthDate,
                            BirthPlace = d.BirthPlace,
                            Country = d.Country,
                            CountryName = d.Country != null ? db.Country.FirstOrDefault(c => c.CountryId == d.Country)!.Name : "Not available",
                            Age = d.Age,
                            Gender = d.Gender ?? 0,
                            Height = d.Height,
                            Rating = d.Rating,
                            Occupations = d.Occupations,
                            BioSummary = d.BioSummary,
                            Photo = d.Photo,
                            PhotoImage = d.Photo == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Photo),
                        }).FirstOrDefault();

            return data ?? new ActorViewModel();
        }

        private bool CanDeleteActor(int Id)
        {
            bool inGallery = db.ActorGallery.Where(x => x.ActorId == Id).Any();
            bool inMovies = db.FilmActor.Where(x => x.ActorId == Id).Any();
            if (inGallery || inMovies)
            {
                return false;
            }
            return true;
        }

        public bool Exists(string? actorName)
        {
            bool exists = db.Actor.Where(d => d.FullName == actorName).Any();
            return exists;
        }

    }
}
