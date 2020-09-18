using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinformControls.Common;

namespace WinformControls.AxControls {
    public class AxGrid : DataGridView {
        private StateStyle _normalStyle;

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if ( _normalStyle != null )
                    _normalStyle.Dispose();

                _normalStyle = value;

                _normalStyle.PropertyChanged += ( propertyName ) => {
                    Refresh();
                };

                Refresh();
            }
        }

        [Browsable( false ) ]
        public new Color BackgroundColor {
            get {
                return _normalStyle.BackColor;
            }
            set {
                _normalStyle.BackColor = value;
            }
        }

        [Browsable( false )]
        public new BorderStyle BorderStyle {
            get {
                return BorderStyle.None;
            }
            set {}
        }

        public AxGrid() {
            base.BorderStyle = BorderStyle.FixedSingle;
            AllowUserToAddRows = false;
            EditMode = DataGridViewEditMode.EditProgrammatically;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;

            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };
            _normalStyle.PropertyChanged += ( propertyName ) => {
                Refresh();
            };

            //OnColumnAdded += ( s, e ) => { 
            //    e.Column.HeaderCell = new AxHeaderCell(); 
            //};
        }

        protected override void OnColumnAdded( DataGridViewColumnEventArgs e ) {
            AxHeaderCell header = new AxHeaderCell();
            //if ( e.Column.Index == 1 )
            //    header.ColumnSpan = 2;

            //header.

            header.Value = e.Column.HeaderCell.Value;

            e.Column.HeaderCell = header;
            base.OnColumnAdded( e );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            BufferedGraphicsContext context;
            BufferedGraphics buffer;
            context = BufferedGraphicsManager.Current;

            buffer = context.Allocate( e.Graphics, ClientRectangle );

            Graphics current = buffer.Graphics; // e.Graphics;// 

            current.FillRectangle( new SolidBrush( _normalStyle.BackColor ), ClientRectangle );

            //current.DrawRectangle( new Pen( _normalStyle.BorderColor ), ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

            #region "Заголовки столбцов"
            Rectangle totalHeaderRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ColumnHeadersHeight );

            SolidBrush brush = new SolidBrush( ColumnHeadersDefaultCellStyle.BackColor );

            current.FillRectangle( brush, totalHeaderRect );
            //current.DrawRectangle( new Pen( NormalStyle.BorderColor ), totalHeaderRect );
            //current.DrawLine( new Pen( NormalStyle.BorderColor ), totalHeaderRect.X, totalHeaderRect.Bottom, totalHeaderRect.Right, totalHeaderRect.Bottom );

            int currentSpan = 0;
            for ( int j = 0 ; j < ColumnCount ; j++ ) {
                Rectangle rect = GetCellDisplayRectangle( j, -1, true );

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisPath;

                AxHeaderCell header = ( AxHeaderCell ) Columns[ j ].HeaderCell;

                int firstVisibleColumnIndex = -1;
                if ( FirstDisplayedCell != null )
                    firstVisibleColumnIndex = FirstDisplayedCell.ColumnIndex;

                int horizontalScrollOffset = 0;
                if ( rect.Width < Columns[ j ].Width && firstVisibleColumnIndex > -1 && firstVisibleColumnIndex == j )
                    horizontalScrollOffset = Columns[ j ].Width - rect.Width;

                if ( header.ColumnSpan > 1 && Columns[ j ].Displayed ) {
                    currentSpan = header.ColumnSpan;
                    int spanWidth = 0;

                    for ( int columnIndex = Columns[ j ].Index ; columnIndex < Columns[ j ].Index + currentSpan ; columnIndex++ ) {
                        spanWidth += Columns[ columnIndex ].Width;
                    }

                    Rectangle spanRect = new Rectangle( rect.X - horizontalScrollOffset, rect.Y - 1, spanWidth, rect.Height / 2 );

                    current.DrawRectangle( new Pen( NormalStyle.BorderColor ), spanRect );
                    current.DrawString(
                                    ( ( AxHeaderCell ) Columns[ j ].HeaderCell ).ColumnSpanText,
                                    Font,
                                    new SolidBrush( Color.Black ),
                                    spanRect, format );
                }

                if ( currentSpan > 0 ) {
                    rect = new Rectangle( rect.X - horizontalScrollOffset, rect.Y + rect.Height / 2, Columns[ j ].Width, rect.Height / 2 );
                    currentSpan--;
                }
                else {
                    rect = new Rectangle( rect.X - horizontalScrollOffset, rect.Y, Columns[ j ].Width, rect.Height );
                }

                if ( Columns[ j ].Displayed ) {

                    if ( Columns[ j ].Selected ) {
                        current.FillRectangle( new SolidBrush( DefaultCellStyle.SelectionBackColor ), rect );
                    }

                    //current.DrawRectangle( new Pen( NormalStyle.BorderColor ), rect );
                    current.DrawLine( new Pen( NormalStyle.BorderColor ), rect.Right, rect.Top, rect.Right, rect.Bottom );
                    current.DrawString( Columns[ j ].HeaderText,
                                    Font,
                                    new SolidBrush( NormalStyle.ForeColor ),
                                    rect,
                                    format );
                }
            }
            #endregion

            

            #region "Ячейки"
            if ( FirstDisplayedCell != null ) {

                var visibleRowsCount = DisplayedRowCount( true );
                var firstDisplayedCellRowIndex = FirstDisplayedCell.RowIndex;
                var cellColumnIndex = FirstDisplayedCell.ColumnIndex;
                var lastvisibleRowIndex = ( firstDisplayedCellRowIndex + visibleRowsCount );

                //Console.WriteLine( "firstDisplayedCellRowIndex: " + firstDisplayedCellRowIndex );
                //Console.WriteLine( "lastvisibleRowIndex: " + lastvisibleRowIndex );

                List<Rectangle> selectedCellsBuffer = new List<Rectangle>();

                for ( int x = cellColumnIndex ; x < ColumnCount ; x++ ) {
                    for ( int y = firstDisplayedCellRowIndex ; y < lastvisibleRowIndex ; y++ ) {
                        Rectangle rect = GetCellDisplayRectangle( x, y, false );
                        Rectangle textRect = new Rectangle( rect.X + 1, rect.Y + 1, Columns[ x ].Width - 1, rect.Height - 1 );
                        Rectangle fillRect = new Rectangle( rect.X + 1, rect.Y + 1, rect.Width - 1, rect.Height - 1 );

                        if ( Rows[ y ].Cells[ x ].Selected ) {
                            selectedCellsBuffer.Add( rect );
                            current.FillRectangle( new SolidBrush( DefaultCellStyle.SelectionBackColor ), fillRect );
                            current.DrawLine( new Pen( DefaultCellStyle.SelectionBackColor.Lighten( 10 ) ), rect.Left, rect.Bottom - 1, rect.Right + 1, rect.Bottom - 1 );
                            current.DrawLine( new Pen( DefaultCellStyle.SelectionBackColor.Lighten( 10 ) ), rect.Left, rect.Top + 1, rect.Right + 1, rect.Top + 1 );
                        }
                        else {
                            current.FillRectangle( new SolidBrush( DefaultCellStyle.BackColor ), fillRect );
                        }
                        //else {
                        current.DrawRectangle( new Pen( NormalStyle.BorderColor ), rect );
                        //}


                        if ( y > -1 && x > -1 && Rows[ y ].Cells[ x ].Value != null && Rows[ y ].Cells[ x ] .Displayed ) {
                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Center;
                            format.FormatFlags = StringFormatFlags.NoWrap;
                            //format.

                            current.DrawString(
                                Rows[ y ].Cells[ x ].Value.ToString(),
                                Font,
                                new SolidBrush( Color.Black ),
                                textRect, format );
                        }
                    }

                    if ( !Rows[ firstDisplayedCellRowIndex ].Cells[ x ].Displayed )
                        break;
                }

                foreach ( Rectangle rect in selectedCellsBuffer ) {
                    //current.DrawRectangle( new Pen( Color.Red ), rect );
                }
            }
            #endregion

            #region "Заголовки строк"
            if ( RowHeadersVisible ) {
                current.FillRectangle( new SolidBrush( ColumnHeadersDefaultCellStyle.BackColor ), ClientRectangle.X, ClientRectangle.Y, RowHeadersWidth, ClientRectangle.Height - 1 );

                if ( FirstDisplayedCell != null ) {
                    var firstDisplayedCellRowIndex = FirstDisplayedCell.RowIndex;

                    for ( int j = firstDisplayedCellRowIndex ; j < firstDisplayedCellRowIndex + DisplayedRowCount( true ) ; j++ ) {
                        Rectangle rowRect = GetCellDisplayRectangle( -1, j, true );


                        if ( Rows[ j ].Selected ) {
                            Rectangle selectedRowRect = new Rectangle( rowRect.X + 2, rowRect.Y + 2, rowRect.Width - 3, rowRect.Height - 3 );
                            /*current.FillRectangle( new SolidBrush( DefaultCellStyle.SelectionBackColor.Darken( 20 ) ), selectedRowRect );*/

                            Point[] points = { 
                       new Point( selectedRowRect.X + selectedRowRect.Width -2 - 5, selectedRowRect.Y + selectedRowRect.Height / 2 - 4 ),
                       new Point( selectedRowRect.X + selectedRowRect.Width -2 + 0, selectedRowRect.Y + selectedRowRect.Height / 2 ), 
                       new Point( selectedRowRect.X + selectedRowRect.Width -2 - 5, selectedRowRect.Y + selectedRowRect.Height / 2 + 4 ) };

                            current.SmoothingMode = SmoothingMode.HighQuality;
                            //current.FillPolygon( new SolidBrush( _normalStyle.ForeColor.SetAlpha( 50 ) ), points );
                            current.DrawPolygon( new Pen( _normalStyle.ForeColor.SetAlpha( 150 ) ), points );
                        }


                        if ( j == firstDisplayedCellRowIndex )
                            current.DrawLine( new Pen( NormalStyle.BorderColor ), rowRect.Left, rowRect.Top, ClientRectangle.Width, rowRect.Top );

                        current.DrawLine( new Pen( NormalStyle.BorderColor ), rowRect.Left, rowRect.Bottom, ClientRectangle.Width, rowRect.Bottom );
                    }
                }
            }
            //for ( int j = 0 ; j < DisplayedRowCount( true ) ; j++ ) {
            //    Rectangle rowRect = GetCellDisplayRectangle( -1, j, true );
                
            //}
            #endregion

            if ( RowHeadersVisible ) {
                current.DrawLine( new Pen( _normalStyle.BorderColor ), RowHeadersWidth+1, ClientRectangle.Y, RowHeadersWidth+1, ColumnHeadersHeight );
            }
            current.DrawRectangle( new Pen( _normalStyle.BorderColor ), ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

            buffer.Render();

            sw.Stop();
            Console.WriteLine( "Grid render: " + sw.ElapsedMilliseconds );
        }
        
        protected override void OnResize( EventArgs e ) {
            PerformLayout();
            Refresh();
        }

        protected override void OnScroll( ScrollEventArgs e ) {
            base.OnScroll( e );
            PerformLayout();
            Refresh();
            //Console.WriteLine( "ScroolPos: " + e.NewValue );
            //Console.WriteLine( "HorizontalScrollingOffset: " + HorizontalScrollingOffset );
        }
       
    }
}
