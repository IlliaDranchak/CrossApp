using Core.Dto;

namespace Core.Import;

public static class MixedCsvImporter
{
    private const char Separator = ';';

    public static MixedImportResult Load(string path)
    {
        var products = new List<ProductDto>();
        var warehouses = new List<WarehouseDto>();
        var errors = new List<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;
            if (number == 1 && line.StartsWith("type", StringComparison.OrdinalIgnoreCase))
                continue;

            switch (ParseLine(line))
            {
                case ParsedProduct p:
                    products.Add(p.Value);
                    break;
                case ParsedWarehouse w:
                    warehouses.Add(w.Value);
                    break;
                case ParseError e:
                    errors.Add($"рядок {number}: {e.Reason}");
                    break;
            }
        }

        return new MixedImportResult(products, warehouses, errors);
    }

    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            // 1. Товари: перша колонка "P", далі 4 колонки (id, name, unit, qty)
            ["P", var id, var name, var unit, var qty] 
                when int.TryParse(qty, out int q) && q >= 0
                => new ParsedProduct(new ProductDto(id, $"SKU-{id}", name, unit, q)),

            // 2. Склади: перша колонка "W", далі 3 колонки (id, name, location)
            ["W", var id, var name, var city, var street]
                => new ParsedWarehouse(new WarehouseDto(id, name, $"{city}, {street}")),

            // 3. Загальні помилки
            { Length: < 2 } 
                => new ParseError("занадто мало колонок"),
                
            _ => new ParseError($"невідомий префікс або некоректний формат: '{parts[0]}'")
        };
    }

    // Внутрішня ієрархія для повернення результату парсингу
    private abstract record ParseOutcome;
    private sealed record ParsedProduct(ProductDto Value) : ParseOutcome;
    private sealed record ParsedWarehouse(WarehouseDto Value) : ParseOutcome;
    private sealed record ParseError(string Reason) : ParseOutcome;
}