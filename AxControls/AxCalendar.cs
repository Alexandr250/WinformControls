using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformControls.AxControls {
    public class AxCalendar : Control {
        #region [ Fields ]
        private DateTime[,] _calendar;

        private DateTime _currentDate = new DateTime( 2021, 1, 1 );
        private const int _daysInWeek = 7;
        private const int _weeksInMonth = 6;

        private Color _linesColor = Color.Black;
        private Color _foreWeekendColor = Color.Red;
        private Color _foreInactiveColor = Color.Gray;
        private Color _foreCurrentDayColor = Color.Black;

        private Color _backWeekendColor = Color.WhiteSmoke;
        private Color _backInactiveColor = Color.WhiteSmoke;
        private Color _backHoverColor = Color.WhiteSmoke;
        private int _backHoverColorAlpha = 255;
        private Color _backCurrentDayColor = Color.WhiteSmoke;

        private Point _activeCell = new Point( 0, 0 ); 
        #endregion

        #region [ Color properties ]
        public Color LinesColor {
            get => _linesColor;
            set {
                _linesColor = value;
                Refresh();
            }
        }

        public Color ForeWeekendColor {
            get => _foreWeekendColor;
            set {
                _foreWeekendColor = value;
                Refresh();
            }
        }

        public Color ForeInactiveColor {
            get => _foreInactiveColor;
            set {
                _foreInactiveColor = value;
                Refresh();
            }
        }

        public Color ForeCurrentDayColor {
            get => _foreCurrentDayColor;
            set {
                _foreCurrentDayColor = value;
                Refresh();
            }
        }

        public Color BackWeekendColor {
            get => _backWeekendColor;
            set {
                _backWeekendColor = value;
                Refresh();
            }
        }

        public Color BackInactiveColor {
            get => _backInactiveColor;
            set {
                _backInactiveColor = value;
                Refresh();
            }
        }

        public Color BackHoverColor {
            get => _backHoverColor;
            set {
                _backHoverColor = value;
                Refresh();
            }
        }

        public int BackHoverColorAlpha {
            get => _backHoverColorAlpha;
            set {
                _backHoverColorAlpha = value;
                Refresh();
            }
        }

        public Color BackCurrentDayColor {
            get => _backCurrentDayColor;
            set {
                _backCurrentDayColor = value;
                Refresh();
            }
        }
        #endregion

        public DateTime CurrentDate {
            get => _currentDate;
            set => _currentDate = value;
        }

        [Browsable( true )]
        public event Action<DateTime> DateChanged;

        public AxCalendar() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint, true );

            _calendar = new DateTime[_daysInWeek, _weeksInMonth];
            RebuildCalendar();

            BackColor = Color.White;
        }

        private void RebuildCalendar() {
            int year = _currentDate.Year;

            int firstDayOfWeek = ( int )new DateTime( _currentDate.Year, _currentDate.Month, 1 ).DayOfWeek - 1;
            int month = firstDayOfWeek > 0 ? _currentDate.Month - 1 : _currentDate.Month;

            if( month < 1 ) {
                year--;
                month = 12;
            }

            int day = firstDayOfWeek > 0 ? DateTime.DaysInMonth( _currentDate.Year, month ) - firstDayOfWeek : 1;

            int lastDayOfMonth = DateTime.DaysInMonth( _currentDate.Year, month );

            for ( int y = 0; y < _weeksInMonth; y++ ) {
                for ( int x = 0; x < _daysInWeek; x++ ) {
                    if ( day > lastDayOfMonth ) {
                        day = 1;
                        month++;

                        if ( month > 12 ) {
                            month = 1;
                            year++;
                        }

                        lastDayOfMonth = DateTime.DaysInMonth( _currentDate.Year, month );
                    }

                    _calendar[ x, y ] = new DateTime( year, month, day );

                    day++;
                }
            }
        }

        protected override void OnMouseMove( MouseEventArgs e ) {
            base.OnMouseMove( e );

            int cellWidth = Width / _daysInWeek;
            int cellHeight = Height / _weeksInMonth;

            int x = e.Location.X / cellWidth;
            int y = e.Location.Y / cellHeight;

            _activeCell = new Point( x, y );
            Refresh();
        }

        protected override void OnMouseClick( MouseEventArgs e ) {
            base.OnMouseClick( e );

            int cellWidth = Width / _daysInWeek;
            int cellHeight = Height / _weeksInMonth;

            int x = e.Location.X / cellWidth;
            int y = e.Location.Y / cellHeight;

            if ( _currentDate.Month != _calendar[x, y].Month ) {
                _currentDate = _calendar[x, y];
                RebuildCalendar();
            }
            else {
                _currentDate = _calendar[x, y];
            }

            DateChanged?.Invoke( _currentDate );

            _activeCell = new Point( x, y );
            Refresh();
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;

            int cellWidth = Width / _daysInWeek;
            int cellHeight = Height / _weeksInMonth;            

            StringFormat sf = new StringFormat {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };
            
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            for ( int y = 0; y < _weeksInMonth; y++ ) {
                for ( int x = 0; x < _daysInWeek; x++ ) {
                    
                    Brush brush = x < 5 ? new SolidBrush( ForeColor ) : new SolidBrush( ForeWeekendColor );

                    if ( _calendar[ x, y ].Month != _currentDate.Month ) {
                        brush = new SolidBrush( ForeInactiveColor );

                        Rectangle rect1 = new Rectangle( x * cellWidth, y * cellHeight, cellWidth, cellHeight );
                        g.FillRectangle( new SolidBrush( BackInactiveColor ), rect1 );
                    }
                    else {
                        if ( x > 4 ) {
                            Rectangle rect1 = new Rectangle( x * cellWidth, y * cellHeight, cellWidth, cellHeight );
                            g.FillRectangle( new SolidBrush( BackWeekendColor ), rect1 );
                        }
                    }

                    if ( _calendar[x, y].Month == _currentDate.Month && _calendar[x, y].Day == _currentDate.Day ) {
                        brush = new SolidBrush( ForeCurrentDayColor );

                        Rectangle rect1 = new Rectangle( x * cellWidth, y * cellHeight, cellWidth, cellHeight );
                        g.FillRectangle( new SolidBrush( BackCurrentDayColor ), rect1 );
                    }

                    if ( x == _activeCell.X && y == _activeCell.Y ) {
                        Rectangle rect1 = new Rectangle( x * cellWidth, y * cellHeight, cellWidth, cellHeight );
                        g.FillRectangle( new SolidBrush( Color.FromArgb( BackHoverColorAlpha, BackHoverColor ) ), rect1 );
                    }

                    Rectangle rect = new Rectangle( x * cellWidth, y * cellHeight, cellWidth, cellHeight );
                    g.DrawString( _calendar[x, y].Day.ToString(), Font, brush, rect, sf );
                }
            }

            for ( int x = 1; x < _daysInWeek; x++ ) {
                g.DrawLine( new Pen( LinesColor, 1 ), x * cellWidth, 0, x * cellWidth, Height );
            }
            for ( int y = 1; y < _weeksInMonth; y++ ) {
                g.DrawLine( new Pen( LinesColor, 1 ), 0, y * cellHeight, Width, y * cellHeight );
            }
        }
    }
}
