# CI/CD

## Инструменты

- GitLab CI/CD
- GitLab Runner (self-hosted, Mac)

## Pipeline

Запускается автоматически при push в `develop` или `production`.

### Этапы

**build** — сборка проекта:
```bash
dotnet restore
dotnet build --no-restore
```

**test** — запуск тестов:
```bash
dotnet test --no-build
```

**deploy** — деплой на сервер (только `production`):
```bash
docker-compose down
docker-compose up --build -d
```

## Ветки

| Ветка | build | test | deploy |
|-------|-------|------|--------|
| develop | ✅ | ✅ | ❌ |
| production | ✅ | ✅ | ✅ |

## GitLab Runner

Runner установлен локально на Mac через Homebrew.
Тег: `self-hosted`