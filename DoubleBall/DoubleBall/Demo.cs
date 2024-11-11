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
    public partial class Demo : SkinMain
    {
        public Demo()
        {
            InitializeComponent();
        }

        private void lblClose_Click(object sender, EventArgs e) => Close();

        private void Demo_Load(object sender, EventArgs e)
        {
            Shown += (o, args) =>
            {
                foreach (Control item in panLotteryRules.Controls)
                {
                    if(!item.Visible)
                    {
                        item.Visible = true;
                    }
                }
            };
        }
    }
}
