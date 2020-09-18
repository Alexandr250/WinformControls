using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformControls {
    public class AxPopup : ToolStripDropDown {
        ToolStripControlHost _controlHost;

        public AxPopup() : base() { }


        public void SetContent( Control control ) {
            _controlHost = new ToolStripControlHost( control );
        }
    }
}
