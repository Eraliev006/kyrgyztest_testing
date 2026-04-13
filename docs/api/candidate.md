# API — Кандидаты

## POST /api/Candidate/register

Регистрация нового кандидата в системе.

**Request body:**
```json
{
  "fullName": "Эралиев Адилет",
  "passportNumber": "ID12345678",
  "category": 0
}
```

**Category:**
- `0` — Student (Студент)
- `1` — Immigrant (Иммигрант)
- `2` — CivilServant (Гос.служащий)

**Response 200:**
```json
{
  "examCode": "260403-3881",
  "fullName": "Эралиев Адилет",
  "category": 0,
  "registeredAt": "2026-04-03T00:24:32Z"
}
```

---

## POST /api/Candidate/assign

Назначение свободного компьютера кандидату.

**Query параметр:** `examCode`

**Response 200:**
```json
{
  "examCode": "260403-3881",
  "stationNumber": 18,
  "startAt": "2026-04-03T00:24:52Z"
}
```

**Ошибки:**
- `400` — сессия уже существует
- `422` — нет свободных компьютеров