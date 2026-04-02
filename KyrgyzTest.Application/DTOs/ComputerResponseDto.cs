using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class ComputerResponseDto
{
    public int StationNumber { get; set; }
    public ComputerStatus Status {get; set; }
    public string? ExamCode {get; set;}
}