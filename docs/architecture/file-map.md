# Карта файлов

## KyrgyzTest.Core
```
Core/
├── Entities/
│   ├── Candidate.cs        ← модель кандидата
│   ├── Computer.cs         ← модель компьютера/станции
│   └── ExamSession.cs      ← модель сессии экзамена
├── Enums/
│   ├── CandidateCategory.cs ← Student, Immigrant, CivilServant
│   └── ComputerStatus.cs    ← Free, Occupied, Offline, Error
├── Interfaces/
│   ├── ICandidateRepository.cs
│   ├── IComputerRepository.cs
│   └── IExamSessionRepository.cs
└── Exceptions/
├── NotFoundException.cs
├── BusinessException.cs
└── ValidationException.cs
```

## KyrgyzTest.Application
```
Application/
├── DTOs/
│   ├── RegisterCandidateDto.cs
│   ├── CandidateResponseDto.cs
│   ├── AssignComputerResponseDto.cs
│   ├── ComputerResponseDto.cs
│   ├── ExamLoginDto.cs
│   └── ExamLoginResponseDto.cs
├── Interfaces/
│   ├── IExamService.cs
│   ├── IComputerService.cs
│   └── IJwtService.cs
└── Services/
├── ExamService.cs
├── ComputerService.cs
└── JwtService.cs
```

## KyrgyzTest.Infrastructure
```
Infrastructure/
├── Persistence/
│   └── AppDbContext.cs      ← DbContext + Data Seeding
├── Repositories/
│   ├── CandidateRepository.cs
│   ├── ComputerRepository.cs
│   └── ExamSessionRepository.cs
└── Migrations/
├── InitialCreate
└── SeedComputers
```

## KyrgyzTest.API
```
API/
├── Controllers/
│   ├── CandidateController.cs  ← register, assign
│   ├── ComputerController.cs   ← list, open-exam
│   └── ExamController.cs       ← login, session
├── Extensions/
│   ├── DatabaseExtensions.cs
│   ├── RepositoryExtension.cs
│   └── JwtExtensions.cs
├── Hubs/
│   └── StationHub.cs           ← SignalR
├── wwwroot/
│   └── station.html            ← тестовый клиент
└── Program.cs
```