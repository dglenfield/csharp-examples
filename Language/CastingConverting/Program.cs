using System.Globalization;
using static System.Console;

#region Converting from a binary object to a string

// Allocate an array of 128 bytes.
byte[] binaryObject = new byte[128];

// Populate the array with random bytes.
Random.Shared.NextBytes(binaryObject);

WriteLine("Binary Object as bytes:");
for (int index = 0; index < binaryObject.Length; index++)
{
    Write($"{binaryObject[index]:x2} ");
}
WriteLine();

// Convert the array to Base64 string and output as text.
string encoded = Convert.ToBase64String(binaryObject);
WriteLine($"Binary Object as Base64: {encoded}");

#endregion

WriteLine();

#region Parsing from strings to numbers or dates and times

// Set the current culture to make sure date parsing works.
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

int friends = int.Parse("27");
DateTime birthday = DateTime.Parse("1 May 1975");
WriteLine($"I have {friends} friends to invite to my party.");
WriteLine($"My birthday is {birthday}.");
WriteLine($"My birthday is {birthday:D}.");

#endregion

WriteLine();

#region Avoiding Parse exceptions by using the TryParse method

Write("How many eggs are there?");
string? input = ReadLine();

if (int.TryParse(input, out int count))
{
    WriteLine($"There are {count} eggs.");
}
else
{
    WriteLine("I could not parse the input.");
}

#endregion
