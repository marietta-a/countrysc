# Release Notes — CountrySC 1.2.0

Release of `CountrySC` 1.2.0, introducing comprehensive country capital cities and continent classification support, new enum and model properties, and new API service methods.

## Added

- **Capital City Support**:
  - Full support for capital cities across **250 countries/territories** (100% dataset coverage).
  - Sourced from the authoritative [Worlddata list of capital cities](https://www.worlddata.info/capital-cities.php).
  - **New `Country` Class Property**:
    - `CapitalCity` — dynamically retrieves the capital city of the country (e.g., `"Washington, D.C."` or `"Athens"`).
  - **New `CountrySCService` Method**:
    - `GetCapitalCity(string? countryCode)` — retrieves the capital city as a string for a given 2-letter country code. Returns `string.Empty` if the code is missing or unknown.

- **Country Continent Classification**:
  - Complete mapping of countries and territories to their respective continents across **250 countries/territories** (100% dataset coverage).
  - Sourced and inferred from [Worlddata](https://www.worlddata.info/capital-cities.php).
  - **New Enum**:
    - `Continent` — representing six major global regions: `Africa`, `Americas`, `Antarctica`, `Asia`, `Europe`, and `Oceania`.
  - **New `Country` Class Property**:
    - `Continent` — dynamically retrieves the continent where the country is located (e.g., `Continent.Americas` or `Continent.Europe`).
  - **New `CountrySCService` Method**:
    - `GetContinent(string? countryCode)` — retrieves the continent details (`Continent?`) for a given 2-letter country code. Returns `null` if the code is missing or unknown.

- **Robust Unit Testing**:
  - Expanded unit test suites (`CountrySCServiceTests` and `CountryTests`) to ensure rigorous validation of capital cities and continent classifications under various boundary conditions, invalid codes, and case-insensitivity scenarios.

- **Documentation Integration**:
  - Full usage guidelines, code snippets, and API references incorporated across both `src\CountrySC\README.md` and the root `README.md`.
