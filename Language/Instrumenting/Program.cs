using static System.Console;
using System.Diagnostics; // To use Debug and Trace.

// Writing to the default trace listener.
Debug.WriteLine("Debug says, I am watching!");
Trace.WriteLine("Trace says, I am watching!");

// Configuring trace listeners.
string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log.txt");

WriteLine($"Writing to: {logPath}");

TextWriterTraceListener logFile = new(File.CreateText(logPath));

Trace.Listeners.Add(logFile);

/* Any type that represents a file usually implements a buffer to improve performance. Instead of writing
 * immediately to the file, data is written to an in-memory buffer, and only once the buffer is full will
 * it be written in one chunk to the file. This behavior can be confusing while debugging because we do
 * not immediately see the results. 
 * 
 * Enabling AutoFlush means the Flush method is called automatically after every write. This reduces 
 * performance, so you should only set it on during debugging and not in production. */
#if DEBUG
// Text writer is buffered, so this option calls Flush() on all listeners after writing.
Trace.AutoFlush = true;
#endif

Debug.WriteLine("Debug says, I am watching!");
Trace.WriteLine("Trace says, I am watching!");

// Close the text file (also flushes) and release resources.
Debug.Close();
Trace.Close();

/* When running with the Debug configuration, both Debug and Trace are active and will write to any trace listeners.
 * When running with the Release configuration, only Trace will write to any trace listeners. 
 * You can therefore use Debug.WriteLine calls liberally throughout your code, knowing they will be stripped out 
 * automatically when you build the release version of your application and will not affect performance. */
