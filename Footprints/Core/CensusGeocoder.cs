using System.Text.Json;

namespace Footprints.Core;

public static class CensusGeocoder
{
    public sealed class GeocodeResult
    {
        public required double Latitude { get; init; }
        public required double Longitude { get; init; }
        public required string MatchedAddress { get; init; }
    }

    public static async Task<GeocodeResult?> GeocodeAddressAsync(string oneLineAddress)
    {
        if (string.IsNullOrWhiteSpace(oneLineAddress))
        {
            return null;
        }

        string encodedAddress = Uri.EscapeDataString(oneLineAddress);
        string url =
            $"https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={encodedAddress}&benchmark=Public_AR_Current&format=json";

        using HttpClient client = new();
        string json = await client.GetStringAsync(url);
        using JsonDocument doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("result", out JsonElement resultElement) ||
            !resultElement.TryGetProperty("addressMatches", out JsonElement matches) ||
            matches.ValueKind != JsonValueKind.Array ||
            matches.GetArrayLength() == 0)
        {
            return null;
        }

        JsonElement firstMatch = matches[0];
        if (!firstMatch.TryGetProperty("coordinates", out JsonElement coordinates))
        {
            return null;
        }

        double lon = coordinates.GetProperty("x").GetDouble();
        double lat = coordinates.GetProperty("y").GetDouble();
        string matchedAddress = firstMatch.TryGetProperty("matchedAddress", out JsonElement matched)
            ? matched.GetString() ?? oneLineAddress
            : oneLineAddress;

        return new GeocodeResult
        {
            Latitude = lat,
            Longitude = lon,
            MatchedAddress = matchedAddress
        };
    }
}
