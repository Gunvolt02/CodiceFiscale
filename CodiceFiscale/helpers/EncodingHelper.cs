using System.Text.RegularExpressions;
using CodiceFiscaleLib.Config;

namespace CodiceFiscaleLib.Helpers;

public static class EncodingHelper
{
    // Method to encode the last name
    public static string EncodeLastname(string lastname)
    {
        string lastnameSlug = StringsHelper.Slugify(lastname);
        var lastnameConsonants = CodeExtractorsHelper.ExtractConsonants(lastnameSlug);
        var lastnameVowels = CodeExtractorsHelper.ExtractVowels(lastnameSlug);
        string lastnameCode = CodeExtractorsHelper.ExtractConsonantsAndVowels(lastnameConsonants, lastnameVowels);
        return lastnameCode;
    }

    // Method to encode the first name
    public static string EncodeFirstname(string firstname)
    {
        string firstnameSlug = StringsHelper.Slugify(firstname);
        var firstnameConsonants = CodeExtractorsHelper.ExtractConsonants(firstnameSlug);

        if (firstnameConsonants.Count > 3)
        {
            firstnameConsonants.RemoveAt(1);
        }

        var firstnameVowels = CodeExtractorsHelper.ExtractVowels(firstnameSlug);
        string firstnameCode = CodeExtractorsHelper.ExtractConsonantsAndVowels(firstnameConsonants, firstnameVowels);
        return firstnameCode;
    }

    // Method to encode the birth date
    public static string EncodeBirthdate(DateTime? birthdate, char gender)
    {
        if (birthdate == null)
            throw new ArgumentException("[codicefiscale] 'birthdate' argument can't be None");

        if (!new[] { 'M', 'F' }.Contains(char.ToUpper(gender)))
            throw new ArgumentException("[codicefiscale] 'gender' argument must be 'M' or 'F'");

        var date = birthdate.Value;
        string yearCode = date.Year.ToString().Substring(2);
        string monthCode = Constants._MONTHS[date.Month - 1].ToString();
        string dayCode = (date.Day + (gender == 'F' ? 40 : 0)).ToString("D2");
        return $"{yearCode}{monthCode}{dayCode}";
    }

    // Method to encode the birth place
    public static string EncodeBirthplace(string birthplace, DateTime? birthdate = null)
    {
        if (string.IsNullOrWhiteSpace(birthplace))
            throw new ArgumentException("[codicefiscale] 'birthplace' argument can't be None");

        string birthplaceWithoutProvince = Regex.Split(birthplace, ",|\\(")[0];
        var birthplaceData = CodeExtractorsHelper.ExtractBirthplace(birthplace, birthdate) ?? CodeExtractorsHelper.ExtractBirthplace(birthplaceWithoutProvince, birthdate);

        if (birthplaceData == null)
        {
            throw new ArgumentException($"[codicefiscale] 'birthplace' / 'birthdate' arguments ({birthplace} / {birthdate}) not mapped to code");
        }

        return birthplaceData[0]["code"].ToString();
    }

    // Method to encode the CIN (Control Internal Number)
    public static char EncodeCin(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("[codicefiscale] 'code' argument can't be None");

        if (code.Length < 15 || code.Length > 16)
            throw new ArgumentException("[codicefiscale] 'code' length must be 15 or 16.");

        int cinTot = 0;
        for (int i = 0; i < 15; i++)
        {
            char c = code[i];
            var tuple = Constants._CIN[c];
            cinTot += (i + 1) % 2 == 0 ? tuple.Item1 : tuple.Item2;
        }

        return Constants._CIN_REMAINDERS[cinTot % 26];
    }

    // Method to encode the Italian Tax Code
    public static string Encode(string lastname, string firstname, char gender, DateTime? birthdate, string birthplace)
    {
        string lastnameCode = EncodeLastname(lastname);
        string firstnameCode = EncodeFirstname(firstname);
        string birthdateCode = EncodeBirthdate(birthdate, gender);
        string birthplaceCode = EncodeBirthplace(birthplace, birthdate);
        string code = $"{lastnameCode}{firstnameCode}{birthdateCode}{birthplaceCode}";
        string cinCode = EncodeCin(code).ToString();
        code = $"{code}{cinCode}";

        // Check if it's valid
        DecodingHelper.Decode(code);

        return code;
    }
}