using System.Globalization;
using System.Text.Json;
using Footprints.Core;

namespace Footprints.WinForms;

public partial class Form1 : Form
{
    private readonly List<List<GeoPoint>> _allBuildingRingsGeo = [];
    private readonly List<List<MapPoint>> _visibleBuildingsProjected = [];
    private string? _loadedGeoJsonPath;

    private double _centerLat;
    private double _centerLon;
    private double _centerX;
    private double _centerY;

    private const double FilterRadiusMeters = 1000.0;
    private const double PixelSizeMetersAt96Dpi = 0.0254 / 96.0;
    private const double EarthRadiusMeters = 6378137.0;
    private const double MaxMercatorLatitude = 85.05112878;

    public Form1()
    {
        InitializeComponent();

        txtAddress.Text = "4600 Silver Hill Rd, Washington, DC 20233";
        txtLat.Text = "39.00000";
        txtLon.Text = "-94.00000";
        txtDownloadDir.Text = Directory.GetCurrentDirectory();
        ApiClient.Logger = Log;
        UpdateScaleLabel();
        UpdateBuildingCountLabel();
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        btnRun.Enabled = false;

        try
        {
            if (!TryGetCenterFromInputs(out double lat, out double lon))
            {
                MessageBox.Show("Invalid latitude or longitude.");
                return;
            }

            int levelOfDetail = 9;
            string downloadDir = txtDownloadDir.Text;

            if (string.IsNullOrWhiteSpace(downloadDir) || !Directory.Exists(downloadDir))
            {
                MessageBox.Show("Download directory does not exist.");
                return;
            }

            string quadKey = Quadkey.LatLonToQuadKey(lat, lon, levelOfDetail);
            Log($"quadKey: {quadKey}");

            string downloadUrl = ApiClient.quadkeyToUrl(quadKey);
            Log($"downloadURL: {downloadUrl}");

            string downloadedFile = await ApiClient.downloadBuildingFootprints(downloadUrl, downloadDir, quadKey);
            if (string.IsNullOrWhiteSpace(downloadedFile))
            {
                MessageBox.Show("Download failed (empty file path).");
                return;
            }

            Log($"downloadedFile: {downloadedFile}");
            ApiClient.extractBF(downloadedFile);
            Log("Extract complete.");

            string extractedGeoJson = downloadedFile.Replace(".gz", "");
            if (File.Exists(extractedGeoJson))
            {
                LoadGeoJson(extractedGeoJson, lat, lon);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error");
        }
        finally
        {
            btnRun.Enabled = true;
        }
    }

    private void Log(string message)
    {
        txtLog.AppendText(message + Environment.NewLine);
    }

    private void btnBrowseDownloadDir_Click(object sender, EventArgs e)
    {
        using FolderBrowserDialog dialog = new();
        dialog.Description = "Select destination folder for downloaded footprints";
        dialog.UseDescriptionForTitle = true;

        if (Directory.Exists(txtDownloadDir.Text))
        {
            dialog.InitialDirectory = txtDownloadDir.Text;
        }

        if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            txtDownloadDir.Text = dialog.SelectedPath;
        }
    }

    private async void btnGeocode_Click(object sender, EventArgs e)
    {
        btnGeocode.Enabled = false;
        lblGeocodeStatus.Text = "Geocoding...";

        try
        {
            CensusGeocoder.GeocodeResult? result = await CensusGeocoder.GeocodeAddressAsync(txtAddress.Text);
            if (result == null)
            {
                lblGeocodeStatus.Text = "No address match found.";
                return;
            }

            txtGeoLat.Text = result.Latitude.ToString("0.000000", CultureInfo.InvariantCulture);
            txtGeoLon.Text = result.Longitude.ToString("0.000000", CultureInfo.InvariantCulture);
            lblGeocodeStatus.Text = $"Matched: {result.MatchedAddress}";
        }
        catch (Exception ex)
        {
            lblGeocodeStatus.Text = "Geocoding failed.";
            MessageBox.Show(ex.Message, "Geocoding Error");
        }
        finally
        {
            btnGeocode.Enabled = true;
        }
    }

