# SignalR

## Хаб

URL: `/hub/station`

## Методы (браузер → сервер)

| Метод | Параметры | Описание |
|-------|-----------|----------|
| RegisterStation | stationNumber | Регистрация станции в группе |

## События (сервер → браузер)

| Событие | Параметры | Описание |
|---------|-----------|----------|
| StationRegistered | stationNumber | Подтверждение регистрации |
| ExamOpened | examCode | Открыть экзамен |

## Пример подключения

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/station")
    .withAutomaticReconnect()
    .build();

connection.on("ExamOpened", (examCode) => {
    // открыть страницу теста
});

await connection.start();
await connection.invoke("RegisterStation", stationNumber);
```