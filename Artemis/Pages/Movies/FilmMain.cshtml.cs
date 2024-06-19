
using Artemis.Pages.Shared;

namespace Artemis.Pages.Movies
{
    public class FilmMainModel : BasePageModel
    {
        private readonly ArtemisDbContext db;
        private readonly IFilmService filmService;

        public FilmViewModel FilmViewModel { get; set; } = new FilmViewModel();

        [TempData]
        public string? StatusMessage { get; set; }

        public FilmMainModel(ArtemisDbContext context, IFilmService filmService) : base(context)
        {
            db = context;
            this.filmService = filmService;
        }

        public void OnGet()
        {
            PopulateLanguages();
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<FilmViewModel> data = filmService.Read();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix="models")] IEnumerable<FilmViewModel> data)
        {
            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    if (!filmService.Exists(item.Title))
                    {
                        filmService.Create(item);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Film with this title already exists. Operation cancelled.");
                    }
                }
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<FilmViewModel> data)
        {
            if (data != null && ModelState.IsValid)
            {
                filmService.UpdateRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<FilmViewModel> data)
        {
            if (data.Any())
            {
                filmService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public IActionResult OnGetFilmRecord(int id)
        {
            FilmViewModel? model = filmService.GetById(id);

            return Partial("_FilmEdit", model);
        }

        public IActionResult OnPostFilmEdit(FilmViewModel model)
        {
            IFormFile? imageFile = null;

            if (Request.Form.Files.Any())
            {
                imageFile = Request.Form.Files[0];
                if (imageFile != null)
                {
                    if (imageFile.Length >= Common.IMAGE_MAXSIZE)
                    {
                        ModelState.AddModelError(string.Empty, $"Uploaded image size must be less than {Common.IMAGE_MAXSIZE / 1024}KB.");
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
                Film? entity = db.Film.Find(model.FilmId);
                if (entity != null)
                {
                    entity.Title = model.Title;
                    entity.PlotSummary = model.PlotSummary;
                    entity.TrailerLink = model.TrailerLink;
                    entity.ReleaseDate = model.ReleaseDate;
                    entity.RunningTime = model.RunningTime;
                    entity.Budget = model.Budget;
                    entity.BoxOffice = model.BoxOffice;
                    entity.Director = model.Director;
                    entity.Producer = model.Producer;
                    entity.Language = model.Language;
                    entity.Rating = model.Rating;
                    entity.Genres = model.SelectedGenres != null ? Common.GenreValuesToString(model.SelectedGenres) : string.Empty;
                    if (imageFile != null)
                    {
                        entity.Poster = Common.FileToByteArray(imageFile);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }

                StatusMessage = "Success: Film data update has been completed successfully.";
                //return RedirectToPage();
            }
            else
            {
                StatusMessage = "Error: ";
                StatusMessage = Errors(StatusMessage);
                //PopulateLanguages();    // must call this, otherwise null exception in foreign key column
                //return Page();
            }
            return RedirectToPage();
        }
    }
}
