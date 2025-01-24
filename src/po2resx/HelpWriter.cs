using System.Reflection;

namespace Po2Resx;

internal class HelpWriter
{
    public void Write()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string? version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        string help = ArgumentList();

        Console.WriteLine($"Po2Resx {version}");
        Console.WriteLine();
        Console.WriteLine(help);
    }

    private string ArgumentList()
    {
        using var argumentsMarkdownStream = GetType().Assembly.GetManifestResourceStream("Po2Resx.help.md");

        if (argumentsMarkdownStream == null)
        {
            return "Error retrieving help arguments.";
        }

        using var sr = new StreamReader(argumentsMarkdownStream);
        var argsMarkdown = sr.ReadToEnd();
        var codeBlockStart = argsMarkdown.IndexOf("```", StringComparison.Ordinal) + 3;
        var codeBlockEnd = argsMarkdown.LastIndexOf("```", StringComparison.Ordinal) - codeBlockStart;
        return argsMarkdown.Substring(codeBlockStart, codeBlockEnd).Trim();
    }
}