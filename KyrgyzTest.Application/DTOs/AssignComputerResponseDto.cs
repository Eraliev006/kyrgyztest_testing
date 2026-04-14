namespace KyrgyzTest.Application.DTOs;

public class AssignComputerResponseDto
{
    public string ExamCode { get; set; }
    public Guid ComputerId { get; set; }
    public int StationNumber { get; set; }
    public DateTime StartAt { get; set; }
}