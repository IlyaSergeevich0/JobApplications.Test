using LogStandardizer.Core.Enumerations;
using LogStandardizer.Core.Utility;
using NUnit.Framework;

namespace LogStandardizer.Tests;

[TestFixture]
public class LogMapperTests
{
    [TestCase("INFORMATION", LogLevel.INFO)]
    [TestCase("INFO", LogLevel.INFO)]
    [TestCase("WARNING", LogLevel.WARN)]
    [TestCase("WARN", LogLevel.WARN)]
    [TestCase("ERROR", LogLevel.ERROR)]
    [TestCase("DEBUG", LogLevel.DEBUG)]
    public void MapLevel_ValidInput_ReturnsExpected(string input, LogLevel expected)
    {
        // Act
        var result = LogMapper.MapLevel(input);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void MapLevel_Unknown_ReturnsInfo()
    {
        // Act
        var result = LogMapper.MapLevel("UNKNOWN");

        // Assert
        Assert.AreEqual(LogLevel.INFO, result);
    }

    [Test]
    public void MapLevel_CaseInsensitive_Works()
    {
        // Act
        var result = LogMapper.MapLevel("information");

        // Assert
        Assert.AreEqual(LogLevel.INFO, result);
    }

    [Test]
    public void MapLevel_WithWhitespace_Works()
    {
        // Act
        var result = LogMapper.MapLevel("  INFO  ");

        // Assert
        Assert.AreEqual(LogLevel.INFO, result);
    }
}