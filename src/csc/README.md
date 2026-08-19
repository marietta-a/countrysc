# csc

A lightweight .NET library providing a complete, offline dataset of countries, states/provinces, and cities — with ISO country codes, flag emojis, and international dial codes derived on the fly. No API calls, no network dependency; the dataset ships embedded (Brotli-compressed) inside the package.

## Install

```bash
dotnet add package csc
```

Targets `net10.0`.

## Quick start

```csharp
using csc;

var geo = new CSCService();

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

// Square SVG flag icon for a country (raw markup)
string flagSvg = geo.GetFlagSvg("US");
```

## API

### `CSCService`

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
- **`State`** — `Id`, `Name`, `Cities`
- **`City`** — `Id`, `Name`
- **`TimeZoneEntry`** — `ZoneName` (IANA identifier), `CountryCode`, plus computed properties:
  - `BaseUtcOffset` — standard (non-DST) UTC offset, resolved from the local system's time zone database
  - `CurrentUtcOffset` — UTC offset right now, including DST if in effect

Time zone data covers 246 countries/territories (418 zones total). Only IANA zone names are embedded; UTC offsets are resolved live via `TimeZoneInfo` so they stay correct across DST transitions instead of going stale.

Official language data covers 201 countries/territories. Coverage follows the source article and skips a handful of micro-territories and a few disputed/partially-recognized territories not present in the country dataset.

## Data sources

- Countries/states/cities, flag emojis, and phone codes — embedded dataset in `countries.json.br`
- Time zones — [TimeZoneDB time zone list](https://timezonedb.com/time-zones)
- Official languages — [Wikipedia: List of official languages by country and territory](https://en.wikipedia.org/wiki/List_of_official_languages_by_country_and_territory)
- Flag icons — [flag-icons](https://github.com/lipis/flag-icons) (MIT licensed), embedded as SVG resources

