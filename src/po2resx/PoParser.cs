namespace Po2Resx;

/// <summary>
/// This class contains the implementation of a PO file parser.
/// </summary>
public static class PoParser
{
    /// <summary>
    /// This method parses the content of a PO/POT file given its file path and returns a dictionary with
    /// the translations inside of it.
    /// </summary>
    /// <param name="filePath">The path of the PO/POT file.</param>
    /// <param name="isTemplateFile">A value indicating whether the file is POT or not.</param>
    /// <returns>A dictionary with the strings and Ids extracted from the PO/POT file.</returns>
    public static Dictionary<string, string> Parse(string filePath, bool isTemplateFile)
    {
        var translations = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(filePath);
        string? key = null;
        string? value = null;
        bool isMsgId = false;
        bool isMsgStr = false;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith("msgid"))
            {
                RegisterTranslation(translations, key, value, isTemplateFile);
                key = line[7..^1]; // Remove 'msgid "' and trailing quotes
                value = null;
                isMsgId = true;
                isMsgStr = false;
            }
            else if (line.StartsWith("msgstr"))
            {
                value = line[8..^1]; // Remove 'msgstr "' and trailing quotes
                isMsgId = false;
                isMsgStr = true;
            }
            else if (line.StartsWith('\"'))
            {
                // Remove opening and trailing quotes
                if (isMsgId && key != null)
                {
                    key += line[1..^1];
                }
                else if (isMsgStr && value != null)
                {
                    value += line[1..^1];
                }
            }
        }

        // Save last key-value pair if any
        RegisterTranslation(translations, key, value, isTemplateFile);

        return translations;
    }

    static void RegisterTranslation(Dictionary<string, string> translations, string? key, string? value, bool isTemplateFile)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        if (isTemplateFile)
        {
            translations[key] = key;
        }
        else if (!string.IsNullOrEmpty(value))
        {
            translations[key] = value;
        }
    }
}