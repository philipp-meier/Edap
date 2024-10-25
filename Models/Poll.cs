using System.ComponentModel.DataAnnotations;

namespace Edap.Models;

public class Poll
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime? CloseDate { get; set; }
    public Guid PollGuid { get; set; } = Guid.NewGuid();
    public ICollection<UserVote> UserVotes { get; set; } = new List<UserVote>();
    public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
}