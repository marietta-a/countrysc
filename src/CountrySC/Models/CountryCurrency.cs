using System;
using System.Text.Json.Serialization;

namespace CountrySC;

/// <summary>
/// Represents a country's currency details.
/// </summary>
public class CountryCurrency
{
    /// <summary>
    /// Name of the currency (e.g., "United States Dollar", "Euro").
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Three-letter ISO 4217 currency code (e.g., "USD", "EUR").
    /// </summary>
    [JsonPropertyName("iso-4217")]
    public string Iso4217 { get; init; } = string.Empty;

    /// <summary>
    /// Currency symbol (e.g., "$", "€").
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;
}
