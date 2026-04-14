using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Exceptions;
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
        var random = Guid.NewGuid().ToString("N")[..6];
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
        if (candidate == null)
            throw new NotFoundException("Candidate not found");
        
        var existingSession = await _examSessionRepository.GetByExamCodeAsync(examCode);
        if (existingSession != null)
            throw new BusinessException("Exam session already exists");
        
        var computer = await _computerRepository.GetAndReserveFreeComputerAsync();
        if (computer == null)
            throw new BusinessException("No free computers available");
        
        var examSession = new ExamSession
        {
            Id = Guid.NewGuid(),
            ExamCode = examCode,
            ComputerId = computer.Id,
            StartAt = DateTime.UtcNow,
            IsCompleted = false
        };
        
        await _examSessionRepository.CreateNewExamSessionAsync(examSession);
        
        return new AssignComputerResponseDto
        {
            ExamCode = examSession.ExamCode,
            ComputerId = examSession.ComputerId,
            StationNumber = computer.StationNumber,
            StartAt = examSession.StartAt
        };
    }

    public async Task<ExamLoginResponseDto> LoginAsync(ExamLoginDto examLogin)
    {
        var existingExamSession = await _examSessionRepository.GetByExamCodeAsync(examLogin.ExamCode);
        
        if (existingExamSession is null)
            throw new NotFoundException("Invalid exam code");
        
        if (existingExamSession.ComputerId != examLogin.ComputerId)
            throw new BusinessException("Invalid station number");
        
        if (existingExamSession.IsCompleted)
            throw new BusinessException("Exam session is already completed");
        
        var candidate = await _candidateRepository.GetByExamCodeAsync(examLogin.ExamCode);
        if (candidate is null)
            throw new NotFoundException("Candidate not found");
        
        var computer = await _GetComputerById(existingExamSession.ComputerId);
        
        return new ExamLoginResponseDto
        {
            ExamCode = existingExamSession.ExamCode,
            FullName = candidate.FullName,
            ComputerId = existingExamSession.ComputerId,
            StationNumber = computer.StationNumber,
            StartAt = existingExamSession.StartAt
        };   
    }

    public async Task<ExamLoginResponseDto> GetSessionAsync(string examCode)
    {
        var session = await _examSessionRepository.GetByExamCodeAsync(examCode);
        
        if (session == null)
            throw new NotFoundException("Exam session not found");
        
        var candidate = await _candidateRepository.GetByExamCodeAsync(examCode);
        if (candidate == null)
            throw new NotFoundException("Candidate not found");
        
        var computer = await _GetComputerById(session.ComputerId);

        
        return new ExamLoginResponseDto
        {
            ExamCode = session.ExamCode,
            FullName = candidate.FullName,
            StationNumber = computer.StationNumber,
            ComputerId = session.ComputerId,
            StartAt = session.StartAt
        };
    }

    private async Task<Computer?> _GetComputerById(Guid computerId)
    {
        return await _computerRepository.GetByComputerIdAsync(computerId);
    }
}