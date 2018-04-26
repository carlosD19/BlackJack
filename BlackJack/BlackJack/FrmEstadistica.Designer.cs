namespace BlackJack
{
    partial class FrmEstadistica
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chartEstadistica = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartEstadistica)).BeginInit();
            this.SuspendLayout();
            // 
            // chartEstadistica
            // 
            chartArea1.Name = "ChartArea1";
            this.chartEstadistica.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartEstadistica.Legends.Add(legend1);
            this.chartEstadistica.Location = new System.Drawing.Point(13, 13);
            this.chartEstadistica.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartEstadistica.Name = "chartEstadistica";
            this.chartEstadistica.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.chartEstadistica.Size = new System.Drawing.Size(727, 428);
            this.chartEstadistica.TabIndex = 0;
            this.chartEstadistica.Text = "chart1";
            // 
            // FrmEstadistica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 454);
            this.Controls.Add(this.chartEstadistica);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmEstadistica";
            this.Text = "FrmEstadistica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEstadistica_FormClosing);
            this.Load += new System.EventHandler(this.FrmEstadistica_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartEstadistica)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartEstadistica;
    }
}