public static class SharedObjects
{
    public static object Conch = new(); // A shared object to lock.
    public static int Counter; // A shared resource.
    public static string? Message; // A shared resource.
}
