using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CSSDependencyAnalyzer;

public partial class DependenencyAnalyzer
{
    [GeneratedRegex(@"className\s*=\s*(?:\{?\s*)?[""']([^""'}]+)[""']", RegexOptions.Compiled)]
    private static partial Regex ClassNamesRegex();

    [GeneratedRegex(@"\.([a-zA-Z0-9_-]+)\s*(?=[,{])", RegexOptions.Compiled)]
    private static partial Regex CssClassesRegex();

    // Pre-compiled regex to extract CSS class definitions from CSS/SCSS files
    // It matches patterns like ".classname {" or ".classname,"
    private static readonly Regex CssClassRegex = CssClassesRegex();

    // Pre-compiled regex to extract className values in TSX/JSX files
    // It matches attributes like: className="foo bar" or className={'foo bar'}
    private static readonly Regex ClassNameUsageRegex = ClassNamesRegex();


    /// <summary>
    /// Builds a mapping of CSS class names to the CSS/SCSS files that define them
    /// </summary>
    /// <param name="directory">The root directory to search</param>
    /// <param name="fileSystem">File System</param>
    /// <returns>
    /// A concurrent dictionary mapping CSS class names (case‑insensitive) to a set of file paths
    /// </returns>
    public static ConcurrentDictionary<string, HashSet<string>> BuildCSSMapping(string directory, IFileSystem fileSystem)
    {
        var mapping = new ConcurrentDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        var cssFiles = fileSystem.GetFilesRecursive(directory, "*.*")
            .Where(file =>
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                return ext == ".css" || ext == ".scss";
            }).ToList();

        Parallel.ForEach(cssFiles, file =>
        {
            try
            {
                string content = fileSystem.ReadAllText(file);
                foreach (Match match in CssClassRegex.Matches(content))
                {
                    string className = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(className))
                    {
                        mapping.AddOrUpdate(
                            className,
                            // If key doesn't exist, create a new HashSet containing this file.
                            [file],
                            // If key exists, add this file to the existing set.
                            (key, existingSet) =>
                            {
                                lock (existingSet)
                                {
                                    existingSet.Add(file);
                                }
                                return existingSet;
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CSS file {file}: {ex.Message}");
            }
        });

        return mapping;
    }


    public static ConcurrentBag<AnalysisResult> AnalyzeComponentFiles(string directory, ConcurrentDictionary<string, HashSet<string>> cssMapping, IFileSystem fileSystem)
    {
        var componentFiles = fileSystem.GetFilesRecursive(directory, "*.*")
               .Where(file =>
               {
                   string ext = Path.GetExtension(file).ToLowerInvariant();
                   return ext == ".tsx" || ext == ".jsx";
               }).ToList();

        var results = new ConcurrentBag<AnalysisResult>();

        Parallel.ForEach(componentFiles, file =>
        {
            var analysis = AnalyzeComponentFile(file, cssMapping, fileSystem);
            if (analysis.RequiredCSSFiles.Count > 0)
            {
                results.Add(analysis);
            }
        });

        return results;
    }


    /// <summary>
    /// Analyzes a React component file (TSX/JSX) to determine which CSS classes are used and, via the mapping, which CSS files are required
    /// </summary>
    /// <param name="filePath">The path of the React component file</param>
    /// <param name="cssMapping">The pre-built CSS class mapping</param>
    /// <returns>An AnalysisResult containing the file name, used classes, and required CSS files</returns>
    private static AnalysisResult AnalyzeComponentFile(string filePath, ConcurrentDictionary<string, HashSet<string>> cssMapping, IFileSystem fileSystem)
    {
        var result = new AnalysisResult(filePath);

        try
        {
            string content = fileSystem.ReadAllText(filePath);
            var usedClasses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match match in ClassNameUsageRegex.Matches(content))
            {
                string classesString = match.Groups[1].Value;
                // Split by whitespace to extract individual classes
                var tokens = classesString.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
                foreach (var token in tokens)
                {
                    usedClasses.Add(token.Trim());
                }
            }
            result.UsedClasses.AddRange(usedClasses);

            string? fileDirectory = new DirectoryInfo(filePath).Parent?.FullName;
            if (fileDirectory is null)
            {
                return result;
            }

            var explicitlyImportedCss = ImportProcessor.ExtractImportedCSSFiles(content, fileDirectory);

            // Determine required CSS files from the CSS mapping            
            Dictionary<string, HashSet<string>> requiredFilesMap = new(StringComparer.OrdinalIgnoreCase);

            foreach (var usedClass in usedClasses)
            {
                if (cssMapping.TryGetValue(usedClass, out var files))
                {
                    foreach (var cssFile in files)
                    {
                        if (!explicitlyImportedCss.Contains(cssFile))
                        {
                            if (requiredFilesMap.TryGetValue(cssFile, out var classList))
                            {
                                classList.Add(usedClass);
                            }

                            else
                            {
                                requiredFilesMap.Add(cssFile, [usedClass]);
                            }
                        }
                    }
                }
            }

            var requiredCssFiles = requiredFilesMap.Select(x => new RequiredFile(x.Key, x.Value));
            result.RequiredCSSFiles.AddRange(requiredCssFiles);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing component file {filePath}: {ex.Message}");
        }

        return result;
    }
}
