using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Web;

namespace Edap.Pages;

public class LogoutModel : PageModel
{
    private readonly IConfiguration _config;

    public LogoutModel(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IActionResult> OnGet()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");
        var idpHost = _config["IdentityProvider:Authority"];
        var redirectUri = $"{Request.Scheme}://{Request.Host}";

        SignOut("cookie", "oidc");

        // To ensure that all auth. cookies are being deleted, since ASP.NET Core uses the ChunkingCookieManager for cookie authentication by default.
        new ChunkingCookieManager().DeleteCookie(HttpContext, _config["IdentityProvider:CookieName"], new CookieOptions());

        return Redirect(idpHost + "oidc/logout?id_token_hint=" + idToken + "&post_logout_redirect_uri=" + HttpUtility.UrlEncode(redirectUri));
    }
}