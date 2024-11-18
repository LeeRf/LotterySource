using DoubleBalls.Model;
using DoubleBalls.Other;
using DoubleBalls.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DoubleBalls
{
    public partial class ViewException : SkinMain
    {
        private ExceptionLog exLog;
        private static List<int> OpenLogList = new List<int>();

        public ViewException(ExceptionLog thatEx) 
        {
            exLog = thatEx;
            InitializeComponent(); 
        }

        private void ViewException_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(Config.Setting.BackColorArgb);
            Shown += (o, args) => LoopDataAnalyse.SetWindowRegion(panBody, 50);

            OpenLogList.Add(exLog.Id);
            this.lblExceptionDate.Text = exLog.ExceptionDate;
            this.lblExceptionSource.Text = exLog.ExceptionSource;
            this.lblExceptionType.Text = exLog.ExceptionType;
            this.lblExceptionMethod.Text = exLog.ExceptionMethod;
            this.lblExceptionMessage.Text = exLog.ExceptionMessage;
            this.callStack.Text = exLog.CallStack;

            isLoad = false;
        }

        private void lblClose_Click(object sender, EventArgs e) => this.Close();

        private void callStack_MouseEnter(object sender, EventArgs e) => callStack.ForeColor = Color.Red;

        private void callStack_MouseLeave(object sender, EventArgs e) => callStack.ForeColor = Color.LightCoral;

        private void lblMin_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        //窗体 Load 时不要触发圆角设置
        bool isLoad = true;
        private void ViewException_SizeChanged(object sender, EventArgs e)
        {
            if (!isLoad)
            {
                //窗体大小变化时重新应用圆角
                LoopDataAnalyse.SetWindowRegion(panBody, 50);
            }
        }

        public static bool isOpenThat(int logId) => OpenLogList.Contains(logId);
        

        private bool _max = false;
        private void lblMaxUndo_Click(object sender, EventArgs e)
        {
            if (!_max)
            {
                _max = true;
                lblMaxUndo.Text = "Uno";
                //获取窗口当前所在的显示器
                Screen currentScreen = Screen.FromControl(this);

                MaximumSize = currentScreen.WorkingArea.Size;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                _max = false;
                lblMaxUndo.Text = "Max";
                WindowState = FormWindowState.Normal;
            }
        }

        private void ViewException_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenLogList.Remove(exLog.Id);
        }
    }
}
