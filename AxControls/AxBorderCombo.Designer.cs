namespace WinformControls {
    partial class AxBorderCombo {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxContent
            // 
            this.textBoxContent.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxContent.Location = new System.Drawing.Point(57, 35);
            this.textBoxContent.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(174, 13);
            this.textBoxContent.TabIndex = 0;
            this.textBoxContent.MouseEnter += new System.EventHandler(this.TextBoxContentMouseEnter);
            this.textBoxContent.MouseLeave += new System.EventHandler(this.TextBoxContentMouseLeave);
            // 
            // AxBorderCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxContent);
            this.Name = "AxBorderCombo";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(288, 83);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxBorderComboMouseDown);
            this.MouseEnter += new System.EventHandler(this.AxBorderComboMouseEnter);
            this.MouseLeave += new System.EventHandler(this.TextBoxContentMouseLeave);
            this.Resize += new System.EventHandler(this.AxBorderComboResize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxContent;
    }
}
