namespace CSSDependencyAnalyzer.Tests;

[TestClass()]
public class DependenencyAnalyzerTests
{
    [TestMethod]
    public void AnalyzeDirectory_ShouldReturnExpectedResults_WithFakeFileSystem()
    {
        // Arrange
        var files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // CSS file with a class definition.
            { @"C:\project\styles.css", ".testClass { color: red; }" },

            // TSX file using the defined class.
            { @"C:\project\Component.tsx", "<div className=\"testClass testclass1\">Hello World</div>" },

            // File in node_modules that should be ignored.
            { @"C:\project\node_modules\ignore.css", ".missingClass { display: none; }" },

            // Another TSX file that uses a non-existent class.
            { @"C:\project\OtherComponent.tsx", "<span className='missingClass'>Test</span>" }
        };

        IFileSystem fakeFileSystem = new FakeFileSystem(files);
        string targetDirectory = @"C:\Project";

        // Act
        var cssMapping = DependenencyAnalyzer.BuildCSSMapping(targetDirectory, fakeFileSystem);
        var results = DependenencyAnalyzer.AnalyzeComponentFiles(targetDirectory, cssMapping, fakeFileSystem);

        // Assert
        Assert.AreEqual(actual: results.Count, expected: 1, message: "Expected only one file as result");

        // Find the analysis result for Component.tsx.
        var compResult = results.FirstOrDefault(r => r.File.Equals(@"C:\project\Component.tsx", StringComparison.OrdinalIgnoreCase));
        Assert.IsNotNull(compResult, "Component.tsx result should not be null.");
        CollectionAssert.Contains(compResult.UsedClasses, "testClass", "Component.tsx should detect 'testClass'.");

        // The required CSS file should be styles.css.
        bool foundCss = Path.GetFileName(compResult.RequiredCSSFiles[0].File).Equals("styles.css", StringComparison.OrdinalIgnoreCase);
        Assert.IsTrue(foundCss, "Component.tsx should have 'styles.css' as a required CSS file.");
        Assert.AreEqual(actual: compResult.RequiredCSSFiles[0].ClassesUsed.Single(), expected: "testClass");
    }
}
