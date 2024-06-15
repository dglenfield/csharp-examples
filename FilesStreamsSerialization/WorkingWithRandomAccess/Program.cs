using System.Text; // To use Encoding.
using Microsoft.Win32.SafeHandles; // To use SafeFileHandle.

using SafeFileHandle handle = 
    File.OpenHandle("coffee.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

string message = "Cafe $4.39";
ReadOnlyMemory<byte> buffer = new(Encoding.UTF8.GetBytes(message));
await RandomAccess.WriteAsync(handle, buffer, 0);

long length = RandomAccess.GetLength(handle);
Memory<byte> contentBytes = new(new byte[length]);
await RandomAccess.ReadAsync(handle, contentBytes, 0);
string content = Encoding.UTF8.GetString(contentBytes.ToArray());
WriteLine($"Content of file: {content}");
