using System.Globalization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Rewdboy.Umbraco.EditLink
{
    public class EditLinkCookieService
    {
        private readonly IDataProtector _protector;

        // Kort TTL gör att en "gammal" cookie dör snabbt om sessionen dör utan logout
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(30);

        public EditLinkCookieService(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector(EditLinkCookie.ProtectorPurpose);
        }

        public void SetSignedCookie(HttpContext ctx, int userId, TimeSpan? ttl = null)
        {
            var expiresUtc = DateTimeOffset.UtcNow.Add(ttl ?? DefaultTtl);

            // payload: userId|expiresUnixSeconds
            var payload = $"{userId.ToString(CultureInfo.InvariantCulture)}|{expiresUtc.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)}";
            var protectedValue = _protector.Protect(payload);

            ctx.Response.Cookies.Append(
                EditLinkCookie.Name,
                protectedValue,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,                 // vi läser serverside i TagHelper
                    Secure = ctx.Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                    Expires = expiresUtc
                });
        }

        public void DeleteCookie(HttpContext ctx)
        {
            ctx.Response.Cookies.Delete(EditLinkCookie.Name, new CookieOptions { Path = "/" });
        }

        public bool IsCookieValid(HttpContext ctx)
        {
            if (!ctx.Request.Cookies.TryGetValue(EditLinkCookie.Name, out var value) || string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                var unprotected = _protector.Unprotect(value);
                var parts = unprotected.Split('|');
                if (parts.Length != 2)
                    return false;

                if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    return false;

                if (!long.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var expUnix))
                    return false;

                var expiresUtc = DateTimeOffset.FromUnixTimeSeconds(expUnix);
                return expiresUtc > DateTimeOffset.UtcNow;
            }
            catch
            {
                // Tampered/invalid/old key ring etc.
                return false;
            }
        }
    }
}
