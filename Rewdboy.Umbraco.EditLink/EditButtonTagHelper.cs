using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;
using System.Linq;

namespace Rewdboy.Umbraco.EditLink.TagHelpers;

[HtmlTargetElement("umbraco-edit-button")]
public class EditButtonTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditButtonTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Vi skickar in Model (IPublishedContent) via ett attribut
    [HtmlAttributeName("model")]
    public IPublishedContent? Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model == null) return;

        // Kontrollera inloggning
        var isLoggedIntoBackoffice = _httpContextAccessor.HttpContext?.Request.Cookies
            .Any(x => x.Key.StartsWith("UMB_UCONTEXT")) ?? false;

        if (!isLoggedIntoBackoffice)
        {
            output.SuppressOutput(); // Rendera ingenting om man inte är inloggad
            return;
        }

        // Konfigurera själva taggen
        output.TagName = "a"; // Gör om <umbraco-edit-button> till <a>
        output.Attributes.SetAttribute("href", $"/umbraco/section/content/workspace/document/edit/{Model.Key}");
        output.Attributes.SetAttribute("target", "_blank");
        output.Attributes.SetAttribute("rel", "noopener noreferrer");
        output.Attributes.SetAttribute("class", "edit-page-btn");
        output.Attributes.SetAttribute("title", "Redigera sida");

        output.PostElement.AppendHtml("<link rel=\"stylesheet\" href=\"/_content/Rewdboy.Umbraco.EditLink/css/editbutton.css\" />"
    );

        // Lägg till SVG-ikonen inuti <a>-taggen
        output.Content.SetHtmlContent(@"
            <svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512' style='width:20px;height:20px;fill:currentColor;'>
                <path d='M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z'/>
            </svg>");
    }
}