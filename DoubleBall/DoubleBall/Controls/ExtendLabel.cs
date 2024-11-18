using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoubleBalls.Controls
{
    [ToolboxItem(true)]
    [DefaultProperty("LeeLabel")]
    public partial class ExtendLabel : Label
    {
        private Color DeafultColor;

        private Font DeafultTextFont;

        [Browsable(true)]
        public Color EnterColor { get; set; } = Color.DarkRed;

        [Browsable(true)]
        public Font EnterFont { get; set; } = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

        [Browsable(true)]
        [Description("鼠标进入是否设置效果")]
        public bool MouseEnterEffect { get; set; } = false;

        [Browsable(true)]
        [Description("是否设置圆角")]
        public bool Fillet { get; set; } = false;

        public static PictureBox WhiteLayer;

        public ExtendLabel()
        {
            if (WhiteLayer == null) WhiteLayer = getWhiteLayer();

            this.MouseEnter += (ev, sn) =>
            {
                //文本颜色
                DeafultColor = ForeColor;
                ForeColor = EnterColor;
                //字体样式
                DeafultTextFont = Font;
                Font = EnterFont;

                if (MouseEnterEffect)
                    (ev as Control).Controls.Add(WhiteLayer);
            };

            this.MouseLeave += (ev, sn) =>
            {
                ForeColor = DeafultColor;
                Font = DeafultTextFont;

                if (MouseEnterEffect) (ev as Control).Controls.Remove(WhiteLayer);
            };
        }

        // 阴影的大小、颜色和偏移量
        public int ShadowWidth { get; set; } = 8;
        public Color ShadowColor { get; set; } = Color.FromArgb(50, 0, 0, 0); // 半透明黑色阴影

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (MouseEnterEffect)
            {
                // 创建渐变阴影
                using (var shadowBrush = new LinearGradientBrush(
                    new Rectangle(-ShadowWidth, -ShadowWidth, this.Width + 2 * ShadowWidth, this.Height + 2 * ShadowWidth),
                    Color.Transparent,
                    ShadowColor,
                    45f)) // 45度的渐变角度
                {
                    e.Graphics.FillRectangle(shadowBrush, -ShadowWidth, -ShadowWidth, this.Width + 2 * ShadowWidth, this.Height + 2 * ShadowWidth);
                }
            }
        }

        public void removePicture()
        {
            this.Controls.Remove(WhiteLayer);
        }

        private PictureBox getWhiteLayer()
        {
            return new PictureBox
            {
                Enabled = false,
                TabStop = false,
                Cursor = Cursors.Hand,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(1258291199)
            };
        }
    }
}
