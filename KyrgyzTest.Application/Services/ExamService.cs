using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ExamService: IExamService
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IComputerRepository _computerRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    
    public ExamService(ICandidateRepository candidateRepository, IComputerRepository computerRepository,
        IExamSessionRepository examSessionRepository)
    {
        _candidateRepository = candidateRepository;
        _computerRepository = computerRepository;
        _examSessionRepository = examSessionRepository;
    }

    public async Task<CandidateResponseDto> RegisterCandidateAsync(RegisterCandidateDto dto)
    {
        var date = DateTime.Now.ToString("yyMMdd");
        var random = new Random().Next(1, 9999).ToString("D4");
        var examCode = $"{date}-{random}";
        
        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            PassportNumber = dto.PassportNumber,
            Category = dto.Category,
            ExamCode = examCode,
            RegisteredAt = DateTime.UtcNow
        };
        await _candidateRepository.CreateAsync(candidate);
        return new CandidateResponseDto
        {
            ExamCode = candidate.ExamCode,
            FullName = candidate.FullName,
            Category = candidate.Category,
            RegisteredAt = candidate.RegisteredAt
        };
    }

    public async Task<AssignComputerResponseDto> AssignComputerAsync(string examCode)
    {
        var candidate = await _candidateRepository.GetByExamCodeAsync(examCode);
        
        var existingSession = await _examSessionRepository.GetByExamCodeAsync(examCode);
        if (existingSession != null)
            throw new Exception("Computer already has an active session");
        
        var freeComputers = await _computerRepository.GetAllFreeComputersAsync();
        var computer = freeComputers.FirstOrDefault();
        
        if (computer == null)
            throw new Exception("No free computers available");

        var examSession = new ExamSession
        {
            Id = Guid.NewGuid(),
            ExamCode = examCode,
            StationNumber = computer.StationNumber,
            StartAt = DateTime.UtcNow,
            IsCompleted = false
        };
        var newExamSession = await _examSessionRepository.CreateNewExamSessionAsync(examSession);

        await _computerRepository.UpdateComputerStatusAsync(computer.Id, ComputerStatus.Occupied);

        return new AssignComputerResponseDto
        {
            ExamCode = examSession.ExamCode,
            StationNumber = examSession.StationNumber,
            StartAt = examSession.StartAt
        };
    }
}