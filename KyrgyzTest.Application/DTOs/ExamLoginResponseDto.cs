namespace KyrgyzTest.Application.DTOs;

public class ExamLoginResponseDto
{
    public string ExamCode { get; set; }
    public string FullName { get; set; }
    public int StationNumber { get; set; }
    public DateTime StartAt { get; set; }
}