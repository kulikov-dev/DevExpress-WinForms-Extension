namespace DevExpressWinFormsExtension.DataControls.Editors
{
    partial class InputBoxValidableDev
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
            this.editValue = new DevExpress.XtraEditors.TextEdit();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.labelText = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.editValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editValue
            // 
            this.editValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editValue.Location = new System.Drawing.Point(65, 8);
            this.editValue.Name = "editValue";
            this.editValue.Properties.Appearance.BackColor = System.Drawing.Color.MistyRose;
            this.editValue.Properties.Appearance.Options.UseBackColor = true;
            this.editValue.Size = new System.Drawing.Size(236, 20);
            this.editValue.TabIndex = 14;
            this.editValue.ToolTipController = this.toolTipController;
            this.editValue.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
            // 
            // toolTipController
            // 
            this.toolTipController.AutoPopDelay = 3000;
            this.toolTipController.ToolTipLocation = DevExpress.Utils.ToolTipLocation.BottomCenter;
            // 
            // labelText
            // 
            this.labelText.Location = new System.Drawing.Point(7, 11);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(52, 13);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "Значение:";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnOk);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.editValue);
            this.panelControl1.Controls.Add(this.labelText);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.MaximumSize = new System.Drawing.Size(600, 36);
            this.panelControl1.MinimumSize = new System.Drawing.Size(306, 36);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(306, 36);
            this.panelControl1.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(145, 32);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 15;
            this.btnOk.Tag = "1";
            this.btnOk.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(226, 32);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Tag = "2";
            this.btnCancel.Text = "Cancel";
            // 
            // InputBoxValidableDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.MaximumSize = new System.Drawing.Size(500, 36);
            this.MinimumSize = new System.Drawing.Size(306, 36);
            this.Name = "InputBoxValidableDev";
            this.Size = new System.Drawing.Size(306, 36);
            ((System.ComponentModel.ISupportInitialize)(this.editValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelText;
        private DevExpress.XtraEditors.TextEdit editValue;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.Utils.ToolTipController toolTipController;
        public DevExpress.XtraEditors.SimpleButton btnOk;
        public DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
