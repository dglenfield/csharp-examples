// Passing arguments to a console app.
WriteLine($"There are {args.Length} arguments.");

foreach (string arg in args)
{
    WriteLine(arg);
}

// Setting options with arguments
if (args.Length < 3)
{
    WriteLine("You must specify two colors and cursor size, e.g.");
    WriteLine("dotnet run red yellow 50");
    return; // Stop running
}

ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[0], true);
BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[1], true);

if (OperatingSystem.IsWindows())
{
    CursorSize = int.Parse(args[2]);
}
else
{
    WriteLine("The current platform does not support changing the size of the cursor.");
}
