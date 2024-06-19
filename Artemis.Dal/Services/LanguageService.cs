using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.Services
{
    public class LanguageService : BaseRepository<Languages>, ILanguageService
    {
        public LanguageService(ArtemisDbContext context) : base(context)
        { }

        public IEnumerable<Languages> Read()
        {
            return GetAll().OrderBy(x => x.Language).ToList();
        }
    }
}
