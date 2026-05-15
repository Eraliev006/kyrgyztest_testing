# API — Кандидаты и организации

## Аутентификация персонала

### POST /api/auth/login

Вход сотрудника системы. Не требует токена.

**Request body:**
```json
{ "login": "superadmin", "password": "admin123" }
```

**Response 200:**
```json
{
  "accessToken": "eyJ...",
  "fullName": "Super Admin",
  "login": "superadmin",
  "role": "SuperAdmin"
}
```

---

## Кандидаты

Все эндпоинты требуют `Authorization: Bearer <token>`.

### GET /api/candidates

Список всех кандидатов.

### GET /api/candidates/{id}

Один кандидат. `404` если не найден.

### GET /api/candidates/search

Поиск по ИНН или коду доступа.

**Query params:** `?inn=12345678901234` или `?code=20260515-1234`

### POST /api/candidates

Создать кандидата.

**Request body:**
```json
{
  "fullName": "Эралиев Адилет",
  "inn": "12345678901234",
  "organizationId": "uuid или null"
}
```

**Response 200:**
```json
{
  "id": "uuid",
  "fullName": "Эралиев Адилет",
  "inn": "12345678901234",
  "accessCode": "20260515-4823",
  "isAllowed": true,
  "createdAt": "2026-05-15T12:00:00Z",
  "organizationId": "uuid или null",
  "photo": null,
  "blockedUntil": null
}
```

`accessCode` генерируется автоматически в формате `YYYYMMDD-NNNN`.

### PUT /api/candidates/{id}/photo
**Роли:** SuperAdmin, Director, Admin

Загрузить или обновить фото кандидата (строка: URL или base64).

**Request body:**
```json
{ "photo": "https://..." }
```

### PUT /api/candidates/{id}/allow

Открыть доступ к экзамену (`IsAllowed = true`).

### PUT /api/candidates/{id}/deny
**Роли:** SuperAdmin, Director, Admin

Закрыть доступ (`IsAllowed = false`).

### PUT /api/candidates/{id}/block
**Роли:** SuperAdmin, Director, Admin

Заблокировать на срок. При попытке войти через `POST /exam/login` вернётся `403` с датой.

**Request body:**
```json
{ "value": 30, "unit": "days" }
```

`unit`: `"days"`, `"weeks"`, `"months"`, `"years"`

### DELETE /api/candidates/{id}

Удалить кандидата.

---

## Организации

### GET /api/organizations

Список организаций. Требует авторизации.

### POST /api/organizations
**Роли:** SuperAdmin, Director

```json
{ "name": "Министерство образования" }
```

### PUT /api/organizations/{id}
**Роли:** SuperAdmin, Director

### DELETE /api/organizations/{id}
**Роли:** SuperAdmin, Director
