using System.Xml.Linq;

namespace Po2Resx;

/// <summary>
/// This class contains the implementation of a RESX file generator.
/// </summary>
public static class ResxGenerator
{
	/// <summary>
	/// This method generates a RESX file with the provided translation messages.
	/// </summary>
	/// <param name="translations">A dictionary with all the translations to be added to the RESX file.</param>
	/// <param name="outputPath">The output path where the RESX file is going to be generated.</param>
	public static void GenerateResxFile(Dictionary<string, string> translations, string outputPath)
	{
		XElement resx = GetHeaders ();

		foreach (KeyValuePair<string, string> translation in translations) {
			if (string.IsNullOrWhiteSpace (translation.Key)) {
				continue;
			}
			
			XElement dataElement = GetTranslationElement (translation);
			resx.Add(dataElement);
		}

		XDocument resxDoc = new(new XDeclaration("1.0", "utf-8", "yes"), resx);
		resxDoc.Save(outputPath);
	}

	static XElement GetHeaders ()
	{
		return new XElement ("root",
			new XElement("resheader", new XAttribute("name", "resmimetype"),
				new XElement("value", "text/microsoft-resx")),
			new XElement("resheader", new XAttribute("name", "version"),
				new XElement("value", "2.0")),
			new XElement("resheader", new XAttribute("name", "reader"),
				new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")),
			new XElement("resheader", new XAttribute("name", "writer"),
				new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"))
		);
	}

	static XElement GetTranslationElement (KeyValuePair<string, string> translation)
	{
		XAttribute nameAttribute = new("name", translation.Key);
		XAttribute spaceAttribute = new(XNamespace.Xml + "space", "preserve");
		string value = string.IsNullOrWhiteSpace (translation.Value) ? translation.Key : translation.Value;
		XElement valueElement = new("value", value);
		
		return new XElement ("data", nameAttribute, spaceAttribute, valueElement);
	}
}