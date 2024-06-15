using System.Text.Json; // To use JsonSerializer.
using Packt.Shared; // To use Book.

Book csharpBook = new("C# 12 and .NET 8 - Modern Cross-Platform Development Fundamentals")
{
    Author = "Mark J Price",
    PublishDate = new(2023, 11, 14),
    Pages = 823,
    Created = DateTimeOffset.Now
};

JsonSerializerOptions options = new() 
{
    IncludeFields = true, // Includes all fields
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

string path = Combine(CurrentDirectory, "book.json");

using (Stream fileStream = File.Create(path))
{
    JsonSerializer.Serialize(fileStream, csharpBook, options);
}

WriteLine("**** File Info ****");
WriteLine($"File: {GetFileName(path)}");
WriteLine($"Path: {GetDirectoryName(path)}");
WriteLine($"Size: {new FileInfo(path).Length:N0} bytes.");
WriteLine("/--------------------");
WriteLine(File.ReadAllText(path));
WriteLine("--------------------/");
