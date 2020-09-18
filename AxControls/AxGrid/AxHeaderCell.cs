using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WinformControls {
    public class AxHeaderCell : DataGridViewColumnHeaderCell {
        private int _columnSpan;

        public int ColumnSpan {
            get {
                return _columnSpan;
            }
            set {
                if ( _columnSpan != value ) {
                    _columnSpan = value;
                    if ( ColumnSpanChanged != null )
                        ColumnSpanChanged();
                }
            }
        }

        public string ColumnSpanText { get; set; }

        public event Action ColumnSpanChanged;

        public AxHeaderCell() { }
        public AxHeaderCell( int columnSpan ) {
            ColumnSpan = columnSpan;            
        }

        protected override Size GetPreferredSize( Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize ) {
            SizeF sizef = base.GetPreferredSize( graphics, cellStyle, rowIndex, constraintSize );// graphics.MeasureString( Value.ToString(), cellStyle.Font );
            //sizef = new SizeF( sizef.Width + cellStyle.Padding.Left + cellStyle.Padding.Right, sizef.Height + 20 + cellStyle.Padding.Top + cellStyle.Padding.Bottom + 20 );
            
            if ( ColumnSpan > 1 )
                return new Size( 
                    ( int ) sizef.Width, 
                    ( ( int ) ( sizef.Height * 1.5 ) ) * 2 );

            return new Size( ( int ) sizef.Width, ( int ) sizef.Height );
        }
    }
}
