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
    public partial class LeeLabel : Label
    {
        private Color DeafultColor;

        [Browsable(true)]
        public Color EnterColor { get; set; } = Color.DarkRed;

        public LeeLabel()
        {
            MouseEnter += (ev, sn) =>
            {
                DeafultColor = ForeColor;
                ForeColor = EnterColor;
            };
            MouseLeave += (ev, sn) => { ForeColor = DeafultColor; };
        }
    }
}
