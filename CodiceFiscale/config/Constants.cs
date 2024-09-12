using System.Text.RegularExpressions;
using CodiceFiscaleLib.Helpers;

namespace CodiceFiscaleLib.Config;

// Constants for consonants, vowels, months, CIN, and omocodia mappings
public static class Constants {
        
    public static readonly List<char> _CONSONANTS = new List<char>("bcdfghjklmnpqrstvwxyz".ToCharArray());
    public static readonly List<char> _VOWELS = new List<char>("aeiou".ToCharArray());
    public static readonly List<char> _MONTHS = new List<char>("ABCDEHLMPRST".ToCharArray());

    public static readonly Dictionary<char, Tuple<int, int>> _CIN = new Dictionary<char, Tuple<int, int>>
    {
        {'0', Tuple.Create(0, 1)}, {'1', Tuple.Create(1, 0)}, {'2', Tuple.Create(2, 5)}, {'3', Tuple.Create(3, 7)},
        {'4', Tuple.Create(4, 9)}, {'5', Tuple.Create(5, 13)}, {'6', Tuple.Create(6, 15)}, {'7', Tuple.Create(7, 17)},
        {'8', Tuple.Create(8, 19)}, {'9', Tuple.Create(9, 21)}, {'A', Tuple.Create(0, 1)}, {'B', Tuple.Create(1, 0)},
        {'C', Tuple.Create(2, 5)}, {'D', Tuple.Create(3, 7)}, {'E', Tuple.Create(4, 9)}, {'F', Tuple.Create(5, 13)},
        {'G', Tuple.Create(6, 15)}, {'H', Tuple.Create(7, 17)}, {'I', Tuple.Create(8, 19)}, {'J', Tuple.Create(9, 21)},
        {'K', Tuple.Create(10, 2)}, {'L', Tuple.Create(11, 4)}, {'M', Tuple.Create(12, 18)}, {'N', Tuple.Create(13, 20)},
        {'O', Tuple.Create(14, 11)}, {'P', Tuple.Create(15, 3)}, {'Q', Tuple.Create(16, 6)}, {'R', Tuple.Create(17, 8)},
        {'S', Tuple.Create(18, 12)}, {'T', Tuple.Create(19, 14)}, {'U', Tuple.Create(20, 16)}, {'V', Tuple.Create(21, 10)},
        {'W', Tuple.Create(22, 22)}, {'X', Tuple.Create(23, 25)}, {'Y', Tuple.Create(24, 24)}, {'Z', Tuple.Create(25, 23)},
    };

    public static readonly List<char> _CIN_REMAINDERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

    public static readonly Dictionary<char, char> _OMOCODIA = new Dictionary<char, char>
    {
        {'0', 'L'}, {'1', 'M'}, {'2', 'N'}, {'3', 'P'}, {'4', 'Q'}, {'5', 'R'}, {'6', 'S'}, {'7', 'T'}, {'8', 'U'}, {'9', 'V'},
    };

    public static readonly string _OMOCODIA_DIGITS = new string(_OMOCODIA.Keys.ToArray());
    public static readonly string _OMOCODIA_LETTERS = new string(_OMOCODIA.Values.ToArray());

    public static readonly Dictionary<int, int> _OMOCODIA_ENCODE_TRANS = new Dictionary<int, int>
    {
    { 48, 76 }, // '0' -> 'L'
    { 49, 77 }, // '1' -> 'M'
    { 50, 78 }, // '2' -> 'N'
    { 51, 80 }, // '3' -> 'P'
    { 52, 81 }, // '4' -> 'Q'
    { 53, 82 }, // '5' -> 'R'
    { 54, 83 }, // '6' -> 'S'
    { 55, 84 }, // '7' -> 'T'
    { 56, 85 }, // '8' -> 'U'
    { 57, 86 }  // '9' -> 'V'
    };

    public static readonly Dictionary<int, int> _OMOCODIA_DECODE_TRANS = new Dictionary<int, int>
    {
    { 76, 48 }, // 'L' -> '0'
    { 77, 49 }, // 'M' -> '1'
    { 78, 50 }, // 'N' -> '2'
    { 80, 51 }, // 'P' -> '3'
    { 81, 52 }, // 'Q' -> '4'
    { 82, 53 }, // 'R' -> '5'
    { 83, 54 }, // 'S' -> '6'
    { 84, 55 }, // 'T' -> '7'
    { 85, 56 }, // 'U' -> '8'
    { 86, 57 }  // 'V' -> '9'
    };

    public static readonly List<int> _OMOCODIA_SUBS_INDEXES = new List<int> { 14, 13, 12, 10, 9, 7, 6 };

    public static readonly List<List<int>> _OMOCODIA_SUBS_INDEXES_COMBINATIONS = OmocodesHelper.GetOmocodiaSubIndexCombinations();

    // Regex for validating the codice fiscale
    public static readonly Regex CODICEFISCALE_RE = new Regex(
        @"^(?<lastname>[a-z]{3})(?<firstname>[a-z]{3})(?<birthdate>(?<birthdate_year>[a-z\d]{2})(?<birthdate_month>[abcdehlmprst]{1})(?<birthdate_day>[a-z\d]{2}))(?<birthplace>[a-z]{1}[a-z\d]{3})(?<cin>[a-z]{1})$",
        RegexOptions.IgnoreCase
    );
}