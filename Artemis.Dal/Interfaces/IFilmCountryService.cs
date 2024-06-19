namespace Artemis.Dal.Interfaces
{
    public interface IFilmCountryService
    {
        void Create(FilmCountryViewModel data, int filmId);
        void CreateRange(IEnumerable<FilmCountryViewModel> data, int filmId);
        void Destroy(FilmCountryViewModel data);
        void DestroyRange(IEnumerable<FilmCountryViewModel> data);
        FilmCountryViewModel? GetById(int id);
        IEnumerable<FilmCountryViewModel> Read(int filmId);
        void Update(FilmCountryViewModel data, int filmId);
        void UpdateRange(IEnumerable<FilmCountryViewModel> data, int filmId);
    }
}