using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto.Controls
{
    [ToolboxItem(true)]
    [DefaultProperty("DoubleBall")]
    public partial class DoubleControl : Control
    {
        private bool _isRed = true;
        [Browsable(true)]
        public bool IsRed 
        {
            get => _isRed;
            set
            {
                _isRed = value;
                OnPaint(null);
            }
        }

        [Browsable(true)]
        [Description("渲染的字体大小")]
        public float _FontSize { get; set; } = 17.5F;

        private Color SolidBrushColor;

        [Browsable(false)]
        public Color RedColor { get; } = Color.FromArgb(230, 0, 20);

        [Browsable(false)]
        public Color BlueColor { get; } = Color.FromArgb(85, 159, 222);

        public DoubleControl()
        {
            Text = "00";
            

            SetStyle(ControlStyles.ResizeRedraw, true);//空间大小改变时，控件会重绘
        }

        public void OnPaint()
        {
            OnPaint(null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isRed)
            {
                SolidBrushColor = RedColor;
            }
            else
            {
                SolidBrushColor = BlueColor;
            }

            int width = this.Width;
            int height = this.Height;
            int radius = width < height ? width : height;//以最短的边最为圆的直径
            Graphics graphics = this.CreateGraphics();
            //graphics.Clear( Color .FromArgb (255,240,240,240) );
            Rectangle rectangle = new Rectangle(0, 0, radius, radius);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;//消除锯齿

            Brush brush = new SolidBrush(SolidBrushColor);//指定画刷的颜色
            graphics.FillEllipse(brush, new Rectangle(0, 0, radius, radius));//填充一个圆


            Pen pen = new Pen(SolidBrushColor, 2);//指定画笔的颜色和线宽
            graphics.DrawEllipse(pen, rectangle);//绘制圆的边界

            string text = this.Text;
            brush = new SolidBrush(Color.White);//指定画刷画文本的颜色

            Font font = new Font("微软雅黑", _FontSize, FontStyle.Bold, GraphicsUnit.Point, 134);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;//字符水平对齐方式
            sf.LineAlignment = StringAlignment.Center;//字符垂直对齐方式
            graphics.DrawString(text, font, brush, new RectangleF(0, 0, radius, radius), sf);


            #region  使控件的单击事件只在圆内触发，而不是在矩形控件范围内触发，消除在圆外和控件内的区域响应单机事件的bug

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, radius, radius);
            this.Region = new Region(path);

            this.BackColor = SolidBrushColor;
            #endregion

            /**
             * 当控件的 size 改变时，如果调用了 font.Dispose()，那么 font.Height 会报异常，因为没有重新实例化font类，
             * 导致 DrawString 会报参数无效，所以也可以不调用 font.dispose，这样就可以不用捕获异常了
             */
            //font.Dispose();
            brush.Dispose();
            //pen.Dispose();
            graphics.Dispose();
        }
    }
}
