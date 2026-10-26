using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class ProductJsonImporter
{
    public static ImportResult<ProductDto> Load(string path)
    {
        var items = new List<ProductDto>();
        var errors = new List<string>();

        try
        {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDto>? parsed = JsonSerializer.Deserialize<List<ProductDto>>(json, options);
            if (parsed is not null)
            {
                items.AddRange(parsed);
            }
        }
        catch (JsonException ex)
        {
            errors.Add($"помилка JSON: {ex.Message}");
        }

        return new ImportResult<ProductDto>(items, errors);
    }
}