using Core.Dto;
using Core.Domain;

namespace Core.Import;

public static class ProductCsvImporter
{
    private const char Separator = ';';

    public static ImportResult<ProductDto> Load(string path)
    {
        var items = new List<ProductDto>();
        var errors = new List<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            if (number == 1 && line.StartsWith("id", StringComparison.OrdinalIgnoreCase))
                continue;

            switch (ParseLine(line))
            {
                case ParseOk ok:
                    items.Add(ok.Value);
                    break;
                case ParseFailed failed:
                    errors.Add($"рядок {number}: {failed.Reason}");
                    break;
            }
        }

        return new ImportResult<ProductDto>(items, errors);
    }

    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            { Length: < 5 } => new ParseFailed($"очікую 5 колонок, отримав {parts.Length}"),
            [_, "", _, _, _] or [_, _, "", _, _] => new ParseFailed("SKU або назва порожні"),
            [_, _, _, _, var qty] when !int.TryParse(qty, out int q) || q < 0 
                => new ParseFailed($"кількість '{qty}' не є невід'ємним числом"),
            [var id, var sku, var name, var unit, var qty] 
                => new ParseOk(new ProductDto(id, sku, name, unit, int.Parse(qty))),
            _ => new ParseFailed($"занадто багато колонок: {parts.Length}")
        };
    }
// Міст між тижнем 3 (DTO) і тижнем 4 (Domain)
    public static ImportResult<Product> ToDomain(ImportResult<ProductDto> dtoResult)
    {
        var items = new List<Product>();
        // Зберігаємо помилки, які вже були на етапі читання CSV
        var errors = new List<string>(dtoResult.Errors); 

        foreach (var dto in dtoResult.Items)
        {
            try
            {
                // FromDto викличе Create, який перевірить інваріанти
                items.Add(Product.FromDto(dto));
            }
            catch (Exception ex)
            {
                // Якщо інваріант порушено, додаємо в список помилок, а не падаємо
                errors.Add($"Порушення інваріанту для {dto.Id}: {ex.Message}");
            }
        }

        return new ImportResult<Product>(items, errors);
    }
    private abstract record ParseOutcome;
    private sealed record ParseOk(ProductDto Value) : ParseOutcome;
    private sealed record ParseFailed(string Reason) : ParseOutcome;
}
    