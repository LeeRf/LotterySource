using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 自选号控件生成设置
    /// </summary>
    public class OneselfControlHelper
    {
        /// <summary>
        /// 初始Panel的大小
        /// </summary>
        public Size initPanelSize { get; } = new Size(60, 31);
        /// <summary>
        /// 累加自选号的位置
        /// </summary>
        public Point cumsumPoint { get; set; } = new Point(34, 7);

        /// <summary>
        /// 双色球生成的Panel
        /// </summary>
        public Panel oneselfPanel { get; set; }
        /// <summary>
        /// 自选号的No
        /// </summary>
        public Label oneselfLabelNo { get; set; }
        /// <summary>
        /// 自选双色球图标(选好完成or删除选号)
        /// </summary>
        public PictureBox buttonImage { get; set; }

        /// <summary>
        /// 是否已经创建OK图标(证明自选号码已经成型)
        /// </summary>
        public bool isCreateOKImage { get; set; }
        /// <summary>
        /// 自选号是否选择完毕
        /// </summary>
        public bool oneselfStatus { get; set; } = true;
        /// <summary>
        /// 记录引用的流控件
        /// </summary>
        public FlowLayoutPanel LayoutPanel { get; set; }

        public OneselfControlHelper(FlowLayoutPanel layoutPanel) => LayoutPanel = layoutPanel;

        public void Reset()
        {
            oneselfPanel = null;
            buttonImage = null;

            cumsumPoint = new Point(34, 7);
            isCreateOKImage = false;
            oneselfStatus = true;
        }
    }
}
