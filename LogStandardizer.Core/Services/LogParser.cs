using LogStandardizer.Core.Models;

namespace LogStandardizer.Core.Services;

public sealed class LogParser(List<LogSubParser> subParsers)
{
    private readonly List<LogSubParser> _subParsers = subParsers;

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
