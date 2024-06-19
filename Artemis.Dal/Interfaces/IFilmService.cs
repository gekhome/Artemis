namespace Artemis.Dal.Services
{
    public interface IFilmService
    {
        void Create(FilmViewModel data);
        void CreateRange(IEnumerable<FilmViewModel> data);
        void Destroy(FilmViewModel data);
        void DestroyRange(IEnumerable<FilmViewModel> data);
        FilmViewModel? GetById(int id);
        IEnumerable<FilmViewModel> Read();
        void Update(FilmViewModel data);
        void UpdateRange(IEnumerable<FilmViewModel> data);
        bool Exists(string? filmTitle);

    }
}