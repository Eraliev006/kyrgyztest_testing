using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class ResultResponseDto
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Guid CandidateId { get; set; }
    public LanguageLevel Level { get; set; }
    public int GrammarScore { get; set; }
    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
    public int TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }
}
