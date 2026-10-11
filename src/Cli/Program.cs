using Core;
using System.Text.Json;
using System.Text.Encodings.Web;


Console.OutputEncoding = System.Text.Encoding.UTF8;


bool outputJson = args.Contains("--json");


EnvironmentReport report = EnvironmentInfo.Collect();

if (outputJson)
{
    //  РЕЖИМ JSON
    var appInfo = new
    {
        Student = "Дранчак Ілля, ФЕІ-35", 
        OS_Description = report.OsDescription,
        OS_Environment = Environment.OSVersion.ToString(),
        Architecture = report.ProcessArchitecture,
        DotNet_Version = Environment.Version.ToString(),
        Runtime = report.FrameworkDescription,
        App_Directory = report.BaseDirectory,
        Current_Directory = Environment.CurrentDirectory,
        DetectedRid = report.DetectedRid,
        ReportedRid = report.ReportedRid,
        Domain = "Склад",
        Entities = new[] { "Product", "StockBatch", "Warehouse", "Movement" },
        Purpose = "облік залишків товарів по партіях"
    };

    var options = new JsonSerializerOptions 
    { 
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    Console.WriteLine(JsonSerializer.Serialize(appInfo, options));
}
else
{

    Console.WriteLine("CrossApp – інформація про середовище");
    Console.WriteLine("Студент: Дранчак Ілля, ФЕІ-35"); 
    Console.WriteLine(new string('-', 52));
    
    Console.WriteLine($"ОС             : {report.OsDescription}");
    Console.WriteLine($"Runtime        : {report.FrameworkDescription}");
    Console.WriteLine($"Архітектура    : {report.ProcessArchitecture}");
    Console.WriteLine($"RID(визначено) : {report.DetectedRid}");
    Console.WriteLine($"RID(від.NET)   : {report.ReportedRid}");
    Console.WriteLine($"Каталог        : {report.BaseDirectory}");
}