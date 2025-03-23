namespace CSSDependencyAnalyzer.Tests;

public class FakeFileSystem : IFileSystem
{
    private readonly Dictionary<string, string> _files;

    /// <summary>
    /// Initialize the fake file system with a dictionary of file paths and contents.
    /// </summary>
    public FakeFileSystem(Dictionary<string, string> files)
    {
        _files = files;
    }

    public IEnumerable<string> GetFilesRecursive(string root, string searchPattern)
    {
        // For the fake implementation, ignore root and searchPattern,
        // and return all file paths that do not contain "node_modules".
        return _files.Keys.Where(path =>
            !path.Contains("node_modules", StringComparison.OrdinalIgnoreCase));
    }

    public string ReadAllText(string path)
    {
        if (_files.TryGetValue(path, out string? content))
        {
            return content;
        }

        throw new Exception($"File not found: {path}");
    }
}