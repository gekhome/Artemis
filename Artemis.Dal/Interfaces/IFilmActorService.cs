namespace Artemis.Dal.Interfaces
{
    public interface IFilmActorService
    {
        void Create(FilmActorViewModel data, int filmId);
        void CreateRange(IEnumerable<FilmActorViewModel> data, int filmId);
        void Destroy(FilmActorViewModel data);
        void DestroyRange(IEnumerable<FilmActorViewModel> data);
        FilmActorViewModel? GetById(int id);
        IEnumerable<FilmActorViewModel> Read(int filmId);
        void Update(FilmActorViewModel data, int filmId);
        void UpdateRange(IEnumerable<FilmActorViewModel> data, int filmId);
    }
}