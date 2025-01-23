namespace Po2Resx;

/// <summary>
/// This class contains the implementation of a PO file parser.
/// </summary>
public static class PoParser
{
    /// <summary>
    /// This method parses the content of a PO file given its file path and returns a dictionary with
    /// the translations inside of it.
    /// </summary>
    /// <param name="filePath">The path of the PO file.</param>
    /// <returns>A dictionary with the strings and Ids extracted from the PO file.</returns>
    public static Dictionary<string, string> Parse(string filePath)
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
                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        value = key;
                    }

                    translations[key] = value;
                }

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
        if (!string.IsNullOrEmpty(key) && value != null)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = key;
            }

            translations[key] = value;
        }

        return translations;
    }

}