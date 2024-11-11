using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto.Controls
{
    [ToolboxItem(true)]
    [DefaultProperty("LeeLabel")]
    public partial class ExtendLabel : Label
    {
        private Color DeafultColor;

        private Font DeafultTextFont;

        [Browsable(true)]
        public Color EnterColor { get; set; } = Color.DarkRed;

        [Browsable(true)]
        public Font EnterFont { get; set; } = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

        public ExtendLabel()
        {
            MouseEnter += (ev, sn) =>
            {
                DeafultColor = ForeColor;
                ForeColor = EnterColor;

                DeafultTextFont = Font;
                Font = EnterFont;
            };
            MouseLeave += (ev, sn) => 
            {
                ForeColor = DeafultColor;
                Font = DeafultTextFont;
            };
        }
    }
}
