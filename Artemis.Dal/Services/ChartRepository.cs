using Artemis.Dal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.Services
{
    public class ChartRepository : IChartRepository
    {
        private readonly ArtemisDbContext db;

        public ChartRepository(ArtemisDbContext context)
        {
            db = context;
        }

        public List<ActorCountRegions> GetActorRegions()
        {
            var data = (from d in db.ActorCountRegions orderby d.Percentage descending select d).ToList();
            return data;
        }

        public List<ActorRatingCount> GetActorRatings()
        {
            var data = (from d in db.ActorRatingCount orderby d.Percentage descending select d).ToList();
            return data;
        }

        public List<ActorGenderCount> GetActorGenders()
        {
            var data = (from d in db.ActorGenderCount orderby d.Percentage descending select d).ToList();
            return data;
        }

    }
}
