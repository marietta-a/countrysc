using System.Linq;
using CountrySC;

namespace CountrySC.Tests;

public class CountrySCServiceTests
{
    private readonly CountrySCService _service = new();

    [Fact]
    public void GetAll_ReturnsNonEmptyList_SortedAlphabeticallyByName()
    {
        var countries = _service.GetAll().ToList();

        Assert.NotEmpty(countries);
        var sorted = countries.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).Select(c => c.Name);
        Assert.Equal(sorted, countries.Select(c => c.Name));
    }

    [Theory]
    [InlineData("US", "United States", "1")]
    [InlineData("us", "United States", "1")]
    [InlineData("GR", "Greece", "30")]
    public void GetByCountryCode_FindsCountry_CaseInsensitive(string code, string expectedName, string expectedPhoneCode)
    {
        var country = _service.GetByCountryCode(code);

        Assert.NotNull(country);
        Assert.Equal(expectedName, country!.Name);
        Assert.Equal(expectedPhoneCode, country.PhoneCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ZZ")]
    public void GetByCountryCode_ReturnsNull_ForInvalidOrMissingCode(string? code)
    {
        Assert.Null(_service.GetByCountryCode(code!));
    }

    [Fact]
    public void GetByPhoneCode_ReturnsAllCountriesSharingThatDialCode()
    {
        var countries = _service.GetByPhoneCode("1").Select(c => c.CountryCode).ToList();

        Assert.Contains("US", countries);
        Assert.Contains("CA", countries);
    }

    [Fact]
    public void GetByPhoneCode_StripsLeadingPlus()
    {
        var withPlus = _service.GetByPhoneCode("+30").Select(c => c.CountryCode).ToList();
        var withoutPlus = _service.GetByPhoneCode("30").Select(c => c.CountryCode).ToList();

        Assert.Equal(withoutPlus, withPlus);
        Assert.Contains("GR", withPlus);
    }

    [Fact]
    public void GetByPhoneCode_ReturnsEmpty_ForBlankInput()
    {
        Assert.Empty(_service.GetByPhoneCode(""));
        Assert.Empty(_service.GetByPhoneCode(null!));
    }

    [Fact]
    public void GetStates_ReturnsStatesForValidCountry_SortedAlphabetically()
    {
        var states = _service.GetStates("US").ToList();

        Assert.NotEmpty(states);
        var sorted = states.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).Select(s => s.Name);
        Assert.Equal(sorted, states.Select(s => s.Name));
        Assert.Contains(states, s => s.Name == "Alabama");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ZZ")]
    public void GetStates_ReturnsEmpty_ForInvalidOrMissingCountry(string? code)
    {
        Assert.Empty(_service.GetStates(code));
    }

    [Fact]
    public void GetCities_ReturnsCitiesForValidCountryAndState_SortedAlphabetically()
    {
        var alabama = _service.GetStates("US").First(s => s.Name == "Alabama");

        var cities = _service.GetCities("US", alabama.Id).ToList();

        Assert.NotEmpty(cities);
        var sorted = cities.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).Select(c => c.Name);
        Assert.Equal(sorted, cities.Select(c => c.Name));
        Assert.Contains(cities, c => c.Name == "Abbeville");
    }

    [Fact]
    public void GetCities_ReturnsEmpty_WhenStateIdIsNull()
    {
        Assert.Empty(_service.GetCities("US", null));
    }

    [Fact]
    public void GetCities_ReturnsEmpty_WhenCountryCodeIsMissing()
    {
        Assert.Empty(_service.GetCities(null, 1456));
        Assert.Empty(_service.GetCities("", 1456));
    }

    [Fact]
    public void GetCities_ReturnsEmpty_WhenStateIdDoesNotBelongToCountry()
    {
        Assert.Empty(_service.GetCities("US", -1));
    }

    [Fact]
    public void GetCities_ReturnsEmpty_ForInvalidCountry()
    {
        Assert.Empty(_service.GetCities("ZZ", 1456));
    }

    [Theory]
    [InlineData("US")]
    [InlineData("us")]
    public void GetTimeZones_ReturnsZonesForValidCountry_CaseInsensitive(string code)
    {
        var zones = _service.GetTimeZones(code).ToList();

        Assert.NotEmpty(zones);
        Assert.Contains(zones, z => z.ZoneName == "America/New_York");
        Assert.All(zones, z => Assert.Equal("US", z.CountryCode));
    }

    [Fact]
    public void GetTimeZones_ReturnsMultipleZones_ForMultiZoneCountry()
    {
        var zones = _service.GetTimeZones("RU").ToList();

        Assert.True(zones.Count > 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ZZ")]
    public void GetTimeZones_ReturnsEmpty_ForInvalidOrMissingCode(string? code)
    {
        Assert.Empty(_service.GetTimeZones(code));
    }

    [Theory]
    [InlineData("IN", "English")]
    [InlineData("in", "English")]
    [InlineData("GR", "Greek")]
    public void GetLanguages_ReturnsLanguagesForValidCountry_CaseInsensitive(string code, string expectedLanguage)
    {
        var languages = _service.GetLanguages(code).ToList();

        Assert.NotEmpty(languages);
        Assert.Contains(expectedLanguage, languages);
    }

    [Fact]
    public void GetLanguages_ReturnsMultipleLanguages_ForMultilingualCountry()
    {
        var languages = _service.GetLanguages("CH").ToList();

        Assert.True(languages.Count > 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ZZ")]
    public void GetLanguages_ReturnsEmpty_ForInvalidOrMissingCode(string? code)
    {
        Assert.Empty(_service.GetLanguages(code));
    }

    [Theory]
    [InlineData("US")]
    [InlineData("us")]
    public void GetFlagSvg_ReturnsSvgMarkup_ForValidCountry_CaseInsensitive(string code)
    {
        var svg = _service.GetFlagSvg(code);

        Assert.StartsWith("<svg", svg);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ZZ")]
    public void GetFlagSvg_ReturnsEmpty_ForInvalidOrMissingCode(string? code)
    {
        Assert.Empty(_service.GetFlagSvg(code));
    }

    [Theory]
    [InlineData("US", "United States Dollar", "USD", "$")]
    [InlineData("us", "United States Dollar", "USD", "$")]
    [InlineData("GR", "Euro", "EUR", "€")]
    [InlineData("GB", "Pound Sterling", "GBP", "£")]
    [InlineData("JP", "Japanese Yen", "JPY", "¥")]
    public void GetCurrency_ReturnsCorrectCurrency_ForValidCountry(string code, string expectedName, string expectedIso, string expectedSymbol)
    {
        var currency = _service.GetCurrency(code);

        Assert.NotNull(currency);
        Assert.Equal(expectedName, currency!.Currency);
        Assert.Equal(expectedIso, currency.Iso4217);
        Assert.Equal(expectedSymbol, currency.Symbol);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ZZ")]
    public void GetCurrency_ReturnsNull_ForInvalidOrMissingCode(string? code)
    {
        Assert.Null(_service.GetCurrency(code));
    }
}
