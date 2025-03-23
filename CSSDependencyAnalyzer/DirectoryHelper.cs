namespace CSSDependencyAnalyzer;

public static class DirectoryHelper
{
    private static readonly string[] ignoredFolders = ["node_modules", "coverage", "dist", ".git"];

    /// <summary>
    /// Recursively enumerates files in a directory, skipping any directory named "node_modules"
    /// </summary>
    /// <param name="root">The root directory to start the search</param>
    /// <param name="searchPattern">The search pattern for files (e.g. "*.*")</param>
    /// <returns>An enumerable of file paths.</returns>
    public static IEnumerable<string> GetFilesRecursive(string root, string searchPattern)
    {
        // Get files in the current directory.
        IEnumerable<string> files;
        try
        {
            files = Directory.EnumerateFiles(root, searchPattern);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating files in {root}: {ex.Message}");
            yield break;
        }

        foreach (var file in files)
        {
            if (file.EndsWith("test.tsx") || file.EndsWith("test.jsx"))
            {
                continue;
            }

            yield return file;
        }

        // Recurse into subdirectories
        IEnumerable<string> directories;
        try
        {
            directories = Directory.EnumerateDirectories(root);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating directories in {root}: {ex.Message}");
            yield break;
        }

        foreach (var dir in directories)
        {
            if (ignoredFolders.Contains(Path.GetFileName(dir)))
            {
                continue;
            }

            foreach (var file in GetFilesRecursive(dir, searchPattern))
            {
                yield return file;
            }
        }
    }
}
