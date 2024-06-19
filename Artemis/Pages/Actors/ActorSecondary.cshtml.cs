using Artemis.Dal.Services;
using Artemis.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace Artemis.Pages.Actors
{
    public class ActorSecondaryModel : BasePageModel
    {
        private readonly ArtemisDbContext db;
        private readonly IActorService actorService;
        private readonly IActorFilmService actorFilmService;
        private readonly IActorGalleryService actorGalleryService;

        [TempData]
        public string? StatusMessage { get; set; }

        public ActorViewModel Actor { get; set; } = new ActorViewModel();
        public ActorGalleryViewModel Gallery { get; set; } = new ActorGalleryViewModel();

        public ActorSecondaryModel(ArtemisDbContext context,
            IActorService actorService,
            IActorFilmService actorFilmService,
            IActorGalleryService actorGalleryService) : base(context)
        {
            db = context;
            this.actorService = actorService;
            this.actorFilmService = actorFilmService;
            this.actorGalleryService = actorGalleryService;
        }

        public IActionResult OnGet(int actorId)
        {
            if (actorId == 0)
            {
                string message = "You must first save the record before editing additional data.";
                return RedirectToPage("/Shared/DataError", new { message });
            }

            if (!YearsExist())
            {
                string message = "Cannot display the page because no years are found in the database.";
                return RedirectToPage("/Shared/DataError", new { message });
            }
            Actor = actorService.GetById(actorId) ?? new ActorViewModel();
            PopulateYears();
            PopulateActors();
            return Page();
        }

        #region Gallery grid

        public JsonResult OnPostGalleryRead([DataSourceRequest] DataSourceRequest request, int actorId)
        {
            IEnumerable<ActorGalleryViewModel> data = actorGalleryService.Read(actorId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostGalleryCreate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorGalleryViewModel> data, int actorId)
        {
            if (data != null && ModelState.IsValid)
            {
                actorGalleryService.CreateRange(data, actorId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostGalleryUpdate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorGalleryViewModel> data, int actorId)
        {
            if (data != null && ModelState.IsValid)
            {
                actorGalleryService.UpdateRange(data, actorId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostGalleryDestroy([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorGalleryViewModel> data)
        {
            if (data.Any())
            {
                actorGalleryService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Filmography grid

        public JsonResult OnPostFilmographyRead([DataSourceRequest] DataSourceRequest request, int actorId)
        {
            IEnumerable<ActorFilmViewModel> data = actorFilmService.Read(actorId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostFilmographyCreate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorFilmViewModel> data, int actorId)
        {
            if (data != null && ModelState.IsValid)
            {
                actorFilmService.CreateRange(data, actorId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostFilmographyUpdate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorFilmViewModel> data, int actorId)
        {
            if (data != null && ModelState.IsValid)
            {
                actorFilmService.UpdateRange(data, actorId);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostFilmographyDestroy([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ActorFilmViewModel> data)
        {
            if (data.Any())
            {
                actorFilmService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        #endregion

        public IActionResult OnGetGalleryRecord(int id)
        {
            ActorGalleryViewModel? model = actorGalleryService.GetById(id);

            return Partial("_GalleryEdit", model);
        }

        public IActionResult OnPostGalleryEdit(ActorGalleryViewModel model)
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
                ActorGallery? entity = db.ActorGallery.Find(model.GalleryId);
                if (entity != null)
                {
                    if (imageFile != null)
                    {
                        entity.Image = Common.FileToByteArray(imageFile);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();

                    model.ActorId = entity.ActorId;
                }
                StatusMessage = "Success: Gallery data update has been completed successfully.";
            }
            else
            {
                ActorGallery? entity = db.ActorGallery.Find(model?.GalleryId);
                model!.ActorId = entity?.ActorId;

                StatusMessage = "Error: ";
                StatusMessage = Errors(StatusMessage);
            }
            return RedirectToPage(new { actorId = model?.ActorId });
        }

    }
}
