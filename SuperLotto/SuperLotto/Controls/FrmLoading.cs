﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadingTimer = System.Threading.Timer;
using UITimer = System.Windows.Forms.Timer;

namespace SuperLotto.Controls
{
    //退出循环委托
    public delegate void ExitLoopWaitEventHandler(bool userExit);

    public partial class FrmLoading : Form
    {
        public event ExitLoopWaitEventHandler exitEvent;

        public FrmLoading(object message, Color color, ExitLoopWaitEventHandler thatExitEvent)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //初始化绘图timer
            _tmrGraphics = new UITimer { Interval = 1 };
            //Invalidate()强制重绘,绘图操作在OnPaint中实现
            _tmrGraphics.Tick += (sender, e) => PnlImage.Invalidate(false);
            _dotSize = PnlImage.Width / 10f;
            //初始化"点"
            _dots = new LoadingDot[5];
            Color = color;
            Message = message.ToString();
            this.exitEvent = thatExitEvent;
        }

        public FrmLoading(object message)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            _tmrGraphics = new UITimer {Interval = 1};
            _tmrGraphics.Tick += (sender, e) => PnlImage.Invalidate(false);
            _dotSize = PnlImage.Width/10f;
            _dots = new LoadingDot[5];
            Color = Color.Orange;
            Message = message.ToString();
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {
            LblMessage.ForeColor = Color;
            var screenRect = Screen.PrimaryScreen.WorkingArea;

            if (Owner != null)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(Owner.Left, Owner.Top);
                Width = Owner.Width;
                Height = Owner.Height;
            }
            else
            {
                Location = new Point((screenRect.Width - Width) / 2, (screenRect.Height - Height) / 2);
            }

            lblExit.Location = new Point((this.Width - lblExit.Width) / 2, lblExit.Location.Y);
            Start();
        }

        public void setExitVisible(bool visible)
        {
            this.lblExit.Visible = visible;
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            exitEvent?.Invoke(true);
        }

        private void FrmLoading_Shown(object sender, EventArgs e)
        {
            if (_workAction != null)
            {
                _workThread = new Thread(ExecWorkAction);
                _workThread.IsBackground = true;
                _workThread.Start();
            }
        }

        [Description("消息")]
        public string Message
        {
            get => LblMessage.Text;
            set => LblMessage.Text = value;
        }

        [Browsable(false), Description("圆心")]
        public PointF CircleCenter
        {
            get => new PointF(PnlImage.Width / 2f, PnlImage.Height / 2f);
            set => throw new NotImplementedException();
        }

        [Browsable(false), Description("半径")]
        public float CircleRadius
        {
            get => PnlImage.Width / 2f - _dotSize;
            set => throw new NotImplementedException();
        }

        [Browsable(true), Category("Appearance"), Description("设置\"点\"的前景色")]
        public Color Color { get; set; }

        [Description("工作是否完成")]
        public bool IsWorkCompleted;

        [Description("工作动作")]
        private ParameterizedThreadStart _workAction;

        [Description("工作动作参数")]
        private object _workActionArg;

        [Description("工作线程")]
        private Thread _workThread;

        [Description("工作异常")]
        public Exception WorkException { get; private set; }

        [Description("点数组")] private readonly LoadingDot[] _dots;

        [Description("UITimer")] private readonly UITimer _tmrGraphics;

        [Description("ThreadingTimer")] private ThreadingTimer _tmrAction;

        [Description("点大小")] private float _dotSize;

        [Description("是否活动")] private bool _isActived;

        [Description("是否绘制：用于状态重置时挂起与恢复绘图")] private bool _isDrawing = true;

        [Description("Timer计数：用于延迟启动每个点 ")] private int _timerCount;

        [Description("动作间隔(Timer)")] private const int ActionInterval = 30;

        [Description("计数基数：用于计算每个点启动延迟：index * timerCountRadix")] private const int TimerCountRadix = 45;

        /// <summary>
        /// 设置工作动作
        /// </summary>
        public void SetWorkAction(ParameterizedThreadStart workAction, object arg)
        {
            _workAction = workAction;
            _workActionArg = arg;
        }

        /// <summary>
        /// 执行工作动作
        /// </summary>
        private void ExecWorkAction()
        {
            try
            {
                var workTask = new Task(arg => { _workAction(arg); }, _workActionArg);
                workTask.Start();
                Task.WaitAll(workTask);
            }
            catch (Exception exception)
            {
                WorkException = exception;
            }
            finally
            {
                IsWorkCompleted = true;
            }
        }

        /// <summary>
        /// 检查是否重置
        /// </summary>
        private bool CheckToReset()
        {
            return _dots.Count(d => d.Opacity > 0) == 0;
        }

        /// <summary>
        /// 初始化点元素
        /// </summary>
        private void CreateLoadingDots()
        {
            for (var i = 0; i < _dots.Length; ++i)
                _dots[i] = new LoadingDot(CircleCenter, CircleRadius);
        }

        /// <summary>  
        /// 开始  
        /// </summary>  
        public void Start()
        {
            CreateLoadingDots();
            _timerCount = 0;
            foreach (var dot in _dots) dot.Reset();
            _tmrGraphics.Start();

            //初始化动作timer  
            _tmrAction = new ThreadingTimer(
                state =>
                {
                    //动画动作  
                    for (var i = 0; i < _dots.Length; i++)
                    {
                        if (_timerCount++ > i*TimerCountRadix)
                        {
                            _dots[i].LoadingDotAction();
                        }
                    }
                    //是否重置  
                    if (CheckToReset())
                    {
                        //重置前暂停绘图  
                        _isDrawing = false;
                        _timerCount = 0;
                        foreach (var dot in _dots) dot.Reset();
                        //恢复绘图  
                        _isDrawing = true;
                    }
                    _tmrAction.Change(ActionInterval, Timeout.Infinite);
                },
                null, ActionInterval, Timeout.Infinite);
            _isActived = true;
        }

        public void Stop()
        {
            _tmrGraphics.Stop();
            _tmrAction.Dispose();
            _isActived = false;
        }

        #region 重写  
        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsWorkCompleted)
            {
                Stop();
                Close();
            }
        }

        private void PnlImage_Paint(object sender, PaintEventArgs e)
        {
            if (_isActived && _isDrawing)
            {
                //抗锯齿  
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                using (var bitmap = new Bitmap(200, 200))
                {
                    //缓冲绘制  
                    using (var bufferGraphics = Graphics.FromImage(bitmap))
                    {
                        //抗锯齿  
                        bufferGraphics.SmoothingMode = SmoothingMode.HighQuality;
                        foreach (var dot in _dots)
                        {
                            var rectangleF = new RectangleF(
                                new PointF(dot.Location.X - _dotSize / 2, dot.Location.Y - _dotSize / 2),
                                new SizeF(_dotSize, _dotSize));
                            bufferGraphics.FillEllipse(new SolidBrush(Color.FromArgb(dot.Opacity, Color)), rectangleF);
                        }
                    }
                    //贴图  
                    e.Graphics.DrawImage(bitmap, new PointF(0, 0));
                }
            }
            base.OnPaint(e);
        }

        private void PnlImage_Resize(object sender, EventArgs e)
        {
            PnlImage.Height = PnlImage.Width;
            _dotSize = PnlImage.Width / 12f;
            OnResize(e);
        }
        #endregion 重写  
    }
}
