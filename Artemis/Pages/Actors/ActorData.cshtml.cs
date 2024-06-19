using Artemis.Dal.Services;
using Artemis.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Actors
{
    public class ActorDataModel : BasePageModel
    {
        private readonly ArtemisDbContext db;
        private readonly IActorService actorService;

        public ActorViewModel? Actor { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        public ActorDataModel(ArtemisDbContext context, IActorService actorService) : base(context)
        {
            db = context;
            this.actorService = actorService;
        }

        public void OnGet()
        {
            PopulateCountries();
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ActorViewModel> data = actorService.Read();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ActorViewModel> data)
        {
            if (data != null && ModelState.IsValid)
            {
                actorService.CreateRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ActorViewModel> data)
        {
            if (data != null && ModelState.IsValid)
            {
                actorService.UpdateRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ActorViewModel> data)
        {
            if (data.Any())
            {
                actorService.DestroyRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public IActionResult OnGetActorRecord(int id)
        {
            ActorViewModel? model = actorService.GetById(id);

            return Partial("_ActorEdit", model);
        }

        public IActionResult OnPostActorEdit(ActorViewModel model)
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
                Actor? entity = db.Actor.Find(model.ActorId);
                if (entity != null)
                {
                    entity.FullName = model.FullName;
                    entity.BirthDate = model.BirthDate;
                    entity.BirthPlace = model.BirthPlace;
                    entity.Country = model.Country;
                    entity.Gender = model.Gender;
                    entity.Height = model.Height;
                    entity.Rating = model.Rating;
                    entity.Occupations = model.Occupations;
                    entity.BioSummary = model.BioSummary;
                    if (imageFile != null)
                    {
                        entity.Photo = Common.FileToByteArray(imageFile);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
                StatusMessage = "Success: Actor data update has been completed successfully.";
            }
            else
            {
                StatusMessage = "Error: ";
                StatusMessage = Errors(StatusMessage);
            }
            return RedirectToPage();
        }

    }
}
