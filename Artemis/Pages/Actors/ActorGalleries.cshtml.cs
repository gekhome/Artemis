using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Actors
{
    public class ActorGalleriesModel : PageModel
    {
        private readonly ArtemisDbContext db;

        public ActorGalleriesModel(ArtemisDbContext context)
        {
            db = context;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostActors_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ActorInfoViewModel> data = GetActorsInfo();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostReadImages([DataSourceRequest] DataSourceRequest request, int actorId)
        {
            List<ActorGalleryViewModel> ScrollViewItems = GetImages(actorId);

            return new JsonResult(ScrollViewItems.ToDataSourceResult(request));
        }

        public IEnumerable<ActorInfoViewModel> GetActorsInfo()
        {
            var data = (from d in db.ActorInfo
                        orderby d.FullName
                        select new ActorInfoViewModel
                        {
                            ActorId = d.ActorId,
                            FullName = d.FullName,
                            BirthPlace = d.BirthPlace,
                            Country = d.Country,
                            Dob = d.Dob,
                            Age = d.Age,
                            Height = d.Height,
                            Occupations = d.Occupations,
                            Rating = d.Rating,
                            BioSummary = d.BioSummary,
                            Photo = d.Photo,
                            PhotoImage = d.Photo == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.Photo),
                        }).ToList();
            return data;
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
