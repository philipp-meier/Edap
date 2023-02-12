using System.ComponentModel.DataAnnotations;

namespace Edap.Models;

public class PollOption
{
    public int Id { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    public ICollection<UserVote> UserVotes { get; set; }
    
    public PollOption() {
        UserVotes = new List<UserVote>();
    }
}