using DoubleBalls.Controls;
using DoubleBalls.Other;
using DoubleBalls.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoubleBalls
{
    /// <summary>
    /// 点击生成双色球号码委托
    /// </summary>
    /// <param name="ballType">0、红球 or 红胆；
    ///                        1、红拖
    ///                        2、蓝球；</param>
    /// <param name="number"></param>
    public delegate void OneselfDoubleBallNumberEventHandler(int ballType, int number);
    public partial class OneselfComplex : SkinMain
    {
        DoubleBallView _doubleBallView;

        /// <summary>
        /// 记录复试红球已选择的数量
        /// </summary>
        public static int _RedCount = 0;
        /// <summary>
        /// 记录复试蓝球已选择的数量
        /// </summary>
        public static int _BlueCount = 0;

        public const int _MAXREDCOUNT = 20;
        public const int _MAXBLUECOUNT = 16;

        private const double _OPACITY = 0.9d;

        public event OneselfDoubleBallNumberEventHandler OneselfSelectComplexNumber;

        public OneselfComplex(DoubleBallView doubleBallView) 
        { 
            InitializeComponent();
            _doubleBallView = doubleBallView;
        }

        private void OneselfNumber_Load(object sender, EventArgs e)
        {
            Left = _doubleBallView.Left;
            Top = _doubleBallView.Top + _doubleBallView.Height - Height;
            
            Width = _doubleBallView.Width;

            _doubleBallView.RefreshResize += RefreshWidth;
            _doubleBallView.RefreshMoveLocation += RefreshMoveLocation;
            OneselfSelectComplexNumber += _doubleBallView.OneselfSelectComplexNumber;

            Win32.AnimateWindow(Handle, 150, Win32.AW_ACTIVATE | Win32.AW_VER_NEGATIVE | Win32.AW_SLIDE);

            Opacity = _OPACITY;
        }

        /// <summary>
        /// 重置红色球和蓝色球号码
        /// </summary>
        public void ResetRedBlueBallCount()
        {
            _RedCount = 0;
            _BlueCount = 0;
        }

        private void lblRed1_Click(object sender, EventArgs e)
        {
            LeeLabel doubleBall = ((LeeLabel)sender);
            string doubleBallType = doubleBall.Tag.ToString();

            #region Check Double Ball Count

            bool isRed = doubleBallType.Equals("Red");

            bool selectOver = _RedCount == 0 && _BlueCount == 0;

            int currentTotalZhu = int.Parse(_doubleBallView.lblComplexBuyZhu.Tag.ToString());
            //以达到最高限制就提示提示
            if (selectOver && (DoubleBallView._ComplexDoubleBallDoubleBallCount == DoubleBallView._MaxSerialNumberTotal || DoubleBallView._MaxOneselfDoubleBallTotalZhu == currentTotalZhu))
            {
                Info.ShowWarningMessage(Info.OneselfRestrict);
                return;
            }

            int checkRedCount = _RedCount;
            int checkBlueCount = _BlueCount;

            if (isRed) checkRedCount++;
            else checkBlueCount++;

            //计算预期总注是否大于最高注数提示
            int anticipateTotalZhu = _doubleBallView._doubleBallToo.GetComplexCombinationTotalZhu(checkRedCount, checkBlueCount) + currentTotalZhu;

            if (anticipateTotalZhu > DoubleBallView._MaxOneselfDoubleBallTotalZhu)
            {
                Info.ShowWarningMessage(Info.AnticipateTotalZhu);
                return;
            }

            //红球达到最高可选注数提示
            if (isRed && _RedCount == _MAXREDCOUNT)
            {
                Info.ShowWarningMessage(Info.RedBallAlreadyMaxCount);
                return;
            }

            #endregion

            if (isRed) _RedCount++;
            else
            {
                _BlueCount++;
            }

            doubleBall.Visible = false;
            OneselfSelectComplexNumber?.Invoke(isRed ? 0 : 2, int.Parse(doubleBall.Text));
        }

        public void ResetDoubleBallNumber()
        {
            foreach (Control item in Controls)
            {
                if (item is LeeLabel && !item.Visible)
                {
                    item.Visible = true;
                }
            }

            _RedCount = 0;
            _BlueCount = 0;
            ResetOpacityAndShow();

        }

        public void RollBackSelectBall(bool isRed, int number)
        {
            if (isRed) _RedCount--;
            else _BlueCount--;

            foreach (Control item in Controls)
            {
                if (item is LeeLabel && !item.Visible)
                {
                    if (int.Parse(item.Text) == number)
                    {
                        item.Visible = true;
                        ResetOpacityAndShow();
                        break;
                    }
                }
            }
        }

        private void ResetOpacityAndShow()
        {
            Opacity = _OPACITY;
            tmrOpacity.Stop();
            tmrOpacity.Start();
        }

        #region Style Code

        private void RefreshMoveLocation()
        {
            Left = _doubleBallView.Left;
            Top = _doubleBallView.Top + _doubleBallView.Height - Height;
        }

        public void HideThis()
        {
            ThisVisible(false);
            Win32.AnimateWindow(Handle, 150, Win32.AW_HIDE | Win32.AW_VER_POSITIVE | Win32.AW_SLIDE);
        }

        public void ShowThis()
        {
            Opacity = _OPACITY;
            ThisVisible(true);
            tmrOpacity.Stop();
            tmrOpacity.Start();
            Win32.AnimateWindow(Handle, 150, Win32.AW_ACTIVATE | Win32.AW_VER_NEGATIVE | Win32.AW_SLIDE);
        }

        public void lblClose_Click(object sender, EventArgs e) => HideThis();

        private void RefreshWidth() => Width = _doubleBallView.Width;

        private void OneselfNumber_MouseEnter(object sender, EventArgs e) => Opacity = _OPACITY;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_doubleBallView.WindowState != FormWindowState.Minimized && Visible)
            {
                Point p = PointToClient(Control.MousePosition);
                //判断鼠标是否在窗口区外
                if (p.X > ClientSize.Width || p.X < 0 || p.Y > ClientSize.Height || p.Y < 0)
                {
                    Opacity = 0.10;
                }
            }
        }

        #endregion
    }
}
