using Edap.Models;
using Microsoft.EntityFrameworkCore;

namespace Edap.Services;

public class PollService
{
    private readonly PollContext _dbContext;

    public PollService(PollContext pc)
    {
        _dbContext = pc;
    }
    
    public async Task<Poll> GetPollAsync(int id)
        => await _dbContext.Polls
            .Where(x => x.Id == id)
            .IncludePollAssocs()
            .SingleOrDefaultAsync();

    public async Task<Poll> GetPollFromGuidAsync(Guid pollGuid)
        => await _dbContext.Polls
            .Where(x => x.PollGuid == pollGuid)
            .IncludePollAssocs()
            .SingleOrDefaultAsync();
    
    public async Task CreatePollAsync(Poll poll, DateTime[] pollOptionDates)
    {
        foreach (var date in pollOptionDates) {
            var pollOption = new PollOption { Date = date };
            poll.Options.Add(pollOption);
        }

        await _dbContext.Polls.AddAsync(poll);
        await _dbContext.SaveChangesAsync();
    }

    public async Task VoteAsync(Guid pollGuid, string username, string email, int[] selectedPollOptions)
    {
        var poll = await GetPollFromGuidAsync(pollGuid);

        // Remove old selection
        var existingUserVote = poll.UserVotes.FirstOrDefault(x => x.Email == email);
        if (existingUserVote != null)
            _dbContext.UserVotes.Remove(existingUserVote);

        // Add new selection
        var userVote = new UserVote {
            Email = email,
            Username = username,
            Poll = poll,
            Options = poll.Options.Where(x => selectedPollOptions.Contains(x.Id)).ToList()
        };
        _dbContext.UserVotes.Add(userVote);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int pollId)
    {
        var poll = await GetPollAsync(pollId);
        _dbContext.PollOptions.RemoveRange(poll.Options);
        _dbContext.Polls.Remove(poll);

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task ToggleCloseAsync(int pollId)
    {
        var poll = await GetPollAsync(pollId);
        poll.CloseDate = poll.CloseDate.HasValue ?
            null : DateTime.Now;
        
        await _dbContext.SaveChangesAsync();
    }
}

public static class PollExtensions
{
    public static IQueryable<Poll> IncludePollAssocs(this IQueryable<Poll> pollQuery)
        => pollQuery
            .Include(x => x.Options)
            .ThenInclude(x => x.UserVotes);
}