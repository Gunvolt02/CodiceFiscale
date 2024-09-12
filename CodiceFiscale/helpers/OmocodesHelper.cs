using CodiceFiscaleLib.Config;

namespace CodiceFiscaleLib.Helpers;

public static class OmocodesHelper {

    // Method to generate the combinations for omocodes
    public static List<List<int>> GetOmocodiaSubIndexCombinations()
    {
        var combinations = new List<List<int>> { new List<int>() };
        for (int comboSize = 1; comboSize <= Constants._OMOCODIA_SUBS_INDEXES.Count; comboSize++)
        {
            foreach (var combo in CombinationsHelper.Combinations(Constants._OMOCODIA_SUBS_INDEXES, comboSize))
            {
                combinations.Add(combo.ToList());
            }
        }
        return combinations;
    }

    // Method to check if it's an omocode
    public static bool IsOmocode(string code)
    {
        var data = DecodingHelper.Decode(code);
        var codes = (List<string>)data["omocodes"];
        codes.RemoveAt(0);  // Remove the root code
        return codes.Contains(code);
    }
}