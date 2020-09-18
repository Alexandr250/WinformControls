using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WinformControls.Helpers;
using System.Drawing.Drawing2D;

namespace WinformControls {
    public partial class AxBorderCombo : UserControl {
        private bool _isSelected;
        private bool _isButtonSelected = false;
        private bool _error;
        private int _borderPadding;
        private int _borderRadius = 2;
        private Color _buttonColor;
        private Color _buttonSelectedColor;
        private ToolStripDropDown _toolStripDropDown = new ToolStripDropDown();
        private DataGridView _dataGridView = new DataGridView();


        #region [ Properties ]
        public Color BorderColor { get; set; }
        public Color BorderErrorColor { get; set; }
        public Color BorderHoverColor { get; set; }
        
        public Color ButtonColor {
            get {
                return _buttonColor;
            }
            set {
                _buttonColor = value;
                Refresh();
            }
        }

        public Color ButtonSelectedColor {
            get {
                return _buttonSelectedColor;
            }
            set {
                _buttonSelectedColor = value;
                Refresh();
            }
        }
        
        public Boolean Error {
            get {
                return _error;
            }
            set {
                _error = value;
                Refresh();
            }
        }
        public Boolean Multiline {
            get {
                return textBoxContent.Multiline;
            }
            set {
                textBoxContent.Multiline = value;
                Refresh();
            }
        }

        [Browsable( true )]
        public new string Text {
            get {
                return textBoxContent.Text;
            }
            set {
                textBoxContent.Text = value;
                Refresh();
            }
        }

        [Browsable( true )]
        public new Font Font {
            get {
                return textBoxContent.Font;
            }
            set {
                base.Font = value;
                textBoxContent.Font = value;
                Refresh();
            }
        }

        public int BorderPadding {
            get {
                return _borderPadding;
            }
            set {
                _borderPadding = value;
                Padding = new Padding( _borderPadding );
                AxBorderComboResize( this, null );
                Refresh();
            }
        }
        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        }

        public int BorderThickness { get; set; }

        private string[] _items;

        public string[] Items {
            get {
                return _items;
            }
            set {
                _items = value;

                _dataGridView.Rows.Clear();

                if ( _items != null )
                    foreach ( string item in _items ) _dataGridView.Rows.Add( new string[] { item } );
            }
        }

        #endregion

        #region [ Events ]

        [Browsable( true )]
        public event EventHandler TextChanged {
            add {
                textBoxContent.TextChanged += value;
            }
            remove {
                textBoxContent.TextChanged -= value;
            }
        }

        [Browsable( true )]
        public event EventHandler TextBoxEnter {
            add {
                textBoxContent.Enter += value;
            }
            remove {
                textBoxContent.Enter -= value;
            }
        }

        [Browsable( true )]
        public event EventHandler TextBoxLeave {
            add {
                textBoxContent.Leave += value;
            }
            remove {
                textBoxContent.Leave -= value;
            }
        }

        [Browsable( true )]
        public event Action ButtonMouseDown;

        #endregion

        public AxBorderCombo() {
            SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );

            InitializeComponent();

            
            Error = false;
            BorderColor = Color.Black;
            BorderErrorColor = Color.Red;
            BorderHoverColor = Color.Blue;
            BorderThickness = 1;
            BorderPadding = 3;
            ButtonColor = Color.Black;
            Height = textBoxContent.Height + BorderPadding * 2;
            AxBorderComboResize( this, null );
            textBoxContent.Resize += ( s, e ) => AxBorderComboResize( s, e );

           
            //_controlHost = new DropDownComboList();
            //_controlHost.AutoSize = false;
            //_controlHost.BackColor = Color.Green;
            //_controlHost.Dock = DockStyle.Fill;
            _toolStripDropDown.Height = 200;


            //_listView1.Columns.Add( "" );
            //_listView1.FullRowSelect = true;
            //_listView1.GridLines = true;
            //_listView1.HeaderStyle = ColumnHeaderStyle.None;            
            //_listView1.UseCompatibleStateImageBehavior = false;
            //_listView1.View = View.Details;
            //_listView1.Width = Width;
            //_listView1.Columns[ 0 ].Width = _listView1.Width;
            //_listView1.Left = 0;
            //_listView1.Visible = true;
            //_listView1.BorderStyle = BorderStyle.None;


            DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
            Column1.HeaderText = "Column1";
            Column1.Name = "Column1";

