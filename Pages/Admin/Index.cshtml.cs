using Edap.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Edap.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public DbSet<Poll> Polls { get; set; }

    public IndexModel(PollContext pc, ILogger<IndexModel> logger)
    {
        Polls = pc.Polls;
        _logger = logger;
    }
}