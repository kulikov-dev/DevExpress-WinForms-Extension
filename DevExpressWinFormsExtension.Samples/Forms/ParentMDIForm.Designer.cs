namespace DevExpressWinFormsExtension.Samples.Forms
{
    partial class ParentMDIForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barEditMDIFormsSelector = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemPopupMatrix = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.popupContainerControlMatrix = new DevExpress.XtraEditors.PopupContainerControl();
            this.matrixGridControl = new DevExpressWinFormsExtension.Utils.MdiManager.MatrixGridControl();
            this.ribbonPageMenu = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupMatrix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControlMatrix)).BeginInit();
            this.popupContainerControlMatrix.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barEditMDIFormsSelector});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 2;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageMenu});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPopupMatrix});
            this.ribbonControl1.Size = new System.Drawing.Size(800, 150);
            // 
            // barEditMDIFormsSelector
            // 
            this.barEditMDIFormsSelector.Caption = "MDI form\'s count:";
            this.barEditMDIFormsSelector.Edit = this.repositoryItemPopupMatrix;
            this.barEditMDIFormsSelector.EditWidth = 70;
            this.barEditMDIFormsSelector.Id = 1;
            this.barEditMDIFormsSelector.Name = "barEditMDIFormsSelector";
            // 
            // repositoryItemPopupMatrix
            // 
            this.repositoryItemPopupMatrix.AutoHeight = false;
            this.repositoryItemPopupMatrix.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupMatrix.Name = "repositoryItemPopupMatrix";
            this.repositoryItemPopupMatrix.PopupControl = this.popupContainerControlMatrix;
            this.repositoryItemPopupMatrix.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.repositoryItemPopupMatrix_CustomDisplayText);
            // 
            // popupContainerControlMatrix
            // 
            this.popupContainerControlMatrix.Controls.Add(this.matrixGridControl);
            this.popupContainerControlMatrix.Location = new System.Drawing.Point(481, 175);
            this.popupContainerControlMatrix.Name = "popupContainerControlMatrix";
            this.popupContainerControlMatrix.Size = new System.Drawing.Size(200, 172);
            this.popupContainerControlMatrix.TabIndex = 33;
            // 
            // matrixGridControl
            // 
            this.matrixGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matrixGridControl.Location = new System.Drawing.Point(0, 0);
            this.matrixGridControl.Name = "matrixGridControl";
            this.matrixGridControl.SelectedSize = new System.Drawing.Size(1, 1);
            this.matrixGridControl.SelectionColor = System.Drawing.Color.DodgerBlue;
            this.matrixGridControl.Size = new System.Drawing.Size(200, 172);
            this.matrixGridControl.TabIndex = 0;
            this.matrixGridControl.OnSizeSelected += new System.EventHandler(this.matrixGridControl_OnSizeSelected);
            // 
            // ribbonPageMenu
            // 
            this.ribbonPageMenu.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup});
            this.ribbonPageMenu.Name = "ribbonPageMenu";
            this.ribbonPageMenu.Text = "Form\'s menu";
            // 
            // ribbonPageGroup
            // 
            this.ribbonPageGroup.ItemLinks.Add(this.barEditMDIFormsSelector);
            this.ribbonPageGroup.Name = "ribbonPageGroup";
            this.ribbonPageGroup.Text = "Default";
            // 
            // ParentMDIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.popupContainerControlMatrix);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Name = "ParentMDIForm";
            this.Text = "MdiParentForm";
            this.ClientSizeChanged += new System.EventHandler(this.ParentMDIForm_ClientSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupMatrix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControlMatrix)).EndInit();
            this.popupContainerControlMatrix.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageMenu;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup;
        private DevExpress.XtraBars.BarEditItem barEditMDIFormsSelector;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupMatrix;
        private DevExpress.XtraEditors.PopupContainerControl popupContainerControlMatrix;
        private DevExpressWinFormsExtension.Utils.MdiManager.MatrixGridControl matrixGridControl;
    }
}