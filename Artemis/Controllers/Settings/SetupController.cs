using Artemis.Dal.Models;

namespace Artemis.Controllers.Settings
{
    //[Authorize(Roles = "Admin, Moderator")]
    public class SetupController : Controller
    {
        private readonly ArtemisDbContext db;
        private readonly ICountryService countryService;
        private readonly IMovieGenreService movieGenreService;
        private readonly IMusicGenreService musicGenreService;
        private readonly IMovieStudioService movieStudioService;
        private readonly IMusicStudioService musicStudioService;

        public SetupController(ArtemisDbContext context, 
            ICountryService countryService, 
            IMovieStudioService movieStudioService, 
            IMusicStudioService musicStudioService, 
            IMovieGenreService movieGenreService,
            IMusicGenreService musicGenreService)
        {
            db = context;
            this.countryService = countryService;
            this.movieStudioService = movieStudioService;
            this.musicStudioService = musicStudioService;
            this.movieGenreService = movieGenreService;
            this.musicGenreService = musicGenreService;
        }

        #region Countries setup

        public ActionResult Countries(string? notify = null)
        {
            if (notify != null)
            {
                this.AddAlertDanger(notify);
            }
            CountryViewModel? model = countryService.Read().FirstOrDefault();
            PopulateRegions();

            return View(model);
        }

