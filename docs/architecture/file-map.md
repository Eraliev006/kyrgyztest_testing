# Карта файлов

## KyrgyzTest.Core
```
Core/
├── Entities/
│   ├── Users.cs               ← пользователи системы (сотрудники)
│   ├── Candidate.cs           ← кандидат на экзамен
│   ├── Organization.cs        ← организация, к которой привязан кандидат
│   ├── Question.cs            ← вопрос
│   ├── AnswerOption.cs        ← вариант ответа
│   ├── MediaGroup.cs          ← аудио/текст к вопросу
│   ├── Topic.cs               ← тема вопроса (справочник)
│   ├── SectionConfig.cs       ← настройки секции (время, кол-во вопросов)
│   ├── TestVariant.cs         ← набор вопросов для экзамена
│   ├── TestVariantQuestion.cs ← позиция вопроса в варианте
│   ├── Attempt.cs             ← попытка прохождения экзамена
│   ├── CandidateAnswer.cs     ← ответ кандидата на вопрос
│   ├── Result.cs              ← итоговый результат попытки
│   └── CompletedSection.cs    ← завершённая секция в попытке
├── Enums/
│   ├── UserRole.cs            ← SuperAdmin, Director, Admin, Examiner, Expert
│   ├── SectionType.cs         ← Grammar, Listening, Reading, Writing, Speaking
│   ├── LanguageLevel.cs       ← A1, A2, B1, B2
│   ├── QuestionType.cs        ← MCQ, WordOrder, SentenceOrder
│   ├── MediaType.cs           ← Audio, Text
│   ├── AttemptStatus.cs       ← InProgress, Completed
│   └── CandidateCategory.cs   ← Student, Immigrant, CivilServant
├── Interfaces/
│   ├── IUserRepository.cs
│   ├── ICandidateRepository.cs
│   ├── IOrganizationRepository.cs
│   ├── IQuestionRepository.cs
│   ├── IMediaGroupRepository.cs
│   ├── ITopicRepository.cs
│   ├── ISectionConfigRepository.cs
│   ├── ITestVariantRepository.cs
│   ├── IAttemptRepository.cs
│   ├── ICandidateAnswerRepository.cs
│   ├── IResultRepository.cs
│   └── ICompletedSectionRepository.cs
└── Exceptions/
    ├── NotFoundException.cs
    ├── BusinessException.cs
    └── ValidationException.cs
```

## KyrgyzTest.Application
```
Application/
├── DTOs/                      ← входные и выходные DTO для всех операций
├── Interfaces/
│   ├── IAuthService.cs
│   ├── IUserService.cs
│   ├── ICandidateService.cs
│   ├── IOrganizationService.cs
│   ├── IQuestionService.cs
│   ├── ITopicService.cs
│   ├── ISectionConfigService.cs
│   ├── ITestVariantService.cs
│   ├── ITestVariantGeneratorService.cs
│   ├── IExamService.cs
│   └── IResultService.cs
└── Services/
    ├── AuthService.cs         ← JWT-логин
    ├── UserService.cs         ← CRUD пользователей + проверка роли создателя
    ├── CandidateService.cs    ← регистрация, доступ, блокировка
    ├── OrganizationService.cs
    ├── QuestionService.cs     ← CRUD вопросов, media-group
    ├── TopicService.cs        ← CRUD тем
    ├── SectionConfigService.cs
    ├── TestVariantService.cs  ← просмотр и замена вопросов
    ├── TestVariantGeneratorService.cs ← генерация варианта из пула вопросов
    ├── ExamService.cs         ← флоу экзамена, подсчёт результата
    └── ResultService.cs       ← фильтрация, статистика
```

## KyrgyzTest.Infrastructure
```
Infrastructure/
├── Persistence/
│   └── AppDbContext.cs        ← DbContext, конфигурация связей, seed данных
├── Repositories/              ← реализации интерфейсов из Core
└── Migrations/                ← EF Core миграции (не редактировать вручную)
```

## KyrgyzTest.API
```
API/
├── Controllers/
│   ├── AuthController.cs       ← POST /api/auth/login
│   ├── UserContoller.cs        ← GET/POST/PUT/DELETE /api/user
│   ├── CandidateContoller.cs   ← /api/candidates
│   ├── OrganizationController.cs ← /api/organizations
│   ├── QuestionController.cs   ← /api/questions
│   ├── TopicController.cs      ← /api/topics
│   ├── SectionConfigController.cs ← /api/section-config
│   ├── TestVariantController.cs   ← /api/variants
│   ├── ExamContoller.cs        ← /api/exam
│   ├── ResultsController.cs    ← /api/results
│   └── FIleContoller.cs        ← /api/files/audio
├── Extensions/
│   ├── DatabaseExtensions.cs   ← подключение PostgreSQL
│   ├── JwtExtensions.cs        ← настройка JWT
│   ├── RepositoryExtension.cs  ← DI-регистрация репозиториев
│   └── ServiceExtension.cs     ← DI-регистрация сервисов
├── ErrorHandlingMiddleware.cs  ← перехват BusinessException / NotFoundException
└── Program.cs                  ← точка входа, миграции, seed SuperAdmin
```

## KyrgyzTest.Tests
```
Tests/
├── ExamScoringTests.cs          ← подсчёт очков, определение уровня
├── CandidateAccessCodeTests.cs  ← формат и уникальность AccessCode
├── CandidateBlockTests.cs       ← блокировка, deny, предикат is-blocked
└── TestVariantReplaceTests.cs   ← валидация замены вопроса в варианте
```
