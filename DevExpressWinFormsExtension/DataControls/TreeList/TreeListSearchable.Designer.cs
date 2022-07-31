namespace DevExpressWinFormsExtension.DataControls.TreeList
{
    partial class TreeListSearchable
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
            this.beSearch = new DevExpress.XtraEditors.ButtonEdit();
            this.dataTreeList = new DevExpressWinFormsExtension.DataControls.TreeList.TreeListDev();
            this.panelFilter = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.beSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFilter)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // beSearch
            // 
            this.beSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.beSearch.EditValue = "";
            this.beSearch.Location = new System.Drawing.Point(5, 5);
            this.beSearch.Name = "beSearch";
            this.beSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.beSearch.Properties.NullText = "Filter condition...";
            this.beSearch.Size = new System.Drawing.Size(220, 20);
            this.beSearch.TabIndex = 1;
            this.beSearch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSearch_ButtonClick);
            this.beSearch.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.beSearch_EditValueChanging);
            // 
            // dataTreeList
            // 
            this.dataTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTreeList.Location = new System.Drawing.Point(0, 30);
            this.dataTreeList.Name = "dataTreeList";
            this.dataTreeList.Size = new System.Drawing.Size(228, 217);
            this.dataTreeList.TabIndex = 2;
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.beSearch);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(228, 30);
            this.panelFilter.TabIndex = 13;
            // 
            // TreeListSearchable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTreeList);
            this.Controls.Add(this.panelFilter);
            this.Name = "TreeListSearchable";
            this.Size = new System.Drawing.Size(228, 247);
            ((System.ComponentModel.ISupportInitialize)(this.beSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFilter)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.ButtonEdit beSearch;
        private TreeListDev dataTreeList;
        private DevExpress.XtraEditors.PanelControl panelFilter;
    }
}
