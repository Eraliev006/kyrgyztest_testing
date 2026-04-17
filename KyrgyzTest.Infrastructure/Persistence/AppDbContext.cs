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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Login)
            .IsUnique();
        
        modelBuilder.Entity<Users>().HasData(new Users
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
            FullName = "Super Admin",
            Login = "superadmin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = UserRole.SuperAdmin,
            CreatedAt = DateTime.UtcNow
        });


        base.OnModelCreating(modelBuilder);
    }
    
}