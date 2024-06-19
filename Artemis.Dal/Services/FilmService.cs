
namespace Artemis.Dal.Services
{
    public class FilmService : IFilmService
    {
        private readonly ArtemisDbContext db;

        public FilmService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<FilmViewModel> Read()
        {
            var data = (from d in db.Film
                        orderby d.ReleaseDate descending, d.Title
                        select new FilmViewModel
                        {
                            FilmId = d.FilmId,
                            Title = d.Title,
                            PlotSummary = d.PlotSummary,
                            ReleaseDate = d.ReleaseDate,
                            ReleaseYear = d.ReleaseYear,
                            RunningTime = d.RunningTime,
                            Budget = d.Budget,
                            BoxOffice = d.BoxOffice,
                            Language = d.Language,
                            Director = d.Director,
                            Producer = d.Producer,
                            TrailerLink = d.TrailerLink,
                            Rating = d.Rating,
                            Genres = d.Genres,
                            SelectedGenres = d.Genres != null ? Common.GenreNamesToValues(d.Genres) : Array.Empty<int>(),
                            Poster = d.Poster,
                            PosterImage = d.Poster == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Poster),
                        }).ToList();
            return data;
        }

        public void Create(FilmViewModel data)
        {
            Film entity = new()
            {
                Title = data.Title,
                ReleaseDate = data.ReleaseDate,
                RunningTime = data.RunningTime,
                Language = data.Language,
            };
            db.Film.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.FilmId = entity.FilmId;
        }

        public void CreateRange(IEnumerable<FilmViewModel> data)
        {
            foreach (var entity in data)
            {
                Create(entity);
            }
        }

        public void Update(FilmViewModel data)
        {
            Film entity = db.Film.Find(data.FilmId)!;

            entity.Title = data.Title;
            entity.ReleaseDate = data.ReleaseDate;
            entity.RunningTime = data.RunningTime;
            entity.Language = data.Language;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<FilmViewModel> data)
        {
            foreach (var entity in data)
            {
                Update(entity);
            }
        }

        public void Destroy(FilmViewModel data)
        {
            Film entity = db.Film.Find(data.FilmId)!;
            if (entity != null)
            {
                if (CanDeleteFilm(data.FilmId))
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Film.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public void DestroyRange(IEnumerable<FilmViewModel> data)
        {
            foreach (var entity in data)
            {
                Destroy(entity);
            }
        }

        public FilmViewModel? GetById(int id)
        {
            var data = (from d in db.Film
                        where d.FilmId == id
                        select new FilmViewModel
                        {
                            FilmId = d.FilmId,
                            Title = d.Title,
                            PlotSummary = d.PlotSummary,
                            ReleaseDate = d.ReleaseDate,
                            ReleaseYear = d.ReleaseYear,
                            RunningTime = d.RunningTime,
                            Budget = d.Budget,
                            BoxOffice = d.BoxOffice,
                            Language = d.Language,
                            Director = d.Director,
                            Producer = d.Producer,
                            TrailerLink = d.TrailerLink,
                            Rating = d.Rating,
                            Genres = d.Genres,
                            SelectedGenres = d.Genres != null ? Common.GenreNamesToValues(d.Genres) : Array.Empty<int>(),
                            Poster = d.Poster,
                            PosterImage = d.Poster == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Poster),
                        }).FirstOrDefault();

            return data ?? new FilmViewModel();
        }

        private bool CanDeleteFilm(int Id)
        {
            bool inCountries = db.FilmCountry.Where(x => x.FilmId == Id).Any();
            bool inCompanies = db.FilmCompany.Where(x => x.FilmId == Id).Any();
            bool inActors = db.FilmActor.Where(x => x.FilmId == Id).Any();
            if (inCountries || inCompanies || inActors)
            {
                return false;
            }
            return true;
        }

        public bool Exists(string? filmTitle)
        {
            bool exists = db.Film.Where(d => d.Title == filmTitle).Any();
            return exists;
        }

    }
}
