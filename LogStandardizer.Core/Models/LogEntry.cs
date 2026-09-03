using LogStandardizer.Core.Enumerations;

namespace LogStandardizer.Core.Models;

public sealed class LogEntry
{
    public required DateTime Date { get; set; }
    public required string Time { get; set; }
    public required LogLevel Level { get; set; }
    public required string? Method { get; set; }
    public required string Message { get; set; }

    public string ToStandardizedString()
    {
        var method = string.IsNullOrEmpty(Method) ? "DEFAULT" : Method;
        var levelStr = Level.ToString();

        return $"{Date:dd-MM-yyyy}\t{Time}\t{levelStr}\t{method}\t{Message}";
    }
}