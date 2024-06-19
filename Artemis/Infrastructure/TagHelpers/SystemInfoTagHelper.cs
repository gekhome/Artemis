﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;

namespace Artemis.Infrastructure.TagHelpers
{
    public class SystemInfoTagHelper : TagHelper
    {
        private readonly HtmlEncoder _htmlEncoder;
        public SystemInfoTagHelper(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        /// <summary>
        /// Show the current <see cref="Environment.MachineName"/>. true by default
        /// </summary>
        [HtmlAttributeName("add-machine")]
        public bool IncludeMachine { get; set; } = true;

        /// <summary>
        /// Show the current OS
        /// </summary>
        [HtmlAttributeName("add-os")]
        public bool IncludeOS { get; set; } = true;

        [HtmlAttributeName("add-processors")]
        public bool IncludeProcessors { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";                    // Replaces <system-info> with <div>
            output.TagMode = TagMode.StartTagAndEndTag;  // <div> is not self closing


            var sb = new StringBuilder();
            if (IncludeMachine)
            {
                sb.Append(" <strong>Machine</strong> ");
                sb.Append(_htmlEncoder.Encode(Environment.MachineName));
            }
            if (IncludeOS)
            {
                sb.Append(" <strong>OS</strong> ");
                sb.Append(_htmlEncoder.Encode(RuntimeInformation.OSDescription));
            }
            if (IncludeProcessors)
            {
                sb.Append(" <strong>Processors</strong> ");
                sb.Append(_htmlEncoder.Encode(Environment.ProcessorCount.ToString()));
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