    private void btnUseCoordinates_Click(object sender, EventArgs e)
    {
        if (!double.TryParse(txtGeoLat.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _) ||
            !double.TryParse(txtGeoLon.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
        {
            MessageBox.Show("Geocode an address first.");
            return;
        }

        txtLat.Text = txtGeoLat.Text;
        txtLon.Text = txtGeoLon.Text;
        tabControlMain.SelectedTab = tabFootprints;

        if (_loadedGeoJsonPath != null)
        {
            RebuildVisibleBuildingsFromCenter();
        }
    }

    private void btnLoadGeoJson_Click(object sender, EventArgs e)
    {
        using OpenFileDialog dialog = new();
        dialog.Filter = "GeoJSON files (*.geojson)|*.geojson|JSON files (*.json)|*.json|All files (*.*)|*.*";
        dialog.Title = "Select GeoJSON file";
        dialog.InitialDirectory = Directory.Exists(txtDownloadDir.Text) ? txtDownloadDir.Text : Directory.GetCurrentDirectory();

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (!TryGetCenterFromInputs(out double lat, out double lon))
            {
                MessageBox.Show("Invalid latitude or longitude.");
                return;
            }

            LoadGeoJson(dialog.FileName, lat, lon);
        }
    }

    private void trkScale_Scroll(object sender, EventArgs e)
    {
        UpdateScaleLabel();
        pnlViewer.Invalidate();
    }

    private void LoadGeoJson(string path, double centerLat, double centerLon)
    {
        try
        {
            string json = File.ReadAllText(path).TrimStart('\uFEFF').Trim();
            List<List<GeoPoint>> parsed = ParseFeatures(json);

            _allBuildingRingsGeo.Clear();
            _allBuildingRingsGeo.AddRange(parsed);
            _loadedGeoJsonPath = path;

            _centerLat = centerLat;
            _centerLon = centerLon;
            (_centerX, _centerY) = ProjectToWebMercator(_centerLon, _centerLat);

            RebuildVisibleBuildingsFromCenter();
            Log($"Loaded {_allBuildingRingsGeo.Count} building footprints from: {path}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load GeoJSON: {ex.Message}", "GeoJSON Error");
        }
    }

    private void RebuildVisibleBuildingsFromCenter()
    {
        if (!TryGetCenterFromInputs(out _centerLat, out _centerLon))
        {
            return;
        }

        (_centerX, _centerY) = ProjectToWebMercator(_centerLon, _centerLat);
        _visibleBuildingsProjected.Clear();

        foreach (List<GeoPoint> ring in _allBuildingRingsGeo)
        {
            if (ring.Count < 3)
            {
                continue;
            }

            List<MapPoint> projected = new(ring.Count);
            double sumX = 0;
            double sumY = 0;

            foreach (GeoPoint point in ring)
            {
                (double x, double y) = ProjectToWebMercator(point.Lon, point.Lat);
                projected.Add(new MapPoint(x, y));
                sumX += x;
                sumY += y;
            }

            double centroidX = sumX / projected.Count;
            double centroidY = sumY / projected.Count;
            double distanceToCenter = Math.Sqrt(Math.Pow(centroidX - _centerX, 2) + Math.Pow(centroidY - _centerY, 2));

            if (distanceToCenter <= FilterRadiusMeters)
            {
                _visibleBuildingsProjected.Add(projected);
            }
        }

        UpdateBuildingCountLabel();
        pnlViewer.Invalidate();
    }

    private List<List<GeoPoint>> ParseFeatures(string json)
    {
        List<List<GeoPoint>> result = [];

        if (TryParseSingleJson(json, result))
        {
            return result;
        }

        string[] lines = json.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (trimmed.Length == 0)
            {
                continue;
            }

            try
            {
                using JsonDocument lineDoc = JsonDocument.Parse(trimmed);
                ExtractFromRoot(lineDoc.RootElement, result);
            }
            catch (JsonException)
            {
                // Continue if a line is malformed.
            }
        }

        return result;
    }

    private static bool TryParseSingleJson(string json, List<List<GeoPoint>> destination)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            ExtractFromRoot(doc.RootElement, destination);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static void ExtractFromRoot(JsonElement root, List<List<GeoPoint>> destination)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (!root.TryGetProperty("type", out JsonElement rootType) || rootType.ValueKind != JsonValueKind.String)
        {
            return;
        }

