using System.Text.RegularExpressions;

namespace CSSDependencyAnalyzer;

public sealed partial class ImportProcessor
{
    [GeneratedRegex(@"import\s+['""](.*?\.s?css)['""];", RegexOptions.Compiled)]
    private static partial Regex importRegex();

    private static readonly Regex ImportRegex = importRegex();

    /// <summary>
    /// Gets Absolute path of import files specified in css / scss file content relative to baseDirectory
    /// </summary>
    /// <param name="fileContent"></param>
    /// <param name="baseDirectory"></param>
    /// <returns></returns>
    public static HashSet<string> ExtractImportedCSSFiles(string fileContent, string baseDirectory)
    {
        var imports = new HashSet<string>();

        foreach (Match match in ImportRegex.Matches(fileContent))
        {
            string relativeFilePath = match.Groups[1].Value;
            string absolutePath = Path.GetFullPath(Path.Combine(baseDirectory,  relativeFilePath));
            imports.Add(absolutePath);
        }

        return imports;
    }
}