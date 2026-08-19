using System;
using System.IO;
using System.Reflection;

namespace CountrySC;

/// <summary>
/// Resolves embedded square SVG flag icons for ISO 3166-1 alpha-2 country codes.
/// Icons are sourced from the flag-icons project (https://github.com/lipis/flag-icons, MIT licensed)
/// and embedded directly in the assembly, so no network access or external files are required.
/// </summary>
public static class CountryFlags
{
    private const string ResourceNamespace = "CountrySC.Assets.flags.";

    /// <summary>
    /// Gets the raw SVG markup for a country's flag icon, or an empty string if no icon
    /// is embedded for the given ISO 3166-1 alpha-2 country code.
    /// </summary>
    public static string GetSvg(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode)) return string.Empty;

        var resourceName = $"{ResourceNamespace}{countryCode.Trim().ToLowerInvariant()}.svg";
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) return string.Empty;

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
