using Edap.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Edap.Pages.Admin;

public class IndexModel(PollContext pc) : PageModel
{
    public DbSet<Poll> Polls { get; set; } = pc.Polls;
}