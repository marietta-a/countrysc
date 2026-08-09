# GeoLocation.Data

A lightweight .NET library providing a complete, offline dataset of countries, states/provinces, and cities — with ISO country codes, flag emojis, and international dial codes derived on the fly. No API calls, no network dependency; the dataset ships embedded (Brotli-compressed) inside the package.

## Install

```bash
dotnet add package GeoLocation.Data
```

Targets `net10.0`.

## Quick start

```csharp
using GeoLocation.Data;

var geo = new GeoLocationService();

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
```

## API

### `GeoLocationService`

| Method | Description |
| --- | --- |
| `GetAll()` | All countries, sorted alphabetically by name. |
| `GetByCountryCode(string countryCode)` | Finds a country by ISO 3166-1 alpha-2 code (e.g. `"US"`, `"GB"`, `"GR"`). Case-insensitive; returns `null` if not found. |
| `GetByPhoneCode(string phoneCode)` | Finds all countries matching an e.164 dial code (e.g. `"1"` or `"+1"`). |
| `GetStates(string? countryCode)` | States/provinces for a country, sorted alphabetically. Empty if the country code is missing or unknown. |
| `GetCities(string? countryCode, int? stateId)` | Cities for a given country + state id, sorted alphabetically. Empty if either argument is missing or unknown. |

### Properties

- **`Country`** — `Id`, `Name`, `States`, plus computed properties:
  - `CountryCode` — ISO alpha-2 code, derived from the flag emoji's Unicode codepoints
  - `FlagEmoji` — the flag emoji (e.g. 🇺🇸)
  - `PhoneCode` — international dialing code (e.g. `"1"`, `"44"`)
  - `Example` — a sample local phone number for the country
  - `DisplayName` — `"United States (US)"`
- **`State`** — `Id`, `Name`, `Cities`
- **`City`** — `Id`, `Name`

