using System.Runtime.InteropServices;

// Фікс для коректного виводу кирилиці в консолі Windows
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("CrossApp – практикум з крос-платформного програмування");
Console.WriteLine("Студент: Дранчак Ілля, група ФЕІ-35");
Console.WriteLine(new string('-', 52));

Console.WriteLine($"ОС(OSDescription)    : {RuntimeInformation.OSDescription}");
Console.WriteLine($"ОС(Environment)      : {Environment.OSVersion}");
Console.WriteLine($"Архітектура процесу  : {RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"Версія .NET (CLR)    : {Environment.Version}");
Console.WriteLine($"Runtime              : {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Каталог застосунку   : {AppContext.BaseDirectory}");
Console.WriteLine($"Поточний каталог     : {Environment.CurrentDirectory}");

Console.WriteLine(new string('-', 52));
Console.WriteLine("Предметна область: Склад (Product, StockBatch, Warehouse, Movement)");
Console.WriteLine("Призначення: облік залишків товарів по партіях.");