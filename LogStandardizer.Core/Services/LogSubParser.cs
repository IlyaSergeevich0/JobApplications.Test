using LogStandardizer.Core.Models;
using LogStandardizer.Core.Utility;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogStandardizer.Core.Services;

/// <summary>
/// Связывает регулярное выражение для разбора строки с фабрикой, создающей запись лога.
/// </summary>
/// <remarks>
/// Используется в <see cref="LogParser"/> для последовательного применения различных форматов.
/// Позволяет легко добавлять новые форматы, создавая дополнительные экземпляры.
/// </remarks>
/// <param name="LogRegex">Регулярное выражение, проверяющее соответствие строки формату.</param>
/// <param name="EntryFactory">Функция, преобразующая результат сопоставления в <see cref="LogEntry"/>.</param>
public sealed record class LogSubParser(Regex LogRegex, Func<Match, LogEntry> EntryFactory)
{
    /// <summary>
    /// Создаёт субпарсер для формата 1: 
    /// <c>10.03.2025 15:14:49.523 INFORMATION Сообщение</c>.
    /// </summary>
    /// <returns>Экземпляр <see cref="LogSubParser"/>, настроенный на формат 1.</returns>
    /// <remarks>
    /// Дата ожидается в формате <c>dd.MM.yyyy</c>, время с миллисекундами.
    /// Уровень логирования маппится через <see cref="LogMapper.MapLevel"/>.
    /// Поле <see cref="LogEntry.Method"/> остаётся <c>null</c>.
    /// </remarks>
    public static LogSubParser CreateStandardFormat1Parser()
    {
        static LogEntry StandardFormat1LogEntryFactory(Match match)
        {
            var dateStr = match.Groups["date"].Value;
            var timeStr = match.Groups["time"].Value;
            var levelStr = match.Groups["level"].Value;
            var message = match.Groups["message"].Value;

            var date = DateTime.ParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var level = LogMapper.MapLevel(levelStr);

            return new LogEntry(
                date,
                timeStr,
                level,
                null,
                message);
        }

        var regex = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+(?<level>\S+)\s+(?<message>.+)$");

        return new(regex, StandardFormat1LogEntryFactory);
    }

    /// <summary>
    /// Создаёт субпарсер для формата 2:
    /// <c>2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Сообщение</c>.
    /// </summary>
    /// <returns>Экземпляр <see cref="LogSubParser"/>, настроенный на формат 2.</returns>
    /// <remarks>
    /// Дата ожидается в формате <c>yyyy-MM-dd</c>, время с миллисекундами.
    /// Уровень логирования маппится через <see cref="LogMapper.MapLevel"/>.
    /// Извлекается поле <see cref="LogEntry.Method"/> (вызвавший метод).
    /// </remarks>
    public static LogSubParser CreateStandardFormat2Parser()
    {
        static LogEntry StandardFormat2LogEntryFactory(Match match)
        {
            var dateStr = match.Groups["date"].Value;
            var timeStr = match.Groups["time"].Value;
            var levelStr = match.Groups["level"].Value;
            var method = match.Groups["method"].Value;
            var message = match.Groups["message"].Value;

            var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var level = LogMapper.MapLevel(levelStr);

            return new LogEntry(
                date,
                timeStr,
                level,
                method,
                message);
        }

        var regex = new Regex(@"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s*\|\s*(?<level>\S+)\s*\|\s*\d+\s*\|\s*(?<method>\S+)\s*\|\s*(?<message>.+)$");

        return new(regex, StandardFormat2LogEntryFactory);
    }
}
