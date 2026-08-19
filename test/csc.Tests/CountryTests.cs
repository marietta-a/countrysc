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
}
