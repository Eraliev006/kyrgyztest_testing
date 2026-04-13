# API — Компьютеры

## GET /api/Computer

Получение списка всех 50 компьютеров с их статусами.

**Response 200:**
```json
[
  {
    "stationNumber": 1,
    "status": 0,
    "examCode": null
  },
  {
    "stationNumber": 2,
    "status": 1,
    "examCode": "260403-3881"
  }
]
```

**Status:**
- `0` — Free (свободен)
- `1` — Occupied (занят)
- `2` — Offline (офлайн)
- `3` — Error (ошибка)

---

## POST /api/Computer/open-exam

Отправить команду браузеру на станции открыть экзамен.
Используется администратором после назначения кандидата.

**Query параметры:**
- `stationNumber` — номер станции
- `examCode` — код экзамена

**Response 200:** OK