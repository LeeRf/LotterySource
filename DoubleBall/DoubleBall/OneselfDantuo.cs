using DoubleBalls.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleBalls.Other;
using System.Windows.Forms;
using DoubleBalls.Controls;

namespace DoubleBalls
{
    public partial class OneselfDantuo : SkinMain
    {
        DoubleBallView _doubleBallView;

        /// <summary>
        /// 记录红球胆已选择的数量
        /// </summary>
        public static int _RedDanCount = 0;
        /// <summary>
        /// 记录红球拖已选择的数量
        /// </summary>
        public static int _RedTuoCount = 0;
        /// <summary>
        /// 记录蓝球已选择的数量
        /// </summary>
        public static int _BlueCount = 0;
        /// <summary>
        /// 红色球胆码最高可选数量
        /// </summary>
        public const int _MAXDREDCOUNT = 5;
        /// <summary>
        /// 红色球拖码最高可选数量
        /// </summary>
        public const int _MAXTREDCOUNT = 20;
        /// <summary>
        /// 蓝色球最高可选数量
        /// </summary>
        public const int _MAXBLUECOUNT = 16;

        private const double _OPACITY = 0.9d;

        public event OneselfDoubleBallNumberEventHandler OneselfSelectDantuoNumber;

        public OneselfDantuo(DoubleBallView doubleBallView)
        {
            InitializeComponent();
            _doubleBallView = doubleBallView;
        }

        private void OneselfDantuo_Load(object sender, EventArgs e)
        {
            Left = _doubleBallView.Left;
            Top = _doubleBallView.Top + _doubleBallView.Height - Height;

            Width = _doubleBallView.Width;

            _doubleBallView.RefreshResize += RefreshWidth;
            _doubleBallView.RefreshMoveLocation += RefreshMoveLocation;
            OneselfSelectDantuoNumber += _doubleBallView.OneselfSelectDantuoNumber;

            Win32.AnimateWindow(Handle, 150, Win32.AW_ACTIVATE | Win32.AW_VER_NEGATIVE | Win32.AW_SLIDE);

            Opacity = _OPACITY;
        }

        private void lblDRed1_Click(object sender, EventArgs e)
        {
            LeeLabel doubleBall = ((LeeLabel)sender);
            string doubleBallType = doubleBall.Tag.ToString();

            int ballType = 0;
            if (doubleBallType.Equals("TRed")) ballType = 1;
            else if (doubleBallType.Equals("Blue")) ballType = 2;

            #region Check Max Count

            bool selectOver = _RedDanCount == 0 && _RedTuoCount == 0 && _BlueCount == 0;

            int currentTotalZhu = int.Parse(_doubleBallView.lblDantuoBuyZhu.Tag.ToString());
            //以达到最高限制就提示提示
            if (selectOver && (DoubleBallView._DantuoDoubleBallDoubleBallCount == DoubleBallView._MaxSerialNumberTotal || DoubleBallView._MaxOneselfDoubleBallTotalZhu == currentTotalZhu))
            {
                Info.ShowWarningMessage(Info.OneselfRestrict);
                return;
            }

            int checkRedDanCount = _RedDanCount;
            int checkRedTuoCount = _RedTuoCount;
            int checkBlueCount = _BlueCount;

            if (ballType == 0) checkRedDanCount++;
            else if (ballType == 1) checkRedTuoCount++;
            else checkBlueCount++;

            //计算预期总注是否大于最高注数提示
            long anticipateTotalZhu = _doubleBallView._doubleBallToo.GetDantuoCombinationTotalZhu(checkRedDanCount, checkRedTuoCount, checkBlueCount) + currentTotalZhu;

            if (anticipateTotalZhu > DoubleBallView._MaxOneselfDoubleBallTotalZhu)
            {
                Info.ShowWarningMessage(Info.AnticipateTotalZhu);
                return;
            }

            if (ballType == 0 && _RedDanCount == _MAXDREDCOUNT)
            {
                Info.ShowWarningMessage(Info.RedBallDanMaxCount);
                return;
            }

            if (ballType == 1 && _RedTuoCount == _MAXTREDCOUNT)
            {
                Info.ShowWarningMessage(Info.RedBallTuoMaxCount);
                return;
            }

            #endregion

            if (ballType == 0) _RedDanCount++;
            else if (ballType == 1) _RedTuoCount++;
            else _BlueCount++;

            doubleBall.Visible = false;
            DisableTheSameNumbers(ballType, doubleBall.Text);
            OneselfSelectDantuoNumber?.Invoke(ballType, int.Parse(doubleBall.Text));
        }

        private void DisableTheSameNumbers(int ballType, string selectBall)
        {
            if (ballType != 2)
            {
                foreach (var item in Controls)
                {
                    //不为蓝球时、还可见时、相等时
                    if (item is LeeLabel lbl)
                    {
                        if (!"Blue".Equals(lbl.Tag.ToString()) && lbl.Visible && lbl.Text == selectBall)
                        {
                            lbl.Enabled = false;
                            break;
                        }
                    }
                }
            }
        }

        public void RollBackSelectBall(string ballType, int number)
        {
            if (ballType == "Red") ballType = "DRed";

            if (ballType == "DRed") _RedDanCount--;
            if (ballType == "TRed") _RedTuoCount--;
            if (ballType == "Blue") _BlueCount--;

            foreach (Control item in Controls)
            {
                if (item is LeeLabel && !item.Visible || !item.Enabled)
                {
                    if (int.Parse(item.Text) == number)
                    {
                        if (ballType != "Blue" && item.Tag.ToString() != "Blue")
                        {
                            item.Visible = true;
                            item.Enabled = true;
                        }

                        if (ballType == "Blue" && item.Tag.ToString() == "Blue")
                        {
                            item.Visible = true;
                            break;
                        }
                    }
                }
            }

            ResetOpacityAndShow();
        }

        public void ResetDoubleBallNumber()
        {
            foreach (Control item in Controls)
            {
                if (item is LeeLabel)
                {
                    if (!item.Visible) item.Visible = true;
                    if (!item.Enabled) item.Enabled = true;
                }
            }

            _RedDanCount = 0;
            _RedTuoCount = 0;
            _BlueCount = 0;
            ResetOpacityAndShow();

        }

        private void ResetOpacityAndShow()
        {
            Opacity = _OPACITY;
            tmrOpacity.Stop();
            tmrOpacity.Start();
        }

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

        private void RefreshWidth() => Width = _doubleBallView.Width;

        private void lblClose_Click(object sender, EventArgs e) => HideThis();

        private void OneselfNumber_MouseEnter(object sender, EventArgs e) => Opacity = _OPACITY;

        private void tmrOpacity_Tick(object sender, EventArgs e)
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
    }
}
