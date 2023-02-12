using Edap.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(PollContext pc, ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}