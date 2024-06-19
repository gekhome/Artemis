namespace Artemis.Dal.Interfaces
{
    public interface IFilmCompanyService
    {
        void Create(FilmCompanyViewModel data, int filmId);
        void CreateRange(IEnumerable<FilmCompanyViewModel> data, int filmId);
        void Destroy(FilmCompanyViewModel data);
        void DestroyRange(IEnumerable<FilmCompanyViewModel> data);
        FilmCompanyViewModel? GetById(int id);
        IEnumerable<FilmCompanyViewModel> Read(int filmId);
        void Update(FilmCompanyViewModel data, int filmId);
        void UpdateRange(IEnumerable<FilmCompanyViewModel> data, int filmId);
    }
}