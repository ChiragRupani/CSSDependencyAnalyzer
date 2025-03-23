namespace CSSDependencyAnalyzer;

/// <summary>
/// Default implementation of IFileSystem using System.IO
/// </summary>
public class DefaultFileSystem : IFileSystem
{
    public IEnumerable<string> GetFilesRecursive(string root, string searchPattern)
    {
        return DirectoryHelper.GetFilesRecursive(root, searchPattern);
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }
}