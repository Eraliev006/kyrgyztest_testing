# Обзор архитектуры

## Паттерн

Clean Architecture — разделение на 4 слоя.

## Слои

**Core (Domain)** — модели, интерфейсы, enums. Никаких зависимостей.

**Application** — бизнес-логика, сервисы, DTOs. Зависит только от Core.

**Infrastructure** — DbContext, репозитории. Реализует интерфейсы Core.

**API** — контроллеры, SignalR хабы, middleware. Точка входа.

## Зависимости
API → Application → Core
Infrastructure → Application → Core

## Технологии

- ASP.NET Core 8
- Entity Framework Core 8 + PostgreSQL
- SignalR — реальное время
- JWT — авторизация станций
- Docker — контейнеризация