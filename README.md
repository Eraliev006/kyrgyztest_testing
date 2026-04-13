# KyrgyzTest — Система тестирования кыргызского языка

Система тестирования на знание кыргызского государственного языка.
Аналог IELTS/TOEFL для 50 компьютеров в одном зале.

## Технологии

- **Backend:** C# / ASP.NET Core 8
- **База данных:** PostgreSQL + Entity Framework Core 8
- **Реальное время:** SignalR (WebSocket)
- **Контейнеризация:** Docker + docker-compose
- **CI/CD:** GitLab CI

## Быстрый старт

```bash
git clone https://github.com/Eraliev006/kyrgyztest_testing.git
cd kyrgyztest_testing
cp .env.example .env
# Заполни .env своими данными
docker-compose up --build -d
```

Swagger UI: http://localhost:8000/swagger


## Документация

- [Архитектура](docs/architecture/index.md)
- [API](docs/api/index.md)
- [Станция](docs/station/index.md)
- [Разработка](docs/development/index.md)
- [Роли](docs/roles/index.md)