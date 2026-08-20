<p align="center">
  <img src=".github/images/banner.svg" alt="CountrySC - offline Countries, States, Cities for .NET" width="100%" />
</p>

# CountrySC

[![NuGet version](https://img.shields.io/nuget/v/CountrySC.svg)](https://www.nuget.org/packages/CountrySC)
[![NuGet downloads](https://img.shields.io/nuget/dt/CountrySC.svg)](https://www.nuget.org/packages/CountrySC)

A lightweight .NET library providing a complete, offline dataset of countries, states/provinces, and cities — with ISO country codes, flag emojis, and international dial codes derived on the fly. No API calls, no network dependency; the dataset ships embedded (Brotli-compressed) inside the package.

If CountrySC is useful to you, consider giving the repo a ⭐ — it helps other .NET developers find the project.

## Install

```bash
dotnet add package CountrySC
```

Targets `net10.0`.

## Quick start

```csharp
using CountrySC;

var geo = new CountrySCService();

// All countries, sorted alphabetically by name
IEnumerable<Country> countries = geo.GetAll();

// Look up a country by its 2-letter ISO code (case-insensitive)
Country? us = geo.GetByCountryCode("US");
Console.WriteLine($"{us!.FlagEmoji} {us.Name} (+{us.PhoneCode})"); // 🇺🇸 United States (+1)

// States/provinces for a country
IEnumerable<State> states = geo.GetStates("US");

// Cities for a given country + state id
var california = states.First(s => s.Name == "California");
IEnumerable<City> cities = geo.GetCities("US", california.Id);

// Find every country sharing an international dial code
IEnumerable<Country> plusOne = geo.GetByPhoneCode("+1"); // US, CA, ...

// IANA time zones for a country
IEnumerable<TimeZoneEntry> zones = geo.GetTimeZones("US"); // America/New_York, America/Chicago, ...
TimeSpan? offset = zones.First().CurrentUtcOffset;

// Official language(s) for a country
IEnumerable<string> languages = geo.GetLanguages("CH"); // French, German, Italian, Romansh

// Currency details for a country (e.g. United States Dollar, USD, $)
CountryCurrency? currency = geo.GetCurrency("US");

// Capital city of a country
string capital = geo.GetCapitalCity("US"); // Washington, D.C.

// Continent of a country
Continent? continent = geo.GetContinent("US"); // Continent.Americas

// Or via country properties:
string currencyName = us.Currency; // "United States Dollar"
string currencyIso = us.Iso4217;  // "USD"
string currencySymbol = us.Symbol; // "$"
string capitalCity = us.CapitalCity; // "Washington, D.C."
Continent continentProperty = us.Continent; // Continent.Americas
```

## API

### `CountrySCService`

| Method | Description |
| --- | --- |
| `GetAll()` | All countries, sorted alphabetically by name. |
| `GetByCountryCode(string countryCode)` | Finds a country by ISO 3166-1 alpha-2 code (e.g. `"US"`, `"GB"`, `"GR"`). Case-insensitive; returns `null` if not found. |
| `GetByPhoneCode(string phoneCode)` | Finds all countries matching an e.164 dial code (e.g. `"1"` or `"+1"`). |
| `GetStates(string? countryCode)` | States/provinces for a country, sorted alphabetically. Empty if the country code is missing or unknown. |
| `GetCities(string? countryCode, int? stateId)` | Cities for a given country + state id, sorted alphabetically. Empty if either argument is missing or unknown. |
| `GetTimeZones(string? countryCode)` | IANA time zones observed in a country (e.g. `"America/New_York"`). Empty if the country code is missing or unknown. |
| `GetLanguages(string? countryCode)` | Official language(s) of a country (e.g. `"English"`). Empty if the country code is missing or unknown. |
| `GetFlagSvg(string? countryCode)` | Raw SVG markup of a country's square flag icon. Empty string if the country code is missing, unknown, or has no embedded icon. |
| `GetCurrency(string? countryCode)` | Gets the currency details for a given 2-letter country code. Returns `null` if the country code is missing or unknown. |
| `GetCapitalCity(string? countryCode)` | Gets the capital city for a given 2-letter country code. Returns `string.Empty` if the country code is missing or unknown. |
| `GetContinent(string? countryCode)` | Gets the continent for a given 2-letter country code. Returns `null` if the country code is missing or unknown. |

### Properties

- **`Country`** — `Id`, `Name`, `States`, plus computed properties:
  - `CountryCode` — ISO alpha-2 code, derived from the flag emoji's Unicode codepoints
  - `FlagEmoji` — the flag emoji (e.g. 🇺🇸)
  - `PhoneCode` — international dialing code (e.g. `"1"`, `"44"`)
  - `Example` — a sample local phone number for the country
  - `DisplayName` — `"United States (US)"`
  - `TimeZones` — the country's `TimeZoneEntry` list
  - `OfficialLanguages` — the country's official language name(s)
  - `FlagSvg` — raw SVG markup of the country's square flag icon, empty string if unavailable
  - `Currency` — name of the country's currency (e.g. `"United States Dollar"`)
  - `Iso4217` — three-letter ISO 4217 currency code (e.g. `"USD"`)
  - `Symbol` — currency symbol (e.g. `"$"` or `"€"`)
  - `CapitalCity` — capital city of the country (e.g. `"Washington, D.C."`)
  - `Continent` — continent where the country is located (`Africa`, `Americas`, `Antarctica`, `Asia`, `Europe`, `Oceania`)
- **`State`** — `Id`, `Name`, `Cities`
- **`City`** — `Id`, `Name`
- **`CountryCurrency`** — `Currency` (name of the currency), `Iso4217` (three-letter code), `Symbol` (symbol)
- **`TimeZoneEntry`** — `ZoneName` (IANA identifier), `CountryCode`, plus computed properties:
  - `BaseUtcOffset` — standard (non-DST) UTC offset, resolved from the local system's time zone database
  - `CurrentUtcOffset` — UTC offset right now, including DST if in effect

Time zone data covers 246 countries/territories (418 zones total). Only IANA zone names are embedded; UTC offsets are resolved live via `TimeZoneInfo` so they stay correct across DST transitions instead of going stale.

Official language data covers 201 countries/territories. Coverage follows the source article and skips a handful of micro-territories and a few disputed/partially-recognized territories not present in the country dataset.

Currency data covers 250 countries/territories. Sourced from the Geocountries currency database (https://www.geocountries.com/country/currencies).

Capital city data covers 250 countries/territories. Sourced from Worlddata (https://www.worlddata.info/capital-cities.php).

Continent data covers 250 countries/territories. Sourced from Worlddata (https://www.worlddata.info/capital-cities.php).

## Data sources

- Countries/states/cities, flag emojis, and phone codes — embedded dataset in `countries.json.br`
- Time zones — [TimeZoneDB time zone list](https://timezonedb.com/time-zones)
- Official languages — [Wikipedia: List of official languages by country and territory](https://en.wikipedia.org/wiki/List_of_official_languages_by_country_and_territory)
- Currency — [Geocountries currency database](https://www.geocountries.com/country/currencies)
- Capital cities — [Worlddata capital cities list](https://www.worlddata.info/capital-cities.php)
- Continents — [Worlddata capital cities list](https://www.worlddata.info/capital-cities.php)

## Repository layout

- [`src/CountrySC`](src/CountrySC) — the library
- [`src/test/CountrySC.Tests`](src/test/CountrySC.Tests) — xUnit test suite
- [`src/test/CountrySC.TestApp`](src/test/CountrySC.TestApp) — Blazor Server sample app with cascading Country → State → City dropdowns

## Try the sample app

[`src/test/CountrySC.TestApp`](src/test/CountrySC.TestApp) is a small Blazor Server app that exercises the library end to end. Run it from the repo root:

```bash
dotnet run --project src/test/CountrySC.TestApp/CountrySC.TestApp.csproj
```

Then open the URL printed in the console (`https://localhost:7280` by default) in your browser. Pick a country from the dropdown to see:

- cascading **State** and **City** dropdowns for that country
- its **time zones**, with live UTC offsets
- its **official language(s)**
- its **flag icon**, rendered from the embedded SVG

## Development

```bash
dotnet build CountrySC.slnx
dotnet test src/test/CountrySC.Tests/CountrySC.Tests.csproj
```

## Disclaimer

This library bundles country, state/province, city, flag, time zone, and language data for general informational and development use. Inclusion of any country, territory, or region - and the names, borders, or flags used to represent it - does not imply any opinion on its political status, sovereignty, or legal recognition. Data is aggregated from the third-party sources listed above, may contain inaccuracies, and can become outdated.

## License

[MIT](LICENSE)
