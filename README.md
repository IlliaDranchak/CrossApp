# CrossApp
Наскрізний проєкт з крос-платформного програмування.

## Предметна область
**Склад**
* **Сутності:** Product (товар), StockBatch (партія), Warehouse (склад), Movement (переміщення).
* **Призначення:** облік залишків товарів по партіях.

## Структура проєкту
```text
CrossApp/
├── CrossApp.sln
├── README.md
├── .gitignore
└── src/
    ├── Core/          # Бібліотека класів (логіка збору даних)
    │   ├── Core.csproj
    │   └── EnvironmentInfo.cs
    └── Cli/           # Консольний застосунок (точка входу, вивід)
        ├── Cli.csproj
        └── Program.cs

# Збірка проєкту
dotnet build

# Запуск у режимі таблиці
dotnet run --project src/Cli

# Запуск з виводом у форматі JSON
dotnet run --project src/Cli -- --json

| Режим публікації        | Аргументи команди                   | Розмір каталогу | Файлів | Потрібен runtime |
|-------------------------|-------------------------------------|-----------------|--------|------------------|
| **Framework-dependent** | `-r win-x64 --self-contained false` | 195 КБ          | 7      | Так (.NET 10)    |
| **Self-contained**      | `-r win-x64 --self-contained true`  | 76.6 МБ         | 194    | Ні               |

