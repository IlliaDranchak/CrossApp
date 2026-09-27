using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Encodings.Web; 

Console.OutputEncoding = System.Text.Encoding.UTF8;

bool outputJson = args.Contains("--json");

var appInfo = new
{
    Student = "Дранчак Ілля, ФЕІ-35",
    OS_Description = RuntimeInformation.OSDescription,
    OS_Environment = Environment.OSVersion.ToString(),
    Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
    DotNet_Version = Environment.Version.ToString(),
    Runtime = RuntimeInformation.FrameworkDescription,
    App_Directory = AppContext.BaseDirectory,
    Current_Directory = Environment.CurrentDirectory,
    Domain = "Склад",
    Entities = new[] { "Product", "StockBatch", "Warehouse", "Movement" },
    Purpose = "облік залишків товарів по партіях"
};

if (outputJson)
{
    var options = new JsonSerializerOptions 
    { 
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    Console.WriteLine(JsonSerializer.Serialize(appInfo, options));
}
else
{
    Console.WriteLine("CrossApp – практикум з крос-платформного програмування");
    Console.WriteLine($"Студент: {appInfo.Student}");
    Console.WriteLine(new string('-', 52));
    
    Console.WriteLine($"ОС(OSDescription)    : {appInfo.OS_Description}");
    Console.WriteLine($"ОС(Environment)      : {appInfo.OS_Environment}");
    Console.WriteLine($"Архітектура процесу  : {appInfo.Architecture}");
    Console.WriteLine($"Версія .NET (CLR)    : {appInfo.DotNet_Version}");
    Console.WriteLine($"Runtime              : {appInfo.Runtime}");
    Console.WriteLine($"Каталог застосунку   : {appInfo.App_Directory}");
    Console.WriteLine($"Поточний каталог     : {appInfo.Current_Directory}");
    
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Предметна область: {appInfo.Domain}");
    Console.WriteLine($"Сутності: {string.Join(", ", appInfo.Entities)}");
    Console.WriteLine($"Призначення: {appInfo.Purpose}");
}