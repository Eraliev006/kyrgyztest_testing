using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IExamService
{
    public Task<CandidateResponseDto> RegisterCandidateAsync(RegisterCandidateDto candidate);
    public Task<AssignComputerResponseDto> AssignComputerAsync(string examCode);
}