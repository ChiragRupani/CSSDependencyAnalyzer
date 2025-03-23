# CSSDependencyAnalyzer

[![NuGet](https://img.shields.io/nuget/v/CSSDependencyAnalyzer.svg)](https://www.nuget.org/packages/CSSDependencyAnalyzer/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

`CSSDependencyAnalyzer` is a high-performance .NET global tool that scans React (`.tsx/.jsx`) components and determines which CSS/SCSS files they rely on. This helps in optimizing CSS usage and enabling lazy-loaded React components.

---

### ✨ Features
- 🔍 **Detects Required CSS Files**: Finds the exact CSS/SCSS files needed by each React component which are not already imported. By identifying such files, we can move such common classes in already imported css files and lazy load the current React Component
- 🚀 **Performance Optimized**: Uses parallel processing and a file system abstraction for fast execution.
- 🔄 **Supports `.tsx` and `.jsx`**: Parses both TypeScript and JavaScript React files and CSS / SCSS files. Currently, it is tested on Repos created from Vite / CRA.

#### Why Use CSSDependencyAnalyzer?
- Improves lazy loading performance by determining only necessary CSS files.
- Identify common classes used in multiple react components.
- Saves development time by automating dependency analysis

---

### 📦 Installation

You can install the package globally using the .NET CLI:

```sh
dotnet tool install -g CSSDependencyAnalyzer
```

Update:
```sh
dotnet tool update -g CSSDependencyAnalyzer
```

Uninstall:
```sh
dotnet tool uninstall -g CSSDependencyAnalyzer
```

---

### 🚀 Usage

Basic Usage

To analyze a React project, navigate to the root folder and run:

```sh
CSSDependencyAnalyzer <project-root>
```

Example:
```
CSSDependencyAnalyzer C:\Projects\MyReactApp
```

This outputs a JSON report detailing:
Each component file (.tsx or .jsx) and the CSS classes it uses.
The corresponding required CSS/SCSS files.

Example Output
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

---

### How It Works
- Parses React component files (.tsx, .jsx) and extracts className="..." values. Identifies all unique CSS classes used.
- Scans CSS/SCSS files (.css, .scss). Detects class definitions (.classname { ... }). 
- Maps each class to its corresponding CSS file.
- Matches used classes to CSS Files: Determines which stylesheets are required for each component. 
- Generates a structured JSON report.

---
### To test the package locally:
Clone the repository:
```sh
git clone https://github.com/ChiragRupani/CSSDependencyAnalyzer
cd CSSDependencyAnalyzer
```

Install local code
```sh
dotnet tool install -g --add-source ./nupkg  CSSDependencyAnalyzer --prerelease
```

Run tests:
```sh
dotnet test
```
---