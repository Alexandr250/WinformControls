namespace WinformControls.AxControls {
    partial class AxBorderDatePicker {
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
            this.borderlessDatePicker1 = new WinformControls.BorderlessDatePicker();
            this.SuspendLayout();
            // 
            // borderlessDatePicker1
            // 
            this.borderlessDatePicker1.ButtonBackColor = System.Drawing.Color.Empty;
            this.borderlessDatePicker1.ButtonBorderColor = System.Drawing.Color.Empty;
            this.borderlessDatePicker1.ButtonBorderHoverColor = System.Drawing.Color.Empty;
            this.borderlessDatePicker1.ButtonHoverColor = System.Drawing.Color.Empty;
            this.borderlessDatePicker1.CustomFormat = "dd.MM.yyyy";
            this.borderlessDatePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.borderlessDatePicker1.Location = new System.Drawing.Point(15, 13);
            this.borderlessDatePicker1.Name = "borderlessDatePicker1";
            this.borderlessDatePicker1.Size = new System.Drawing.Size(200, 20);
            this.borderlessDatePicker1.TabIndex = 0;
            this.borderlessDatePicker1.ValueChanged += new System.EventHandler(this.borderlessDatePicker1_ValueChanged);
            this.borderlessDatePicker1.MouseEnter += new System.EventHandler(this.borderlessDatePicker1_MouseEnter);
            this.borderlessDatePicker1.MouseLeave += new System.EventHandler(this.borderlessDatePicker1_MouseLeave);
            // 
            // AxBorderDatePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderlessDatePicker1);
            this.Name = "AxBorderDatePicker";
            this.Size = new System.Drawing.Size(359, 150);
            this.MouseEnter += new System.EventHandler(this.AxBorderDatePicker_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.AxBorderDatePicker_MouseLeave);
            this.Resize += new System.EventHandler(this.AxBorderDatePicker_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private BorderlessDatePicker borderlessDatePicker1;

    }
}
