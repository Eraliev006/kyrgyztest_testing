# KyrgyzTest — Система тестирования кыргызского языка

Система тестирования знания кыргызского государственного языка. Кандидат приходит в центр, получает код доступа, садится за компьютер и последовательно проходит секции теста. Система автоматически подсчитывает результат и определяет уровень (A1–B2).

## Технологии

- **Backend:** ASP.NET Core 8, C#
- **База данных:** PostgreSQL 16 + Entity Framework Core 8
- **Аутентификация:** JWT Bearer + bcrypt
- **Контейнеризация:** Docker + Docker Compose
- **CI/CD:** GitLab CI (self-hosted runner)
- **Frontend:** React 19 + TypeScript + Vite (отдельный репозиторий `kyrgyztest-frontend`)

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

## CI/CD и деплой

Два репозитория, каждый со своим self-hosted GitLab runner на офисном
сервере: этот (бэкенд) и `kyrgyztest-frontend`. Ветки в обоих одинаковые —
`develop` и `production`.

**Этот репозиторий** (`.gitlab-ci.yml`):

| Stage  | Когда              | Что делает |
|--------|--------------------|------------|
| build  | develop, production | `dotnet restore && dotnet build` |
| test   | develop, production | `dotnet test` |
| deploy | **только production** | генерит `.env` из CI/CD-переменных (`PROD_*`), поднимает `docker compose -p kyrgyztest-prod up --build -d` |

**Фронтенд** (`kyrgyztest-frontend/.gitlab-ci.yml`): `build` + `test`
(lint/tsc) на обеих ветках, `smoke` (Playwright) только на `develop`.
Деплой-стадии у фронта нет вообще.

Пуш в `develop` — это только сборка и тесты, **никуда не деплоится**.
Реальный деплой запускается только пушем/мержем в `production`.

⚠️ Публичная демо-ссылка (ngrok на офисном сервере, для показа директору)
— это **не** часть этого пайплайна. Бэкенд там крутится из
`~/kyrgyztest` на сервере, фронт — статикой за Caddy в
`/var/www/kyrgyztest-front`, и то и другое обновляется вручную (scp +
пересборка), пока `production`-ветка не используется. Пуш в `develop`
демо-стенд сам по себе не обновит.

## Документация

- [Архитектура](docs/architecture/index.md)
- [API](docs/api/index.md)
- [Станция](docs/station/index.md)
- [Разработка](docs/development/index.md)
- [Роли](docs/roles/index.md)
