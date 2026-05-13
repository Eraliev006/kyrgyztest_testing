namespace KyrgyzTest.Application.DTOs;

public class SaveAnswerDto
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public string? OrderedAnswer { get; set; }
}
