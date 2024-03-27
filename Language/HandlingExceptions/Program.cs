using static System.Console;

/* Avoid writing code that will throw an exception whenever possible. 
 * Avoid over-catching exceptions. They should often be allowed to propagate up the call stack to be handled at a level 
 * where more information is known about the circumstances that could change the logic of how they should be handled. */

#region Wrapping error-prone code in a try block

WriteLine("Before parsing");
Write("What is your age? ");
string? input = ReadLine();

try
{
    int age = int.Parse(input!); // Use the null-forgiving operator ! to disable compiler warning about possible null value.
    WriteLine($"You are {age} years old.");
}
catch (OverflowException) 
{
    WriteLine("Your age is a valid number format but it is either too big or small.");
}
catch (FormatException)
{
    WriteLine("The age you entered is not a valid number format.");
}
catch (Exception ex)
{
    WriteLine($"{ex.GetType()} says {ex.Message}");
}

WriteLine("After parsing");

#endregion

WriteLine();

#region Catching with filters

Write("Enter an amount: ");
string amount = ReadLine()!;
if (string.IsNullOrEmpty(amount)) return;

try
{
    decimal amountValue = decimal.Parse(amount);
    WriteLine($"Amount formatted as currency: {amountValue:C}");
}
catch (FormatException) when (amount.Contains("$"))
{
    WriteLine("Amounts cannot use the dollar sign!");
}
catch (FormatException)
{
    WriteLine("Amount must contain digits!");
}

#endregion

WriteLine();

#region Throwing overflow exceptions with the checked statement

/* The checked statement tells .NET to throw an exception when an overflow happens 
 * instead of allowing it to happen silently, which is done by default for performance reasons. */

try
{
    checked
    {
        int x = int.MaxValue - 1;
        WriteLine($"Initial value: {x}");
        x++;
        WriteLine($"After incrementing: {x}");
        x++;
        WriteLine($"After incrementing: {x}");
        x++;
        WriteLine($"After incrementing: {x}");
    }
}
catch (OverflowException)
{
    WriteLine("The code overflowed but I caught the exception.");
}

#endregion

WriteLine();

#region Disabling compiler overflow checks with the unchecked statement

unchecked
{
    int y = int.MaxValue + 1;
    WriteLine($"Initial value: {y}");
    y--;
    WriteLine($"After decrementing: {y}");
    y--;
    WriteLine($"After decrementing: {y}");
}

#endregion
