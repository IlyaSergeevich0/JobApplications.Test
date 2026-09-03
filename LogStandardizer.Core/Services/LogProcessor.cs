namespace LogStandardizer.Core.Services;

/// <summary>
/// Обрабатывает поток строк, разделяя валидные и невалидные записи в разные выходные потоки.
/// </summary>
/// <remarks>
/// Валидные строки преобразуются в стандартизированный формат и записываются в <paramref name="goodWriter"/>.
/// Невалидные строки записываются в <paramref name="problemWriter"/> в исходном виде.
/// </remarks>
public sealed class LogProcessor(
    LogParser parser,
    TextWriter goodWriter,
    TextWriter problemWriter)
{
    private readonly LogParser _parser = parser;
    private readonly TextWriter _goodWriter = goodWriter;
    private readonly TextWriter _problemWriter = problemWriter;

    /// <summary>
    /// Обрабатывает перечисление строк, разделяя их на валидные и невалидные.
    /// </summary>
    /// <param name="lines">Перечисление строк для обработки.</param>
    /// <exception cref="ArgumentNullException">Если <paramref name="lines"/> равен <c>null</c>.</exception>
    /// <remarks>
    /// Пустые или состоящие только из пробелов строки игнорируются (не попадают ни в один выходной поток).
    /// </remarks>
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