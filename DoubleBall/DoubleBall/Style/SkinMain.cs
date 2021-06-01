using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using DoubleBalls.Style.Helper;

namespace DoubleBalls.Style
{
    //控件层
    public partial class SkinMain : Form
    {
        //绘制层
        private SkinForm _skin;
        //淡出淡入时间
        public int _dwTime { get; set; } = 5;
        //窗体圆角
        public int _filletSize { get; set; } = 5;
        public bool WinSize { get; set; } = false;
        /// <summary>
        /// 是否可移动
        /// </summary>
        public bool _isMove { get; set; } = true;

        public SkinMain() {
            InitializeComponent(); SetStyles();
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }

        //设置阴影层的显示/隐藏
        public void ThisVisible(bool visible) {
            _skin.Visible = visible;
        }

        protected override CreateParams CreateParams {
            get {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }
        
        //减少闪烁
        public void SetStyles() {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }

        //Show或Hide被调用时
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                //启用窗口淡入淡出
                if (!DesignMode)
                {
                    //淡入特效
                    Win32.AnimateWindow(this.Handle, _dwTime, Win32.AW_BLEND | Win32.AW_ACTIVATE);
                }
                //判断不是在设计器中
                if (!DesignMode && _skin == null)
                {
                    _skin = new SkinForm(this);
                    _skin.Show(this);
                }
                base.OnVisibleChanged(e);
            }
            else
            {
                base.OnVisibleChanged(e);
                Win32.AnimateWindow(this.Handle, _dwTime, Win32.AW_BLEND | Win32.AW_HIDE);
            }
        }

        //窗体关闭时
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            //先关闭阴影窗体并且当为主窗体时不可关闭阴影窗体
            if (_skin != null && this.GetType().Name != "Main"){
                _skin.Close();
            }
            //在Form_FormClosing中添加代码实现窗体的淡出
            Win32.AnimateWindow(this.Handle, _dwTime, Win32.AW_BLEND | Win32.AW_HIDE);
        }

        //控件首次创建时被调用
        protected override void OnCreateControl() {
            base.OnCreateControl();
            SetReion();
        }

        //圆角
        private void SetReion() {
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(new Rectangle(Point.Empty, base.Size), _filletSize, RoundStyle.All, true))
            {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                this.Region = region;
            }
        }

        //改变窗体大小时
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            SetReion();
        }

        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            //是否可以改变窗体大小
            if (m.Msg == 0x0084 && WinSize)
            {
                base.WndProc(ref m);
                Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                vPoint = PointToClient(vPoint);
                if (vPoint.X <= 5)
                    if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else m.Result = (IntPtr)HTLEFT;
                else if (vPoint.X >= ClientSize.Width - 5)
                    if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else m.Result = (IntPtr)HTRIGHT;
                else if (vPoint.Y <= 5)
                    m.Result = (IntPtr)HTTOP;
                else if (vPoint.Y >= ClientSize.Height - 5)
                    m.Result = (IntPtr)HTBOTTOM;
            }
            else if (m.Msg == 0x0201 && _isMove)
            {
                m.Msg = 0x00A1;//更改消息为非客户区按下鼠标 
                m.LParam = IntPtr.Zero;//默认值 
                m.WParam = new IntPtr(2);//鼠标放在标题栏内 
                if (m.Msg != 0x0014)
                    base.WndProc(ref m);
            }
            else
            {
                if (m.Msg != 0x0014)
                    base.WndProc(ref m);
            }
        }
    }
}