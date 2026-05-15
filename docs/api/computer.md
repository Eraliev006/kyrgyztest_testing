# API — Управление вопросами и результатами

## Вопросы

**Роли:** SuperAdmin, Expert

### GET /api/questions

Список вопросов с фильтрацией.

**Query params:** `?section=Grammar&level=A1`

`section`: `Grammar`, `Listening`, `Reading`, `Writing`, `Speaking`
`level`: `A1`, `A2`, `B1`, `B2`

**Response 200:** массив вопросов:
```json
[
  {
    "id": "uuid",
    "section": "Grammar",
    "level": "A1",
    "type": "MCQ",
    "content": "Текст вопроса",
    "mediaGroupId": null,
    "mediaUrl": null,
    "mediaText": null,
    "topicId": "uuid или null",
    "topicName": "Зат атооч",
    "answerOptions": [
      { "id": "uuid", "content": "Вариант", "isCorrect": true, "orderIndex": 0 }
    ],
    "createdAt": "2026-05-15T12:00:00Z"
  }
]
```

### GET /api/questions/{id}

### POST /api/questions

**Request body:**
```json
{
  "section": "Grammar",
  "level": "A1",
  "type": "MCQ",
  "content": "Текст вопроса",
  "mediaGroupId": null,
  "topicId": "uuid или null",
  "answerOptions": [
    { "content": "Верный вариант", "isCorrect": true, "orderIndex": 0 },
    { "content": "Неверный вариант", "isCorrect": false, "orderIndex": 1 }
  ]
}
```

### PUT /api/questions/{id}

Тот же формат, что и POST.

### DELETE /api/questions/{id}

### POST /api/questions/media-group

Создать группу медиафайлов для вопроса.

**Request body:**
```json
{ "type": "Audio", "content": "/uploads/audio.mp3" }
```

`type`: `"Audio"` или `"Text"`

---

## Темы (Topics)

**Роли:** SuperAdmin, Director, Expert

Справочник подтем для вопросов. При старте заполняется seed-данными для Grammar.

### GET /api/topics

**Query params:** `?section=Grammar` — фильтр по секции (если не указан — все темы)

**Response 200:**
```json
[
  { "id": "uuid", "name": "Зат атооч", "sectionType": "Grammar" },
  { "id": "uuid", "name": "Сын атооч", "sectionType": "Grammar" }
]
```

### GET /api/topics/{id}

### POST /api/topics

```json
{ "name": "Байламта", "sectionType": "Grammar" }
```

### PUT /api/topics/{id}

### DELETE /api/topics/{id}

---

## Варианты теста

**Роли:** SuperAdmin, Director

### GET /api/variants

Список вариантов с количеством вопросов.

### GET /api/variants/{id}

Детали варианта: вопросы сгруппированы по секциям.

### GET /api/variants/{id}/available-questions

Вопросы, доступные для замены в варианте.

**Query params:** `?section=Grammar&level=A1`

### PUT /api/variants/{id}/questions/{questionId}

Заменить вопрос в варианте. Новый вопрос должен совпадать по секции, уровню и типу.

**Request body:**
```json
{ "newQuestionId": "uuid" }
```

---

## Конфигурация секций

**Роли:** SuperAdmin, Director

### GET /api/section-config

Список настроек всех секций (время, количество вопросов).

### GET /api/section-config/{id}

### POST /api/section-config

### PUT /api/section-config/{id}

### DELETE /api/section-config/{id}

---

## Результаты

Требует авторизации `[Authorize]`.

### GET /api/results

Список результатов с фильтрацией.

**Query params:** `?organizationId=uuid&level=B1&dateFrom=2026-01-01&dateTo=2026-12-31`

### GET /api/results/{candidateId}

Последний результат кандидата.

### GET /api/results/stats

Агрегированная статистика.

**Query params:** `?organizationId=uuid&dateFrom=...&dateTo=...`

---

## Пользователи системы

**Роли:** SuperAdmin, Director

### GET /api/user

### GET /api/user/{id}

### POST /api/user

```json
{ "fullName": "Имя", "login": "login", "password": "pass", "role": "Admin" }
```

`role`: `SuperAdmin`, `Director`, `Admin`, `Examiner`, `Expert`

### PUT /api/user/{id}

### DELETE /api/user/{id}

---

## Файлы

**Роли:** SuperAdmin, Expert

### POST /api/files/audio

Загрузить аудиофайл для вопроса. Принимает `multipart/form-data`.

Допустимые форматы: `.mp3`, `.wav`, `.ogg`

**Response 200:**
```json
{ "url": "/uploads/abc123.mp3" }
```
