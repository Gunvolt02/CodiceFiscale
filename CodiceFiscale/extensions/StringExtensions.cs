using System.Text;

namespace CodiceFiscaleLib.Extensions;

public static class StringExtensions
{
	// Method to translate strings using the translation table
	public static string Translate(this string input, Dictionary<char, char> translationTable)
	{
    	return new string(input.Select(c => translationTable.ContainsKey(c) ? translationTable[c] : c).ToArray());
	}

	// Method to translate strings using the translation maps
	public static string Translate(this string input, Dictionary<int, int> translationMap)
	{
    	if (input == null)
        	throw new ArgumentNullException(nameof(input));

    	var result = new StringBuilder();
    	foreach (var c in input)
    	{
        	int charCode = (int)c;
        	if (translationMap.TryGetValue(charCode, out var translatedCode))
        	{
            	result.Append((char)translatedCode);
        	}
        	else
        	{
            	result.Append(c); // If the char is not in the map,leave it as is
        	}
    	}
    	return result.ToString();
	}
}
