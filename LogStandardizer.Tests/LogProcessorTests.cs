using LogStandardizer.Core.Services;
using NUnit.Framework;

namespace LogStandardizer.Tests;

[TestFixture]
public class LogProcessorTests
{
    [Test]
    public void ProcessLines_GoodAndBad_OutputsToCorrectWriters()
    {
        // Arrange
        var subParsers = new List<LogSubParser>
        {
            LogSubParser.CreateStandardFormat1Parser(),
            LogSubParser.CreateStandardFormat2Parser()
        };
        var parser = new LogParser(subParsers);

        var lines = new[]
        {
            "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
            "невалидная строка",
            "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'"
        };

        using var goodWriter = new StringWriter();
        using var problemWriter = new StringWriter();

        var processor = new LogProcessor(parser, goodWriter, problemWriter);

        // Act
        processor.ProcessLines(lines);

        // Assert
        var goodOutput = goodWriter.ToString();
        var problemOutput = problemWriter.ToString();

        var goodLines = goodOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var problemLines = problemOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        Assert.AreEqual(2, goodLines.Length);
        Assert.AreEqual(1, problemLines.Length);
        Assert.That(problemOutput, Does.Contain("невалидная строка"));
    }

    [Test]
    public void ProcessLines_EmptyLines_Ignored()
    {
        // Arrange
        var subParsers = new List<LogSubParser>
        {
            LogSubParser.CreateStandardFormat1Parser()
        };
        var parser = new LogParser(subParsers);

        var lines = new[]
        {
            "",
            "10.03.2025 15:14:49.523 INFORMATION Test",
            "   "
        };

        using var goodWriter = new StringWriter();
        using var problemWriter = new StringWriter();

        var processor = new LogProcessor(parser, goodWriter, problemWriter);

        // Act
        processor.ProcessLines(lines);

        // Assert
        var goodOutput = goodWriter.ToString();
        var problemOutput = problemWriter.ToString();

        var goodLines = goodOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var problemLines = problemOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        Assert.AreEqual(1, goodLines.Length);
        Assert.AreEqual(0, problemLines.Length);
    }

    [Test]
    public void ProcessLines_AllInvalid_AllGoToProblem()
    {
        // Arrange
        var subParsers = new List<LogSubParser>
        {
            LogSubParser.CreateStandardFormat1Parser()
        };
        var parser = new LogParser(subParsers);

        var lines = new[]
        {
            "первая невалидная",
            "вторая невалидная"
        };

        using var goodWriter = new StringWriter();
        using var problemWriter = new StringWriter();

        var processor = new LogProcessor(parser, goodWriter, problemWriter);

        // Act
        processor.ProcessLines(lines);

        // Assert
        var goodOutput = goodWriter.ToString();
        var problemOutput = problemWriter.ToString();

        var goodLines = goodOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var problemLines = problemOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        Assert.AreEqual(0, goodLines.Length);
        Assert.AreEqual(2, problemLines.Length);
        Assert.That(problemOutput, Does.Contain("первая невалидная"));
        Assert.That(problemOutput, Does.Contain("вторая невалидная"));
    }

    [Test]
    public void ProcessLines_AllValid_AllGoToGood()
    {
        // Arrange
        var subParsers = new List<LogSubParser>
        {
            LogSubParser.CreateStandardFormat1Parser()
        };
        var parser = new LogParser(subParsers);

        var lines = new[]
        {
            "10.03.2025 15:14:49.523 INFORMATION Сообщение 1",
            "10.03.2025 15:14:49.523 DEBUG Сообщение 2"
        };

        using var goodWriter = new StringWriter();
        using var problemWriter = new StringWriter();

        var processor = new LogProcessor(parser, goodWriter, problemWriter);

        // Act
        processor.ProcessLines(lines);

        // Assert
        var goodOutput = goodWriter.ToString();
        var problemOutput = problemWriter.ToString();

        var goodLines = goodOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var problemLines = problemOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        Assert.AreEqual(2, goodLines.Length);
        Assert.AreEqual(0, problemLines.Length);
    }
}