using SuperLotto.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperLotto.Other;
using System.Windows.Forms;
using SuperLotto.Controls;

namespace SuperLotto
{
    public partial class OneselfDantuo : SkinMain
    {
        SuperLottoView _superLottoView;

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

        public event OneselfSuperLottoNumberEventHandler OneselfSelectDantuoNumber;

        public OneselfDantuo(SuperLottoView superLottoView)
        {
            InitializeComponent();
            _superLottoView = superLottoView;
        }

        private void OneselfDantuo_Load(object sender, EventArgs e)
        {
            Left = _superLottoView.Left;
            Top = _superLottoView.Top + _superLottoView.Height - Height;

            Width = _superLottoView.Width;

            _superLottoView.RefreshResize += RefreshWidth;
            _superLottoView.RefreshMoveLocation += RefreshMoveLocation;
            OneselfSelectDantuoNumber += _superLottoView.OneselfSelectDantuoNumber;

            Win32.AnimateWindow(Handle, 150, Win32.AW_ACTIVATE | Win32.AW_VER_NEGATIVE | Win32.AW_SLIDE);

            Opacity = _OPACITY;
        }

        private void lblDRed1_Click(object sender, EventArgs e)
        {
            LeeLabel superLotto = ((LeeLabel)sender);
            string superLottoType = superLotto.Tag.ToString();

            int ballType = 0;
            if (superLottoType.Equals("TRed")) ballType = 1;
            else if (superLottoType.Equals("Blue")) ballType = 2;

            #region Check Max Count

            bool selectOver = _RedDanCount == 0 && _RedTuoCount == 0 && _BlueCount == 0;

            int currentTotalZhu = int.Parse(_superLottoView.lblDantuoBuyZhu.Tag.ToString());
            //以达到最高限制就提示提示
            if (selectOver && (SuperLottoView._DantuoSuperLottoSuperLottoCount == SuperLottoView._MaxSerialNumberTotal || SuperLottoView._MaxOneselfSuperLottoTotalZhu == currentTotalZhu))
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
            long anticipateTotalZhu = _superLottoView._superLottoToo.GetDantuoCombinationTotalZhu(checkRedDanCount, checkRedTuoCount, checkBlueCount) + currentTotalZhu;

            if (anticipateTotalZhu > SuperLottoView._MaxOneselfSuperLottoTotalZhu)
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

            superLotto.Visible = false;
            DisableTheSameNumbers(ballType, superLotto.Text);
            OneselfSelectDantuoNumber?.Invoke(ballType, int.Parse(superLotto.Text));
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

        public void ResetSuperLottoNumber()
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

        private void RefreshWidth() => Width = _superLottoView.Width;

        private void lblClose_Click(object sender, EventArgs e) => HideThis();

        private void OneselfNumber_MouseEnter(object sender, EventArgs e) => Opacity = _OPACITY;

        private void tmrOpacity_Tick(object sender, EventArgs e)
        {
            if (_superLottoView.WindowState != FormWindowState.Minimized && Visible)
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
