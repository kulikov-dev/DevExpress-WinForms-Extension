
namespace DevExpressWinFormsExtension.DataControls
{
    partial class GroupControlCheckedDev
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
            this.header = new DevExpress.XtraEditors.PanelControl();
            this.checkEdit = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.header)).BeginInit();
            this.header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Controls.Add(this.checkEdit);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(253, 19);
            this.header.TabIndex = 0;
            // 
            // checkEdit
            // 
            this.checkEdit.AutoSizeInLayoutControl = true;
            this.checkEdit.EditValue = true;
            this.checkEdit.Location = new System.Drawing.Point(5, 0);
            this.checkEdit.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkEdit.Name = "checkEdit";
            this.checkEdit.Properties.AllowFocused = false;
            this.checkEdit.Properties.AutoWidth = true;
            this.checkEdit.Properties.Caption = "GroupBox Title";
            this.checkEdit.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.CheckBox;
            this.checkEdit.Size = new System.Drawing.Size(92, 19);
            this.checkEdit.TabIndex = 1;
            this.checkEdit.CheckedChanged += new System.EventHandler(this.CheckEditCheckedChanged);
            // 
            // GroupControlWithCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.header);
            this.Name = "GroupControlWithCheckBox";
            this.Size = new System.Drawing.Size(253, 86);
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.GroupControlWithCheckBoxControlAdded);
            ((System.ComponentModel.ISupportInitialize)(this.header)).EndInit();
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl header;
        private DevExpress.XtraEditors.CheckEdit checkEdit;
    }
}
