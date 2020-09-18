using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformControls {
    public class DropDownComboList : ToolStripControlHost {
        private ListView _list;

        public DropDownComboList() : base( new ListView() ) {
            _list = Control as ListView;
            Visible = true;
            Enabled = true;
            _list.AutoSize = false;
            _list.Size = new Size( Width, Height );
            _list.MinimumSize = _list.Size;
            //_list.Dock = DockStyle.Fill;
            _list.FullRowSelect = true;
            _list.GridLines = true;
            _list.HeaderStyle = ColumnHeaderStyle.None;
            _list.View = View.Details;
            //_list.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            BackColor = Color.Red;
        }

        protected override void OnHostedControlResize( EventArgs e ) {
            base.OnHostedControlResize( e );

        }

        public new int Width {
            get {
                return this.Width;
            }
            set {
                base.Width = value;
                _list.Width = value;
                _list.MinimumSize = new Size( value, Height );
            }
        }
        
    }
}
