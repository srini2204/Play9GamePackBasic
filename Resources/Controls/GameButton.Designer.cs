namespace Play9GamePackBasic.Resources.Controls
{
    partial class GameButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pbButton = new System.Windows.Forms.PictureBox();
            this.pbBorder = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBorder)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblDesc.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(7, 286);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(0, 22);
            this.lblDesc.TabIndex = 7;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 242);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(0, 45);
            this.lblHeader.TabIndex = 6;
            // 
            // pbButton
            // 
            this.pbButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbButton.BackColor = System.Drawing.Color.Transparent;
            this.pbButton.Location = new System.Drawing.Point(5, 5);
            this.pbButton.Margin = new System.Windows.Forms.Padding(0);
            this.pbButton.Name = "pbButton";
            this.pbButton.Size = new System.Drawing.Size(399, 232);
            this.pbButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbButton.TabIndex = 5;
            this.pbButton.TabStop = false;
            // 
            // pbBorder
            // 
            this.pbBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBorder.BackColor = System.Drawing.Color.Transparent;
            this.pbBorder.Location = new System.Drawing.Point(0, 0);
            this.pbBorder.Margin = new System.Windows.Forms.Padding(0);
            this.pbBorder.Name = "pbBorder";
            this.pbBorder.Size = new System.Drawing.Size(409, 242);
            this.pbBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBorder.TabIndex = 4;
            this.pbBorder.TabStop = false;
            // 
            // GameButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pbButton);
            this.Controls.Add(this.pbBorder);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GameButton";
            this.Size = new System.Drawing.Size(409, 310);
            ((System.ComponentModel.ISupportInitialize)(this.pbButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBorder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox pbButton;
        private System.Windows.Forms.PictureBox pbBorder;

    }
}
