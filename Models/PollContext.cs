using Microsoft.EntityFrameworkCore;

namespace Edap.Models;

public class PollContext : DbContext
{
    public PollContext(DbContextOptions<PollContext> options)
        : base (options) { }

    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<UserVote> UserVotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Poll>().ToTable("Poll");
        modelBuilder.Entity<PollOption>().ToTable("PollOption");
        modelBuilder.Entity<UserVote>().ToTable("UserVote");

        modelBuilder.Entity<UserVote>()
            .HasMany(x => x.Options)
            .WithMany(x => x.UserVotes);
        
        modelBuilder.Entity<Poll>()
            .HasMany(x => x.UserVotes)
            .WithOne(x => x.Poll);
    }
}