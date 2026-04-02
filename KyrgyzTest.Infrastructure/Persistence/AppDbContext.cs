using Microsoft.EntityFrameworkCore;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Computer> Computers { get; set; }
    public DbSet<ExamSession> ExamSessions { get; set; }
    public DbSet<Candidate>  Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var computers = Enumerable.Range(1, 50).Select(i => new Computer
        {
            Id = Guid.NewGuid(),
            StationNumber = i,
            Status = ComputerStatus.Free,
            LastHeartbeat = DateTime.UtcNow,
        });
        modelBuilder.Entity<Computer>().HasData(computers);
    }
}