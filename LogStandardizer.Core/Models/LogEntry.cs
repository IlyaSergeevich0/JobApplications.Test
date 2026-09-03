using LogStandardizer.Core.Enumerations;

namespace LogStandardizer.Core.Models;

public sealed record class LogEntry(
    DateTime Date,
    string Time,
    LogLevel Level,
    string? Method,
    string Message)
{
    public string ToStandardizedString()
    {
        var method = string.IsNullOrWhiteSpace(Method) ? "DEFAULT" : Method;
        var levelStr = Level.ToString();

        return $"{Date:dd-MM-yyyy}\t{Time}\t{levelStr}\t{method}\t{Message}";
    }
}