using System.Xml.Linq;

namespace Po2Resx;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: potoresx <input.po> <output.resx>");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = args[1];

        var parser = new PoParser();
        var translations = parser.Parse(inputFilePath);
        GenerateResxFile(translations, outputFilePath);
    }

    static void GenerateResxFile(Dictionary<string, string> translations, string filePath)
    {
        var resx = new XElement("root",
            new XElement("resheader", new XAttribute("name", "resmimetype"),
                new XElement("value", "text/microsoft-resx")),
            new XElement("resheader", new XAttribute("name", "version"),
                new XElement("value", "2.0")),
            new XElement("resheader", new XAttribute("name", "reader"),
                new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")),
            new XElement("resheader", new XAttribute("name", "writer"),
                new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"))
        );
        
        foreach (var entry in translations)
        {
            resx.Add(new XElement("data", new XAttribute("name", entry.Key), new XAttribute("xml:space", "preserve"),
                new XElement("value", entry.Value)));
        }

        var resxDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), resx);
        resxDoc.Save(filePath);
    }
}
