partial class Program
{
    #region Using lock

    //private static void MethodA()
    //{
    //    lock (SharedObjects.Conch)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            // Simulate two seconds of work on the current thread.
    //            Thread.Sleep(Random.Shared.Next(2000));

    //            // Concatenate the letter "A" to the shared message.
    //            SharedObjects.Message += "A";

    //            // Show some activity in the console output.
    //            Write(".");
    //        }
    //    }
    //}

    //private static void MethodB() 
    //{
    //    lock (SharedObjects.Conch)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            Thread.Sleep(Random.Shared.Next(2000));
    //            SharedObjects.Message += "B";
    //            Write(".");
    //        }
    //    }
    //}
    
    #endregion

    #region Avoiding deadlocks
    
    /* Only use the lock keyword if you can write your code such that it avoids potential deadlocks.
     * If you cannot avoid potential deadlocks, then always use the Monitor.TryEnter method instead
     * of lock, in combination with a try-finally statement, so that you can supply a timeout and 
     * one of the threads will back out of a deadlock if it occurs. */

    private static void MethodA()
    {
        try
        {
            if (Monitor.TryEnter(SharedObjects.Conch, TimeSpan.FromSeconds(15)))
            {
                for (int i = 0; i < 5; i++)
                {
                    // Simulate two seconds of work on the current thread.
                    Thread.Sleep(Random.Shared.Next(2000));

                    // Concatenate the letter "A" to the shared message.
                    SharedObjects.Message += "A";

                    // Safely increment the counter. Not actually needed here because this block of code is locked by the conch.
                    // If we had not already been protecting another shared resource like Message, then using Interlocked would be necessary.
                    Interlocked.Increment(ref SharedObjects.Counter);

                    // Show some activity in the console output.
                    Write(".");
                }
            }
            else
            {
                WriteLine("Method A timed out when entering a monitor on conch.");
            }
        }
        finally
        {
            Monitor.Exit(SharedObjects.Conch);
        }
    }

    private static void MethodB()
    {
        try
        {
            if (Monitor.TryEnter(SharedObjects.Conch, TimeSpan.FromSeconds(15)))
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(Random.Shared.Next(2000));
                    SharedObjects.Message += "B";
                    Interlocked.Increment(ref SharedObjects.Counter);
                    Write(".");
                }
            }
            else
            {
                WriteLine("Method B timed out when entering a monitor on conch.");
            }
        }
        finally
        {
            Monitor.Exit(SharedObjects.Conch);
        }
    }

    #endregion
}
