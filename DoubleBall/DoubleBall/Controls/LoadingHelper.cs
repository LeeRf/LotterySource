using System.Drawing;
using System.Dynamic;
using System.Threading;
using System.Windows.Forms;

namespace DoubleBalls.Controls
{
    public class LoadingHelper
    {
        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="color">颜色</param>
        /// <param name="ownerForm">父窗体</param>
        /// <param name="work">待执行工作</param>
        /// <param name="workArg">工作参数</param>
        public static void ShowLoading(object message, Color color, Form ownerForm, ParameterizedThreadStart work, object workArg = null)
        {
            ShowThat(message, color, null, ownerForm, work, workArg);
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="color">颜色</param>
        /// <param name="ownerForm">父窗体</param>
        /// <param name="work">待执行工作</param>
        /// <param name="exitButton">是否展示退出按钮</param>
        /// <param name="workArg">工作参数</param>
        public static void ShowLoading(object message, Color color, ExitLoopWaitEventHandler exitEvent, Form ownerForm, ParameterizedThreadStart work, object workArg = null)
        {
            ShowThat(message, color, exitEvent, ownerForm, work, workArg);
        }

        private static void ShowThat(object message, Color color, ExitLoopWaitEventHandler exitEvent, Form ownerForm, ParameterizedThreadStart work, object workArg)
        {
            var loadingForm = new FrmLoading(message, color, exitEvent);
            dynamic expandoObject = new ExpandoObject();
            expandoObject.Form = loadingForm;
            expandoObject.WorkArg = workArg;

            if (exitEvent != null)
                loadingForm.setExitVisible(true);

            loadingForm.SetWorkAction(work, expandoObject);
            loadingForm.ShowDialog(ownerForm);
            if (loadingForm.WorkException != null)
            {
                throw loadingForm.WorkException;
            }
        }
    }
}
