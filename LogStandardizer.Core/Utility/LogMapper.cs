using LogStandardizer.Core.Enumerations;

namespace LogStandardizer.Core.Utility;

public static class LogMapper
{
    private static readonly Dictionary<string, LogLevel> LevelMap = new(StringComparer.OrdinalIgnoreCase) {
        ["INFORMATION"] = LogLevel.INFO,
        ["INFO"] = LogLevel.INFO,
        ["WARNING"] = LogLevel.WARN,
        ["WARN"] = LogLevel.WARN,
        ["ERROR"] = LogLevel.ERROR,
        ["DEBUG"] = LogLevel.DEBUG
    };

    public static LogLevel MapLevel(string levelStr)
    {
        return LevelMap.TryGetValue(levelStr.Trim(), out var level) ? level : LogLevel.INFO;
    }
}
