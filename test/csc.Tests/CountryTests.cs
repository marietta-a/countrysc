using csc;

namespace csc.Tests;

public class CountryTests
{
    [Theory]
    [InlineData("U+1F1FA U+1F1F8", "US")]
    [InlineData("U+1F1EC U+1F1F7", "GR")]
    public void GetCountryCodeFromEmojiU_DecodesRegionalIndicatorPairs(string emojiU, string expected)
    {
        Assert.Equal(expected, Country.GetCountryCodeFromEmojiU(emojiU));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("U+1F1FA")]
    [InlineData("not-a-codepoint not-either")]
    public void GetCountryCodeFromEmojiU_ReturnsEmpty_ForMalformedInput(string emojiU)
    {
        Assert.Equal(string.Empty, Country.GetCountryCodeFromEmojiU(emojiU));
    }

    [Fact]
    public void DisplayName_CombinesNameAndCountryCode()
    {
        var country = new Country { Name = "United States", EmojiU = "U+1F1FA U+1F1F8" };

        Assert.Equal("United States (US)", country.DisplayName);
    }

    [Fact]
    public void TimeZones_ReturnsZonesForCountry()
    {
        var country = new Country { Name = "Greece", EmojiU = "U+1F1EC U+1F1F7" };

        Assert.Contains(country.TimeZones, z => z.ZoneName == "Europe/Athens");
        Assert.All(country.TimeZones, z => Assert.Equal("GR", z.CountryCode));
    }

    [Fact]
    public void TimeZones_ReturnsEmpty_WhenCountryCodeCannotBeDerived()
    {
        var country = new Country { Name = "Unknown" };

        Assert.Empty(country.TimeZones);
    }

    [Fact]
    public void OfficialLanguages_ReturnsLanguagesForCountry()
    {
        var country = new Country { Name = "Greece", EmojiU = "U+1F1EC U+1F1F7" };

        Assert.Contains("Greek", country.OfficialLanguages);
    }

    [Fact]
    public void OfficialLanguages_ReturnsEmpty_WhenCountryCodeCannotBeDerived()
    {
        var country = new Country { Name = "Unknown" };

        Assert.Empty(country.OfficialLanguages);
    }
}
