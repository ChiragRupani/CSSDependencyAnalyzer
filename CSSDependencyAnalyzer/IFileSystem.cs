namespace CSSDependencyAnalyzer;

/// <summary>
/// Abstraction over file system operations
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Recursively enumerates files starting at the given root using the specified search pattern.
    /// </summary>
    IEnumerable<string> GetFilesRecursive(string root, string searchPattern);

    /// <summary>
    /// Reads the content of a file
    /// </summary>
    string ReadAllText(string path);
}