# CrossApp
Наскрізний проєкт з крос-платформного програмування.

## Предметна область
**Склад**
* **Сутності:** Product (товар), StockBatch (партія), Warehouse (склад), Movement (переміщення).
* **Призначення:** облік залишків товарів по партіях.

## Структура проєкту
```text
CrossApp/
├── data/
│   ├── sample.csv      # Тестові дані (10+ рядків, 3 з дефектами)
│   ├── sample.json     # Тестові дані у форматі JSON
│   └── mixed.csv       # Різнорідні дані (товари P та склади W)
│   └── clean.csv #Тестові дані без дефектів
── src/
│   ├── Core/
│   │   ├── EnvironmentInfo.cs  # Інформація про середовище 
│   │   ├── Dto/                # Незмінні типи даних (DTO)
│   │   │   ├── ProductDto.cs
│   │   │   ├── WarehouseDto.cs
│   │   │   ├── ImportResult.cs
│   │   │   └── MixedImportResult.cs
│   │   └── Import/             # Логіка розбору файлів
│   │       ├── ProductCsvImporter.cs
│   │       ├── ProductJsonImporter.cs
│   │       └── MixedCsvImporter.cs
│   ── Cli/
│       └── Program.cs          # Точка входу та вивід
└── CrossApp.sln

# Збірка проєкту
dotnet build

# Імпорт стандартного CSV-файлу складу (data/sample.csv за замовчуванням)
dotnet run --project src/Cli

# Імпорт JSON-файлу (автоматичний вибір парсера за розширенням .json)
dotnet run --project src/Cli -- data/sample.json

# Імпорт змішаного CSV-файлу товарів та складів (префікси P і W)
dotnet run --project src/Cli -- --mixed

# Запуск з довільним шляхом до файлу
dotnet run --project src/Cli -- data/not-exist.csv

### Додатково реалізовано:

JSON-імпортер (ProductJsonImporter) з використанням System.Text.Json та PropertyNameCaseInsensitive.

Змішаний імпорт (MixedCsvImporter), який за префіксом (P або W) в одному файлі розподіляє рядки по різних DTO за допомогою одного switch.

Статистика імпорту (Total, Accepted, Rejected, ErrorRate) в ImportResult.

### Формат файлів

Роздільник: крапка з комою (;), щоб уникнути конфліктів з комами в назвах.

Кодування: UTF-8.

Заголовок: перший рядок може містити заголовки (наприклад, id;sku;name...), програма автоматично його пропускає.

Коментарі: рядки, що починаються з #, ігноруються.

| Режим публікації        | Аргументи команди                   | Розмір каталогу | Файлів | Потрібен runtime |
|-------------------------|-------------------------------------|-----------------|--------|------------------|
| **Framework-dependent** | `-r win-x64 --self-contained false` | 195 КБ          | 7      | Так (.NET 10)    |
| **Self-contained**      | `-r win-x64 --self-contained true`  | 76.6 МБ         | 194    | Ні               |
| **Single-File**         | `... -p:PublishSingleFile=true`     | 70.1 МБ         | 3      | Ні               |
| **Trimmed**             | `... -p:PublishTrimmed=true`        | 12.5 МБ         | 3      | Ні               |


### 4. Multi-targeting
Бібліотеку `Core` налаштовано на збірку одразу для двох фреймворків: `<TargetFrameworks>net8.0;net10.0<
/TargetFrameworks>`. 
За допомогою директив умовної компіляції (`#if NET10_0_OR_GREATER`) вивід програми містить примітку про те, під яку версію .NET було зібрано код.
