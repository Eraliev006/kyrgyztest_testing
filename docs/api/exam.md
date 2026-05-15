# API — Экзамен

Все эндпоинты публичные (токен не нужен) — их вызывает браузер на станции.

## Флоу прохождения

```
1. POST /exam/unlock       — разблокировать станцию
2. POST /exam/login        — кандидат вводит код доступа
3. POST /exam/start        — получить список секций
4. POST /exam/section/start  — загрузить вопросы секции
5. POST /exam/answer         — сохранить ответ (повторять для каждого вопроса)
6. POST /exam/section/submit — завершить секцию
7. Повторить 4–6 для каждой секции
8. POST /exam/submit         — финальная сдача (вызывается автоматически после последней секции)
```

---

## POST /api/exam/unlock

Разблокировать станцию паролем. Пароль задаётся в `ExamSettings__AccessPassword`.

**Request body:**
```json
{ "password": "station_secret" }
```

**Response 200:** `"Доступ открыт"`

**Ошибки:** `401` — неверный пароль

---

## POST /api/exam/login

Вход кандидата по коду доступа.

**Request body:**
```json
{ "accessCode": "20260515-4823" }
```

**Response 200:** объект кандидата (см. `CandidateResponseDto`)

**Ошибки:**
- `404` — кандидат не найден
- `403` — кандидат заблокирован (`"Доступ заблокирован до 15.06.2026"`)
- `403` — кандидату не разрешён доступ (`IsAllowed = false`)

---

## POST /api/exam/start

Создать попытку (Attempt). Возвращает список секций.

**Request body:**
```json
{ "candidateId": "uuid" }
```

**Response 200:**
```json
{
  "attemptId": "uuid",
  "sections": [
    { "section": "Grammar", "timeLimitMinutes": 30, "isCompleted": false },
    { "section": "Listening", "timeLimitMinutes": 20, "isCompleted": false },
    { "section": "Reading", "timeLimitMinutes": 25, "isCompleted": false },
    { "section": "Writing", "timeLimitMinutes": 30, "isCompleted": false }
  ]
}
```

---

## POST /api/exam/section/start

Загрузить вопросы выбранной секции.

**Request body:**
```json
{ "attemptId": "uuid", "section": "Grammar" }
```

**Response 200:**
```json
{
  "section": "Grammar",
  "timeLimitMinutes": 30,
  "questions": [
    {
      "id": "uuid",
      "content": "Текст вопроса",
      "type": "MCQ",
      "answerOptions": [
        { "id": "uuid", "content": "Вариант А" },
        { "id": "uuid", "content": "Вариант Б" }
      ]
    }
  ]
}
```

`type`: `"MCQ"`, `"WordOrder"`, `"SentenceOrder"`

---

## POST /api/exam/answer

Сохранить ответ на вопрос. Вызывается при смене ответа — последний вызов перезаписывает предыдущий.

**Request body для MCQ:**
```json
{
  "attemptId": "uuid",
  "questionId": "uuid",
  "selectedOptionId": "uuid"
}
```

**Request body для WordOrder/SentenceOrder:**
```json
{
  "attemptId": "uuid",
  "questionId": "uuid",
  "orderedAnswer": "[\"uuid1\",\"uuid2\",\"uuid3\"]"
}
```

---

## POST /api/exam/section/submit

Завершить секцию. Если это последняя секция — автоматически вызывает финальный подсчёт.

**Request body:**
```json
{ "attemptId": "uuid", "section": "Grammar" }
```

**Response 200:**
```json
{
  "isExamCompleted": false,
  "result": null
}
```

Когда `isExamCompleted = true`, в поле `result` содержится итоговый результат.

---

## POST /api/exam/submit

Принудительное завершение экзамена (можно вызвать напрямую).

**Request body:**
```json
{ "attemptId": "uuid" }
```

**Response 200:**
```json
{
  "id": "uuid",
  "attemptId": "uuid",
  "candidateId": "uuid",
  "level": "B1",
  "grammarScore": 14,
  "listeningScore": 8,
  "readingScore": 10,
  "writingScore": 9,
  "totalScore": 41,
  "createdAt": "2026-05-15T14:30:00Z"
}
```

## Определение уровня

| Процент правильных | Уровень |
|--------------------|---------|
| < 10%              | A1      |
| 10% – 34%          | A2      |
| 35% – 59%          | B1      |
| ≥ 60%              | B2      |

После завершения экзамена `IsAllowed` кандидата автоматически сбрасывается в `false`.
