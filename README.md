# Контур.Библиотека

## Сервисы Контур.Библиотека

Сервисы Контур.Библиотека - на основе него стедунты ФИИТа учатся писать автотесты

[readme для разработчика](./readmeForDeveloper.md)

## Необходимые требования:

1. [NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. [Node.js](https://nodejs.org/en/) (рекомендуется версия 16.20.1)

Если версия `node` не совпадает, можно использовать [nvm](https://github.com/nvm-sh/nvm)

Версия указана в [.nvmrc](./Source/Kontur.BigLibrary.Service/ClientApp/.nvmrc)

## Запуск проекта:

### Установить пакеты

```shell
npm ci
```
Если команда падает с ошибкой, то:
1. Если вы находитесь в сети контура, то выполните команду 
```npm config set registry https://nexus.kontur.host/repository/kontur-npm-group```
2. Если вы НЕ в сети Контура, то выполните команду 
```npm config set registry https://registry.npmjs.org/
```

Далее 
 - запустите ```npm install``` из корня проекта и из папки ClientApp
 - повторно запустите ```npm ci```

### Сбилдить проект

```shell
npm run build
```

### Запустить

```shell
npm run start
```

### Запустить в dev режиме

```shell
npm run dev
```

### Запустить в dev режиме только frontend

```shell
npm run dev:fronend
```

### Запустить в dev режиме только backend

```shell
npm run dev:backend
```

### Запустить storybook

```shell
npm run storybook
```

## Настроить JWT

В файле [appsettings.json](./Source/Kontur.BigLibrary.Service/appsettings.json) 

`DurationInHours` означает время действия токена авторизации в часах.

```json
"JwtSettings": {
    "Issuer": "https://example.com",
    "Audience": "myapp",
    "Secret": "supersecretkey123",
    "DurationInHours": "1"
  },
```

## Настроить Google Drive интеграцию

В файле [appsettings.json](./Source/Kontur.BigLibrary.Service/appsettings.json) необходимо указать `spreadsheetId`, который находится в URL документа, к которому нужно дать доступ

По дефолту рассматривается весь первый лист, 1 и 4 столбцы

```json
  "GoogleDriveIntegration": {
    "spreadsheetId": "",
    "range": "Лист1"
  },
```

После нужно создать проект для интеграции и получить файлик секретов, который нужно переименовать в  `secrets.json` и положить вместо [secrets.json](./Source/Kontur.BigLibrary.Service/Integration/secrets.json)


## Навигация:

[frontend](./Source/Kontur.BigLibrary.Service/ClientApp)
