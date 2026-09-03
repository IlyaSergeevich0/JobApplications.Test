using LogStandardizer.Core.Models;
using LogStandardizer.Core.Utility;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogStandardizer.Core.Services;

public sealed record class LogSubParser(Regex LogRegex, Func<Match, LogEntry> EntryFactory)
{
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

            return new LogEntry {
                Date = date,
                Time = timeStr,
                Level = level,
                Method = null,
                Message = message
            };
        }

        var regex = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+(?<level>\S+)\s+(?<message>.+)$");

        return new(regex, StandardFormat1LogEntryFactory);
    }

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

            return new LogEntry {
                Date = date,
                Time = timeStr,
                Level = level,
                Method = method,
                Message = message
            };
        }

        var regex = new Regex(@"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s*\|\s*(?<level>\S+)\s*\|\s*\d+\s*\|\s*(?<method>\S+)\s*\|\s*(?<message>.+)$");

        return new(regex, StandardFormat2LogEntryFactory);
    }
}
