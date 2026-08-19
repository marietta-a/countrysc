# Release Notes — CountrySC 1.1.0

Release of `CountrySC` 1.1.0, introducing comprehensive country currency details, new API methods, and user interface enhancements.

## Added

- **Country Currency Support**:
  - Full support for currency name, ISO 4217 code, and currency symbol across **250 countries/territories** (100% dataset coverage).
  - Sourced from the authoritative [Geocountries currency database](https://www.geocountries.com/country/currencies).
- **New Models**:
  - `CountryCurrency` containing `Currency` (name of currency), `Iso4217` (three-letter ISO code), and `Symbol`.
  - `CountryCurrencies` static lookup mapping for offline, zero-dependency, and fast access.
- **New `Country` Class Properties**:
  - `Currency` — dynamically retrieves the currency name for the country (e.g., `"United States Dollar"`).
  - `Iso4217` — dynamically retrieves the three-letter currency code (e.g., `"USD"`).
  - `Symbol` — dynamically retrieves the currency symbol (e.g., `"$"` or `"€"`).
- **New `CountrySCService` Method**:
  - `GetCurrency(string? countryCode)` — retrieves currency details (`CountryCurrency`) for a given 2-letter country code. Returns `null` if the code is missing or unknown.
- **Cascading Dropdowns Sample App Integration**:
  - Updated the Blazor Server sample test application (`test/CountrySC.TestApp`) to display the currency details of the selected country dynamically under a new dedicated Currency section.
- **Comprehensive Unit Testing**:
  - Expanded unit test suite (`CountrySCServiceTests` and `CountryTests`) to thoroughly cover currency lookup, case-insensitivity, and fallback scenarios.
