using System.Diagnostics;
using System.Text.Json;
using static CSSDependencyAnalyzer.DependenencyAnalyzer;

namespace CSSDependencyAnalyzer;

internal class Program
{
    private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true, TypeInfoResolver = AnalysisResultSourceGenerationContext.Default };

    private static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: CSSDependencyAnalyzer <directory>");
            Environment.Exit(1);
        }

        string targetDir = args[0];
        Stopwatch sw = Stopwatch.StartNew();
        if (!Directory.Exists(targetDir))
        {
            Console.Error.WriteLine($"Directory {targetDir} does not exist.");
            Environment.Exit(1);
        }

        DefaultFileSystem fileSystem = new();

        // Step 1: Build the CSS mapping concurrently.
        var cssMapping = BuildCSSMapping(targetDir, fileSystem);

        // Step 2: Gather all React component files (.tsx and .jsx).
        var results = AnalyzeComponentFiles(targetDir, cssMapping, fileSystem);

        sw.Stop();

        // Output the aggregated results as formatted JSON.
        string jsonOutput = JsonSerializer.Serialize(results.Where(x => x.UsedClasses.Count > 0)
            .ToList(), jsonOptions);
        Console.WriteLine(jsonOutput);

        Console.WriteLine($"Time taken: {Math.Round(sw.ElapsedMilliseconds / 1d)} milliseconds");
    }
}
