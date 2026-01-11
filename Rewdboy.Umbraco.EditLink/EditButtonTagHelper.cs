using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace Rewdboy.Umbraco.EditLink
{
    [HtmlTargetElement("umbraco-edit-button")]
    public class EditButtonTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        // För att bara injicera CSS en gång per request
        private const string CssInjectedKey = "Rewdboy.Umbraco.EditLink.CssInjected";

        public EditButtonTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IUmbracoContextAccessor umbracoContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        [HtmlAttributeName("model")]
        public IPublishedContent? Model { get; set; }

        /// <summary>
        /// Om true så injiceras en CSS-link (en gång per request).
        /// </summary>
        [HtmlAttributeName("inject-css")]
        public bool InjectCss { get; set; } = true;

        /// <summary>
        /// Om du vill override:a CSS-url.
        /// - I webbprojekt: "/css/editbutton.css"
        /// - I paket/RCL: "/_content/Rewdboy.Umbraco.EditLink/css/editbutton.css"
        /// </summary>
        [HtmlAttributeName("css-url")]
        public string? CssUrl { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; } = "Edit page";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var http = _httpContextAccessor.HttpContext;

            if (http is null || Model is null)
            {
                output.SuppressOutput();
                return;
            }

            // 1) Visa aldrig i preview
            if (IsPreviewRequest(http))
            {
                output.SuppressOutput();
                return;
            }

            // 2) Är vår egna auth-cookie giltig?
            var authResult = http.AuthenticateAsync(EditLinkComposer.Scheme).GetAwaiter().GetResult();
            if (!authResult.Succeeded)
            {
                output.SuppressOutput();
                return;
            }

            // 3) Inject CSS en gång per request (valfritt)
            if (InjectCss)
            {
                var cssUrl = CssUrl;

                // När du kör koden i webbprojekt (debug): lägg CSS i wwwroot/css/editbutton.css
                //cssUrl ??= "/css/editbutton.css";

                // När du går tillbaka till paket/RCL senare, byt till:
                 cssUrl ??= "/_content/Rewdboy.Umbraco.EditLink/css/editbutton.css";

                if (!http.Items.ContainsKey(CssInjectedKey))
                {
                    http.Items[CssInjectedKey] = true;
                    output.PreElement.AppendHtml($@"<link rel=""stylesheet"" href=""{cssUrl}"" />");
                }
            }

            // 4) Rendera som fristående element (så den aldrig kan wrapa sidan)
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "rewdboy-editlink-container");

            var editUrl = $"/umbraco/section/content/workspace/document/edit/{Model.Key:D}";

            output.Content.SetHtmlContent($@"
<a href=""{editUrl}""
   target=""_blank""
   rel=""noopener noreferrer""
   class=""edit-page-btn""
   title=""{HtmlEncode(Title)}"">
    <svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'
         style='width:20px;height:20px;fill:currentColor;'>
      <path d='M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z'/>
    </svg>
</a>");
        }

        private bool IsPreviewRequest(HttpContext http)
        {
            // Bästa sättet: UmbracoContext → PublishedRequest.IsPreview (kan variera per version)
            if (_umbracoContextAccessor.TryGetUmbracoContext(out var umbCtx))
            {
                var pr = umbCtx.PublishedRequest;
                if (pr is not null)
                {
                    var prop = pr.GetType().GetProperty("IsPreview");
                    if (prop?.PropertyType == typeof(bool))
                    {
                        var val = (bool?)prop.GetValue(pr);
                        if (val == true) return true;
                    }
                }
            }

            // Fallback: vanlig preview-flagga i querystring
            if (http.Request.Query.Keys.Any(k => k.Equals("umbPreview", StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        private static string HtmlEncode(string input)
        {
            // Minimal encoding för Title-attributet
            return input
                .Replace("&", "&amp;", StringComparison.Ordinal)
                .Replace("\"", "&quot;", StringComparison.Ordinal)
                .Replace("<", "&lt;", StringComparison.Ordinal)
                .Replace(">", "&gt;", StringComparison.Ordinal);
        }
    }
}
