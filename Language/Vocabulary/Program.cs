// Literal strings (with escape characters)
string fullNameWithTabSeparator = "Bob\tSmith";
Console.WriteLine(fullNameWithTabSeparator);

// Verbatim strings
/* A literal string prefixed with @ to disable escape characters. */
string filePath = @"c:\televisions\sony\bravia.txt";
Console.WriteLine(filePath);

// Raw string literals
string xml = """
    <person age="50">
        <first_name>Mark</first_name>
    </person>
    """;
Console.WriteLine(xml);

// Raw interpolated string literals
/* A literal string prefixed with $ to enable embedded formatted variables. 
   The number of dollar signs tells the compiler how many curly braces are needed to
   become recognized as an interpolated expression. */
var person = new { FirstName = "Alice", Age = 56 };
string json = $$"""
    {
        "first_name": "{{person.FirstName}}",
        "age": {{person.Age}},
        "calculation": "{{{ 1 + 2 }}}"
    }
    """;
Console.WriteLine(json);

// Using target-typed new to instantiate objects
System.Xml.XmlDocument xmlDocument = new(); // Target-typed new in C# 9 or later.

// Getting and setting the default values for types.
Console.WriteLine($"default(int) = {default(int)}");
Console.WriteLine($"default(bool) = {default(bool)}");
Console.WriteLine($"default(DateTime) = {default(DateTime)}");
Console.WriteLine($"default(string) = {default(string)}");

int number = 13;
Console.WriteLine($"number set to: {number}");
number = default;
Console.WriteLine($"number reset to its default: {number}");
