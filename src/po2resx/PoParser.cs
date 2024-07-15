namespace Po2Resx;

public class PoParser
{
    /// <summary>
    /// This method parses the content of a PO file given its file path and returns a dictionary with
    /// the translations inside of it.
    /// </summary>
    /// <param name="filePath">The path of the PO file.</param>
    /// <returns>A dictionary with the strings and Ids extracted from the PO file.</returns>
    public Dictionary<string, string> Parse(string filePath)
    {
        var translations = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(filePath);
        string? key = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("msgid "))
            {
                key = line.Substring(7, line.Length - 8); // Remove 'msgid "' and the trailing '"'
            }
            else if (line.StartsWith("msgstr "))
            {
                string? value = line.Substring(8, line.Length - 9);

                if (key != null && value != null)
                {
                    translations[key] = value;
                    key = null;
                    value = null;
                }
            }
        }

        return translations;
    }
}