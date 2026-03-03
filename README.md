# Footprints

`Footprints` is a .NET solution for finding and visualizing building footprints from latitude/longitude coordinates.

It contains:
- `Core`: shared logic (quadkey generation, dataset URL lookup, download/extract, Census geocoding)
- `Console`: CLI workflow to fetch and extract a building-footprint file
- `WinForms`: desktop UI to geocode an address, download/load GeoJSON, and render nearby buildings

## Solution Structure

- `Footprints.sln`
- `Core/`
  - `Quadkey.cs`: converts WGS84 lat/lon to Bing-style quadkeys
  - `ApiClient.cs`: maps quadkey to dataset URL (via `dataset-links.csv`), downloads `.gz`, extracts output
  - `CensusGeocoder.cs`: geocodes one-line addresses via U.S. Census Geocoder API
  - `dataset-links.csv`: quadkey -> downloadable dataset URL index
- `Console/`
  - `Program.cs`: sample end-to-end download/extract flow from a fixed lat/lon
- `WinForms/`
  - `Form1.cs`: two-tab UI for geocoding + footprint loading/visualization
  - `Form1.Designer.cs`: WinForms layout

## Prerequisites

- Windows (projects target `net10.0-windows`)
- .NET SDK that supports `net10.0-windows`
- Internet access (required for dataset download and Census geocoding)

## Build

```powershell
dotnet build Footprints.sln
```

## Run

### WinForms app (recommended)

```powershell
dotnet run --project .\WinForms\WinForms.csproj
```

Workflow:
1. (Optional) In `Address -> Lat/Lon`, geocode an address.
2. Click `Use In Footprints Tab` to transfer coordinates.
3. In `Lat/Lon -> Footprints`, choose a download directory.
4. Click `Run` to resolve quadkey, download footprints, extract, and visualize buildings within 1000m.
5. Use `Load GeoJSON` to view an existing file.

### Console app

```powershell
dotnet run --project .\Console\Console.csproj
```

Current behavior:
- Uses hardcoded coordinates and level-of-detail 9 in `Console/Program.cs`
- Writes to a hardcoded local path in `Console/Program.cs`
- Downloads `.geojson.gz` then extracts to `.geojson`

## Data and APIs

- Building footprint index and files are read via URLs listed in `Core/dataset-links.csv`.
- Address geocoding uses the U.S. Census endpoint:
  - `https://geocoding.geo.census.gov/geocoder/locations/onelineaddress`

## Notes / Known Limitations

- `ApiClient.quadkeyToUrl` reads `dataset-links.csv` from a relative path (`..\\..\\..\\..\\Core\\dataset-links.csv`), which assumes a specific working directory layout.
- `ApiClient.extractBF` counts features by scanning lines containing the string `Feature`; this is a rough count, not a strict GeoJSON parser count.
- WinForms viewer currently renders exterior rings only and filters buildings to a fixed 1000m radius around center.
- Repository currently includes `bin/` and `obj/` build outputs; adding a `.gitignore` is recommended.

## Suggested Next Improvements

- Accept CLI args in `Console` (`lat`, `lon`, `downloadDir`, `levelOfDetail`) instead of hardcoded values.
- Make `dataset-links.csv` path resolution robust (absolute/base-directory based).
- Add `.gitignore` for .NET (`bin/`, `obj/`, `.idea/`).
- Add tests for quadkey conversion and URL lookup behavior.
