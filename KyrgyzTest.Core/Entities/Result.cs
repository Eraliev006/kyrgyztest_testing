using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Result
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;
    public LanguageLevel Level { get; set; }
    public int GrammarScore { get; set; }
    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
    public int TotalScore { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}