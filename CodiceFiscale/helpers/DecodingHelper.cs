using CodiceFiscaleLib.Config;
using CodiceFiscaleLib.Extensions;

namespace CodiceFiscaleLib.Helpers;

public static class DecodingHelper
{
    // Method to decode the raw Italian Tax Code
    public static Dictionary<string, string> DecodeRaw(string code)
    {
        code = StringsHelper.Slugify(code).Replace("-", "").ToUpper();

        var match = Constants.CODICEFISCALE_RE.Match(code);
        if (!match.Success)
        {
            throw new ArgumentException($"[codicefiscale] invalid syntax: {code}");
        }

        return new Dictionary<string, string>
       {
           { "code", code },
           { "lastname", match.Groups["lastname"].Value },
           { "firstname", match.Groups["firstname"].Value },
           { "birthdate", match.Groups["birthdate"].Value },
           { "birthdate_year", match.Groups["birthdate_year"].Value },
           { "birthdate_month", match.Groups["birthdate_month"].Value },
           { "birthdate_day", match.Groups["birthdate_day"].Value },
           { "birthplace", match.Groups["birthplace"].Value },
           { "cin", match.Groups["cin"].Value }
       };
    }

    // Method to decode the full Italian Tax Code
    public static Dictionary<string, object> Decode(string code)
    {
        var raw = DecodeRaw(code);
        code = raw["code"];

        int birthdateYear = int.Parse(raw["birthdate_year"].Translate(Constants._OMOCODIA_DECODE_TRANS));
        int birthdateMonth = Constants._MONTHS.IndexOf(Char.Parse(raw["birthdate_month"])) + 1;
        int birthdateDay = int.Parse(raw["birthdate_day"].Translate(Constants._OMOCODIA_DECODE_TRANS));

        string gender;
        if (birthdateDay > 40)
        {
            birthdateDay -= 40;
            gender = "F";
        }
        else
        {
            gender = "M";
        }

        int currentYear = DateTime.Now.Year;
        string currentYearCenturyPrefix = currentYear.ToString().Substring(0, 2);
        string birthdateYearSuffix = birthdateYear.ToString("D2");
        birthdateYear = int.Parse($"{currentYearCenturyPrefix}{birthdateYearSuffix}");
        if (birthdateYear > currentYear)
        {
            birthdateYear -= 100;
        }

        string birthdateStr = $"{birthdateYear}/{birthdateMonth}/{birthdateDay}";
        var birthdate = CodeExtractorsHelper.ExtractDate(birthdateStr, "/");
        if (birthdate == null)
        {
            throw new ArgumentException($"[codicefiscale] invalid date: {birthdateStr}");
        }

        string birthplaceCode = raw["birthplace"][0] + raw["birthplace"].Substring(1).Translate(Constants._OMOCODIA_DECODE_TRANS);
        var birthplace = CodeExtractorsHelper.ExtractBirthplace(birthplaceCode, birthdate);
        if (birthplace == null)
        {
            throw new ArgumentException($"[codicefiscale] wrong birthplace code: {birthplaceCode} / birthdate: {birthdate?.ToString("o")}");
        }

        string cin = raw["cin"];
        string cinCheck = EncodingHelper.EncodeCin(code).ToString();
        if (cin != cinCheck)
        {
            throw new ArgumentException($"[codicefiscale] wrong CIN (Control Internal Number): expected {cinCheck}, found {cin}");
        }

        return new Dictionary<string, object>
       {
           { "code", code },
           { "omocodes", CodeExtractorsHelper.ExtractOmocodes(code) },
           { "gender", gender },
           { "birthdate", birthdate },
           { "birthplace", birthplace },
           { "raw", raw }
       };
    }

    // Method to check if the Italian Tax Code is valid
    public static bool IsValid(string code)
    {
        try
        {
            Decode(code);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}