using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WinformControls {
    public class NonFocusableForm : Form {
        [DllImport( "User32.dll" )]
        static extern bool MoveWindow( IntPtr handle, int x, int y, int width, int height, bool redraw );

        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private Button button1;
        private const int MA_NOACTIVATE = 0x0003;
        private const int WM_NCACTIVATE = 0x86;
        public void SetLocation( int x, int y ) {
            MoveWindow( this.Handle, x, y, this.Width, this.Height, false );
        }

        public NonFocusableForm() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_NOACTIVATE;
                return createParams;
            }
        }

        protected override bool ShowWithoutActivation {
            get {
                return true;
            }
        }

        protected override void WndProc( ref Message m ) {
            
            if ( m.Msg == WM_MOUSEACTIVATE ) {
                m.Result = ( IntPtr ) MA_NOACTIVATE;
                return;
            }
            //Console.WriteLine( m.Msg );
            base.WndProc( ref m );

            if ( m.Msg == WM_NCACTIVATE ) {
                if ( this.Visible ) {
                    if ( !this.RectangleToScreen( this.DisplayRectangle ).Contains( Cursor.Position ) )
                        this.Close();
                }
            }
        }

        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 237);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NonFocusableForm
            // 
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Name = "NonFocusableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Deactivate += new System.EventHandler(this.NonFocusableForm_Deactivate);
            this.Load += new System.EventHandler(this.NonFocusableForm_Load);
            this.Leave += new System.EventHandler(this.NonFocusableForm_Leave);
            this.ResumeLayout(false);

        }

        private void NonFocusableForm_Load( object sender, EventArgs e ) {

        }

        private void NonFocusableForm_Deactivate( object sender, EventArgs e ) {
            
        }

        private void NonFocusableForm_Leave( object sender, EventArgs e ) {
//this.Hide();
        }

        private void button1_Click( object sender, EventArgs e ) {

        }

        protected override void OnLostFocus( EventArgs e ) {
            
            //this.Hide();
            //base.OnLostFocus( e );
        }
    }
}
