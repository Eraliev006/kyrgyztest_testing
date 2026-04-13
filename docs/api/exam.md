# API — Экзамен

## POST /api/Exam/login

Вход кандидата на станции. Проверяет что ExamCode назначен именно этой станции.

**Request body:**
```json
{
  "examCode": "260403-3881",
  "stationNumber": 18
}
```

**Response 200:**
```json
{
  "examCode": "260403-3881",
  "fullName": "Эралиев Адилет",
  "stationNumber": 18,
  "startAt": "2026-04-03T00:24:52Z"
}
```

**Ошибки:**
- `404` — ExamCode не найден
- `422` — ExamCode назначен другой станции
- `422` — Сессия уже завершена

---

## GET /api/Exam/session/{examCode}

Получение данных активной сессии.

**Response 200:**
```json
{
  "examCode": "260403-3881",
  "fullName": "Эралиев Адилет",
  "stationNumber": 18,
  "startAt": "2026-04-03T00:24:52Z"
}
```

**Ошибки:**
- `404` — сессия не найдена