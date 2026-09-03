using LogStandardizer.Core.Services;

if (args.Length < 2)
{
    Console.WriteLine("Usage: LogStandardizer <inputFile> <outputFile>");
    return;
}

var inputPath = args[0];
var outputPath = args[1];
var problemPath = Path.Combine(Path.GetDirectoryName(outputPath) ?? ".", "problems.txt");

try
{
    using var goodWriter = new StreamWriter(outputPath);
    using var problemWriter = new StreamWriter(problemPath);

    var logSubParsers = new List<LogSubParser>() {
        LogSubParser.CreateStandardFormat1Parser(),
        LogSubParser.CreateStandardFormat2Parser()
    };
    var logParser = new LogParser(logSubParsers);
    var logProcessor = new LogProcessor(logParser, goodWriter, problemWriter);
    var lines = File.ReadLines(inputPath);

    logProcessor.ProcessLines(lines);

    Console.WriteLine("Processing completed.");
    Console.WriteLine($"Good records: {outputPath}");
    Console.WriteLine($"Problem records: {problemPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}