# Интеграция фронтенда со станцией

## Стек

Фронтенд написан на Vue 3 + Vite + TypeScript. Взаимодействие с API через Axios. Состояние приложения управляется через Pinia.

## Схема запросов

### Разблокировка станции

```
POST /api/exam/unlock
Body: { "password": "..." }
→ 200 OK | 403 Forbidden
```

### Вход кандидата

```
POST /api/exam/login
Body: { "accessCode": "20260515-1234" }
→ 200 { candidateId, name, ... }
→ 403 если заблокирован или доступ закрыт
```

### Запуск экзамена

```
POST /api/exam/start
Body: { "candidateId": "..." }
→ 200 { attemptId, sections: [...] }
```

### Секция

```
POST /api/exam/section/start
Body: { "attemptId": "...", "sectionType": "Grammar" }
→ 200 { sectionId, questions: [...] }

POST /api/exam/section/answer
Body: { "sectionId": "...", "questionId": "...", "answerId": "..." }
→ 200 OK

POST /api/exam/section/submit
Body: { "sectionId": "..." }
→ 200 OK
```

### Завершение экзамена

```
POST /api/exam/submit
Body: { "attemptId": "..." }
→ 200 { level: "B1", score: 72, ... }
```

## Примечания

- Все enums передаются как строки (JsonStringEnumConverter)
- CORS настроен на `http://localhost:5173` (Vue dev-сервер)
- В продакшне фронтенд и бэкенд доступны на одном домене через nginx/proxy
