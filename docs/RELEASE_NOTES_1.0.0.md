# Release Notes — GeoLocation.Data

- **Initial implementation** — `GeoLocationService` with an embedded, Brotli-compressed dataset (`countries.json.br`) of countries, states/provinces, and cities, loaded from an assembly resource at startup.
  - `GetAll()` — all countries, sorted alphabetically by name.
  - `GetByCountryCode(string)` — case-insensitive lookup by ISO 3166-1 alpha-2 code.
  - `GetByPhoneCode(string)` — lookup countries by e.164 dial code (accepts a leading `+`).
  - `GetStates(string?)` — states/provinces for a country, sorted alphabetically.
  - `GetCities(string?, int?)` — cities for a country + state id, sorted alphabetically.
  - `Country` model with computed `CountryCode` (derived from flag emoji Unicode codepoints), `FlagEmoji`, `PhoneCode`, `Example` phone number, and `DisplayName`.
  - `State` and `City` models.
