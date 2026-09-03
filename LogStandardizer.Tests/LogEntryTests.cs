using LogStandardizer.Core.Enumerations;
using LogStandardizer.Core.Models;
using NUnit.Framework;

namespace LogStandardizer.Tests;

[TestFixture]
public class LogEntryTests
{
    [Test]
    public void ToStandardizedString_NoMethod_InsertsDEFAULT()
    {
        // Arrange
        var entry = new LogEntry(
            new DateTime(2025, 3, 10),
            "15:14:49.523",
            LogLevel.INFO,
            null,
            "Test message"
        );

        // Act
        var result = entry.ToStandardizedString();

        // Assert
        Assert.AreEqual("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tTest message", result);
    }

    [Test]
    public void ToStandardizedString_WithMethod_IncludesMethod()
    {
        // Arrange
        var entry = new LogEntry(
            new DateTime(2025, 3, 10),
            "15:14:51.5882",
            LogLevel.INFO,
            "MobileComputer.GetDeviceId",
            "Код устройства: '@MINDEO-M40-D-410244015546'"
        );

        // Act
        var result = entry.ToStandardizedString();

        // Assert
        Assert.AreEqual("10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'", result);
    }

    [Test]
    public void ToStandardizedString_WithEmptyMethod_InsertsDEFAULT()
    {
        // Arrange
        var entry = new LogEntry(
            new DateTime(2025, 3, 10),
            "15:14:49.523",
            LogLevel.INFO,
            "",
            "Test message"
        );

        // Act
        var result = entry.ToStandardizedString();

        // Assert
        Assert.AreEqual("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tTest message", result);
    }

    [Test]
    public void ToStandardizedString_WithWhitespaceMethod_InsertsDEFAULT()
    {
        // Arrange
        var entry = new LogEntry(
            new DateTime(2025, 3, 10),
            "15:14:49.523",
            LogLevel.INFO,
            "   ",
            "Test message"
        );

        // Act
        var result = entry.ToStandardizedString();

        // Assert
        Assert.AreEqual("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tTest message", result);
    }
}