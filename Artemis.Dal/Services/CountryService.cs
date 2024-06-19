using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artemis.Dal.Services
{
    public class CountryService : ICountryService
    {
        private readonly ArtemisDbContext db;

        public CountryService(ArtemisDbContext context)
        {
            db = context;
        }

        public IEnumerable<CountryViewModel> Read()
        {
            var data = (from d in db.Country
                        select new CountryViewModel
                        {
                            CountryId = d.CountryId,
                            Name = d.Name ?? string.Empty,
                            Capital = d.Capital,
                            Population = d.Population,
                            Region = d.Region ?? 0,
                            Flag = d.Flag,
                            FlagImage = d.Flag == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Flag)
                        }).OrderBy(x => x.Name).ToList();
            return data;
        }

        public void Create(CountryViewModel data)
        {
            Country entity = new()
            {
                Name = data.Name,
                Capital = data.Capital,
                Population = data.Population,
                Region = data.Region,
            };
            db.Country.Add(entity);
            db.SaveChanges();

            // Important for the grid!
            data.CountryId = entity.CountryId;
        }

        public void Update(CountryViewModel data)
        {
            Country entity = db.Country.Find(data.CountryId)!;

            entity.Name = data.Name;
            entity.Capital = data.Capital;
            entity.Population = data.Population;
            entity.Region = data.Region;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Destroy(CountryViewModel data)
        {
            Country entity = db.Country.Find(data.CountryId)!;

            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.Country.Remove(entity);
                db.SaveChanges();
            }
        }

        public CountryViewModel GetById(int Id)
        {
            Country? entity = db.Country.Find(Id);

            if (entity != null)
            {
                CountryViewModel data = new()
                {
                    CountryId = entity.CountryId,
                    Name = entity.Name ?? string.Empty,
                    Capital = entity.Capital,
                    Population = entity.Population,
                    Region = entity.Region ?? 0,
                    Flag = entity.Flag,
                    FlagImage = entity.Flag == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(entity.Flag)
                };
                return data;
            }
            return new CountryViewModel();
        }
    }
}
