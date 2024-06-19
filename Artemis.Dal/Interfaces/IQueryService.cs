namespace Artemis.Dal.Interfaces
{
    public interface IQueryService
    {
        List<ActorInfoViewModel> GetActors();
        List<FilmInfoViewModel> GetFilms();
        List<FilmActorsViewModel> GetFilmActors(int filmId);
        List<ActorFilmViewModel> GetActorFilms(int actorId);
        List<StudiosInFilms> GetStudiosInFilms();
        IEnumerable<FilmViewModel> GetStudioFilms(int studioId);
    }
}