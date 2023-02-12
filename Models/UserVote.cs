using System.ComponentModel.DataAnnotations;

namespace Edap.Models;

public class UserVote
{
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public Poll Poll { get; set; }
    
    [Required]
    public ICollection<PollOption> Options { get; set; }
}