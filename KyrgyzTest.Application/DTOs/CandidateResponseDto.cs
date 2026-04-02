using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class CandidateResponseDto
{
    public string ExamCode {get; set;}
    public string FullName {get; set;}
    public CandidateCategory Category {get; set;}
    public DateTime RegisteredAt {get; set;}
}