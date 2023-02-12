using Edap.Models;
using Edap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages.Polls;

public class IndexModel : PageModel
{
    private readonly PollService _pollService;
    
    public Poll Poll { get; set; }
    
    public IndexModel(PollService pollService)
    {
        _pollService = pollService;
    }
    
    public async Task OnGetAsync(int id)
    {
        Poll = await _pollService.GetPollAsync(id);
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _pollService.DeleteAsync(id);
        return RedirectToPage("/Index");
    }
    
    public async Task<IActionResult> OnPostToggleCloseAsync(int id)
    {
        await _pollService.ToggleCloseAsync(id);
        return RedirectToPage("/Polls/Index", new { id = id.ToString() });
    }
}