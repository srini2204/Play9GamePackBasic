using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play9GamePackBasic.Resources.Controls
{
    class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_LBUTTONDOWN = 0x201;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                // do something
                ((Play9GamePackBasic.Pay9MainForm)Form.ActiveForm).Pay9MainForm_Click(null, null);
            }
            return false;
        }
    }
}
