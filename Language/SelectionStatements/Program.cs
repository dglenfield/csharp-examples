using SelectionStatements;
using static System.Console;

#region Pattern matching with the if statement

// Add and remove the "" to change between string and int.
object o = "3";
int j = 4;

if (o is int i) // If the value stored in the variable named o is an int, then the value is assigned to the local variable named i.
{
    WriteLine($"{i} x {j} = {i * j}");
}
else
{
    WriteLine("o is not an int so it cannot multiply!");
}

#endregion

#region Branching with the switch statement

// Generate a random number between 1 and 6 (inclusive lower bound but exclusive upper bound).
int number = Random.Shared.Next(minValue: 1, maxValue: 7);
WriteLine($"\nMy random number is {number}");

switch (number)
{
    case 1:
        WriteLine("One");
        break;
    case 2:
        WriteLine("Two");
        goto case 1;
    case 3: // Multiple case section.
    case 4:
        WriteLine("Three or four");
        goto case 1;
    case 5:
        goto A_label;
    default:
        WriteLine("Default");
        break;
}

WriteLine("After end of switch");

A_label:
WriteLine("After A_label");

#endregion

WriteLine();

#region Pattern matching with the switch statement

var animals = new Animal?[]
{
    new Cat { Name = "Karen", Born = new(2022, 8, 23), Legs = 4, IsDomestic = true },
    null,
    new Cat { Name = "Mufasa", Born = new(1994, 6,12) },
    new Spider { Name = "Sid Vicious", Born = DateTime.Today, IsPoisonous = true },
    new Spider { Name = "Captain Furry", Born= DateTime.Today }
};

foreach (Animal? animal in animals)
{
    string message;

    switch (animal)
    {
        case Cat fourLeggedCat when fourLeggedCat.Legs == 4:
            message = $"The cat named {fourLeggedCat.Name} has four legs.";
            break;
        case Cat { IsDomestic: false } wildCat: // case statement written using the more concise property pattern-matching syntax.
            message = $"The non-domestic cat is named {wildCat.Name}.";
            break;
        case Cat cat:
            message = $"The cat is named {cat.Name}.";
            break;
        case Spider spider when spider.IsPoisonous:
            message = $"The {spider.Name} spider is poisonous. Run!";
            break;
        case null:
            message = "The animal is null.";
            break;
        default:
            message = $"{animal.Name} is a {animal.GetType().Name}.";
            break;
    }

    WriteLine($"switch statement: {message}");
}

#endregion

WriteLine();

#region Simplifying switch statements with switch expressions

foreach (Animal? animal in animals)
{
    string message = animal switch
    {
        Cat { Legs: 4 } fourLeggedCat => $"The cat named {fourLeggedCat.Name} has four legs.", // case statement written using the more concise property pattern-matching syntax.
        Cat { IsDomestic: false } wildCat => $"The non-domestic cat is named {wildCat.Name}.", // case statement written using the more concise property pattern-matching syntax.
        Cat cat => $"The cat is named {cat.Name}.",
        Spider spider when spider.IsPoisonous => $"The {spider.Name} spider is poisonous. Run!",
        null => "The animal is null.",
        _ => $"{animal.Name} is a {animal.GetType().Name}."
    };

    WriteLine($"switch expression: {message}");
}

#endregion
