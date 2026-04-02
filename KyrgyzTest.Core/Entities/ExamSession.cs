namespace KyrgyzTest.Core.Entities;

public class ExamSession
{
    public Guid Id { get; set; }
    public string ExamCode { get; set; }
    public int StationNumber { get; set; }
    public DateTime StartAt { get; set; }
    public bool IsCompleted { get; set; }
}