using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.Services
{
    public class QueryService : IQueryService
    {
        private readonly ArtemisDbContext db;

        public QueryService(ArtemisDbContext context)
        {
            db = context;
        }

        public List<FilmInfoViewModel> GetFilms()
        {
            var data = (from d in db.FilmInfo
                        orderby d.ReleaseYear descending, d.Title
                        select new FilmInfoViewModel
                        {
                            FilmId = d.FilmId,
                            Title = d.Title,
                            ReleaseDate = d.ReleaseDate,
                            ReleaseYear = d.ReleaseYear,
                            Language = d.Language,
                            Rating = d.Rating,
                            RunningTime = d.RunningTime,
                            BoxOffice = d.BoxOffice,
                            Budget = d.Budget,
                            Director = d.Director,
                            Genres = d.Genres,
                            TrailerLink = d.TrailerLink,
                            PlotSummary = d.PlotSummary,
                            Poster = d.Poster,
                            PosterImage = d.Poster == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Poster),
                        }).ToList();
            return data;
        }

        public List<ActorInfoViewModel> GetActors()
        {
            var data = (from d in db.ActorInfo
                        orderby d.FullName
                        select new ActorInfoViewModel
                        {
                            ActorId = d.ActorId,
                            FullName = d.FullName,
                            BirthPlace = d.BirthPlace,
                            Country = d.Country,
                            Dob = d.Dob,
                            Age = d.Age,
                            Height = d.Height,
                            Occupations = d.Occupations,
                            Rating = d.Rating,
                            BioSummary = d.BioSummary,
                            Photo = d.Photo,
                            PhotoImage = d.Photo == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Photo),
                        }).ToList();
            return data;
        }

        public List<FilmActorsViewModel> GetFilmActors(int filmId)
        {
            var data = (from d in db.FilmActors where d.FilmId == filmId
                        orderby d.ActorName
                        select new FilmActorsViewModel
                        {
                            Id = d.Id,
                            FilmId = d.FilmId,
                            ActorName = d.ActorName,
                            Role = d.Role,
                            Notes = d.Notes,
                            Photo = d.Photo,
                            ActorImage = d.Photo == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Photo)
                        }).ToList();
            return data;
        }

        public List<ActorFilmViewModel> GetActorFilms(int actorId)
        {
            var data = (from d in db.ActorFilms where d.ActorId == actorId 
                        select new  ActorFilmViewModel
                        {
                            Id= d.Id,
                            ActorId = d.ActorId,
                            Year = d.Year,
                            Title = d.Title,
                            Role = d.Role,
                        }).ToList();
            return data;
        }

        public List<StudiosInFilms> GetStudiosInFilms()
        {
            var data = (from d in db.StudiosInFilms orderby d.Name select d).ToList();
            return data;
        }

        public IEnumerable<FilmViewModel> GetStudioFilms(int studioId)
        {
            var data = (from d in db.FilmCompany.Include(d => d.Film)
                        where d.StudioId == studioId
                        orderby d.Film.ReleaseDate descending, d.Film.Title
                        select new FilmViewModel
                        {
                            FilmId = d.FilmId,
                            Title = d.Film.Title,
                            PlotSummary = d.Film.PlotSummary,
                            ReleaseDate = d.Film.ReleaseDate,
                            ReleaseYear = d.Film.ReleaseYear,
                            RunningTime = d.Film.RunningTime,
                            Budget = d.Film.Budget,
                            BoxOffice = d.Film.BoxOffice,
                            Director = d.Film.Director,
                            Producer = d.Film.Producer,
                            Rating = d.Film.Rating,
                            Genres = d.Film.Genres,
                            Poster = d.Film.Poster,
                            PosterImage = d.Film.Poster == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Film.Poster),
                        }).ToList();
            return data;
        }

    }
}
