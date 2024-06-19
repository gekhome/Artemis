using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;

namespace Artemis.Infrastructure.Notification
{
    public class AlertsTagHelper : TagHelper
    {
        private const string AlertKey = "Alert";

        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        protected ITempDataDictionary TempData => ViewContext.TempData;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            if (TempData[AlertKey] == null)
                TempData[AlertKey] = JsonConvert.SerializeObject(new HashSet<Alert>());

            var alerts = JsonConvert.DeserializeObject<ICollection<Alert>>(TempData[AlertKey]?.ToString()!);

            var html = string.Empty;
            string icon = string.Empty;

            foreach (var alert in alerts!)
            {
                switch (alert.Type)
                {
                    case "alert-success":
                        icon = "<i class='fa-solid fa-circle-check fa-lg'></i>";
                        break;
                    case "alert-info":
                        icon = "<i class='fa-solid fa-circle-info fa-lg'></i>";
                        break;
                    case "alert-warning":
                        icon = "<i class='fa-solid fa-triangle-exclamation fa-lg'></i>";
                        break;
                    case "alert-danger":
                        icon = "<i class='fa-solid fa-triangle-exclamation fa-lg'></i>";
                        break;
                    default:
                        break;
                }

                html += $"<div class='alert {alert.Type} alert-dismissible fade show d-flex' id='inner-alert' role='alert'>" +     
                            $"<button type='button' class='btn-close rounded-circle' data-bs-dismiss='alert' aria-label='Close'>" +
                            $"</button>" +
                            $"<span class='flex-shrink-0 me-2'>{icon}</span>" +
                            $"{alert.Message}" +
                        $"</div>";
            }
            output.Content.SetHtmlContent(html);
        }
    }
}
