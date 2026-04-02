using Microsoft.EntityFrameworkCore;
using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Computer> Computers { get; set; }
    public DbSet<ExamSession> ExamSessions { get; set; }
    public DbSet<Candidate>  Candidates { get; set; }
}