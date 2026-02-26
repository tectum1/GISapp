using System.ComponentModel;
using System.Windows.Forms;

namespace Footprints.WinForms;

partial class Form1
{
    private IContainer? components = null;

    private TabControl tabControlMain;
    private TabPage tabGeocode;
    private TabPage tabFootprints;

    private Label lblAddress;
    private TextBox txtAddress;
    private Button btnGeocode;
    private Label lblGeoLat;
    private TextBox txtGeoLat;
    private Label lblGeoLon;
    private TextBox txtGeoLon;
    private Button btnUseCoordinates;
    private Label lblGeocodeStatus;

    private Label lblLat;
    private TextBox txtLat;
    private Label lblLon;
    private TextBox txtLon;
    private Label lblDownloadDir;
    private TextBox txtDownloadDir;
    private Button btnBrowseDownloadDir;
    private Button btnRun;
    private Button btnLoadGeoJson;
    private Label lblScale;
    private TrackBar trkScale;
    private Label lblScaleValue;
    private Label lblBuildingCount;
    private Panel pnlViewer;
    private TextBox txtLog;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.tabControlMain = new TabControl();
        this.tabGeocode = new TabPage();
        this.tabFootprints = new TabPage();
        this.lblAddress = new Label();
        this.txtAddress = new TextBox();
        this.btnGeocode = new Button();
        this.lblGeoLat = new Label();
        this.txtGeoLat = new TextBox();
        this.lblGeoLon = new Label();
        this.txtGeoLon = new TextBox();
        this.btnUseCoordinates = new Button();
        this.lblGeocodeStatus = new Label();
        this.lblLat = new Label();
        this.txtLat = new TextBox();
        this.lblLon = new Label();
        this.txtLon = new TextBox();
        this.lblDownloadDir = new Label();
        this.txtDownloadDir = new TextBox();
        this.btnBrowseDownloadDir = new Button();
        this.btnRun = new Button();
        this.btnLoadGeoJson = new Button();
        this.lblScale = new Label();
        this.trkScale = new TrackBar();
        this.lblScaleValue = new Label();
        this.lblBuildingCount = new Label();
        this.pnlViewer = new Panel();
        this.txtLog = new TextBox();
        this.tabControlMain.SuspendLayout();
        this.tabGeocode.SuspendLayout();
        this.tabFootprints.SuspendLayout();
        ((ISupportInitialize)(this.trkScale)).BeginInit();
        this.SuspendLayout();
        // 
        // tabControlMain
        // 
        this.tabControlMain.Controls.Add(this.tabGeocode);
        this.tabControlMain.Controls.Add(this.tabFootprints);
        this.tabControlMain.Location = new System.Drawing.Point(12, 12);
        this.tabControlMain.Name = "tabControlMain";
        this.tabControlMain.SelectedIndex = 0;
        this.tabControlMain.Size = new System.Drawing.Size(916, 596);
        this.tabControlMain.TabIndex = 0;
        // 
        // tabGeocode
        // 
        this.tabGeocode.Controls.Add(this.lblAddress);
        this.tabGeocode.Controls.Add(this.txtAddress);
        this.tabGeocode.Controls.Add(this.btnGeocode);
        this.tabGeocode.Controls.Add(this.lblGeoLat);
        this.tabGeocode.Controls.Add(this.txtGeoLat);
        this.tabGeocode.Controls.Add(this.lblGeoLon);
        this.tabGeocode.Controls.Add(this.txtGeoLon);
        this.tabGeocode.Controls.Add(this.btnUseCoordinates);
        this.tabGeocode.Controls.Add(this.lblGeocodeStatus);
        this.tabGeocode.Location = new System.Drawing.Point(4, 24);
        this.tabGeocode.Name = "tabGeocode";
        this.tabGeocode.Padding = new Padding(3);
        this.tabGeocode.Size = new System.Drawing.Size(908, 568);
        this.tabGeocode.TabIndex = 0;
        this.tabGeocode.Text = "Address -> Lat/Lon";
        this.tabGeocode.UseVisualStyleBackColor = true;
        // 
        // tabFootprints
        // 
        this.tabFootprints.Controls.Add(this.txtLog);
        this.tabFootprints.Controls.Add(this.pnlViewer);
        this.tabFootprints.Controls.Add(this.lblBuildingCount);
        this.tabFootprints.Controls.Add(this.lblScaleValue);
        this.tabFootprints.Controls.Add(this.trkScale);
        this.tabFootprints.Controls.Add(this.lblScale);
        this.tabFootprints.Controls.Add(this.btnLoadGeoJson);
        this.tabFootprints.Controls.Add(this.btnRun);
        this.tabFootprints.Controls.Add(this.btnBrowseDownloadDir);
        this.tabFootprints.Controls.Add(this.txtDownloadDir);
        this.tabFootprints.Controls.Add(this.lblDownloadDir);
        this.tabFootprints.Controls.Add(this.txtLon);
        this.tabFootprints.Controls.Add(this.lblLon);
        this.tabFootprints.Controls.Add(this.txtLat);
        this.tabFootprints.Controls.Add(this.lblLat);
        this.tabFootprints.Location = new System.Drawing.Point(4, 24);
        this.tabFootprints.Name = "tabFootprints";
        this.tabFootprints.Padding = new Padding(3);
        this.tabFootprints.Size = new System.Drawing.Size(908, 568);
        this.tabFootprints.TabIndex = 1;
        this.tabFootprints.Text = "Lat/Lon -> Footprints";
        this.tabFootprints.UseVisualStyleBackColor = true;
        // 
        // lblAddress
        // 
        this.lblAddress.AutoSize = true;
        this.lblAddress.Location = new System.Drawing.Point(20, 24);
        this.lblAddress.Name = "lblAddress";
        this.lblAddress.Size = new System.Drawing.Size(109, 15);
        this.lblAddress.TabIndex = 0;
        this.lblAddress.Text = "Address (one line)";
        // 
        // txtAddress
        // 
        this.txtAddress.Location = new System.Drawing.Point(20, 46);
        this.txtAddress.Name = "txtAddress";
        this.txtAddress.Size = new System.Drawing.Size(760, 23);
        this.txtAddress.TabIndex = 1;
        // 
        // btnGeocode
        // 
        this.btnGeocode.Location = new System.Drawing.Point(794, 45);
        this.btnGeocode.Name = "btnGeocode";
        this.btnGeocode.Size = new System.Drawing.Size(90, 25);
        this.btnGeocode.TabIndex = 2;
        this.btnGeocode.Text = "Geocode";
        this.btnGeocode.UseVisualStyleBackColor = true;
        this.btnGeocode.Click += new System.EventHandler(this.btnGeocode_Click);
        // 
        // lblGeoLat
        // 
        this.lblGeoLat.AutoSize = true;
        this.lblGeoLat.Location = new System.Drawing.Point(20, 90);
        this.lblGeoLat.Name = "lblGeoLat";
        this.lblGeoLat.Size = new System.Drawing.Size(50, 15);
        this.lblGeoLat.TabIndex = 3;
        this.lblGeoLat.Text = "Latitude";
        // 
        // txtGeoLat
        // 
        this.txtGeoLat.Location = new System.Drawing.Point(20, 112);
        this.txtGeoLat.Name = "txtGeoLat";
        this.txtGeoLat.ReadOnly = true;
        this.txtGeoLat.Size = new System.Drawing.Size(170, 23);
        this.txtGeoLat.TabIndex = 4;
        // 
        // lblGeoLon
        // 
        this.lblGeoLon.AutoSize = true;
        this.lblGeoLon.Location = new System.Drawing.Point(210, 90);
        this.lblGeoLon.Name = "lblGeoLon";
        this.lblGeoLon.Size = new System.Drawing.Size(59, 15);
        this.lblGeoLon.TabIndex = 5;
        this.lblGeoLon.Text = "Longitude";
        // 
        // txtGeoLon
        // 
        this.txtGeoLon.Location = new System.Drawing.Point(210, 112);
        this.txtGeoLon.Name = "txtGeoLon";
        this.txtGeoLon.ReadOnly = true;
        this.txtGeoLon.Size = new System.Drawing.Size(170, 23);
        this.txtGeoLon.TabIndex = 6;
        // 
        // btnUseCoordinates
        // 
        this.btnUseCoordinates.Location = new System.Drawing.Point(20, 150);
        this.btnUseCoordinates.Name = "btnUseCoordinates";
        this.btnUseCoordinates.Size = new System.Drawing.Size(180, 25);
        this.btnUseCoordinates.TabIndex = 7;
        this.btnUseCoordinates.Text = "Use In Footprints Tab";
        this.btnUseCoordinates.UseVisualStyleBackColor = true;
        this.btnUseCoordinates.Click += new System.EventHandler(this.btnUseCoordinates_Click);
        // 
        // lblGeocodeStatus
        // 
        this.lblGeocodeStatus.BorderStyle = BorderStyle.FixedSingle;
        this.lblGeocodeStatus.Location = new System.Drawing.Point(20, 191);
        this.lblGeocodeStatus.Name = "lblGeocodeStatus";
        this.lblGeocodeStatus.Size = new System.Drawing.Size(864, 88);
        this.lblGeocodeStatus.TabIndex = 8;
        this.lblGeocodeStatus.Text = "Status: idle";
        // 
        // lblLat
        // 
        this.lblLat.AutoSize = true;
        this.lblLat.Location = new System.Drawing.Point(16, 18);
        this.lblLat.Name = "lblLat";
        this.lblLat.Size = new System.Drawing.Size(48, 15);
        this.lblLat.TabIndex = 0;
        this.lblLat.Text = "Latitude";
        // 
        // txtLat
        // 
        this.txtLat.Location = new System.Drawing.Point(112, 15);
        this.txtLat.Name = "txtLat";
        this.txtLat.Size = new System.Drawing.Size(220, 23);
        this.txtLat.TabIndex = 1;
        // 
        // lblLon
        // 
        this.lblLon.AutoSize = true;
        this.lblLon.Location = new System.Drawing.Point(16, 51);
        this.lblLon.Name = "lblLon";
        this.lblLon.Size = new System.Drawing.Size(57, 15);
        this.lblLon.TabIndex = 2;
        this.lblLon.Text = "Longitude";
        // 
        // txtLon
        // 
        this.txtLon.Location = new System.Drawing.Point(112, 48);
        this.txtLon.Name = "txtLon";
        this.txtLon.Size = new System.Drawing.Size(220, 23);
        this.txtLon.TabIndex = 3;
        // 
        // lblDownloadDir
        // 
        this.lblDownloadDir.AutoSize = true;
        this.lblDownloadDir.Location = new System.Drawing.Point(16, 84);
        this.lblDownloadDir.Name = "lblDownloadDir";
        this.lblDownloadDir.Size = new System.Drawing.Size(84, 15);
        this.lblDownloadDir.TabIndex = 4;
        this.lblDownloadDir.Text = "Download Dir";
        // 
        // txtDownloadDir
        // 
        this.txtDownloadDir.Location = new System.Drawing.Point(112, 81);
        this.txtDownloadDir.Name = "txtDownloadDir";
        this.txtDownloadDir.Size = new System.Drawing.Size(520, 23);
        this.txtDownloadDir.TabIndex = 5;
        // 
        // btnBrowseDownloadDir
        // 
        this.btnBrowseDownloadDir.Location = new System.Drawing.Point(640, 80);
        this.btnBrowseDownloadDir.Name = "btnBrowseDownloadDir";
        this.btnBrowseDownloadDir.Size = new System.Drawing.Size(110, 25);
        this.btnBrowseDownloadDir.TabIndex = 6;
        this.btnBrowseDownloadDir.Text = "Browse...";
        this.btnBrowseDownloadDir.UseVisualStyleBackColor = true;
        this.btnBrowseDownloadDir.Click += new System.EventHandler(this.btnBrowseDownloadDir_Click);
        // 
        // btnRun
        // 
        this.btnRun.Location = new System.Drawing.Point(760, 80);
        this.btnRun.Name = "btnRun";
        this.btnRun.Size = new System.Drawing.Size(120, 25);
        this.btnRun.TabIndex = 7;
        this.btnRun.Text = "Run";
        this.btnRun.UseVisualStyleBackColor = true;
        this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
        // 
        // btnLoadGeoJson
        // 
        this.btnLoadGeoJson.Location = new System.Drawing.Point(16, 120);
        this.btnLoadGeoJson.Name = "btnLoadGeoJson";
        this.btnLoadGeoJson.Size = new System.Drawing.Size(120, 25);
        this.btnLoadGeoJson.TabIndex = 8;
        this.btnLoadGeoJson.Text = "Load GeoJSON";
        this.btnLoadGeoJson.UseVisualStyleBackColor = true;
        this.btnLoadGeoJson.Click += new System.EventHandler(this.btnLoadGeoJson_Click);
        // 
        // lblScale
        // 
        this.lblScale.AutoSize = true;
        this.lblScale.Location = new System.Drawing.Point(160, 125);
        this.lblScale.Name = "lblScale";
        this.lblScale.Size = new System.Drawing.Size(34, 15);
        this.lblScale.TabIndex = 9;
        this.lblScale.Text = "Scale";
        // 
        // trkScale
        // 
        this.trkScale.Location = new System.Drawing.Point(200, 117);
        this.trkScale.Maximum = 10000;
        this.trkScale.Minimum = 500;
        this.trkScale.Name = "trkScale";
        this.trkScale.Size = new System.Drawing.Size(210, 45);
        this.trkScale.SmallChange = 100;
        this.trkScale.LargeChange = 500;
        this.trkScale.TabIndex = 10;
        this.trkScale.TickFrequency = 500;
        this.trkScale.Value = 1000;
        this.trkScale.Scroll += new System.EventHandler(this.trkScale_Scroll);
        // 
        // lblScaleValue
        // 
        this.lblScaleValue.AutoSize = true;
        this.lblScaleValue.Location = new System.Drawing.Point(420, 125);
        this.lblScaleValue.Name = "lblScaleValue";
        this.lblScaleValue.Size = new System.Drawing.Size(53, 15);
        this.lblScaleValue.TabIndex = 11;
        this.lblScaleValue.Text = "1 : 1000";
        // 
        // lblBuildingCount
        // 
        this.lblBuildingCount.AutoSize = true;
        this.lblBuildingCount.Location = new System.Drawing.Point(520, 125);
        this.lblBuildingCount.Name = "lblBuildingCount";
        this.lblBuildingCount.Size = new System.Drawing.Size(122, 15);
        this.lblBuildingCount.TabIndex = 12;
        this.lblBuildingCount.Text = "Buildings in 1000m: 0";
        // 
        // pnlViewer
        // 
        this.pnlViewer.BackColor = System.Drawing.Color.White;
        this.pnlViewer.BorderStyle = BorderStyle.FixedSingle;
        this.pnlViewer.Location = new System.Drawing.Point(16, 155);
        this.pnlViewer.Name = "pnlViewer";
        this.pnlViewer.Size = new System.Drawing.Size(864, 260);
        this.pnlViewer.TabIndex = 13;
        this.pnlViewer.Paint += new PaintEventHandler(this.pnlViewer_Paint);
        // 
        // txtLog
        // 
        this.txtLog.Location = new System.Drawing.Point(16, 425);
        this.txtLog.Multiline = true;
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = ScrollBars.Vertical;
        this.txtLog.Size = new System.Drawing.Size(864, 120);
        this.txtLog.TabIndex = 14;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(940, 620);
        this.Controls.Add(this.tabControlMain);
        this.Name = "Form1";
        this.Text = "Footprints Tool";
        this.tabControlMain.ResumeLayout(false);
        this.tabGeocode.ResumeLayout(false);
        this.tabGeocode.PerformLayout();
        this.tabFootprints.ResumeLayout(false);
        this.tabFootprints.PerformLayout();
        ((ISupportInitialize)(this.trkScale)).EndInit();
        this.ResumeLayout(false);
    }
}
