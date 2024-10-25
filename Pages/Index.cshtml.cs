using Edap.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages;

public class IndexModel : PageModel
{
    public IndexModel(PollContext pc)
    {
    }

    public void OnGet()
    {
    }
}