using System.Collections;
using System.Resources.NetStandard;
namespace Po2Resx.Tests;

public class Tests
{
	string _outputFilePath = string.Empty;
	
	[OneTimeSetUp]
	public void OneTimeSetup ()
	{
		string outputPath = Path.GetTempPath ();
		_outputFilePath = Path.Join (outputPath, "translations.resx");
	}
	
	[TearDown]
	public void TearDown ()
	{
		if (!File.Exists (_outputFilePath)) {
			return;
		}

		File.Delete (_outputFilePath);
	}
	
	[Test]
	public void ParsePOFile_NotExistingFile_ExceptionRaised ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "unexisting.po");
		
		// When & Then
		Assert.Throws<FileNotFoundException>(() => PoParser.Parse (inputFilePath));
	}
	
	[Test]
	public void ParsePOFile_ExistingPOFile_TranslationsParsed ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "translations.po");
		
		// When
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// Then
		Assert.That (translations, Has.Count.EqualTo (2));
	}
	
	[Test]
	public void ParsePOFile_EmptyPOFile_EmptyResultReturned ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "empty.po");
		
		// When
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// Then
		Assert.That (translations, Has.Count.EqualTo (0));
	}
	
	[Test]
	public void ParsePOFile_ExistingPOTemplateFile_TranslationsParsed ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "translations_template.pot");
		
		// When
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// Then
		Assert.That (translations, Has.Count.EqualTo (2));
	}

	[Test]
	public void GenerateRESXFile_ParseExistingPOFile_FileGeneratedWithTranslations ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "translations.po");
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// When
		ResxGenerator.GenerateResxFile(translations, _outputFilePath);
		
		// Then
		Assert.That (File.Exists (_outputFilePath), Is.True);
		AssertResxFileContainsTranslations(_outputFilePath, translations);
	}
	
	[Test]
	public void GenerateRESXFile_ParseEmptyPOFile_FileGeneratedWithoutTranslations ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "empty.po");
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// When
		ResxGenerator.GenerateResxFile(translations, _outputFilePath);
		
		// Then
		Assert.That (File.Exists (_outputFilePath), Is.True);
		Assert.That (translations, Has.Count.EqualTo (0));
	}
	
	[Test]
	public void GenerateRESXFile_ParseExistingPOTemplateFile_FileGeneratedWithoutTranslations ()
	{
		// Given
		string inputFilePath = Path.Join ("data", "translations_template.pot");
		Dictionary<string, string> translations = PoParser.Parse(inputFilePath);
		
		// When
		ResxGenerator.GenerateResxFile(translations, _outputFilePath);
		
		// Then
		Assert.That (File.Exists (_outputFilePath), Is.True);
		AssertResxFileContainsTranslations(_outputFilePath, translations);
	}
	
	void AssertResxFileContainsTranslations(string resxFilePath, Dictionary<string, string> expectedTranslations)
	{
		using ResXResourceReader resxReader = new (resxFilePath);
		
		foreach (DictionaryEntry entry in resxReader)
		{
			string key = (string)entry.Key;
			string value = (string)entry.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(expectedTranslations.ContainsKey(key), Is.True);
                Assert.That(expectedTranslations[key], Is.EqualTo(value));
            });
        }
	}
}