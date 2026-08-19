using System.Linq;
using csc;

namespace csc.Tests;

public class CSCServiceTests
{
    private readonly CSCService _service = new();

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
}