        public ActionResult Countries_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<CountryViewModel> data = countryService.Read();

            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public ActionResult Countries_Create([DataSourceRequest] DataSourceRequest request, CountryViewModel data)
        {
            if (data != null && ModelState.IsValid)
            {
                countryService.Create(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult Countries_Update([DataSourceRequest] DataSourceRequest request, CountryViewModel data)
        {
            if (data != null && ModelState.IsValid)
            {
                countryService.Update(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult Countries_Destroy([DataSourceRequest] DataSourceRequest request, CountryViewModel data)
        {
            if (data != null)
            {
                countryService.Destroy(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetCountryRecord(int Id)
        {
            CountryViewModel model = countryService.GetById(Id);

            return PartialView("CountryEditPartial", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CountryEdit(CountryViewModel model)
        {
            string? message = null;
            IFormFile? imageFile = null;

            if (HttpContext.Request.Form.Files.Any())
            {
                imageFile = HttpContext.Request.Form.Files[0];
                if (imageFile != null)
                {
                    if (imageFile.Length >= Common.LOGO_IMAGE_MAXSIZE)
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded image size must be less than {Common.LOGO_IMAGE_MAXSIZE / 1024}KB.");
                    }
                    string extension = Path.GetExtension(imageFile.FileName);
                    if (!Common.ValidImageExtension(extension))
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded file extension '{extension}' is not accepted.");
                    }
                }
            }

            if (model != null && ModelState.IsValid) 
            {
                Country? entity = db.Country.Find(model.CountryId);
                if (entity != null)
                {
                    entity.Name = model.Name;
                    entity.Capital = model.Capital;
                    entity.Population = model.Population;
                    entity.Region = model.Region;
                    if (imageFile != null)
                    {
                        entity.Flag = Common.FileToByteArray(imageFile);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else 
            {
                message = "Data was not saved due to validation errors. ";
                message = Errors(message);
            }
            return RedirectToAction("Countries", new { notify = message });
        }

        #endregion

        #region Movie Genres setup

        public IActionResult MovieGenres()
        {
            return View();
        }

        public ActionResult MovieGenre_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<MovieGenre> data = movieGenreService.Read();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieGenre_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MovieGenre> data)
        {
            if (data != null && ModelState.IsValid)
            {
                movieGenreService.AddRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieGenre_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MovieGenre> data)
        {
            if (data != null && ModelState.IsValid)
            {
                movieGenreService.UpdateRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieGenre_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MovieGenre> data)
        {
            if (data.Any())
            {
                movieGenreService.DeleteRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Music Genres setup

        public IActionResult MusicGenres()
        {
            return View();
        }

        public ActionResult MusicGenre_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<MusicGenre> data = musicGenreService.Read();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicGenre_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MusicGenre> data)
        {
            if (data != null && ModelState.IsValid)
            {
                musicGenreService.AddRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicGenre_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MusicGenre> data)
        {
            if (data != null && ModelState.IsValid)
            {
                musicGenreService.UpdateRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicGenre_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MusicGenre> data)
        {
            if (data.Any())
            {
                musicGenreService.DeleteRange(data);
            }

            return Json(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Movie Studios setup

        public ActionResult MovieStudios(string? notify = null)
        {
            if (notify != null)
            {
                this.AddAlertDanger(notify);
            }
            MovieStudio? model = movieStudioService.Read().FirstOrDefault();

            return View(model);
        }

        public ActionResult MovieStudio_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<MovieStudio> data = movieStudioService.Read();

            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieStudio_Create([DataSourceRequest] DataSourceRequest request, MovieStudio data)
        {
            if (data != null && ModelState.IsValid)
            {
                movieStudioService.Add(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieStudio_Update([DataSourceRequest] DataSourceRequest request, MovieStudio data)
        {
            if (data != null && ModelState.IsValid)
            {
                movieStudioService.Update(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MovieStudio_Destroy([DataSourceRequest] DataSourceRequest request, MovieStudio data)
        {
            if (data != null)
            {
                movieStudioService.Delete(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetMovieStudioRecord(int Id)
        {
            MovieStudio model = movieStudioService.GetById(Id) ?? new MovieStudio();

            return PartialView("MovieStudioPartial", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MovieStudioEdit(MovieStudio model)
        {
            string? message = null;
            IFormFile? imageFile = null;

            if (HttpContext.Request.Form.Files.Any())
            {
                imageFile = HttpContext.Request.Form.Files[0];
                if (imageFile != null)
                {
                    if (imageFile.Length >= Common.LOGO_IMAGE_MAXSIZE)
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded image size must be less than {Common.LOGO_IMAGE_MAXSIZE / 1024}KB.");
                    }
                    string extension = Path.GetExtension(imageFile.FileName);
                    if (!Common.ValidImageExtension(extension))
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded file extension '{extension}' is not accepted.");
                    }
                }
            }

            if (model != null && ModelState.IsValid)
            {
                MovieStudio? entity = db.MovieStudio.Find(model.StudioId);
                if (entity != null)
                {
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    if (imageFile != null)
                    {
                        entity.LogoBytes = Common.FileToByteArray(imageFile);
                        entity.LogoImage = Convert.ToBase64String(entity.LogoBytes);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                message = "Data was not saved due to validation errors. ";
                message = Errors(message);
            }
            return RedirectToAction("MovieStudios", new { notify = message });
        }

        #endregion

        #region Music Studios setup

        public ActionResult MusicStudios(string? notify = null)
        {
            if (notify != null)
            {
                this.AddAlertDanger(notify);
            }
            MusicStudio? model = musicStudioService.Read().FirstOrDefault();

            return View(model);
        }

        public ActionResult MusicStudio_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<MusicStudio> data = musicStudioService.Read();

            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicStudio_Create([DataSourceRequest] DataSourceRequest request, MusicStudio data)
        {
            if (data != null && ModelState.IsValid)
            {
                musicStudioService.Add(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicStudio_Update([DataSourceRequest] DataSourceRequest request, MusicStudio data)
        {
            if (data != null && ModelState.IsValid)
            {
                musicStudioService.Update(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult MusicStudio_Destroy([DataSourceRequest] DataSourceRequest request, MusicStudio data)
        {
            if (data != null)
            {
                musicStudioService.Delete(data);
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetMusicStudioRecord(int Id)
        {
            MusicStudio model = musicStudioService.GetById(Id) ?? new MusicStudio();

            return PartialView("MusicStudioPartial", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MusicStudioEdit(MusicStudio model)
        {
            string? message = null;
            IFormFile? imageFile = null;

            if (HttpContext.Request.Form.Files.Any())
            {
                imageFile = HttpContext.Request.Form.Files[0];
                if (imageFile != null)
                {
                    if (imageFile.Length >= Common.LOGO_IMAGE_MAXSIZE)
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded image size must be less than {Common.LOGO_IMAGE_MAXSIZE / 1024}KB.");
                    }
                    string extension = Path.GetExtension(imageFile.FileName);
                    if (!Common.ValidImageExtension(extension))
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded file extension '{extension}' is not accepted.");
                    }
                }
            }

            if (model != null && ModelState.IsValid)
            {
                MusicStudio? entity = db.MusicStudio.Find(model.StudioId);
                if (entity != null)
                {
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    if (imageFile != null)
                    {
                        entity.LogoBytes = Common.FileToByteArray(imageFile);
                        entity.LogoImage = Convert.ToBase64String(entity.LogoBytes);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                message = "Data was not saved due to validation errors. ";
                message = Errors(message);
            }
            return RedirectToAction("MusicStudios", new { notify = message });
        }

        #endregion


        public JsonResult Regions_Read()
        {
            var data = (from d in db.Regions select d).ToList();

            return Json(data);
        }

        public void PopulateRegions()
        {
            var data = (from d in db.Regions select d).ToList();

            ViewData["regions"] = data;
            ViewData["defaultRegion"] = data.FirstOrDefault()?.RegionId;
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
    }
}
