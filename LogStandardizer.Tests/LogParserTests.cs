using LogStandardizer.Core.Enumerations;
using LogStandardizer.Core.Services;
using NUnit.Framework;

namespace LogStandardizer.Tests;

[TestFixture]
public class LogParserTests
{
    private LogParser _parser = default!;

    [SetUp]
    public void Setup()
    {
        var subParsers = new List<LogSubParser>
        {
            LogSubParser.CreateStandardFormat1Parser(),
            LogSubParser.CreateStandardFormat2Parser()
        };

        _parser = new LogParser(subParsers);
    }

    [Test]
    public void ParseLine_ValidFormat1_ReturnsEntry()
    {
        // Arrange
        var line = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual(new DateTime(2025, 3, 10), entry?.Date);
        Assert.AreEqual("15:14:49.523", entry?.Time);
        Assert.AreEqual(LogLevel.INFO, entry?.Level);
        Assert.IsNull(entry?.Method);
        Assert.AreEqual("Версия программы: '3.4.0.48729'", entry?.Message);
    }

    [Test]
    public void ParseLine_ValidFormat2_ReturnsEntry()
    {
        // Arrange
        var line = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual(new DateTime(2025, 3, 10), entry?.Date);
        Assert.AreEqual("15:14:51.5882", entry?.Time);
        Assert.AreEqual(LogLevel.INFO, entry?.Level);
        Assert.AreEqual("MobileComputer.GetDeviceId", entry?.Method);
        Assert.AreEqual("Код устройства: '@MINDEO-M40-D-410244015546'", entry?.Message);
    }

    [Test]
    public void ParseLine_InvalidLine_ReturnsNull()
    {
        // Arrange
        var line = "Это не лог";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNull(entry);
    }

    [Test]
    public void ParseLine_EmptyLine_ReturnsNull()
    {
        // Act
        var entry = _parser.ParseLine(string.Empty);

        // Assert
        Assert.IsNull(entry);
    }

    [Test]
    public void ParseLine_Format1WithWarning_ReturnsWarn()
    {
        // Arrange
        var line = "10.03.2025 15:14:49.523 WARNING Это предупреждение";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual(LogLevel.WARN, entry?.Level);
    }

    [Test]
    public void ParseLine_Format2WithMethodWithDots_Works()
    {
        // Arrange
        var line = "2025-03-10 15:14:51.5882| INFO|11|Namespace.Class.Method| Сообщение";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual("Namespace.Class.Method", entry?.Method);
    }

    [Test]
    public void ParseLine_Format1WithErrorLevel_ReturnsError()
    {
        // Arrange
        var line = "10.03.2025 15:14:49.523 ERROR Ошибка";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual(LogLevel.ERROR, entry?.Level);
    }

    [Test]
    public void ParseLine_Format2WithDebugLevel_ReturnsDebug()
    {
        // Arrange
        var line = "2025-03-10 15:14:51.5882| DEBUG|42|SomeClass.SomeMethod| Отладочное сообщение";

        // Act
        var entry = _parser.ParseLine(line);

        // Assert
        Assert.IsNotNull(entry);
        Assert.AreEqual(LogLevel.DEBUG, entry?.Level);
    }
}