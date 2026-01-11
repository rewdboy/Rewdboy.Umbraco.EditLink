using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Authorization;

namespace Rewdboy.Umbraco.EditLink
{
    // URL:
    // /umbraco/backoffice/Rewdboy/EditLinkAuth/Ping
    [PluginController("Rewdboy")]
    [Route("/umbraco/api/rewdboy/editlinkauth")]
    [Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
    public class EditLinkAuthController : ControllerBase
    {

        // GET /umbraco/api/rewdboy/editlinkauth/ping
        [HttpGet("ping")]
        public IActionResult Ping() => Ok();


        //private readonly IBackOfficeSecurityAccessor _security;
        //private readonly IUserService _userService;

        //public EditLinkAuthController(
        //    IBackOfficeSecurityAccessor security,
        //    IUserService userService)
        //{
        //    _security = security;
        //    _userService = userService;
        //}

        ///// <summary>
        ///// Ping endpoint used by frontend JS to verify
        ///// if a backoffice user is logged in.
        ///// </summary>
        //[HttpGet]
        //public IActionResult Ping()
        //{
        //    var currentUser = _security.BackOfficeSecurity?.CurrentUser;
        //    if (currentUser is null)
        //        return Unauthorized();

        //    // 🔐 Valfritt: begränsa till specifika backoffice-grupper
        //    // Ta bort detta block om ALLA inloggade ska få knappen
        //    var allowedGroupAliases = new[] { "admin", "editor" };

        //    var fullUser = _userService.GetUserById(currentUser.Id);
        //    var userGroups = fullUser?.Groups?
        //        .Select(g => g.Alias)
        //        .ToArray() ?? Array.Empty<string>();

        //    if (!userGroups.Intersect(allowedGroupAliases, StringComparer.OrdinalIgnoreCase).Any())
        //        return Forbid();

        //    return Ok();
        //}
    }
}
