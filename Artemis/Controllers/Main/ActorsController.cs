using Artemis.Dal.Services;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Controllers.Main
{
    public class ActorsController : Controller
    {
        private readonly ArtemisDbContext db;
        private readonly IQueryService queryService;

        public ActorsController(ArtemisDbContext context, IQueryService queryService)
        {
            db = context;
            this.queryService = queryService;
        }

        public IActionResult ActorGallery()
        {
            return View();
        }

        public ActionResult ActorInfo_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ActorInfoViewModel> data = queryService.GetActors();

            return Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        public IActionResult Images_Read([DataSourceRequest] DataSourceRequest request, int actorId)
        {
            List<ActorGalleryViewModel> ScrollViewItems = GetImages(actorId);

            return Json(ScrollViewItems.ToDataSourceResult(request));
        }

        public List<ActorGalleryViewModel> GetImages(int actorId)
        {
            var data = (from d in db.ActorGallery
                        where d.ActorId == actorId
                        select new ActorGalleryViewModel
                        {
                            GalleryId = d.GalleryId,
                            ActorId = d.ActorId,
                            Image = d.Image,
                            GalleryImage = d.Image == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Image),
                        }).ToList();
            return data;
        }
    }
}
