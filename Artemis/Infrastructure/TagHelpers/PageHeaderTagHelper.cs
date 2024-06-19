using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;

namespace Artemis.Infrastructure.TagHelpers
{
    public class PageHeaderTagHelper : TagHelper
    {
        private readonly HtmlEncoder _htmlEncoder;
        public PageHeaderTagHelper(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        /// <summary>
        /// Show the current <see cref="Environment.MachineName"/>. true by default
        /// </summary>
        [HtmlAttributeName("add-title")]
        public string Title { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            string innerHtml = $"<h3 class=\"d-flex float-start\" style=\"font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">{Title}</h3>";

            var sb = new StringBuilder();
            sb.Append(innerHtml);
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
