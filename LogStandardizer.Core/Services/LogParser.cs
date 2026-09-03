using LogStandardizer.Core.Models;

namespace LogStandardizer.Core.Services;

/// <summary>
/// Парсер логов, применяющий список субпарсеров для разбора строки в <see cref="LogEntry"/>.
/// </summary>
/// <remarks>
/// При попытке разбора строки субпарсеры применяются последовательно.
/// Как только один из них успешно сопоставляется, возвращается результат.
/// Если ни один не подошёл, возвращается null.
/// </remarks>
public sealed class LogParser(List<LogSubParser> subParsers)
{
    private readonly List<LogSubParser> _subParsers = subParsers;

    /// <summary>
    /// Пытается разобрать строку в запись лога.
    /// </summary>
    /// <param name="line">Строка, которую необходимо разобрать.</param>
    /// <returns>
    /// Экземпляр <see cref="LogEntry"/> при успешном разборе, или null, если строка пуста или не соответствует ни одному формату.
    /// </returns>
    /// <exception cref="ArgumentNullException">Если <paramref name="line"/> равен null.</exception>
    /// <remarks>
    /// Пустые или состоящие только из пробелов строки считаются невалидными и возвращают null.
    /// </remarks>
    public LogEntry? ParseLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        foreach (var subParser in _subParsers)
        {
            var match = subParser.LogRegex.Match(line);

            if (match.Success)
                return subParser.EntryFactory(match);
        }

        return null;
    }
}
