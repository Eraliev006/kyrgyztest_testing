namespace KyrgyzTest.Application.DTOs;

public class ExaminerQueueItemDto
{
    public Guid AttemptId { get; set; }
    public Guid CandidateId { get; set; }
    public string CandidateFullName { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public int UngradedCount { get; set; }
}

public class ExaminerReviewDto
{
    public Guid AttemptId { get; set; }
    public string CandidateFullName { get; set; } = string.Empty;
    public List<ExaminerAnswerDto> Answers { get; set; } = new();
}

public class ExaminerAnswerDto
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? AudioAnswerUrl { get; set; }
    public int? Score { get; set; }
}

public class GradeAnswerDto
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public int Score { get; set; }
}
