namespace SuperLotto.Controls
{
    partial class FrmLoading
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
            this.LblMessage = new System.Windows.Forms.Label();
            this.PnlImage = new System.Windows.Forms.Panel();
            this.lblExit = new SuperLotto.Controls.ExtendLabel();
            this.SuspendLayout();
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.BackColor = System.Drawing.Color.Transparent;
            this.LblMessage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblMessage.ForeColor = System.Drawing.Color.White;
            this.LblMessage.Location = new System.Drawing.Point(33, 300);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(574, 64);
            this.LblMessage.TabIndex = 0;
            this.LblMessage.Text = "正在处理中，请稍候……";
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PnlImage
            // 
            this.PnlImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PnlImage.BackColor = System.Drawing.Color.Transparent;
            this.PnlImage.Location = new System.Drawing.Point(258, 145);
            this.PnlImage.Name = "PnlImage";
            this.PnlImage.Size = new System.Drawing.Size(98, 86);
            this.PnlImage.TabIndex = 1;
            this.PnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlImage_Paint);
            this.PnlImage.Resize += new System.EventHandler(this.PnlImage_Resize);
            // 
            // lblExit
            // 
            this.lblExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblExit.AutoSize = true;
            this.lblExit.BackColor = System.Drawing.Color.Transparent;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.EnterColor = System.Drawing.Color.Red;
            this.lblExit.EnterFont = new System.Drawing.Font("微软雅黑", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExit.Fillet = false;
            this.lblExit.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExit.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblExit.Location = new System.Drawing.Point(279, 364);
            this.lblExit.MouseEnterEffect = false;
            this.lblExit.Name = "lblExit";
            this.lblExit.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExit.ShadowWidth = 8;
            this.lblExit.Size = new System.Drawing.Size(48, 26);
            this.lblExit.TabIndex = 620;
            this.lblExit.Text = "exit";
            this.lblExit.Visible = false;
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            // 
            // FrmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(646, 453);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.LblMessage);
            this.Controls.Add(this.PnlImage);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmLoading";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmLoading";
            this.Load += new System.EventHandler(this.FrmLoading_Load);
            this.Shown += new System.EventHandler(this.FrmLoading_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.Panel PnlImage;
        private ExtendLabel lblExit;
    }
}