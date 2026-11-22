using Core.Dto;

namespace Core.Domain;

public enum ProductStatus { Draft, Active, Archived }

public sealed class Product
{
    // Приватне поле для зберігання стану (інкапсуляція)
    private int _quantity;
    private ProductStatus _status = ProductStatus.Draft;
    public ProductStatus Status => _status;

    // Властивості лише для читання (без set і init)
    public string Id { get; }
    public string Sku { get; }
    public string Name { get; }
    public string Unit { get; }
    
    // Властивість-вираз, яка читає приватне поле
    public int Quantity => _quantity;

    // Приватний конструктор: зовнішній код не може викликати new Product(...)
    private Product(string id, string sku, string name, string unit, int quantity)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Unit = unit;
        _quantity = quantity;
    }

    // Єдиний публічний спосіб створити товар (Фабричний метод)
    public static Product Create(string id, string sku, string name, string unit, int quantity)
    {
        // ІНВАРІАНТ 1: Ідентифікатор не може бути порожнім
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор обов'язковий", nameof(id));
            
        // ІНВАРІАНТ 2: SKU не може бути порожнім
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU не може бути порожнім", nameof(sku));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва не може бути порожньою", nameof(name));
            
        // ІНВАРІАНТ 3: Початкова кількість не може бути від'ємною
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Початковий залишок не може бути від'ємним");

        // Нормалізація даних перед збереженням
        return new Product(
            id.Trim(), 
            sku.Trim().ToUpperInvariant(), 
            name.Trim(), 
            unit.Trim(), 
            quantity
        );
    }

    // Метод зміни стану: Прихід товару
    public void RegisterArrival(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Кількість приходу має бути більшою за нуль");
        
        _quantity += amount;
    }

    // Метод зміни стану: Видача товару
    public void Issue(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Кількість видачі має бути більшою за нуль");
            
        // ІНВАРІАНТ 4: Не можна видати більше, ніж є на складі
        if (amount > _quantity)
            throw new InvalidOperationException($"Не можна видати {amount}: залишок {Sku}={_quantity}");
            
        _quantity -= amount;
    }

    public void ChangeStatus(ProductStatus newStatus)
    {
        // Перевірка допустимих переходів через кортежі та switch expression
        bool allowed = (_status, newStatus) switch
        {
            (ProductStatus.Draft, ProductStatus.Active) => true,
            (ProductStatus.Active, ProductStatus.Archived) => true,
            _ => false
        };

        if (!allowed)
            throw new InvalidOperationException($"Недопустимий перехід статусу: з {_status} у {newStatus}");

        _status = newStatus;
    }
    // Мапінг у DTO (для сховища з 5-го тижня)
    public ProductDto ToDto() => new(Id, Sku, Name, Unit, Quantity);
    
    // Мапінг з DTO (ОБОВ'ЯЗКОВО проходить через Create для перевірки інваріантів!)
    public static Product FromDto(ProductDto dto) => Create(dto.Id, dto.Sku, dto.Name, dto.Unit, dto.Quantity);

    // Перевизначення ToString для красивого виводу в консоль
    public override string ToString() => $"{Id}[{Sku}] {Name} — {Quantity} {Unit}";
}