        string? type = rootType.GetString();
        if (type == "FeatureCollection" &&
            root.TryGetProperty("features", out JsonElement featuresElement) &&
            featuresElement.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement feature in featuresElement.EnumerateArray())
            {
                ExtractFeatureGeometry(feature, destination);
            }
            return;
        }

        if (type == "Feature")
        {
            ExtractFeatureGeometry(root, destination);
        }
    }

    private static void ExtractFeatureGeometry(JsonElement feature, List<List<GeoPoint>> destination)
    {
        if (!feature.TryGetProperty("geometry", out JsonElement geometry) || geometry.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (!geometry.TryGetProperty("type", out JsonElement typeElement) || typeElement.ValueKind != JsonValueKind.String)
        {
            return;
        }

        if (!geometry.TryGetProperty("coordinates", out JsonElement coords))
        {
            return;
        }

        string? geometryType = typeElement.GetString();
        if (geometryType == "Polygon")
        {
            AddPolygonExteriorRing(coords, destination);
        }
        else if (geometryType == "MultiPolygon")
        {
            foreach (JsonElement polygonCoords in coords.EnumerateArray())
            {
                AddPolygonExteriorRing(polygonCoords, destination);
            }
        }
    }

    private static void AddPolygonExteriorRing(JsonElement polygonCoords, List<List<GeoPoint>> destination)
    {
        if (polygonCoords.ValueKind != JsonValueKind.Array || polygonCoords.GetArrayLength() == 0)
        {
            return;
        }

        JsonElement exteriorRing = polygonCoords[0];
        if (exteriorRing.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        List<GeoPoint> points = [];
        foreach (JsonElement coord in exteriorRing.EnumerateArray())
        {
            if (coord.ValueKind != JsonValueKind.Array || coord.GetArrayLength() < 2)
            {
                continue;
            }

            points.Add(new GeoPoint(coord[0].GetDouble(), coord[1].GetDouble()));
        }

        if (points.Count >= 3)
        {
            destination.Add(points);
        }
    }

    private void pnlViewer_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(Color.White);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        RectangleF viewport = pnlViewer.ClientRectangle;
        float centerScreenX = viewport.Width / 2f;
        float centerScreenY = viewport.Height / 2f;
        double metersPerPixel = trkScale.Value * PixelSizeMetersAt96Dpi;

        if (_visibleBuildingsProjected.Count == 0)
        {
            DrawCenteredText(e.Graphics, "No buildings loaded in 1000m radius.", pnlViewer.ClientRectangle);
        }
        else
        {
            using SolidBrush fillBrush = new(Color.FromArgb(70, 0, 120, 215));
            using Pen outlinePen = new(Color.FromArgb(0, 70, 140), 1.2f);

            foreach (List<MapPoint> ring in _visibleBuildingsProjected)
            {
                if (ring.Count < 3)
                {
                    continue;
                }

                PointF[] screenPoints = ring.Select(point =>
                {
                    float x = (float)((point.X - _centerX) / metersPerPixel + centerScreenX);
                    float y = (float)(centerScreenY - (point.Y - _centerY) / metersPerPixel);
                    return new PointF(x, y);
                }).ToArray();

                e.Graphics.FillPolygon(fillBrush, screenPoints);
                e.Graphics.DrawPolygon(outlinePen, screenPoints);
            }
        }

        DrawRadiusGuide(e.Graphics, centerScreenX, centerScreenY, metersPerPixel);
        DrawCrosshairAndLabel(e.Graphics, centerScreenX, centerScreenY);
    }

    private void DrawRadiusGuide(Graphics g, float centerX, float centerY, double metersPerPixel)
    {
        float radiusPixels = (float)(FilterRadiusMeters / metersPerPixel);
        if (radiusPixels <= 1)
        {
            return;
        }

        using Pen radiusPen = new(Color.LightGray, 1f)
        {
            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        };
        g.DrawEllipse(radiusPen, centerX - radiusPixels, centerY - radiusPixels, radiusPixels * 2, radiusPixels * 2);
    }

    private void DrawCrosshairAndLabel(Graphics g, float centerX, float centerY)
    {
        using Pen crosshairPen = new(Color.DarkRed, 1.5f);
        g.DrawLine(crosshairPen, centerX - 10, centerY, centerX + 10, centerY);
        g.DrawLine(crosshairPen, centerX, centerY - 10, centerX, centerY + 10);

        string centerLabel = $"{_centerLat:0.000000}, {_centerLon:0.000000}";
        using SolidBrush textBrush = new(Color.DarkRed);
        g.DrawString(centerLabel, SystemFonts.DefaultFont, textBrush, centerX + 12, centerY + 8);
    }

    private static void DrawCenteredText(Graphics graphics, string text, Rectangle bounds)
    {
        using StringFormat format = new()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using SolidBrush brush = new(Color.Gray);
        graphics.DrawString(text, SystemFonts.DefaultFont, brush, bounds, format);
    }

    private void UpdateScaleLabel()
    {
        lblScaleValue.Text = $"1 : {trkScale.Value}";
    }

    private void UpdateBuildingCountLabel()
    {
        lblBuildingCount.Text = $"Buildings in 1000m: {_visibleBuildingsProjected.Count}";
    }

    private bool TryGetCenterFromInputs(out double lat, out double lon)
    {
        bool parsedLat = double.TryParse(txtLat.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
        bool parsedLon = double.TryParse(txtLon.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);
        return parsedLat && parsedLon;
    }

    private static (double X, double Y) ProjectToWebMercator(double lon, double lat)
    {
        double clampedLat = Math.Max(-MaxMercatorLatitude, Math.Min(MaxMercatorLatitude, lat));
        double lonRad = lon * Math.PI / 180.0;
        double latRad = clampedLat * Math.PI / 180.0;
        double x = EarthRadiusMeters * lonRad;
        double y = EarthRadiusMeters * Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0));
        return (x, y);
    }

    private readonly record struct GeoPoint(double Lon, double Lat);
    private readonly record struct MapPoint(double X, double Y);
}
