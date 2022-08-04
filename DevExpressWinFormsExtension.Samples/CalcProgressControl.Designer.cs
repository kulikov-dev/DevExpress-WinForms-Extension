namespace DevExpressWinFormsExtension.Samples
{
    partial class CalcProgressControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartCalc = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnStartCalc
            // 
            this.btnStartCalc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnStartCalc.Location = new System.Drawing.Point(138, 41);
            this.btnStartCalc.Name = "btnStartCalc";
            this.btnStartCalc.Size = new System.Drawing.Size(156, 22);
            this.btnStartCalc.TabIndex = 9;
            this.btnStartCalc.Text = "Start long calculations...";
            this.btnStartCalc.Click += new System.EventHandler(this.btnStartCalc_Click);
            // 
            // CalcProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnStartCalc);
            this.Name = "CalcProgressControl";
            this.Size = new System.Drawing.Size(434, 104);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnStartCalc;
    }
}
