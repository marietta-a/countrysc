# Release Notes — csc 1.0.0

Initial release of `csc`: a lightweight, offline dataset of countries, states/provinces, and cities for .NET. No API calls, no network dependency — the dataset ships embedded (Brotli-compressed) inside the package.

## Added

- `CSCService` for querying the embedded dataset:
  - `GetAll()` — all countries, sorted alphabetically by name.
  - `GetByCountryCode(string)` — case-insensitive lookup by ISO 3166-1 alpha-2 code.
  - `GetByPhoneCode(string)` — lookup countries by e.164 dial code (accepts a leading `+`).
  - `GetStates(string?)` — states/provinces for a country, sorted alphabetically.
  - `GetCities(string?, int?)` — cities for a country + state id, sorted alphabetically.
  - `GetTimeZones(string?)` — IANA time zones observed in a country.
  - `GetLanguages(string?)` — official language(s) of a country.
  - `GetFlagSvg(string?)` — raw SVG markup of a country's square flag icon.
- `Country` model with computed properties: `CountryCode` (derived from the flag emoji's Unicode codepoints), `FlagEmoji`, `PhoneCode`, `Example` phone number, `DisplayName`, `TimeZones`, `OfficialLanguages`, and `FlagSvg`.
- `State` and `City` models.
- `TimeZoneEntry` model with `BaseUtcOffset` and `CurrentUtcOffset`, resolved live from the local system's time zone database so offsets stay correct across DST transitions.
- Embedded SVG flag icons per country, sourced from the [flag-icons](https://github.com/lipis/flag-icons) project (MIT licensed).
- Official language data covering 201 countries/territories, sourced from Wikipedia.
- IANA time zone data covering 246 countries/territories (418 zones total), sourced from the TimeZoneDB time zone list.
- MIT license 
