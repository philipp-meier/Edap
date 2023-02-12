using System.ComponentModel.DataAnnotations;
using Edap.Models;
using Edap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edap.Pages.Polls;

public class ParticipateModel : PageModel
{
    private readonly PollService _pollService;
    
    [Required]
    public Poll Poll { get; set; }
    
    [BindProperty]
    [Required]
    public string Username { get; set; }

    [BindProperty]
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [BindProperty]
    [Required, MinLength(1)]
    public int[] SelectedPollOptionIds { get; set; }
    
    public ParticipateModel(PollService pollService)
    {
        _pollService = pollService;
    }
    
    public async Task OnGetAsync(Guid pollGuid) {
        Poll = await _pollService.GetPollFromGuidAsync(pollGuid);
    }

    public async Task<IActionResult> OnPostAsync(Guid pollGuid) {
        Poll = await _pollService.GetPollFromGuidAsync(pollGuid);
        if (ModelState.IsValid)
            await _pollService.VoteAsync(pollGuid, Username, Email, SelectedPollOptionIds);

        return Page();
    }
}