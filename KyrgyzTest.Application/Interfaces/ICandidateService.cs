using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface ICandidateService
{
    Task<CandidateResponseDto> CreateAsync(CreateCandidateDto dto);
    Task<CandidateResponseDto?> GetByIdAsync(Guid id);
    Task<CandidateResponseDto?> GetByInnAsync(string inn);
    Task<CandidateResponseDto?> GetByAccessCodeAsync(string accessCode);
    Task<List<CandidateResponseDto>> GetAllAsync();
    Task<CandidateResponseDto> AllowAccessAsync(Guid id);
    Task<CandidateResponseDto> DenyAccessAsync(Guid id);
    Task<CandidateResponseDto> BlockAsync(Guid id, BlockCandidateDto dto);
    Task<CandidateResponseDto> UploadPhotoAsync(Guid id, string photo);
    Task DeleteAsync(Guid id);
}