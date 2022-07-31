namespace DevExpressWinFormsExtension.DataControls
{
    partial class DateDoubleTrackBarControlDev
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
            this.components = new System.ComponentModel.Container();
            this.btnLeftDate = new DevExpress.XtraEditors.SimpleButton();
            this.btnMiddle = new DevExpress.XtraEditors.SimpleButton();
            this.btnRightDate = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.SuspendLayout();
            // 
            // btnLeftDate
            // 
            this.btnLeftDate.AllowFocus = false;
            this.btnLeftDate.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.btnLeftDate.Location = new System.Drawing.Point(3, 3);
            this.btnLeftDate.Name = "btnLeftDate";
            this.btnLeftDate.Size = new System.Drawing.Size(25, 31);
            this.btnLeftDate.TabIndex = 1;
            this.btnLeftDate.Text = "<";
            this.btnLeftDate.ToolTipController = this.toolTipController;
            this.btnLeftDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseDown);
            this.btnLeftDate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseMove);
            this.btnLeftDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseUp);
            // 
            // btnMiddle
            // 
            this.btnMiddle.AllowFocus = false;
            this.btnMiddle.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.btnMiddle.Location = new System.Drawing.Point(34, 3);
            this.btnMiddle.Name = "btnMiddle";
            this.btnMiddle.Size = new System.Drawing.Size(25, 31);
            this.btnMiddle.TabIndex = 1;
            this.btnMiddle.Text = "|";
            this.btnMiddle.ToolTipController = this.toolTipController;
            this.btnMiddle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseDown);
            this.btnMiddle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseMove);
            this.btnMiddle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseUp);
            // 
            // btnRightDate
            // 
            this.btnRightDate.AllowFocus = false;
            this.btnRightDate.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.btnRightDate.Location = new System.Drawing.Point(65, 3);
            this.btnRightDate.Name = "btnRightDate";
            this.btnRightDate.Size = new System.Drawing.Size(25, 31);
            this.btnRightDate.TabIndex = 1;
            this.btnRightDate.Text = ">";
            this.btnRightDate.ToolTipController = this.toolTipController;
            this.btnRightDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseDown);
            this.btnRightDate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseMove);
            this.btnRightDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragButton_MouseUp);
            // 
            // DateDoubleTrackBarControlDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLeftDate);
            this.Controls.Add(this.btnMiddle);
            this.Controls.Add(this.btnRightDate);
            this.DoubleBuffered = true;
            this.Name = "DateDoubleTrackBarControlDev";
            this.Size = new System.Drawing.Size(253, 61);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnLeftDate;
        private DevExpress.XtraEditors.SimpleButton btnMiddle;
        private DevExpress.XtraEditors.SimpleButton btnRightDate;
        private DevExpress.Utils.ToolTipController toolTipController;
    }
}
