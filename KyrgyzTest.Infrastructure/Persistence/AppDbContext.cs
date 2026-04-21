using Microsoft.EntityFrameworkCore;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Users> Users { get; set; }
    public DbSet<Candidate>  Candidates { get; set; }
    public DbSet<MediaGroup> MediaGroups { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<SectionConfig> SectionConfigs { get; set; }
    public DbSet<TestVariant> TestVariants { get; set; }
    public DbSet<TestVariantQuestion> TestVariantQuestions { get; set; }
    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<CandidateAnswer> CandidateAnswers { get; set; }
    public DbSet<Result> Results { get; set; }
    
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
        
        modelBuilder.Entity<Question>()
            .HasOne(q => q.MediaGroup)
            .WithMany(m => m.Questions)
            .HasForeignKey(q => q.MediaGroupId)
            .IsRequired(false);

        modelBuilder.Entity<AnswerOption>()
            .HasOne(a => a.Question)
            .WithMany(q => q.AnswerOptions)
            .HasForeignKey(a => a.QuestionId);

        modelBuilder.Entity<TestVariantQuestion>()
            .HasOne(t => t.TestVariant)
            .WithMany(v => v.Questions)
            .HasForeignKey(t => t.TestVariantId);

        modelBuilder.Entity<TestVariantQuestion>()
            .HasOne(t => t.Question)
            .WithMany()
            .HasForeignKey(t => t.QuestionId);

        modelBuilder.Entity<SectionConfig>()
            .HasIndex(s => s.Section)
            .IsUnique();
        
        modelBuilder.Entity<Attempt>()
            .HasOne(a => a.Candidate)
            .WithMany()
            .HasForeignKey(a => a.CandidateId);

        modelBuilder.Entity<Attempt>()
            .HasOne(a => a.TestVariant)
            .WithMany()
            .HasForeignKey(a => a.TestVariantId);

        modelBuilder.Entity<CandidateAnswer>()
            .HasOne(a => a.Attempt)
            .WithMany()
            .HasForeignKey(a => a.AttemptId);

        modelBuilder.Entity<CandidateAnswer>()
            .HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId);

        modelBuilder.Entity<Result>()
            .HasOne(r => r.Attempt)
            .WithMany()
            .HasForeignKey(r => r.AttemptId);

        modelBuilder.Entity<Result>()
            .HasOne(r => r.Candidate)
            .WithMany()
            .HasForeignKey(r => r.CandidateId);


        base.OnModelCreating(modelBuilder);
    }
    
}