            _dataGridView.BackgroundColor = Color.White;
            _dataGridView.BorderStyle = BorderStyle.None;
            _dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dataGridView.ColumnHeadersVisible = false;
            _dataGridView.Width = Width;
            _dataGridView.Columns.AddRange( new DataGridViewColumn[] { Column1 } );
            Column1.Width = _dataGridView.Width;
            
            _dataGridView.RowHeadersVisible = false;
            _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = false;


            ToolStripControlHost _controlHost = new ToolStripControlHost( _dataGridView );
            //_controlHost.AutoSize = false;
            //_controlHost.Height = dropDownMenu.Height - 4;
            _controlHost.Width = 200;
            //_controlHost.Dock = DockStyle.Fill;
            _controlHost.Padding = _controlHost.Margin = Padding.Empty;

            _toolStripDropDown.Items.Add( _controlHost );
            
            
        }

        private void AxBorderComboResize( object sender, EventArgs e ) {
            Height = textBoxContent.Height + BorderPadding * 2;
            textBoxContent.Left = BorderPadding;
            textBoxContent.Width = Width - Height - BorderPadding * 2;
            textBoxContent.Top = Height / 2 - textBoxContent.Height / 2;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - BorderThickness, ClientRectangle.Height - BorderThickness );
            g.FillPath( new SolidBrush( Color.White ), ExtendedForms.RoundedRect( rect, BorderRadius ) );

            Rectangle buttonRect = new Rectangle( ClientRectangle.Width - Height, ClientRectangle.Y, Height - 1, Height - 1 );
            

            if ( !Error ) {
                if ( _isSelected ) {
                    g.DrawPath( new Pen( BorderHoverColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
                    g.DrawLine( new Pen( BorderHoverColor, BorderThickness ), ClientRectangle.Width - Height, ClientRectangle.Y, ClientRectangle.Width - Height, ClientRectangle.Height );

                    g.FillPath( new SolidBrush( ButtonSelectedColor ), ExtendedForms.RoundedRightRect( buttonRect, BorderRadius ) );
                }
                else {
                    g.DrawPath( new Pen( BorderColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
                    g.DrawLine( new Pen( BorderColor, BorderThickness ), ClientRectangle.Width - Height, ClientRectangle.Y, ClientRectangle.Width - Height, ClientRectangle.Height );

                    g.FillPath( new SolidBrush( ButtonColor ), ExtendedForms.RoundedRightRect( buttonRect, BorderRadius ) );
                }
            }
            else {
                g.DrawPath( new Pen( BorderErrorColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
                g.DrawLine( new Pen( BorderErrorColor, BorderThickness ), ClientRectangle.Width - Height, ClientRectangle.Y, ClientRectangle.Width - Height, ClientRectangle.Height );
            }

            g.DrawLine( new Pen( ForeColor, 2 ), buttonRect.X + buttonRect.Width / 2 - 4, buttonRect.Height / 2 - 2, buttonRect.X + buttonRect.Width / 2, buttonRect.Height / 2 + 2 );
            g.DrawLine( new Pen( ForeColor, 2 ), buttonRect.X + buttonRect.Width / 2 + 4, buttonRect.Height / 2 - 2, buttonRect.X + buttonRect.Width / 2, buttonRect.Height / 2 + 2 );
        }

        private void AxBorderComboMouseEnter( object sender, EventArgs e ) {
            _isSelected = true;
            Refresh();
        }

        private void AxBorderComboMouseLeave( object sender, EventArgs e ) {
            _isSelected = false;
            Refresh();
        }

        private void TextBoxContentMouseEnter( object sender, EventArgs e ) {
            _isSelected = true;
            Refresh();
        }

        private void TextBoxContentMouseLeave( object sender, EventArgs e ) {
            _isSelected = false;
            Refresh();
        }

        private void AxBorderComboMouseDown( object sender, MouseEventArgs e ) {
            if ( e.X > Width - Height ) {
                if ( ButtonMouseDown != null )
                    ButtonMouseDown();


                _toolStripDropDown.Width = Width;
                //_comboList.Height = dropDownMenu.Height;
                //_comboList.Width = dropDownMenu.Width;
                Point ptLowerRight = new Point( 0, Bounds.Height );
                ptLowerRight = PointToScreen( ptLowerRight );
                _toolStripDropDown.Show( ptLowerRight );
                
                //if ( _contextMenu..Visible == true )
                //    _toolContainer.Hide();
                //else
                //    _toolContainer.Show();
            }
        }

        public override void Refresh() {
            base.Refresh();
        }
    }
}
