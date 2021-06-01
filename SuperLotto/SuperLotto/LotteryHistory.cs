using SuperLotto.Controls;
using SuperLotto.Model;
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
    public partial class LotteryHistory : SkinMain
    {
        SuperLottoView _superLottoView;

        public LotteryHistory(SuperLottoView superLottoView)
        {
            InitializeComponent();
            _superLottoView = superLottoView;
        }

        private void LotteryHistory_Load(object sender, EventArgs e)
        {
            _redBallLabelStyles[0] = lblRedBallA;
            _redBallLabelStyles[1] = lblRedBallB;
            _redBallLabelStyles[2] = lblRedBallC;
            _redBallLabelStyles[3] = lblRedBallD;
            _redBallLabelStyles[4] = lblRedBallE;
            _blueBallLabelStyles[0] = lblBlueBall1;
            _blueBallLabelStyles[1] = lblBlueBall2;

            Shown += (o, args) => { LoadHistoryNumbers(); };
            _superLottoView.RefreshResize += RefreshHeight;
            _superLottoView.RefreshMoveLocation += PositionCorrection;

            CorrectPositionAndShow();
        }

        private void CorrectPositionAndShow()
        {
            Height = _superLottoView.Height;

            if (_superLottoView.Left >= Width)
            {
                Top = _superLottoView.Top;
                Left = _superLottoView.Left - Width - 1;

                Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_NEGATIVE | Win32.AW_SLIDE);
            }
            else
            {
                Top = _superLottoView.Top;
                Left = 0;

                Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_POSITIVE | Win32.AW_SLIDE);
            }
        }

        private void LoadHistoryNumbers()
        {
            flpHistoryNumber.Controls.Clear();

            var queue = Config._historyPublicSuperLottoNumbers.OrderByDescending(db => db.Periods);
            var loop = queue.GetEnumerator();

            while (loop.MoveNext())
            {
                CreateSuperLottoControlAndShow(loop.Current);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void lblRefresh_Click(object sender, EventArgs e) => LoadHistoryNumbers();

        /// <summary>
        /// 机选号红球的5个 Label 数组样式
        /// </summary>
        private Label[] _redBallLabelStyles = new Label[5];
        private Label[] _blueBallLabelStyles = new Label[2];


        /// <summary>
        /// 创建一个历史号码
        /// </summary>
        /// <param name="simplexSuperLottoNumber"></param>
        private void CreateSuperLottoControlAndShow(SimplexSuperLottoNumber simplexSuperLottoNumber)
        {
            Panel copyPanel = new Panel();
            copyPanel.Size = panHistory.Size;
            copyPanel.Tag = simplexSuperLottoNumber.Periods;

            LeeLabel copyPeriod = new LeeLabel();
            SuperLottoView.CopyLabelStyle(lblHistoryPeriods, copyPeriod);
            copyPeriod.EnterColor = lblHistoryPeriods.EnterColor;

            for (int i = 0; i < simplexSuperLottoNumber.redBalls.Length; i++)
            {
                Label copyRedBall = new Label();
                SuperLottoView.CopyLabelStyle(_redBallLabelStyles[i], copyRedBall);
                copyPanel.Controls.Add(copyRedBall);
            }

            for (int i = 0; i < simplexSuperLottoNumber.blueBalls.Length; i++)
            {
                Label copyBlueBall = new Label();
                SuperLottoView.CopyLabelStyle(_blueBallLabelStyles[i], copyBlueBall);
                copyPanel.Controls.Add(copyBlueBall);
            }

            copyPanel.Controls.Add(copyPeriod);
            flpHistoryNumber.Controls.Add(copyPanel);

            SuperLottoView.FormatRandomPanelContext(copyPanel, simplexSuperLottoNumber, 0);
        }

        /// <summary>
        /// 矫正窗体位置始终在主窗体左边
        /// </summary>
        public void PositionCorrection()
        {
            if (LeftMagneticDistance())
            {
                Top = _superLottoView.Top;
                Left = _superLottoView.Left - Width - 1;
            }

            if (RightMagneticDistance())
            {
                Top = _superLottoView.Top;
                Left = _superLottoView.Left + _superLottoView.Width + 1;
            }
        }

        /// <summary>
        /// 判断左边磁性距离是否足够
        /// </summary>
        private bool LeftMagneticDistance()
        {
            int top = _superLottoView.Top - Top;
            int left = _superLottoView.Left - Left - Width;

            return top <= 50 && top >= -50 && left <= 50 && left >= -50;
        }

        /// <summary>
        /// 判断右边磁性距离是否足够
        /// </summary>
        /// <returns></returns>
        private bool RightMagneticDistance()
        {
            int top = _superLottoView.Top - Top;
            int right = _superLottoView.Right - Right + Width;

            return top <= 50 && top >= -50 && right <= 50 && right >= -50;
        }

        private void lblClose_Click(object sender, EventArgs e) => HideThis();

        private void RefreshHeight()
        {
            if (_superLottoView.WindowState == FormWindowState.Maximized)
            {
                Top = 0;
                Left = 0;
            }
            else if (_superLottoView.WindowState == FormWindowState.Normal)
            {
                PositionCorrection();
                Top = _superLottoView.Top;
            }

            if (_superLottoView.WindowState != FormWindowState.Minimized)
            {
                Height = _superLottoView.Height;
            }
        }

        public void ClearHistoryNumbers()
        {
            flpHistoryNumber.Controls.Clear();
        }

        public void HideThis()
        {
            ThisVisible(false);
            flpHistoryNumber.Visible = false;
            Win32.AnimateWindow(Handle, 200, Win32.AW_HIDE | Win32.AW_HOR_POSITIVE | Win32.AW_SLIDE);
        }

        public void ShowThis()
        {
            CorrectPositionAndShow();
            flpHistoryNumber.Visible = true;
            Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_NEGATIVE | Win32.AW_SLIDE);
            ThisVisible(true);
        }

        private void LotteryHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
