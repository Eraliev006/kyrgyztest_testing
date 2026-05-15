# Настройка окружения

## Клонировать репозиторий

```bash
git clone https://github.com/Eraliev006/kyrgyztest_testing.git
cd kyrgyztest_testing
```

## Настроить переменные окружения

Создай `.env` в корне проекта:

```env
# PostgreSQL (используется контейнером базы данных)
POSTGRES_DB=kyrgyztest
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres

# JWT
Jwt__Key=your_super_secret_key_at_least_32_chars
Jwt__Issuer=KyrgyzTest
Jwt__Audience=KyrgyzTestUsers

# Пароль для разблокировки станции (POST /api/exam/unlock)
ExamSettings__AccessPassword=your_station_password

# Директория для загрузки аудиофайлов
FileStorage__Path=uploads
```

## Запустить через Docker

```bash
docker compose up --build -d
```

Приложение будет доступно на порту `8000`. При первом старте автоматически применяются миграции и создаётся пользователь `superadmin` / `admin123`.

## Проверить

- Swagger UI: http://localhost:8000/swagger
- API base: http://localhost:8000/api

## Локальный запуск без Docker

Требования: .NET 8 SDK, PostgreSQL.

```bash
# Запустить только базу данных
docker compose up kyrgyztest_database -d

# Применить миграции
dotnet ef database update --project KyrgyzTest.Infrastructure --startup-project KyrgyzTest.API

# Запустить API
dotnet run --project KyrgyzTest.API
```

## Тесты

```bash
dotnet test
```

## Ветки

- `develop` — основная ветка разработки
- `production` — продакшн, только через merge request
