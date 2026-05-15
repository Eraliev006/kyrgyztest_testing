# KyrgyzTest — Система тестирования кыргызского языка

Система тестирования знания кыргызского государственного языка. Кандидат приходит в центр, получает код доступа, садится за компьютер и последовательно проходит секции теста. Система автоматически подсчитывает результат и определяет уровень (A1–B2).

## Технологии

- **Backend:** ASP.NET Core 8, C#
- **База данных:** PostgreSQL 16 + Entity Framework Core 8
- **Аутентификация:** JWT Bearer + bcrypt
- **Контейнеризация:** Docker + Docker Compose
- **CI/CD:** GitLab CI (self-hosted runner)
- **Frontend:** Vue 3 + Vite + TypeScript, Pinia, Axios (отдельный репозиторий)

## Быстрый старт

```bash
git clone https://github.com/Eraliev006/kyrgyztest_testing.git
cd kyrgyztest_testing
```

Создай `.env` в корне проекта:

```env
# PostgreSQL
POSTGRES_DB=kyrgyztest
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres

# JWT
Jwt__Key=your_super_secret_key_min_32_chars
Jwt__Issuer=KyrgyzTest
Jwt__Audience=KyrgyzTestUsers

# Экзамен
ExamSettings__AccessPassword=your_station_password

# Хранилище файлов
FileStorage__Path=uploads
```

Запустить:

```bash
docker compose up --build -d
```

При первом запуске автоматически применяются все миграции и создаётся пользователь `superadmin` / `admin123`.

Swagger UI: http://localhost:8000/swagger

## Документация

- [Архитектура](docs/architecture/index.md)
- [API](docs/api/index.md)
- [Станция](docs/station/index.md)
- [Разработка](docs/development/index.md)
- [Роли](docs/roles/index.md)
