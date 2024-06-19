namespace Artemis.Dal.Services
{
    public class FilmCompanyService : IFilmCompanyService
    {
        private readonly ArtemisDbContext db;

        public FilmCompanyService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<FilmCompanyViewModel> Read(int filmId)
        {
            var data = (from d in db.FilmCompany
                        where d.FilmId == filmId
                        orderby d.Studio.Name
                        select new FilmCompanyViewModel
                        {
                            Id = d.Id,
                            FilmId = d.FilmId,
                            StudioId = d.StudioId,
                        }).ToList();
            return data;
        }

        public void Create(FilmCompanyViewModel data, int filmId)
        {
            FilmCompany entity = new()
            {
                FilmId = filmId,
                StudioId = data.StudioId,
            };
            db.FilmCompany.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.Id = entity.Id;
        }

        public void CreateRange(IEnumerable<FilmCompanyViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                if (!Exists(filmId, entity.StudioId))
                {
                    Create(entity, filmId);
                }
            }
        }

        public void Update(FilmCompanyViewModel data, int filmId)
        {
            FilmCompany entity = db.FilmCompany.Find(data.Id)!;

            entity.FilmId = filmId;
            entity.StudioId = data.StudioId;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<FilmCompanyViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                Update(entity, filmId);
            }
        }

        public void Destroy(FilmCompanyViewModel data)
        {
            FilmCompany? entity = db.FilmCompany.Find(data.Id);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.FilmCompany.Remove(entity);
                db.SaveChanges();
            }
        }

        public void DestroyRange(IEnumerable<FilmCompanyViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public FilmCompanyViewModel? GetById(int id)
        {
            FilmCompanyViewModel? data = new();

            data = (from d in db.FilmCompany
                    where d.Id == id
                    select new FilmCompanyViewModel
                    {
                        Id = d.Id,
                        FilmId = d.FilmId,
                        StudioId = d.StudioId,
                    }).FirstOrDefault();

            return data ?? new FilmCompanyViewModel();
        }

        private bool Exists(int? filmId, int? studioId)
        {
            bool exists = db.FilmCompany.Where(c => c.FilmId == filmId && c.StudioId == studioId).Any();
            return exists;
        }
    }
}
