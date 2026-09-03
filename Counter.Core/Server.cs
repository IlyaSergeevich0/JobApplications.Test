namespace Counter.Core;

public static class Server
{
    private static int Count = 0;
    private static readonly ReaderWriterLockSlim Lock = new();

    public static int GetCount()
    {
        Lock.EnterReadLock();

        try
        {
            return Count;
        }
        finally
        {
            Lock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        Lock.EnterWriteLock();

        try
        {
            Count += value;
        }
        finally
        {
            Lock.ExitWriteLock();
        }
    }

    public static void Reset()
    {
        Count = 0;
    }
}