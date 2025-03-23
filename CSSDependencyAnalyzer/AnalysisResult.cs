using System.Text.Json.Serialization;

namespace CSSDependencyAnalyzer;

/// <summary>
/// Represents the analysis result for a single React component file
/// </summary>
/// <param name="File"></param>
public record AnalysisResult(string File)
{
    [JsonIgnore]
    public List<string> UsedClasses { get; set; } = [];

    public List<RequiredFile> RequiredCSSFiles { get; set; } = [];
}

public record RequiredFile(string File, IEnumerable<string> ClassesUsed) { }


/// <summary>
/// JSON Serializer source generated code for List`AnalysisResult`
/// </summary>
[JsonSerializable(typeof(List<AnalysisResult>))]
internal partial class AnalysisResultSourceGenerationContext : JsonSerializerContext
{
}