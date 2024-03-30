using Microsoft.Extensions.Configuration; // To use ConfigurationBuilder.
using System.Diagnostics; // To use Debug and Trace.

// Configuring trace listeners.
string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log.txt");
WriteLine($"Writing to: {logPath}");
TextWriterTraceListener logFile = new(File.CreateText(logPath));
Trace.Listeners.Add(logFile);

#if DEBUG
Trace.AutoFlush = true;
#endif

Debug.WriteLine("Debug says, I am watching!");
Trace.WriteLine("Trace says, I am watching!");

// Create a configuration builder that looks in the current folder for a file named appsettings.json 
string settingsFile = "appsettings.json";
string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), settingsFile);

WriteLine("Processing: {0}", settingsPath);
WriteLine("--{0} contents--", settingsFile);
WriteLine(File.ReadAllText(settingsPath));
WriteLine("----");

ConfigurationBuilder builder = new();

// Add the settings file to the processed configuration and make it
// mandatory so an exception will be thrown if the file is not found.
builder.AddJsonFile(settingsFile, optional: false, reloadOnChange: true);

IConfigurationRoot configuration = builder.Build();

TraceSwitch ts = new("PacktSwitch", "This switch is set via a JSON config.");

configuration.GetSection("PacktSwitch").Bind(ts);

WriteLine($"Trace switch value: {ts.Value}");
WriteLine($"Trace switch level: {ts.Level}");

Trace.WriteLineIf(ts.TraceError, "Trace error");
Trace.WriteLineIf(ts.TraceWarning, "Trace warning");
Trace.WriteLineIf(ts.TraceInfo, "Trace information");
Trace.WriteLineIf(ts.TraceVerbose, "Trace verbose");

int unitsInStock = 12;
LogSourceDetails(unitsInStock > 10);

// Close the text file (also flushes) and release resources.
Debug.Close();
Trace.Close();

WriteLine("Press Enter to exit.");
ReadLine();
