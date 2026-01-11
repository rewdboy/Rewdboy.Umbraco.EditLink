using Microsoft.AspNetCore.Builder;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Rewdboy.Umbraco.EditLink
{
    public class EditLinkPipelineFilter : IUmbracoPipelineFilter
    {
        public string Name => "Rewdboy.Umbraco.EditLink.CookieRefresh";

        public void OnPrePipeline(IApplicationBuilder app) { }

        public void OnPostPipeline(IApplicationBuilder app)
        {
            // Kör efter Umbraco byggt sin pipeline
            app.UseMiddleware<EditLinkAuthCookieRefreshMiddleware>();
        }

        public void OnEndpoints(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}
