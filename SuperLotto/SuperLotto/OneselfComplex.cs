using SuperLotto.Controls;
using SuperLotto.Other;
using SuperLotto.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto
{
    /// <summary>
    /// 点击生成大乐透号码委托
    /// </summary>
    /// <param name="ballType">0、红球 or 红胆；
    ///                        1、红拖
    ///                        2、蓝球；</param>
    /// <param name="number"></param>
    public delegate void OneselfSuperLottoNumberEventHandler(int ballType, int number);
    public partial class OneselfComplex : SkinMain
    {
        SuperLottoView _superLottoView;

        /// <summary>
        /// 记录复试红球已选择的数量
        /// </summary>
        public static int _RedCount = 0;
        /// <summary>
        /// 记录复试蓝球已选择的数量
        /// </summary>
        public static int _BlueCount = 0;

        public const int _MAXREDCOUNT = 18;
        public const int _MAXBLUECOUNT = 12;

        private const double _OPACITY = 0.9d;

        public event OneselfSuperLottoNumberEventHandler OneselfSelectComplexNumber;

        public OneselfComplex(SuperLottoView superLottoView) 
        { 
            InitializeComponent();
            _superLottoView = superLottoView;
        }

        private void OneselfNumber_Load(object sender, EventArgs e)
        {
            Left = _superLottoView.Left;
            Top = _superLottoView.Top + _superLottoView.Height - Height;
            
            Width = _superLottoView.Width;

            _superLottoView.RefreshResize += RefreshWidth;
            _superLottoView.RefreshMoveLocation += RefreshMoveLocation;
            OneselfSelectComplexNumber += _superLottoView.OneselfSelectComplexNumber;

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
            LeeLabel superLotto = ((LeeLabel)sender);
            string superLottoType = superLotto.Tag.ToString();

            #region Check Double Ball Count

            bool isRed = superLottoType.Equals("Red");

            bool selectOver = _RedCount == 0 && _BlueCount == 0;

            int currentTotalZhu = int.Parse(_superLottoView.lblComplexBuyZhu.Tag.ToString());
            //以达到最高限制就提示提示
            if (selectOver && (SuperLottoView._ComplexSuperLottoSuperLottoCount == SuperLottoView._MaxSerialNumberTotal || SuperLottoView._MaxOneselfSuperLottoTotalZhu == currentTotalZhu))
            {
                Info.ShowWarningMessage(Info.OneselfRestrict);
                return;
            }

            int checkRedCount = _RedCount;
            int checkBlueCount = _BlueCount;

            if (isRed) checkRedCount++;
            else checkBlueCount++;

            //计算预期总注是否大于最高注数提示
            int anticipateTotalZhu = _superLottoView._superLottoToo.GetComplexCombinationTotalZhu(checkRedCount, checkBlueCount) + currentTotalZhu;

            if (anticipateTotalZhu > SuperLottoView._MaxOneselfSuperLottoTotalZhu)
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

            superLotto.Visible = false;
            OneselfSelectComplexNumber?.Invoke(isRed ? 0 : 2, int.Parse(superLotto.Text));
        }

        public void ResetSuperLottoNumber()
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
            Left = _superLottoView.Left;
            Top = _superLottoView.Top + _superLottoView.Height - Height;
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

        private void RefreshWidth() => Width = _superLottoView.Width;

        private void OneselfNumber_MouseEnter(object sender, EventArgs e) => Opacity = _OPACITY;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_superLottoView.WindowState != FormWindowState.Minimized && Visible)
            {
                Point p = PointToClient(Control.MousePosition);
                //判断鼠标是否在窗口区外
                if (p.X > ClientSize.Width || p.X < 0 || p.Y > ClientSize.Height || p.Y < 0)
                {
                    Opacity = 0.10d;
                }
            }
        }

        #endregion
    }
}
