using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Website.Controllers;

namespace Rewdboy.Umbraco.EditLink.Controllers
{
    // Auto-route:
    // GET /umbraco/surface/EditLinkAuthSurface/Ping
    public class EditLinkAuthSurfaceController : SurfaceController
    {
        private readonly IBackOfficeSecurityAccessor _security;
        private readonly IUserService _userService;

        public EditLinkAuthSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IBackOfficeSecurityAccessor security,
            IUserService userService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _security = security;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
        public IActionResult Ping()
        {
            var current = _security.BackOfficeSecurity?.CurrentUser;
            if (current is null)
                return Unauthorized();

            // Valfritt: begränsa till vissa grupper
            var allowedGroupAliases = new[] { "admin", "editor" };

            var fullUser = _userService.GetUserById(current.Id);
            var groups = fullUser?.Groups?.Select(g => g.Alias).ToArray() ?? Array.Empty<string>();

            if (!groups.Intersect(allowedGroupAliases, StringComparer.OrdinalIgnoreCase).Any())
                return Forbid();

            return Ok();
        }
    }
}
