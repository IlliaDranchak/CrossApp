using Core.Domain;

Console.WriteLine("=== Сценарій 1: успіх ===");
Product product = Product.Create("P-001", "sku-001", "Цемент М400 25кг", "шт", 100);
Console.WriteLine(product);

product.RegisterArrival(50);
product.Issue(30);
Console.WriteLine(product);

Console.WriteLine();
Console.WriteLine("=== Сценарій 2: порушення інваріантів ===");

TryDo("видача більша за залишок", () => product.Issue(1000));
TryDo("порожній SKU", () => Product.Create("P-002", " ", "Пісок", "т", 10));
TryDo("від'ємний залишок", () => Product.Create("P-003", "SKU-003", "Цегла", "шт", -5));

// Перевірка, що стан не змінився після помилок
Console.WriteLine($"\n[Перевірка] Залишок після всіх помилок: {product.Quantity} (має бути 120)");


Console.WriteLine("\n=== Додаткове завдання 3: Статуси та переходи ===");
Product statusProduct = Product.Create("P-001", "SKU-001", "Цемент", "шт", 100);
Console.WriteLine($"Створено: Статус = {statusProduct.Status}");

statusProduct.ChangeStatus(ProductStatus.Active);
Console.WriteLine($"Змінено на Active: Статус = {statusProduct.Status}");

TryDo("Недопустимий перехід (Active -> Draft)", () => statusProduct.ChangeStatus(ProductStatus.Draft));

Console.WriteLine("\n=== Додаткове завдання 1: Зв'язок з тижнем 3 (DTO -> Domain) ===");
// Симулюємо результат імпорту з тижня 3: 1 хороший DTO, 1 поганий (порушує інваріант)
var goodDto = new Core.Dto.ProductDto("P-GOOD", "SKU-GOOD", "Хороший", "шт", 50);
var badDto = new Core.Dto.ProductDto("P-BAD", "", "Поганий", "шт", -10); // Порожній SKU і від'ємна кількість
var dtoResult = new Core.Dto.ImportResult<Core.Dto.ProductDto>(
    new[] { goodDto, badDto }, 
    new[] { "рядок 99: синтаксична помилка CSV" } // Помилка ще з етапу читання файлу
);

// Викликаємо наш новий метод
var domainResult = Core.Import.ProductCsvImporter.ToDomain(dtoResult);

Console.WriteLine($"Успішно перетворено на сутності: {domainResult.Items.Count}");
Console.WriteLine($"Всього помилок (синтаксис + інваріанти): {domainResult.Errors.Count}");
foreach (var err in domainResult.Errors)
{
    Console.WriteLine($" ! {err}");
}
// Локальна функція для обробки винятків
static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($" {title}: виняток НЕ спрацював — інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" {title}: {ex.GetType().Name} — {ex.Message}");
    }
}