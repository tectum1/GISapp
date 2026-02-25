using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Footprints.Core;

namespace Footprints.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // Optional defaults
        txtLat.Text = "39.00000";
        txtLon.Text = "-94.00000";
        txtDownloadDir.Text = Directory.GetCurrentDirectory();
        ApiClient.Logger = Log;
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        btnRun.Enabled = false;

        try
        {
            // 1) Read/validate inputs
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

            // 2) Run your pipeline (same as console)
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

            // If extractBF is synchronous:
            ApiClient.extractBF(downloadedFile);
            Log("Extract complete.");
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
        // If you have a multiline TextBox named txtLog:
        txtLog.AppendText(message + Environment.NewLine);
    }
}
