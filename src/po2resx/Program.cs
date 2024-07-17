namespace Po2Resx;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: po2resx <input.po> <output.resx>");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = args[1];

        Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
        ResxGenerator.GenerateResxFile(translations, outputFilePath);
    }
}
