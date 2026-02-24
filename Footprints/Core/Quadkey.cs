using System;
using System.Text;

namespace Footprints.Core;

public class Quadkey
{
    private const double  EarthRadius = 6378137;
    private const double MinLatitude = -85.05112878;
    private const double MaxLatitude = 85.05112878;
    private const double MinLongitude = -180;
    private const double MaxLongitude = 180;

    /// <summary>
    /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees) 
    /// to a QuadKey at a specified level of detail.
    /// </summary>
    /// <param name="latitude">The latitude in degrees (-85.05112878 to 85.05112878).</param>
    /// <param name="longitude">The longitude in degrees (-180 to 180).</param>
    /// <param name="levelOfDetail">The level of detail, from 1 to 23.</param>
    /// <returns>A string containing the QuadKey.</returns>
    public static string LatLonToQuadKey(double latitude, double longitude, int levelOfDetail)
    {
        // Clip latitude and longitude values
        latitude = Clip(latitude, MinLatitude, MaxLatitude);
        longitude = Clip(longitude, MinLongitude, MaxLongitude);

        // Convert to pixel coordinates
        uint pixelX, pixelY;
        LatLonToPixelXY(latitude, longitude, levelOfDetail, out pixelX, out pixelY);

        // Convert to tile coordinates
        uint tileX, tileY;
        PixelXYToTileXY(pixelX, pixelY, out tileX, out tileY);

        // Convert to quadkey
        return TileXYToQuadKey(tileX, tileY, levelOfDetail);
    }

    /// <summary>
    /// Clips a number to a specified minimum and maximum value.
    /// </summary>
    private static double Clip(double n, double minValue, double maxValue)
    {
        return Math.Min(Math.Max(n, minValue), maxValue);
    }

    /// <summary>
    /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees)
    /// to pixel coordinates at a specified level of detail.
    /// </summary>
    private static void LatLonToPixelXY(double latitude, double longitude, int levelOfDetail, out uint pixelX, out uint pixelY)
    {
        double sinLatitude = Math.Sin(latitude * Math.PI / 180.0);
        double mapSize = 256 * Math.Pow(2, levelOfDetail);

        pixelX = (uint)((longitude + 180.0) / 360.0 * mapSize);
        pixelY = (uint)((0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI)) * mapSize);
    }

    /// <summary>
    /// Converts pixel coordinates to tile coordinates.
    /// </summary>
    private static void PixelXYToTileXY(uint pixelX, uint pixelY, out uint tileX, out uint tileY)
    {
        tileX = pixelX / 256;
        tileY = pixelY / 256;
    }

    /// <summary>
    /// Converts tile coordinates into a QuadKey string.
    /// </summary>
    private static string TileXYToQuadKey(uint tileX, uint tileY, int levelOfDetail)
    {
        StringBuilder quadKey = new StringBuilder();
        for (int i = levelOfDetail; i > 0; i--)
        {
            int digit = 0;
            uint mask = 1U << (i - 1);

            if ((tileX & mask) != 0)
            {
                digit += 1;
            }
            if ((tileY & mask) != 0)
            {
                digit += 2;
            }

            quadKey.Append(digit.ToString());
        }
        return quadKey.ToString();
    }
}