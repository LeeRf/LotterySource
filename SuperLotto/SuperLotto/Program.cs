using SuperLotto.Data;
using SuperLotto.Model;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SuperLotto
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 设置全局异常处理
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SuperLottoView());
        }

        // 捕获 UI 线程中的未处理异常
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;

            Logger.Error(ex);

            SqlLite.Save<ExceptionLog>(ToExceptionLog(ex));

            ShowMessage(e.Exception);
        }

        // 捕获非 UI 线程中的未处理异常
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;

            Logger.Error(ex);

            SqlLite.Save<ExceptionLog>(ToExceptionLog(ex));

            ShowMessage(ex);
        }

        private static void ShowMessage(Exception ex)
        {
            MessageBox.Show($"发生未处理的异常: {ex.Message}\n调用堆栈：\n{ex.StackTrace}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static ExceptionLog ToExceptionLog(Exception ex)
        {
            return new ExceptionLog()
            {
                ExceptionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDisasterError = true,
                ExceptionMessage = ex.Message,
                ExceptionMethod = ex.TargetSite.ToString(),
                ExceptionType = ex.GetType().ToString(),
                ExceptionSource = ex.Source,
                Encoding = ex.HResult.ToString(),
                CallStack = ex.StackTrace
            };
        }
    }
}
