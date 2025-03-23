# CssDependencyAnalyzer

[![NuGet](https://img.shields.io/nuget/v/CssDependencyAnalyzer.svg)](https://www.nuget.org/packages/CssDependencyAnalyzer/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

`CssDependencyAnalyzer` is a high-performance .NET global tool that scans React (`.tsx/.jsx`) components and determines which CSS/SCSS files they rely on. This helps optimize CSS usage, reduce unnecessary stylesheets, and improve the efficiency of lazy-loaded React components.

## 📌 Features
- 🔍 **Detects Required CSS Files**: Finds the exact CSS/SCSS files needed by each React component.
- 🚀 **Performance Optimized**: Uses parallel processing and a file system abstraction for fast execution.
- 🛑 **Ignores `node_modules`**: Prevents unnecessary traversal of dependency folders.
- 🔄 **Supports `.tsx` and `.jsx`**: Parses both TypeScript and JavaScript React files.
- ✅ **Unit-Tested with Fake File System**: Can be tested without real file creation.

---

## 📦 Installation

You can install the package globally using the .NET CLI:

```sh
dotnet tool install -g CssDependencyAnalyzer
```

Update:
```sh
dotnet tool update -g CssDependencyAnalyzer
```

Uninstall:
```sh
dotnet tool uninstall -g CssDependencyAnalyzer
```


🚀 Usage
Basic Usage
To analyze a React project, navigate to the root folder and run:

```sh
CssDependencyAnalyzer <project-root>
```

Example:
```
CssDependencyAnalyzer C:\Projects\MyReactApp
```

This outputs a JSON report detailing:
Each component file (.tsx or .jsx) and the CSS classes it uses.
The corresponding required CSS/SCSS files.
📊 Example Output
```json
[
  {
    "File": "C:\\Projects\\MyReactApp\\components\\Button.tsx",
    "UsedClasses": ["btn-primary", "btn-lg"],
    "RequiredCssFiles": ["C:\\Projects\\MyReactApp\\styles\\buttons.css"]
  },
  {
    "File": "C:\\Projects\\MyReactApp\\components\\Header.tsx",
    "UsedClasses": ["header", "nav"],
    "RequiredCssFiles": ["C:\\Projects\\MyReactApp\\styles\\header.scss"]
  }
]
```

🏗 How It Works
Parses React component files (.tsx, .jsx):

Extracts className="..." values.
Identifies all unique CSS classes used.
Scans CSS/SCSS files (.css, .scss):

Detects class definitions (.classname { ... }).
Maps each class to its corresponding CSS file.
Matches Used Classes to CSS Files:

Determines which stylesheets are required for each component.
Generates a structured JSON report.

🧪 Running Unit Tests
To test the package locally:

Clone the repository:
```sh
git clone https://github.com/your-repo/CssDependencyAnalyzer.git
cd CssDependencyAnalyzer
```

Install local code
```sh
dotnet tool install -g --add-source ./nupkg  CSSDependencyAnalyzer --prerelease
```

Run tests:
```sh
dotnet test
```

📜 License
This project is licensed under the MIT License.


🔗 Links

📦 NuGet Package

📝 GitHub Repository

📚 .NET Tool Documentation

🤝 Contributing
Pull requests and feature suggestions are welcome! Open an issue if you find any bugs or improvements.


💡 Why Use CssDependencyAnalyzer?
✅ Helps reduce CSS file bloat in large React projects.
✅ Improves lazy loading performance by determining only necessary CSS files.
✅ Saves development time by automating dependency analysis.

