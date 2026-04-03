using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IExamService
{
    public Task<CandidateResponseDto> RegisterCandidateAsync(RegisterCandidateDto candidate);
    public Task<AssignComputerResponseDto> AssignComputerAsync(string examCode);
    public Task<ExamLoginResponseDto> LoginAsync(ExamLoginDto examLogin);
    
    public Task<ExamLoginResponseDto> GetSessionAsync(string examCode);
}