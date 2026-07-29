using Microsoft.EntityFrameworkCore;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Users> Users { get; set; }
    public DbSet<Candidate>  Candidates { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<MediaGroup> MediaGroups { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<SectionConfig> SectionConfigs { get; set; }
    public DbSet<TestVariant> TestVariants { get; set; }
    public DbSet<TestVariantQuestion> TestVariantQuestions { get; set; }
    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<CandidateAnswer> CandidateAnswers { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<CompletedSection> CompletedSections { get; set; }
    public DbSet<SectionTiming> SectionTimings { get; set; }
    public DbSet<ManualGrade> ManualGrades { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    public DbSet<ExamAccessSettings> ExamAccessSettings { get; set; }

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
            PasswordHash = "$2a$11$sOqeCeQBLHNVc9dJp3HB5uop0mKLiXdJw8CGFyp7KBTn4kJM3I49i",
            Role = UserRole.SuperAdmin,
            CreatedAt = new DateTime(2026, 5, 16, 20, 1, 14, 887, DateTimeKind.Utc).AddTicks(780)
        });
        
        modelBuilder.Entity<Candidate>()
            .HasOne(c => c.Organization)
            .WithMany()
            .HasForeignKey(c => c.OrganizationId)
            .IsRequired(false);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.MediaGroup)
            .WithMany(m => m.Questions)
            .HasForeignKey(q => q.MediaGroupId)
            .IsRequired(false);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Topic)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TopicId)
            .IsRequired(false);

        modelBuilder.Entity<Topic>().HasData(
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000001"), Name = "Зат атооч", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000002"), Name = "Сын атооч", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000003"), Name = "Этиш", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000004"), Name = "Ат атооч", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000005"), Name = "Сан атооч", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000006"), Name = "Тактооч", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000007"), Name = "Байламта", SectionType = SectionType.Grammar },
            new Topic { Id = Guid.Parse("a0000001-0000-0000-0000-000000000008"), Name = "Жалгоо", SectionType = SectionType.Grammar }
        );

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


        modelBuilder.Entity<CompletedSection>()
            .HasOne(cs => cs.Attempt)
            .WithMany()
            .HasForeignKey(cs => cs.AttemptId);

        modelBuilder.Entity<SectionTiming>()
            .HasOne(st => st.Attempt)
            .WithMany()
            .HasForeignKey(st => st.AttemptId);

        modelBuilder.Entity<ManualGrade>()
            .HasOne(g => g.Attempt)
            .WithMany()
            .HasForeignKey(g => g.AttemptId);

        modelBuilder.Entity<ManualGrade>()
            .HasOne(g => g.Question)
            .WithMany()
            .HasForeignKey(g => g.QuestionId);

        modelBuilder.Entity<ManualGrade>()
            .HasOne(g => g.GradedBy)
            .WithMany()
            .HasForeignKey(g => g.GradedByUserId);

        modelBuilder.Entity<Candidate>()
            .HasIndex(c => c.AccessCode)
            .IsUnique();

        modelBuilder.Entity<Candidate>()
            .HasIndex(c => c.Inn)
            .IsUnique();

        modelBuilder.Entity<CompletedSection>()
            .HasIndex(cs => new { cs.AttemptId, cs.Section })
            .IsUnique();

        modelBuilder.Entity<SectionTiming>()
            .HasIndex(st => new { st.AttemptId, st.Section })
            .IsUnique();

        modelBuilder.Entity<ManualGrade>()
            .HasIndex(g => new { g.AttemptId, g.QuestionId })
            .IsUnique();

        modelBuilder.Entity<CandidateAnswer>()
            .HasIndex(ca => new { ca.AttemptId, ca.QuestionId })
            .IsUnique();

        modelBuilder.Entity<Result>()
            .HasIndex(r => r.AttemptId)
            .IsUnique();

        modelBuilder.Entity<PasswordResetRequest>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<PasswordResetRequest>()
            .HasOne(r => r.ReviewedBy)
            .WithMany()
            .HasForeignKey(r => r.ReviewedByUserId)
            .IsRequired(false);

        modelBuilder.Entity<ExamAccessSettings>()
            .HasOne(s => s.UpdatedBy)
            .WithMany()
            .HasForeignKey(s => s.UpdatedByUserId)
            .IsRequired(false);

        base.OnModelCreating(modelBuilder);
    }
    
}