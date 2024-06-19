namespace Artemis.Dal.Services
{
    public class FilmCountryService : IFilmCountryService
    {
        private readonly ArtemisDbContext db;

        public FilmCountryService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<FilmCountryViewModel> Read(int filmId)
        {
            var data = (from d in db.FilmCountry
                        where d.FilmId == filmId
                        orderby d.Country.Name
                        select new FilmCountryViewModel
                        {
                            Id = d.Id,
                            FilmId = d.FilmId,
                            CountryId = d.CountryId,
                        }).ToList();
            return data;
        }

        public void Create(FilmCountryViewModel data, int filmId)
        {
            FilmCountry entity = new()
            {
                FilmId = filmId,
                CountryId = data.CountryId,
            };
            db.FilmCountry.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.Id = entity.Id;
        }

        public void CreateRange(IEnumerable<FilmCountryViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                if (!Exists(filmId, entity.CountryId))
                {
                    Create(entity, filmId);
                }
            }
        }

        public void Update(FilmCountryViewModel data, int filmId)
        {
            FilmCountry entity = db.FilmCountry.Find(data.Id)!;

            entity.FilmId = filmId;
            entity.CountryId = data.CountryId;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<FilmCountryViewModel> data, int filmId)
        {
            foreach (var entity in data)
            {
                Update(entity, filmId);
            }
        }

        public void Destroy(FilmCountryViewModel data)
        {
            FilmCountry? entity = db.FilmCountry.Find(data.Id);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.FilmCountry.Remove(entity);
                db.SaveChanges();
            }
        }

        public void DestroyRange(IEnumerable<FilmCountryViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public FilmCountryViewModel? GetById(int id)
        {
            FilmCountryViewModel? data = new();

            data = (from d in db.FilmCountry
                    where d.Id == id
                    select new FilmCountryViewModel
                    {
                        Id = d.Id,
                        FilmId = d.FilmId,
                        CountryId = d.CountryId,
                    }).FirstOrDefault();

            return data ?? new FilmCountryViewModel();
        }

        private bool Exists(int? filmId, int? countryId)
        {
            bool exists = db.FilmCountry.Where(c => c.FilmId == filmId &&  c.CountryId == countryId).Any();
            return exists;
        }
    }
}
