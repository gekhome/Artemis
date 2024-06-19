using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.Services
{
    public class YearService : BaseRepository<Years>, IYearService
    {
        private const int MAX_YEARS = 30;
        private readonly ArtemisDbContext db;

        public YearService(ArtemisDbContext context) : base(context)
        {
            db = context;
        }

        public IEnumerable<Years> Read()
        {
            return GetAll().OrderBy(x => x.Year).ToList();
        }

        public void Initialize()
        {
            int maxYear = MAX_YEARS;
            List<Years> data = (from x in Enumerable.Range(2000, ++maxYear)
                                select new Years { Year = x }).ToList();

            if (!db.Years.Any())
            {
                AddRange(data);
            }
        }
    }
}
