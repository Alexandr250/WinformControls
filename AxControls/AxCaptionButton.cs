using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformControls.AxControls.ControlTemplate;
using System.ComponentModel;

namespace WinformControls.AxControls {
    public class AxCaptionButton : AxCaptionControlBase<AxButton> {
        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        public AxButton AxButton {
            get {
                return ( AxButton ) Control;
            }
        }

        public AxCaptionButton() : base() {
        }
    }
}
