namespace LogStandardizer.Core.Services;

public sealed class LogProcessor(
    LogParser parser,
    TextWriter goodWriter,
    TextWriter problemWriter)
{
    private readonly LogParser _parser = parser;
    private readonly TextWriter _goodWriter = goodWriter;
    private readonly TextWriter _problemWriter = problemWriter;

    public void ProcessLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var entry = _parser.ParseLine(line);

            if (entry != null)
                _goodWriter.WriteLine(entry.ToStandardizedString());
            else
                _problemWriter.WriteLine(line);
        }
    }
}