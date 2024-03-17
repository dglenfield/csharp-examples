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
