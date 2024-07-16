# Po2Resx-dotnet-tool

`po2resx` is a .NET global tool that converts PO (Portable Object) files to RESX (Resource) files. This tool is useful for .NET developers who need to manage localization and internationalization in their projects.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Features

- Parses PO files and extracts translations.
- Generates RESX files from the extracted translations.
- Easy to install and use as a .NET global tool.

## Installation

### Local installation

To install `po2resx` as a global tool, first clone the repository and navigate to the project directory. Then, run the following commands:

```sh
dotnet pack --configuration Release
dotnet tool install --global --add-source ./bin/Release net8.0/PoToResxTool
```

### dotnet tool installation

To install `po2resx` as a global tool, run the following command:

```sh
dotnet tool install po2resx-dotnet-tool -g
```

## Usage

```sh
po2resx <input.po> <output.resx>
```
