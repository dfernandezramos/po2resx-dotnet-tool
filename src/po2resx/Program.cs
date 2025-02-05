namespace Po2Resx;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help" || args[0] == "/h" || args[0] == "/help"))
        {
            HelpWriter help = new();
            help.Write();
            return;
        }

        if (args.Length != 2)
        {
            Console.WriteLine("Usage: po2resx <input.po/pot> <output.resx>");
            return;
        }


        string inputFilePath = args[0];
        string outputFilePath = args[1];

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Source file {inputFilePath} does not exist.");
            return;
        }

        bool isTemplateFile = Path.GetExtension(inputFilePath) == ".pot";
        Dictionary<string, string> translations = PoParser.Parse(inputFilePath, isTemplateFile);
        Console.WriteLine($"{translations.Count} translations detected.");
        ResxGenerator.GenerateResxFile(translations, outputFilePath);
        Console.WriteLine($"Resource file {outputFilePath} generated.");
    }
}
