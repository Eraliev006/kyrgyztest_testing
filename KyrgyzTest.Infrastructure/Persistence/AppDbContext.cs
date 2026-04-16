using Microsoft.EntityFrameworkCore;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<ExamSession> ExamSessions { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Candidate>  Candidates { get; set; }
    
}