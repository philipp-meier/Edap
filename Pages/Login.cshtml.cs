using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages;

public class LoginModel : PageModel
{
    public IActionResult OnGet()
    {
        // Auth. process is triggered by the razor page conventions.
        return RedirectToPage("Index");
    }
}