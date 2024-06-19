using Artemis.Dal.Models;
using Artemis.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Movies
{
    public class FilmSecondaryModel : BasePageModel
    {
        private readonly IFilmService filmService;
        private readonly IFilmCountryService filmCountryService;
        private readonly IFilmCompanyService filmCompanyService;
        private readonly IFilmActorService filmActorService;

        [TempData]
        public string? StatusMessage { get; set; }

        public FilmViewModel Film { get; set; } = new FilmViewModel();

        public FilmSecondaryModel(ArtemisDbContext context, 
            IFilmService filmService, 
            IFilmCountryService filmCountryService, 
            IFilmCompanyService filmCompanyService,
            IFilmActorService filmActorService) : base(context)
        {
            this.filmService = filmService;
            this.filmCountryService = filmCountryService;
            this.filmCompanyService = filmCompanyService;
            this.filmActorService = filmActorService;
        }

        public IActionResult OnGet(int filmId)
        {
            if (filmId == 0)
            {
                string message = "You must first save the record before editing additional data.";
                return RedirectToPage("/Shared/DataError", new { message });
            }

            if (!CountriesExist() || !CompaniesExist() || !ActorsExist())
            {
                string message = "Cannot display the page because no countries or companies or actors are found.";
                return RedirectToPage("/Shared/DataError", new { message });
            }
            Film = filmService.GetById(filmId) ?? new FilmViewModel();
            PopulateCountries();
            PopulateCompanies();
            PopulateActors();

            return Page();
        }

        #region film countries grid

        public JsonResult OnPostCountryRead([DataSourceRequest] DataSourceRequest request, int filmId)
        {
            IEnumerable<FilmCountryViewModel> data = filmCountryService.Read(filmId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCountryCreate([DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<FilmCountryViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmCountryService.CreateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCountryUpdate([DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<FilmCountryViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmCountryService.UpdateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCountryDestroy([DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<FilmCountryViewModel> data)
        {
            if (data.Any())
            {
                filmCountryService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region film companies grid

        public JsonResult OnPostCompanyRead([DataSourceRequest] DataSourceRequest request, int filmId)
        {
            IEnumerable<FilmCompanyViewModel> data = filmCompanyService.Read(filmId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCompanyCreate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmCompanyViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmCompanyService.CreateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCompanyUpdate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmCompanyViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmCompanyService.UpdateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCompanyDestroy([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmCompanyViewModel> data)
        {
            if (data.Any())
            {
                filmCompanyService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region film actors grid

        public JsonResult OnPostActorRead([DataSourceRequest] DataSourceRequest request, int filmId)
        {
            IEnumerable<FilmActorViewModel> data = filmActorService.Read(filmId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostActorCreate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmActorViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmActorService.CreateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostActorUpdate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmActorViewModel> data, int filmId)
        {
            if (data != null && ModelState.IsValid)
            {
                filmActorService.UpdateRange(data, filmId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostActorDestroy([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<FilmActorViewModel> data)
        {
            if (data.Any())
            {
                filmActorService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }


        #endregion

    }
}
