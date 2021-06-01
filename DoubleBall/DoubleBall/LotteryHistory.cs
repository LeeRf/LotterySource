using DoubleBalls.Controls;
using DoubleBalls.Model;
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
    public partial class LotteryHistory : SkinMain
    {
        DoubleBallView _doubleBallView;

        public LotteryHistory(DoubleBallView doubleBallView)
        {
            InitializeComponent();
            _doubleBallView = doubleBallView;
        }

        private void LotteryHistory_Load(object sender, EventArgs e)
        {
            _redBallLabelStyles[0] = lblRedBallA;
            _redBallLabelStyles[1] = lblRedBallB;
            _redBallLabelStyles[2] = lblRedBallC;
            _redBallLabelStyles[3] = lblRedBallD;
            _redBallLabelStyles[4] = lblRedBallE;
            _redBallLabelStyles[5] = lblRedBallF;

            Shown += (o, args) => { LoadHistoryNumbers(); };
            _doubleBallView.RefreshResize += RefreshHeight;
            _doubleBallView.RefreshMoveLocation += PositionCorrection;

            CorrectPositionAndShow();
        }

        private void CorrectPositionAndShow()
        {
            Height = _doubleBallView.Height;

            if (_doubleBallView.Left >= Width)
            {
                Top = _doubleBallView.Top;
                Left = _doubleBallView.Left - Width - 1;

                Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_NEGATIVE | Win32.AW_SLIDE);
            }
            else
            {
                Top = _doubleBallView.Top;
                Left = 0;

                Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_POSITIVE | Win32.AW_SLIDE);
            }
        }

        private void LoadHistoryNumbers()
        {
            flpHistoryNumber.Controls.Clear();

            var queue = Config._historyPublicDoubleBallNumbers.OrderByDescending(db => db.Periods);
            var loop = queue.GetEnumerator();

            while (loop.MoveNext())
            {
                CreateDoubleBallControlAndShow(loop.Current);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void lblRefresh_Click(object sender, EventArgs e) => LoadHistoryNumbers();

        /// <summary>
        /// 机选号红球的六个 Label 数组样式
        /// </summary>
        private Label[] _redBallLabelStyles = new Label[6];

        /// <summary>
        /// 创建一个历史号码
        /// </summary>
        /// <param name="simplexDoubleBallNumber"></param>
        private void CreateDoubleBallControlAndShow(SimplexDoubleBallNumber simplexDoubleBallNumber)
        {
            Panel copyPanel = new Panel();
            copyPanel.Size = panHistory.Size;
            copyPanel.Tag = simplexDoubleBallNumber.Periods;

            LeeLabel copyPeriod = new LeeLabel();
            DoubleBallView.CopyLabelStyle(lblHistoryPeriods, copyPeriod);
            copyPeriod.EnterColor = lblHistoryPeriods.EnterColor;

            for (int i = 0; i < simplexDoubleBallNumber.redBalls.Length; i++)
            {
                Label copyRedBall = new Label();
                DoubleBallView.CopyLabelStyle(_redBallLabelStyles[i], copyRedBall);
                copyPanel.Controls.Add(copyRedBall);
            }

            Label copyBlueBall = new Label();
            DoubleBallView.CopyLabelStyle(lblBlueBall, copyBlueBall);

            copyPanel.Controls.Add(copyPeriod);
            copyPanel.Controls.Add(copyBlueBall);
            flpHistoryNumber.Controls.Add(copyPanel);

            DoubleBallView.FormatRandomPanelContext(copyPanel, simplexDoubleBallNumber, 0);
        }

        /// <summary>
        /// 矫正窗体位置始终在主窗体左边
        /// </summary>
        public void PositionCorrection()
        {
            if (LeftMagneticDistance())
            {
                Top = _doubleBallView.Top;
                Left = _doubleBallView.Left - Width - 1;
            }

            if (RightMagneticDistance())
            {
                Top = _doubleBallView.Top;
                Left = _doubleBallView.Left + _doubleBallView.Width + 1;
            }
        }

        /// <summary>
        /// 判断左边磁性距离是否足够
        /// </summary>
        private bool LeftMagneticDistance()
        {
            int top = _doubleBallView.Top - Top;
            int left = _doubleBallView.Left - Left - Width;

            return top <= 50 && top >= -50 && left <= 50 && left >= -50;
        }

        /// <summary>
        /// 判断右边磁性距离是否足够
        /// </summary>
        /// <returns></returns>
        private bool RightMagneticDistance()
        {
            int top = _doubleBallView.Top - Top;
            int right = _doubleBallView.Right - Right + Width;

            return top <= 50 && top >= -50 && right <= 50 && right >= -50;
        }

        private void lblClose_Click(object sender, EventArgs e) => HideThis();

        private void RefreshHeight()
        {
            if (_doubleBallView.WindowState == FormWindowState.Maximized)
            {
                Top = 0;
                Left = 0;
            }
            else if (_doubleBallView.WindowState == FormWindowState.Normal)
            {
                PositionCorrection();
                Top = _doubleBallView.Top;
            }

            if (_doubleBallView.WindowState != FormWindowState.Minimized)
            {
                Height = _doubleBallView.Height;
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
