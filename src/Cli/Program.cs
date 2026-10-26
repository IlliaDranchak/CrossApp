using Core.Dto;
using Core.Import;

string path = args.Contains("--mixed") 
    ? Path.Combine("data", "mixed.csv") 
    : (args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv"));

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

if (path.EndsWith("mixed.csv"))
{
    // === ЗМІШАНИЙ ІМПОРТ ===
    MixedImportResult mixed = MixedCsvImporter.Load(path);
    
    Console.WriteLine($"[Товари] Завантажено: {mixed.Products.Count}");
    foreach (var p in mixed.Products.Take(3))
        Console.WriteLine($"  P: {p.Id,-6} {p.Name,-20} {p.Quantity,5}{p.Unit}");

    Console.WriteLine($"[Склади] Завантажено: {mixed.Warehouses.Count}");
    foreach (var w in mixed.Warehouses.Take(3))
        Console.WriteLine($"  W: {w.Id,-6} {w.Name,-20} {w.Location}");

    if (mixed.Errors.Count > 0)
    {
        Console.WriteLine($"Пропущено рядків: {mixed.Errors.Count}");
        foreach (string e in mixed.Errors)
            Console.WriteLine($" ! {e}");
    }
}
else
{
    // === ЗВИЧАЙНИЙ ІМПОРТ (CSV або JSON) ===
    ImportResult<ProductDto> result = Path.GetExtension(path).ToLowerInvariant() switch
    {
        ".csv" => ProductCsvImporter.Load(path),
        ".json" => ProductJsonImporter.Load(path),
        _ => throw new NotSupportedException($"Непідтримуване розширення: {Path.GetExtension(path)}")
    };

    Console.WriteLine($"Завантажено записів: {result.Items.Count}");
    Console.WriteLine($"Статистика: усього {result.Total}, прийнято {result.Accepted}, пропущено {result.Rejected} ({result.ErrorRate:F1}%)");

    foreach (ProductDto p in result.Items.Take(5))
        Console.WriteLine($" {p.Id,-6}{p.Sku,-10}{p.Name,-26}{p.Quantity,5}{p.Unit}");

    if (result.Errors.Count > 0)
    {
        Console.WriteLine($"Пропущено рядків: {result.Errors.Count}");
        foreach (string e in result.Errors)
            Console.WriteLine($" ! {e}");
    }
}

return 0;