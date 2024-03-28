
TimesTable(7);
TimesTable(number: 7, size: 20);

// Writing a function that returns a value.
ConfigureConsole(culture: "fr-FR");
decimal taxToPay = CalculateTax(149, "FR");
WriteLine($"You must pay {taxToPay:C} in tax.");

ConfigureConsole();

WriteLine();

// Converting numbers from cardinal to ordinal.
RunCardinalToOrdinal();

WriteLine();

// Calculating factorials with recursion.
RunFactorial();

WriteLine();

// Using lambdas in function implementations.
RunFibImperative();
WriteLine();
RunFibFunctional();

