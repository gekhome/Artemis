namespace Artemis.Dal.Interfaces
{
    public interface ICountryService
    {
        void Create(CountryViewModel data);
        void Destroy(CountryViewModel data);
        CountryViewModel GetById(int Id);
        IEnumerable<CountryViewModel> Read();
        void Update(CountryViewModel data);
    }
}