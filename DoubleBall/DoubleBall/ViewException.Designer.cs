namespace DoubleBalls
{
    partial class ViewException
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewException));
            this.panBody = new System.Windows.Forms.Panel();
            this.extendLabel7 = new DoubleBalls.Controls.ExtendLabel();
            this.lblExceptionMessage = new DoubleBalls.Controls.ExtendLabel();
            this.lblExceptionMethod = new DoubleBalls.Controls.ExtendLabel();
            this.lblExceptionType = new DoubleBalls.Controls.ExtendLabel();
            this.lblExceptionSource = new DoubleBalls.Controls.ExtendLabel();
            this.lblExceptionDate = new DoubleBalls.Controls.ExtendLabel();
            this.label13 = new System.Windows.Forms.Label();
            this.extendLabel5 = new DoubleBalls.Controls.ExtendLabel();
            this.extendLabel4 = new DoubleBalls.Controls.ExtendLabel();
            this.extendLabel3 = new DoubleBalls.Controls.ExtendLabel();
            this.extendLabel2 = new DoubleBalls.Controls.ExtendLabel();
            this.extendLabel1 = new DoubleBalls.Controls.ExtendLabel();
            this.callStack = new System.Windows.Forms.RichTextBox();
            this.extendLabel6 = new DoubleBalls.Controls.ExtendLabel();
            this.lblClose = new DoubleBalls.Controls.LeeLabel();
            this.lblMin = new DoubleBalls.Controls.LeeLabel();
            this.lblMaxUndo = new DoubleBalls.Controls.LeeLabel();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panBody.BackColor = System.Drawing.Color.White;
            this.panBody.Controls.Add(this.extendLabel7);
            this.panBody.Controls.Add(this.pictureBox1);
            this.panBody.Controls.Add(this.lblExceptionMessage);
            this.panBody.Controls.Add(this.lblExceptionMethod);
            this.panBody.Controls.Add(this.lblExceptionType);
            this.panBody.Controls.Add(this.lblExceptionSource);
            this.panBody.Controls.Add(this.lblExceptionDate);
            this.panBody.Controls.Add(this.label13);
            this.panBody.Controls.Add(this.extendLabel5);
            this.panBody.Controls.Add(this.extendLabel4);
            this.panBody.Controls.Add(this.extendLabel3);
            this.panBody.Controls.Add(this.extendLabel2);
            this.panBody.Controls.Add(this.extendLabel1);
            this.panBody.Controls.Add(this.callStack);
            this.panBody.Location = new System.Drawing.Point(0, 41);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(868, 555);
            this.panBody.TabIndex = 0;
            // 
            // extendLabel7
            // 
            this.extendLabel7.AutoSize = true;
            this.extendLabel7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel7.EnterColor = System.Drawing.Color.Black;
            this.extendLabel7.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel7.Fillet = false;
            this.extendLabel7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel7.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel7.Location = new System.Drawing.Point(62, 11);
            this.extendLabel7.MouseEnterEffect = false;
            this.extendLabel7.Name = "extendLabel7";
            this.extendLabel7.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel7.ShadowWidth = 8;
            this.extendLabel7.Size = new System.Drawing.Size(68, 17);
            this.extendLabel7.TabIndex = 619;
            this.extendLabel7.Text = "发生日期：";
            // 
            // lblExceptionMessage
            // 
            this.lblExceptionMessage.AutoSize = true;
            this.lblExceptionMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExceptionMessage.EnterColor = System.Drawing.Color.OrangeRed;
            this.lblExceptionMessage.EnterFont = new System.Drawing.Font("微软雅黑", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionMessage.Fillet = false;
            this.lblExceptionMessage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionMessage.ForeColor = System.Drawing.Color.DimGray;
            this.lblExceptionMessage.Location = new System.Drawing.Point(131, 147);
            this.lblExceptionMessage.MouseEnterEffect = false;
            this.lblExceptionMessage.Name = "lblExceptionMessage";
            this.lblExceptionMessage.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExceptionMessage.ShadowWidth = 8;
            this.lblExceptionMessage.Size = new System.Drawing.Size(587, 17);
            this.lblExceptionMessage.TabIndex = 657;
            this.lblExceptionMessage.Text = "未能找到文件“D:\\GitSource\\LotterySource\\SuperLotto\\SuperLotto\\bin\\Debug\\explain.data”。";
            // 
            // lblExceptionMethod
            // 
            this.lblExceptionMethod.AutoSize = true;
            this.lblExceptionMethod.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExceptionMethod.EnterColor = System.Drawing.Color.OrangeRed;
            this.lblExceptionMethod.EnterFont = new System.Drawing.Font("微软雅黑", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionMethod.Fillet = false;
            this.lblExceptionMethod.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionMethod.ForeColor = System.Drawing.Color.DimGray;
            this.lblExceptionMethod.Location = new System.Drawing.Point(131, 114);
            this.lblExceptionMethod.MouseEnterEffect = false;
            this.lblExceptionMethod.Name = "lblExceptionMethod";
            this.lblExceptionMethod.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExceptionMethod.ShadowWidth = 8;
            this.lblExceptionMethod.Size = new System.Drawing.Size(242, 17);
            this.lblExceptionMethod.TabIndex = 656;
            this.lblExceptionMethod.Text = "Void WinIOError(Int32, System.String)";
            // 
            // lblExceptionType
            // 
            this.lblExceptionType.AutoSize = true;
            this.lblExceptionType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExceptionType.EnterColor = System.Drawing.Color.OrangeRed;
            this.lblExceptionType.EnterFont = new System.Drawing.Font("微软雅黑", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionType.Fillet = false;
            this.lblExceptionType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionType.ForeColor = System.Drawing.Color.DimGray;
            this.lblExceptionType.Location = new System.Drawing.Point(131, 80);
            this.lblExceptionType.MouseEnterEffect = false;
            this.lblExceptionType.Name = "lblExceptionType";
            this.lblExceptionType.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExceptionType.ShadowWidth = 8;
            this.lblExceptionType.Size = new System.Drawing.Size(146, 17);
            this.lblExceptionType.TabIndex = 655;
            this.lblExceptionType.Text = "System.IO.IOException";
            // 
            // lblExceptionSource
            // 
            this.lblExceptionSource.AutoSize = true;
            this.lblExceptionSource.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExceptionSource.EnterColor = System.Drawing.Color.OrangeRed;
            this.lblExceptionSource.EnterFont = new System.Drawing.Font("微软雅黑", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionSource.Fillet = false;
            this.lblExceptionSource.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionSource.ForeColor = System.Drawing.Color.DimGray;
            this.lblExceptionSource.Location = new System.Drawing.Point(131, 46);
            this.lblExceptionSource.MouseEnterEffect = false;
            this.lblExceptionSource.Name = "lblExceptionSource";
            this.lblExceptionSource.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExceptionSource.ShadowWidth = 8;
            this.lblExceptionSource.Size = new System.Drawing.Size(153, 17);
            this.lblExceptionSource.TabIndex = 654;
            this.lblExceptionSource.Text = "System.Windows.Forms";
            // 
            // lblExceptionDate
            // 
            this.lblExceptionDate.AutoSize = true;
            this.lblExceptionDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExceptionDate.EnterColor = System.Drawing.Color.OrangeRed;
            this.lblExceptionDate.EnterFont = new System.Drawing.Font("微软雅黑", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionDate.Fillet = false;
            this.lblExceptionDate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExceptionDate.ForeColor = System.Drawing.Color.DimGray;
            this.lblExceptionDate.Location = new System.Drawing.Point(131, 11);
            this.lblExceptionDate.MouseEnterEffect = false;
            this.lblExceptionDate.Name = "lblExceptionDate";
            this.lblExceptionDate.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExceptionDate.ShadowWidth = 8;
            this.lblExceptionDate.Size = new System.Drawing.Size(126, 17);
            this.lblExceptionDate.TabIndex = 653;
            this.lblExceptionDate.Text = "2024-11-17 20:01:19";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Gainsboro;
            this.label13.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.label13.Enabled = false;
            this.label13.ForeColor = System.Drawing.Color.Gainsboro;
            this.label13.Location = new System.Drawing.Point(63, 205);
            this.label13.MaximumSize = new System.Drawing.Size(0, 1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(449, 1);
            this.label13.TabIndex = 651;
            this.label13.Text = "__________________________________________________________________________";
            // 
            // extendLabel5
            // 
            this.extendLabel5.AutoSize = true;
            this.extendLabel5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel5.EnterColor = System.Drawing.Color.Black;
            this.extendLabel5.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel5.Fillet = false;
            this.extendLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel5.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel5.Location = new System.Drawing.Point(62, 181);
            this.extendLabel5.MouseEnterEffect = false;
            this.extendLabel5.Name = "extendLabel5";
            this.extendLabel5.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel5.ShadowWidth = 8;
            this.extendLabel5.Size = new System.Drawing.Size(68, 17);
            this.extendLabel5.TabIndex = 624;
            this.extendLabel5.Text = "调用堆栈：";
            // 
            // extendLabel4
            // 
            this.extendLabel4.AutoSize = true;
            this.extendLabel4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel4.EnterColor = System.Drawing.Color.Black;
            this.extendLabel4.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel4.Fillet = false;
            this.extendLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel4.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel4.Location = new System.Drawing.Point(62, 147);
            this.extendLabel4.MouseEnterEffect = false;
            this.extendLabel4.Name = "extendLabel4";
            this.extendLabel4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel4.ShadowWidth = 8;
            this.extendLabel4.Size = new System.Drawing.Size(68, 17);
            this.extendLabel4.TabIndex = 623;
            this.extendLabel4.Text = "异常消息：";
            // 
            // extendLabel3
            // 
            this.extendLabel3.AutoSize = true;
            this.extendLabel3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel3.EnterColor = System.Drawing.Color.Black;
            this.extendLabel3.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel3.Fillet = false;
            this.extendLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel3.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel3.Location = new System.Drawing.Point(62, 114);
            this.extendLabel3.MouseEnterEffect = false;
            this.extendLabel3.Name = "extendLabel3";
            this.extendLabel3.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel3.ShadowWidth = 8;
            this.extendLabel3.Size = new System.Drawing.Size(68, 17);
            this.extendLabel3.TabIndex = 622;
            this.extendLabel3.Text = "异常函数：";
            // 
            // extendLabel2
            // 
            this.extendLabel2.AutoSize = true;
            this.extendLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel2.EnterColor = System.Drawing.Color.Black;
            this.extendLabel2.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel2.Fillet = false;
            this.extendLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel2.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel2.Location = new System.Drawing.Point(62, 80);
            this.extendLabel2.MouseEnterEffect = false;
            this.extendLabel2.Name = "extendLabel2";
            this.extendLabel2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel2.ShadowWidth = 8;
            this.extendLabel2.Size = new System.Drawing.Size(68, 17);
            this.extendLabel2.TabIndex = 621;
            this.extendLabel2.Text = "异常类型：";
            // 
            // extendLabel1
            // 
            this.extendLabel1.AutoSize = true;
            this.extendLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel1.EnterColor = System.Drawing.Color.Black;
            this.extendLabel1.EnterFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extendLabel1.Fillet = false;
            this.extendLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel1.ForeColor = System.Drawing.Color.DimGray;
            this.extendLabel1.Location = new System.Drawing.Point(62, 46);
            this.extendLabel1.MouseEnterEffect = false;
            this.extendLabel1.Name = "extendLabel1";
            this.extendLabel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel1.ShadowWidth = 8;
            this.extendLabel1.Size = new System.Drawing.Size(68, 17);
            this.extendLabel1.TabIndex = 620;
            this.extendLabel1.Text = "异常对象：";
            // 
            // callStack
            // 
            this.callStack.AcceptsTab = true;
            this.callStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.callStack.BackColor = System.Drawing.Color.White;
            this.callStack.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.callStack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.callStack.EnableAutoDragDrop = true;
            this.callStack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.callStack.ForeColor = System.Drawing.Color.LightCoral;
            this.callStack.Location = new System.Drawing.Point(63, 209);
            this.callStack.MaxLength = 0;
            this.callStack.Name = "callStack";
            this.callStack.ReadOnly = true;
            this.callStack.Size = new System.Drawing.Size(804, 346);
            this.callStack.TabIndex = 652;
            this.callStack.Text = resources.GetString("callStack.Text");
            this.callStack.WordWrap = false;
            this.callStack.MouseEnter += new System.EventHandler(this.callStack_MouseEnter);
            this.callStack.MouseLeave += new System.EventHandler(this.callStack_MouseLeave);
            // 
            // extendLabel6
            // 
            this.extendLabel6.AutoSize = true;
            this.extendLabel6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.extendLabel6.EnterColor = System.Drawing.Color.Black;
            this.extendLabel6.EnterFont = new System.Drawing.Font("微软雅黑", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.extendLabel6.Fillet = false;
            this.extendLabel6.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.extendLabel6.ForeColor = System.Drawing.Color.Gray;
            this.extendLabel6.Location = new System.Drawing.Point(40, 13);
            this.extendLabel6.MouseEnterEffect = false;
            this.extendLabel6.Name = "extendLabel6";
            this.extendLabel6.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.extendLabel6.ShadowWidth = 8;
            this.extendLabel6.Size = new System.Drawing.Size(67, 16);
            this.extendLabel6.TabIndex = 628;
            this.extendLabel6.Text = "-> 异常详情";
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.AutoSize = true;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.EnterColor = System.Drawing.Color.Red;
            this.lblClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblClose.ForeColor = System.Drawing.Color.DarkSalmon;
            this.lblClose.Location = new System.Drawing.Point(823, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(41, 17);
            this.lblClose.TabIndex = 629;
            this.lblClose.Tag = "0";
            this.lblClose.Text = "Close";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lblMin
            // 
            this.lblMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMin.AutoSize = true;
            this.lblMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMin.EnterColor = System.Drawing.Color.Red;
            this.lblMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMin.ForeColor = System.Drawing.Color.DarkSalmon;
            this.lblMin.Location = new System.Drawing.Point(757, 3);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(32, 17);
            this.lblMin.TabIndex = 630;
            this.lblMin.Tag = "0";
            this.lblMin.Text = "Min";
            this.lblMin.Click += new System.EventHandler(this.lblMin_Click);
            // 
            // lblMaxUndo
            // 
            this.lblMaxUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxUndo.AutoSize = true;
            this.lblMaxUndo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMaxUndo.EnterColor = System.Drawing.Color.Red;
            this.lblMaxUndo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaxUndo.ForeColor = System.Drawing.Color.DarkSalmon;
            this.lblMaxUndo.Location = new System.Drawing.Point(789, 3);
            this.lblMaxUndo.Name = "lblMaxUndo";
            this.lblMaxUndo.Size = new System.Drawing.Size(34, 17);
            this.lblMaxUndo.TabIndex = 631;
            this.lblMaxUndo.Tag = "0";
            this.lblMaxUndo.Text = "Max";
            this.lblMaxUndo.Click += new System.EventHandler(this.lblMaxUndo_Click);
            // 
            // pictureBox19
            // 
            this.pictureBox19.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox19.Enabled = false;
            this.pictureBox19.Image = global::DoubleBalls.Properties.Resources.exception01;
            this.pictureBox19.Location = new System.Drawing.Point(10, 9);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(25, 22);
            this.pictureBox19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox19.TabIndex = 627;
            this.pictureBox19.TabStop = false;
            this.pictureBox19.Tag = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = global::DoubleBalls.Properties.Resources.exception08;
            this.pictureBox1.Location = new System.Drawing.Point(5, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 629;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "0";
            // 
            // ViewException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(868, 595);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblMaxUndo);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.extendLabel6);
            this.Controls.Add(this.pictureBox19);
            this.Controls.Add(this.panBody);
            this.Name = "ViewException";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewException";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewException_FormClosing);
            this.Load += new System.EventHandler(this.ViewException_Load);
            this.SizeChanged += new System.EventHandler(this.ViewException_SizeChanged);
            this.panBody.ResumeLayout(false);
            this.panBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panBody;
        private Controls.ExtendLabel extendLabel7;
        private Controls.ExtendLabel extendLabel1;
        private Controls.ExtendLabel extendLabel3;
        private Controls.ExtendLabel extendLabel2;
        private Controls.ExtendLabel extendLabel4;
        private Controls.ExtendLabel extendLabel5;
        private System.Windows.Forms.Label label13;
        private Controls.ExtendLabel extendLabel6;
        private System.Windows.Forms.PictureBox pictureBox19;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.RichTextBox callStack;
        private Controls.ExtendLabel lblExceptionDate;
        private Controls.ExtendLabel lblExceptionSource;
        private Controls.ExtendLabel lblExceptionType;
        private Controls.ExtendLabel lblExceptionMethod;
        private Controls.ExtendLabel lblExceptionMessage;
        private Controls.LeeLabel lblClose;
        private Controls.LeeLabel lblMin;
        private Controls.LeeLabel lblMaxUndo;
    }
}