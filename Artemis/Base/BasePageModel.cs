namespace Artemis.Base
{
    public class BasePageModel : PageModel
    {
        private readonly ArtemisDbContext db;

        public BasePageModel(ArtemisDbContext context)
        {
            db = context;
        }

        #region Populators

        public void PopulateCountries()
        {
            var data = db.Country.OrderBy(c => c.Name).ToList();
            ViewData["countries"] = data;
            ViewData["defaultCountry"] = data.FirstOrDefault()?.CountryId;
        }

        public void PopulateCompanies()
        {
            var data = db.MovieStudio.OrderBy(c => c.Name).ToList();
            ViewData["companies"] = data;
            ViewData["defaultCompany"] = data.FirstOrDefault()?.StudioId;
        }

        public void PopulateActors()
        {
            var data = db.Actor.OrderBy(c => c.FullName).ToList();
            ViewData["actors"] = data;
            ViewData["defaultActor"] = data.FirstOrDefault()?.ActorId;
        }

        public void PopulateYears()
        {
            var data = db.Years.OrderBy(c => c.Year).ToList();
            ViewData["years"] = data;
            ViewData["defaultYear"] = data.FirstOrDefault()?.Year;
        }

        public void PopulateLanguages()
        {
            List<Languages> data = db.Languages.OrderBy(x => x.Language).ToList();
            ViewData["languages"] = data;
            ViewData["defaultLanguage"] = data.FirstOrDefault()?.LanguageId;
        }

        public void PopulateGenders()
        {
            var data = db.Genders.ToList();
            ViewData["genders"] = data;
            ViewData["defaultgender"] = data.FirstOrDefault()?.GenderId;
        }


        #endregion

        #region Data sources

        public JsonResult OnGetLanguages_Read()
        {
            List<Languages> data = db.Languages.OrderBy(x => x.Language).ToList();
            return new JsonResult(data);
        }

        public JsonResult OnGetGenres_Read()
        {
            List<MovieGenre> data = db.MovieGenre.OrderBy(x => x.GenreName).ToList();
            return new JsonResult(data);
        }

        public JsonResult OnGetCountries_Read()
        {
            List<Country> data = db.Country.OrderBy(x => x.Name).ToList();
            return new JsonResult(data);
        }

        public JsonResult OnGetGenders_Read()
        {
            List<Genders> data = db.Genders.ToList();
            return new JsonResult(data);
        }

        #endregion

        #region Validations

        public bool CountriesExist()
        {
            return db.Country.Any();
        }

        public bool CompaniesExist()
        {
            return db.MovieStudio.Any();
        }

        public bool ActorsExist()
        {
            return db.Actor.Any();
        }

        public bool YearsExist()
        {
            return db.Years.Any();
        }

        public string Errors(string message)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            foreach (ModelError error in allErrors)
            {
                message += error.ErrorMessage + " ";
            }
            return message;
        }

        #endregion

    }
}
