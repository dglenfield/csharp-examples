// Statically import the System.Console class so that the Console type name won't be needed throughout the code.
using static System.Console;

int numberOfApples = 12;
decimal pricePerApple = 0.35M;

// Formatting using numbered positional arguments.
/* Boxing happens when value types like int and DateTime are passed as positional arguments to string formats.
 * This is a problem for Unity projects because they use a different memory garbage collector to the normal .NET runtime.
 */
WriteLine("{0} apples cost {1:C}", numberOfApples, pricePerApple * numberOfApples);

string formatted = string.Format("{0} apples cost {1:C}", numberOfApples, pricePerApple * numberOfApples);
WriteLine(formatted);

// Formatting using interpolated strings
/* For short, formatted string values, aan interpolated string can be easier to read. 
 * Using interpolated string formats is an easy way to avoid boxing when writing code that will be paart of a Unity project. 
 * A reason to avoid interpolated strings is that they can't be read from resource files to be localized. 
 */
WriteLine($"{numberOfApples} apples cost {pricePerApple * numberOfApples:C}");

// Output a table of fruit and how many of each there are.
// Left-align the names within a column of 10 characters and right-align the counts formatted as numbers with zero decmal places
// within a column of six characters.
string applesText = "Apples";
int applesCount = 1234;
string bananasText = "Bananas";
int bananasCount = 56789;
WriteLine();
WriteLine("{0,-10} {1,6}", "Name", "Count");
WriteLine("{0,-10} {1,6:N0}", applesText, applesCount);
WriteLine("{0,-10} {1,6:N0}", bananasText, bananasCount);

// Getting text input from the user.
Write("\nType your first name and press ENTER: ");
string? firstName = ReadLine(); /* Tells the compiler we are expecting a possible null value so it does not need to warn us.
                                 * If the varable is null then when it is later output with WriteLine, it will just be blank. 
                                 * The case where it is null would need to be handled to access any of the members of the firstName variable.
                                 */

Write("Type your age and press ENTER: ");
string age = ReadLine()!; /* This uses the null-forgiving operator to tell the compiler that ReadLine will not return null,
                           * so it can stop showing the warning. 
                           */

WriteLine($"Hello {firstName}, you look good for {age}.");

// Getting key input from the user.
Write("\nPress any key combination: ");
ConsoleKeyInfo key = ReadKey();
WriteLine("\nKey: {0}, Char: {1}, Modifiers: {2}", key.Key, key.KeyChar, key.Modifiers);
