using System.ComponentModel;
using System.Windows.Forms;

namespace Footprints.WinForms;

partial class Form1
{
    private TextBox txtLat;
    private TextBox txtLon;
    private TextBox txtDownloadDir;
    private Button btnRun;
    private Button btnLoadGeoJson;
    private Button btnPrev;
    private Button btnPlayPause;
    private Button btnNext;
    private TextBox txtLog;
    private Label lblLat;
    private Label lblLon;
    private Label lblDownloadDir;
    private Label lblFeatureIndex;
    private Panel pnlViewer;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer? components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.txtLat = new TextBox();
        this.txtLon = new TextBox();
        this.txtDownloadDir = new TextBox();
        this.btnRun = new Button();
        this.btnLoadGeoJson = new Button();
        this.btnPrev = new Button();
        this.btnPlayPause = new Button();
        this.btnNext = new Button();
        this.txtLog = new TextBox();
        this.lblLat = new Label();
        this.lblLon = new Label();
        this.lblDownloadDir = new Label();
        this.lblFeatureIndex = new Label();
        this.pnlViewer = new Panel();
        this.SuspendLayout();
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
        this.txtDownloadDir.Size = new System.Drawing.Size(640, 23);
        this.txtDownloadDir.TabIndex = 5;
        // 
        // btnRun
        // 
        this.btnRun.Location = new System.Drawing.Point(770, 80);
        this.btnRun.Name = "btnRun";
        this.btnRun.Size = new System.Drawing.Size(120, 25);
        this.btnRun.TabIndex = 6;
        this.btnRun.Text = "Run";
        this.btnRun.UseVisualStyleBackColor = true;
        this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
        // 
        // btnLoadGeoJson
        // 
        this.btnLoadGeoJson.Location = new System.Drawing.Point(16, 120);
        this.btnLoadGeoJson.Name = "btnLoadGeoJson";
        this.btnLoadGeoJson.Size = new System.Drawing.Size(120, 25);
        this.btnLoadGeoJson.TabIndex = 7;
        this.btnLoadGeoJson.Text = "Load GeoJSON";
        this.btnLoadGeoJson.UseVisualStyleBackColor = true;
        this.btnLoadGeoJson.Click += new System.EventHandler(this.btnLoadGeoJson_Click);
        // 
        // btnPrev
        // 
        this.btnPrev.Location = new System.Drawing.Point(150, 120);
        this.btnPrev.Name = "btnPrev";
        this.btnPrev.Size = new System.Drawing.Size(70, 25);
        this.btnPrev.TabIndex = 8;
        this.btnPrev.Text = "Prev";
        this.btnPrev.UseVisualStyleBackColor = true;
        this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
        // 
        // btnPlayPause
        // 
        this.btnPlayPause.Location = new System.Drawing.Point(230, 120);
        this.btnPlayPause.Name = "btnPlayPause";
        this.btnPlayPause.Size = new System.Drawing.Size(90, 25);
        this.btnPlayPause.TabIndex = 9;
        this.btnPlayPause.Text = "Play";
        this.btnPlayPause.UseVisualStyleBackColor = true;
        this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
        // 
        // btnNext
        // 
        this.btnNext.Location = new System.Drawing.Point(330, 120);
        this.btnNext.Name = "btnNext";
        this.btnNext.Size = new System.Drawing.Size(70, 25);
        this.btnNext.TabIndex = 10;
        this.btnNext.Text = "Next";
        this.btnNext.UseVisualStyleBackColor = true;
        this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
        // 
        // lblFeatureIndex
        // 
        this.lblFeatureIndex.AutoSize = true;
        this.lblFeatureIndex.Location = new System.Drawing.Point(420, 125);
        this.lblFeatureIndex.Name = "lblFeatureIndex";
        this.lblFeatureIndex.Size = new System.Drawing.Size(79, 15);
        this.lblFeatureIndex.TabIndex = 11;
        this.lblFeatureIndex.Text = "Feature: 0 / 0";
        // 
        // pnlViewer
        // 
        this.pnlViewer.BackColor = System.Drawing.Color.White;
        this.pnlViewer.BorderStyle = BorderStyle.FixedSingle;
        this.pnlViewer.Location = new System.Drawing.Point(16, 155);
        this.pnlViewer.Name = "pnlViewer";
        this.pnlViewer.Size = new System.Drawing.Size(874, 230);
        this.pnlViewer.TabIndex = 12;
        this.pnlViewer.Paint += new PaintEventHandler(this.pnlViewer_Paint);
        // 
        // txtLog
        // 
        this.txtLog.Location = new System.Drawing.Point(16, 395);
        this.txtLog.Multiline = true;
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = ScrollBars.Vertical;
        this.txtLog.Size = new System.Drawing.Size(874, 145);
        this.txtLog.TabIndex = 13;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(920, 560);
        this.Controls.Add(this.pnlViewer);
        this.Controls.Add(this.lblFeatureIndex);
        this.Controls.Add(this.btnNext);
        this.Controls.Add(this.btnPlayPause);
        this.Controls.Add(this.btnPrev);
        this.Controls.Add(this.btnLoadGeoJson);
        this.Controls.Add(this.txtLog);
        this.Controls.Add(this.btnRun);
        this.Controls.Add(this.txtDownloadDir);
        this.Controls.Add(this.lblDownloadDir);
        this.Controls.Add(this.txtLon);
        this.Controls.Add(this.lblLon);
        this.Controls.Add(this.txtLat);
        this.Controls.Add(this.lblLat);
        this.Name = "Form1";
        this.Text = "Footprints Downloader";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
