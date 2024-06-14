string name = "Samantha Jones";

// Getting the length of the first and last names.
int lengthOfFirst = name.IndexOf(' ');
int lengthOfLast = name.Length - lengthOfFirst - 1;

// Using Substring
string firstName = name.Substring(0, lengthOfFirst);
string lastName = name.Substring(name.Length - lengthOfLast, lengthOfLast);

WriteLine($"First: {firstName}, Last: {lastName}");

// Using spans.
ReadOnlySpan<char> nameAsSpan = name.AsSpan();
ReadOnlySpan<char> firstNameAsSpan = nameAsSpan[0..lengthOfFirst];
ReadOnlySpan<char> lastNameAsSpan = nameAsSpan[^lengthOfLast..];

WriteLine($"First: {firstNameAsSpan}, Last: {lastNameAsSpan}");
