using Xunit;
using Xunit.Abstractions;
using CodiceFiscaleLib.Helpers;

namespace CodiceFiscaleTest;

public class CodiceFiscaleTest
{
    private readonly ITestOutputHelper _output;

    public CodiceFiscaleTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestCodiceFiscaleValido()
    {        
        bool res = DecodingHelper.IsValid("RSSMRA80E20F205I");

        Assert.True(res);
    }

    [Fact]
    public void TestEncodingCodiceFiscale()
    {        
        string code = "RSSMRA80E20F205I";

        string esito = EncodingHelper.Encode("Rossi", "Mario", 'M', new DateTime (1980, 5, 20), "Milano");

        Assert.Equal(code, esito);
    }

    [Fact]
    public void TestDecodingCodiceFiscale()
    {        
        string code = "RSSMRA80E20F205I";

        // check values
        string sex = "M";
        string firstName = "MRA";
        string lastName = "RSS";
        //DateTime birthDate = new DateTime (1980, 5, 20);
        string birthPlace = "MI";

        var res = DecodingHelper.Decode(code);

        // check if it's a dictionary and cast it
        Assert.True(res["raw"] is Dictionary<string, string>);
        var innerDict = (Dictionary<string, string>)res["raw"];

        // check if it's a list of dictionary and cast it
        Assert.True(res["birthplace"] is List<Dictionary<string, object>>);
        var innerDictPlaces = (List<Dictionary<string, object>>)res["birthplace"];

        Assert.Equal(code, res["code"]);
        Assert.Equal(sex, res["gender"]);        
        //Assert.Equal(birthDate, res["birthdate"]);
        Assert.Equal(birthPlace, innerDictPlaces[0]["province"]);
        Assert.Equal(firstName, innerDict["firstname"]);
        Assert.Equal(lastName, innerDict["lastname"]);
    }
}