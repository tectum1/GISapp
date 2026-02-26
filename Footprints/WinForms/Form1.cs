using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Footprints.Core;

namespace Footprints.WinForms;

public partial class Form1 : Form
{
    private readonly List<List<PointF>> _features = [];
    private readonly System.Windows.Forms.Timer _slideshowTimer = new();
    private int _currentFeatureIndex = -1;
    private bool _isPlaying;

    public Form1()
    {
        InitializeComponent();

        txtLat.Text = "39.00000";
        txtLon.Text = "-94.00000";
        txtDownloadDir.Text = Directory.GetCurrentDirectory();
        ApiClient.Logger = Log;

        _slideshowTimer.Interval = 800;
        _slideshowTimer.Tick += (_, _) => ShowNextFeature();
        UpdateFeatureLabel();
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        btnRun.Enabled = false;

        try
        {
            if (!double.TryParse(txtLat.Text, out double lat))
            {
                MessageBox.Show("Invalid latitude.");
                return;
            }

            if (!double.TryParse(txtLon.Text, out double lon))
            {
                MessageBox.Show("Invalid longitude.");
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

            string downloadURL = ApiClient.quadkeyToUrl(quadKey);
            Log($"downloadURL: {downloadURL}");

            string downloadedFile = await ApiClient.downloadBuildingFootprints(downloadURL, downloadDir, quadKey);

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
                LoadGeoJson(extractedGeoJson);
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

    private void btnLoadGeoJson_Click(object sender, EventArgs e)
    {
        using OpenFileDialog dialog = new();
        dialog.Filter = "GeoJSON files (*.geojson)|*.geojson|JSON files (*.json)|*.json|All files (*.*)|*.*";
        dialog.Title = "Select GeoJSON file";
        dialog.InitialDirectory = Directory.Exists(txtDownloadDir.Text) ? txtDownloadDir.Text : Directory.GetCurrentDirectory();

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LoadGeoJson(dialog.FileName);
        }
    }

    private void btnPrev_Click(object sender, EventArgs e)
    {
        if (_features.Count == 0)
        {
            return;
        }

        _currentFeatureIndex = (_currentFeatureIndex - 1 + _features.Count) % _features.Count;
        UpdateFeatureLabel();
        pnlViewer.Invalidate();
    }

    private void btnPlayPause_Click(object sender, EventArgs e)
    {
        if (_features.Count == 0)
        {
            return;
        }

        _isPlaying = !_isPlaying;
        btnPlayPause.Text = _isPlaying ? "Pause" : "Play";

        if (_isPlaying)
        {
            _slideshowTimer.Start();
        }
        else
        {
            _slideshowTimer.Stop();
        }
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        ShowNextFeature();
    }

    private void ShowNextFeature()
    {
        if (_features.Count == 0)
        {
            return;
        }

        _currentFeatureIndex = (_currentFeatureIndex + 1) % _features.Count;
        UpdateFeatureLabel();
        pnlViewer.Invalidate();
    }

    private void LoadGeoJson(string path)
    {
        try
        {
            string json = File.ReadAllText(path).TrimStart('\uFEFF').Trim();
            List<List<PointF>> parsed = ParseFeatures(json);

            _features.Clear();
            _features.AddRange(parsed);

            _currentFeatureIndex = _features.Count > 0 ? 0 : -1;
            _isPlaying = false;
            _slideshowTimer.Stop();
            btnPlayPause.Text = "Play";
            UpdateFeatureLabel();
            pnlViewer.Invalidate();

            Log($"Loaded {_features.Count} features from: {path}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load GeoJSON: {ex.Message}", "GeoJSON Error");
        }
    }

    private List<List<PointF>> ParseFeatures(string json)
    {
        List<List<PointF>> result = [];

        if (TryParseSingleJson(json, result))
        {
            return result;
        }

        // Fallback for NDJSON / JSONL where each line is a Feature object.
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
                // Skip malformed lines and continue loading remaining features.
            }
        }

        return result;
    }

    private static bool TryParseSingleJson(string json, List<List<PointF>> destination)
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

    private static void ExtractFromRoot(JsonElement root, List<List<PointF>> destination)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (root.TryGetProperty("type", out JsonElement rootType) && rootType.ValueKind == JsonValueKind.String)
        {
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
    }

    private static void ExtractFeatureGeometry(JsonElement feature, List<List<PointF>> destination)
    {
        if (!feature.TryGetProperty("geometry", out JsonElement geometry) ||
            geometry.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (!geometry.TryGetProperty("type", out JsonElement typeElement) ||
            typeElement.ValueKind != JsonValueKind.String)
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
            AddPolygon(coords, destination);
        }
        else if (geometryType == "MultiPolygon")
        {
            foreach (JsonElement polygonCoords in coords.EnumerateArray())
            {
                AddPolygon(polygonCoords, destination);
            }
        }
    }

    private static void AddPolygon(JsonElement polygonCoords, List<List<PointF>> destination)
    {
        if (polygonCoords.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (JsonElement ring in polygonCoords.EnumerateArray())
        {
            if (ring.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            List<PointF> points = [];
            foreach (JsonElement coord in ring.EnumerateArray())
            {
                if (coord.ValueKind != JsonValueKind.Array || coord.GetArrayLength() < 2)
                {
                    continue;
                }

                float lon = (float)coord[0].GetDouble();
                float lat = (float)coord[1].GetDouble();
                points.Add(new PointF(lon, lat));
            }

            if (points.Count >= 3)
            {
                destination.Add(points);
            }
        }
    }

    private void pnlViewer_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(Color.White);

        if (_currentFeatureIndex < 0 || _currentFeatureIndex >= _features.Count)
        {
            DrawCenteredText(e.Graphics, "Load a GeoJSON file to begin.", pnlViewer.ClientRectangle);
            return;
        }

        List<PointF> feature = _features[_currentFeatureIndex];
        if (feature.Count < 3)
        {
            return;
        }

        float minX = feature.Min(p => p.X);
        float maxX = feature.Max(p => p.X);
        float minY = feature.Min(p => p.Y);
        float maxY = feature.Max(p => p.Y);

        RectangleF viewport = pnlViewer.ClientRectangle;
        float width = Math.Max(maxX - minX, 0.0001f);
        float height = Math.Max(maxY - minY, 0.0001f);
        float padding = 12f;

        float scaleX = (viewport.Width - 2 * padding) / width;
        float scaleY = (viewport.Height - 2 * padding) / height;
        float scale = Math.Min(scaleX, scaleY);

        PointF[] screenPoints = feature.Select(p =>
        {
            float x = padding + (p.X - minX) * scale;
            float y = padding + (maxY - p.Y) * scale;
            return new PointF(x, y);
        }).ToArray();

        using SolidBrush fillBrush = new(Color.FromArgb(64, 0, 120, 215));
        using Pen outlinePen = new(Color.FromArgb(0, 80, 150), 1.5f);

        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.FillPolygon(fillBrush, screenPoints);
        e.Graphics.DrawPolygon(outlinePen, screenPoints);
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

    private void UpdateFeatureLabel()
    {
        if (_features.Count == 0 || _currentFeatureIndex < 0)
        {
            lblFeatureIndex.Text = "Feature: 0 / 0";
            return;
        }

        lblFeatureIndex.Text = $"Feature: {_currentFeatureIndex + 1} / {_features.Count}";
    }
}
