using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WinformControls.AxControls {
    public class AxMenuStrip : MenuStrip {
        private AxToolStripRenderer _renderer;

        public int ToolStripGradientAngle {
            get {
                return _renderer.ToolStripGradientAngle;
            }
            set {
                _renderer.ToolStripGradientAngle = value;
            }
        }

        public int ToolStripBackgroundAlpha {
            get {
                return _renderer.ToolStripBackgroundAlpha;
            }
            set {
                _renderer.ToolStripBackgroundAlpha = value;
            }
        }

        public int ToolStripButtonBackgroundAlpha {
            get {
                return _renderer.ToolStripButtonBackgroundAlpha;
            }
            set {
                _renderer.ToolStripButtonBackgroundAlpha = value;
                this.Refresh();
            }
        }

        public int ToolStripButtonBorderAlpha {
            get {
                return _renderer.ToolStripButtonBorderAlpha;
            }
            set {
                _renderer.ToolStripButtonBorderAlpha = value;
                this.Refresh();
            }
        }

        public Color ToolStripTopGradienColor {
            get {
                return _renderer.ToolStripTopGradienColor;
            }
            set {
                _renderer.ToolStripTopGradienColor = value;
            }
        }

        public Color ToolStripBottomGradienColor {
            get {
                return _renderer.ToolStripBottomGradienColor;
            }
            set {
                _renderer.ToolStripBottomGradienColor = value;
            }
        }

        public Color ToolStripBorderColor {
            get {
                return _renderer.ToolStripBorderColor;
            }
            set {
                _renderer.ToolStripBorderColor = value;
                this.Refresh();
            }
        }

        public Color ToolStripButtonHoverColor {
            get {
                return _renderer.ToolStripButtonHoverColor;
            }
            set {
                _renderer.ToolStripButtonHoverColor = value;
                this.Refresh();
            }
        }

        public Color ToolStripButtonBorderHoverColor {
            get {
                return _renderer.ToolStripButtonBorderHoverColor;
            }
            set {
                _renderer.ToolStripButtonBorderHoverColor = value;
                this.Refresh();
            }
        }

        public bool ToolStripDrawTopShadow {
            get {
                return _renderer.ToolStripDrawTopShadow;
            }
            set {
                _renderer.ToolStripDrawTopShadow = value;
            }
        }

        public bool ToolStripDrawBottomShadow {
            get {
                return _renderer.ToolStripDrawBottomShadow;
            }
            set {
                _renderer.ToolStripDrawBottomShadow = value;
            }
        }

        public bool ToolStripDrawTopBorderPart {
            get {
                return _renderer.ToolStripDrawTopBorderPart;
            }
            set {
                _renderer.ToolStripDrawTopBorderPart = value;
            }
        }

        public bool ToolStripDrawBorder {
            get {
                return _renderer.ToolStripDrawBorder;
            }
            set {
                _renderer.ToolStripDrawBorder = value;
            }
        }

        public Color ItemForeColor {
            get {
                return _renderer.ItemForeColor;
            }
            set {
                _renderer.ItemForeColor = value;
            }
        }

        public AxMenuStrip() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true );

            _renderer = new AxToolStripRenderer();
            this.Renderer = _renderer;
        }
    }

    
}
