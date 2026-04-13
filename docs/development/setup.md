# Настройка окружения

## Клонировать репозиторий

```bash
git clone https://github.com/Eraliev006/kyrgyztest_testing.git
cd kyrgyztest_testing
```

## Настроить переменные окружения

```bash
cp .env.example .env
```

Заполни `.env`:
```
POSTGRES_DB=kyrgyztest
POSTGRES_USER=admin
POSTGRES_PASSWORD=yourpassword
POSTGRES_HOST=kyrgyztest_database
POSTGRES_PORT=5432
JWT_SECRET=your_super_secret_key_min_32_chars
JWT_ISSUER=KyrgyzTest
JWT_AUDIENCE=KyrgyzTestStations
```

## Запустить через Docker

```bash
docker-compose up --build -d
```

## Проверить

- Swagger UI: http://localhost:8000/swagger
- Station Client: http://localhost:8000/station.html

## Ветки

- `develop` — основная ветка разработки
- `production` — продакшн, только через merge request