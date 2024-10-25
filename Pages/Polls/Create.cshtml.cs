using Edap.Models;
using Edap.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages.Polls;

public class CreateModel : PageModel
{
    private readonly PollService _pollService;
    
    [BindProperty]
    public Poll Poll { get; set; }
    
    [BindProperty]
    [Required, MinLength(1)]
    public DateTime[] PollOptionDates { get; set; }
    
    public CreateModel(PollService pollService)
    {
        _pollService = pollService;
    }
    
    public async Task<IActionResult> OnPostAsync() {
        if (!ModelState.IsValid || Poll == null)
            return Page();
        
        await _pollService.CreatePollAsync(Poll, PollOptionDates);
        return RedirectToPage("Index", new { id = Poll.Id });
    }
}