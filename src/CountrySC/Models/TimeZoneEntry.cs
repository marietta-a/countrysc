using System;

namespace CountrySC;

/// <summary>
/// Represents a single IANA time zone belonging to a country.
/// </summary>
public class TimeZoneEntry
{
    /// <summary>
    /// IANA time zone identifier (e.g., "America/New_York", "Asia/Kabul").
    /// </summary>
    public string ZoneName { get; init; } = string.Empty;

    /// <summary>
    /// Two-letter ISO country code this time zone belongs to.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    /// Standard (non-daylight-saving) UTC offset for this zone, resolved from the local
    /// system's time zone database. Null if the zone name could not be resolved.
    /// </summary>
    public TimeSpan? BaseUtcOffset => TimeZoneInfo.TryFindSystemTimeZoneById(ZoneName, out var tz) ? tz.BaseUtcOffset : null;

    /// <summary>
    /// Current UTC offset for this zone right now, including daylight saving time if in effect.
    /// Null if the zone name could not be resolved.
    /// </summary>
    public TimeSpan? CurrentUtcOffset => TimeZoneInfo.TryFindSystemTimeZoneById(ZoneName, out var tz) ? tz.GetUtcOffset(DateTimeOffset.UtcNow) : null;
}
