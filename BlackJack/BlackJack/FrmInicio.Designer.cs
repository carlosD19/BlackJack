namespace BlackJack
{
    partial class FrmInicio
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
            this.btnFacebook = new System.Windows.Forms.Button();
            this.btnTwitter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFacebook
            // 
            this.btnFacebook.BackColor = System.Drawing.Color.Transparent;
            this.btnFacebook.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFacebook.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFacebook.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFacebook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacebook.ForeColor = System.Drawing.Color.Transparent;
            this.btnFacebook.Image = global::BlackJack.Properties.Resources.facebook;
            this.btnFacebook.Location = new System.Drawing.Point(255, 183);
            this.btnFacebook.Name = "btnFacebook";
            this.btnFacebook.Size = new System.Drawing.Size(298, 73);
            this.btnFacebook.TabIndex = 0;
            this.btnFacebook.UseVisualStyleBackColor = false;
            this.btnFacebook.Click += new System.EventHandler(this.btnFacebook_Click);
            // 
            // btnTwitter
            // 
            this.btnTwitter.BackColor = System.Drawing.Color.Transparent;
            this.btnTwitter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnTwitter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTwitter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTwitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTwitter.Image = global::BlackJack.Properties.Resources.twitter;
            this.btnTwitter.Location = new System.Drawing.Point(255, 285);
            this.btnTwitter.Name = "btnTwitter";
            this.btnTwitter.Size = new System.Drawing.Size(298, 72);
            this.btnTwitter.TabIndex = 1;
            this.btnTwitter.UseVisualStyleBackColor = false;
            this.btnTwitter.Click += new System.EventHandler(this.btnTwitter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bell MT", 51.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(245, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 77);
            this.label1.TabIndex = 2;
            this.label1.Text = "Black Jack";
            // 
            // FrmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BlackJack.Properties.Resources.inicio;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTwitter);
            this.Controls.Add(this.btnFacebook);
            this.Name = "FrmInicio";
            this.Text = "FrmInicio";
            this.Load += new System.EventHandler(this.FrmInicio_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFacebook;
        private System.Windows.Forms.Button btnTwitter;
        private System.Windows.Forms.Label label1;
    }
}