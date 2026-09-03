using Counter.Core;
using NUnit.Framework;

namespace Counter.Tests;

[TestFixture]
[NonParallelizable]
public class ServerTests
{
    [SetUp]
    public void Setup()
    {
        Server.Reset();
    }

    [Test]
    public void GetCount_InitialValue_ShouldBeZero()
    {
        Assert.That(Server.GetCount(), Is.EqualTo(0));
    }

    [Test]
    public void AddToCount_SingleAddition_ShouldIncrementCorrectly()
    {
        Server.AddToCount(5);
        Assert.That(Server.GetCount(), Is.EqualTo(5));
    }

    [Test]
    public void AddToCount_MultipleAdditions_ShouldSumCorrectly()
    {
        Server.AddToCount(10);
        Server.AddToCount(-3);
        Server.AddToCount(7);
        Assert.That(Server.GetCount(), Is.EqualTo(14));
    }

    [Test]
    public void AddToCount_WithNegativeValue_ShouldDecreaseCorrectly()
    {
        Server.AddToCount(100);
        Server.AddToCount(-30);
        Assert.That(Server.GetCount(), Is.EqualTo(70));
    }

    [Test]
    public void ConcurrentReaders_ShouldNotBlockEachOther()
    {
        Server.AddToCount(100);

        var tasks = new List<Task<int>>();

        for (int i = 0; i < 10; i++)
            tasks.Add(Task.Run(Server.GetCount));

        Task.WaitAll([.. tasks]);

        foreach (var task in tasks)
            Assert.That(task.Result, Is.EqualTo(100));
    }

    [Test]
    public void ConcurrentWriters_ShouldExecuteSequentially()
    {
        var iterations = 1000;
        var tasks = new List<Task>();

        for (int i = 0; i < iterations; i++)
            tasks.Add(Task.Run(() => Server.AddToCount(1)));

        Task.WaitAll([.. tasks]);

        Assert.That(Server.GetCount(), Is.EqualTo(iterations));
    }

    [Test]
    public void LargeNumberOfOperations_ShouldMaintainConsistency()
    {
        var iterations = 10000;
        var tasks = new List<Task>();
        var rand = new Random();

        for (int i = 0; i < iterations; i++)
        {
            if (rand.Next(2) == 0)            
                tasks.Add(Task.Run(Server.GetCount));            
            else            
                tasks.Add(Task.Run(() => Server.AddToCount(1)));            
        }

        Task.WaitAll([.. tasks]);

        var final = Server.GetCount();

        Assert.That(final, Is.GreaterThanOrEqualTo(0));
        Assert.That(final, Is.LessThanOrEqualTo(iterations));
    }

    [Test]
    public void AddToCount_ShouldBeAtomic_UnderHeavyConcurrency()
    {
        var iterations = 10000;
        var tasks = new List<Task>();

        for (var i = 0; i < iterations; i++)
            tasks.Add(Task.Run(() => Server.AddToCount(1)));

        Task.WaitAll([.. tasks]);

        Assert.That(Server.GetCount(), Is.EqualTo(iterations));
    }

    [Test]
    public void GetCount_ShouldReturnConsistentValue_DuringConcurrentReads()
    {
        Server.AddToCount(42);

        var tasks = new List<Task<int>>();

        for (int i = 0; i < 100; i++)
            tasks.Add(Task.Run(Server.GetCount));

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks)
            Assert.That(task.Result, Is.EqualTo(42));
    }

    [Test]
    public void MixedReadWrite_ShouldMaintainCorrectTotal()
    {
        var addOperations = 500;
        var readOperations = 500;
        var tasks = new List<Task>();

        for (int i = 0; i < addOperations; i++)
            tasks.Add(Task.Run(() => Server.AddToCount(2)));

        for (int i = 0; i < readOperations; i++)
            tasks.Add(Task.Run(Server.GetCount));

        Task.WaitAll([.. tasks]);

        Assert.That(Server.GetCount(), Is.EqualTo(addOperations * 2));
    }
}