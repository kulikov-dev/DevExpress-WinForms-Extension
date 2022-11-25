using DevExpress.XtraGrid;

namespace DevExpressWinFormsExtension.Utils.MdiManager
{
    partial class MatrixGridControl
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
            this.gridControlMatrix = new DevExpress.XtraGrid.GridControl();
            this.gridViewMatrix = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMatrix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMatrix)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlMatrix
            // 
            this.gridControlMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlMatrix.Location = new System.Drawing.Point(0, 0);
            this.gridControlMatrix.MainView = this.gridViewMatrix;
            this.gridControlMatrix.Name = "gridControlMatrix";
            this.gridControlMatrix.Size = new System.Drawing.Size(441, 394);
            this.gridControlMatrix.TabIndex = 0;
            this.gridControlMatrix.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMatrix});
            this.gridControlMatrix.Click += new System.EventHandler(this.gridControlMatrix_Click);
            this.gridControlMatrix.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridControlMatrix_MouseMove);
            // 
            // gridViewMatrix
            // 
            this.gridViewMatrix.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewMatrix.GridControl = this.gridControlMatrix;
            this.gridViewMatrix.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridViewMatrix.Name = "gridViewMatrix";
            this.gridViewMatrix.OptionsBehavior.Editable = false;
            this.gridViewMatrix.OptionsBehavior.ReadOnly = true;
            this.gridViewMatrix.OptionsBehavior.SmartVertScrollBar = false;
            this.gridViewMatrix.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewMatrix.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewMatrix.OptionsCustomization.AllowFilter = false;
            this.gridViewMatrix.OptionsCustomization.AllowGroup = false;
            this.gridViewMatrix.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewMatrix.OptionsCustomization.AllowSort = false;
            this.gridViewMatrix.OptionsDetail.AllowZoomDetail = false;
            this.gridViewMatrix.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewMatrix.OptionsDetail.ShowDetailTabs = false;
            this.gridViewMatrix.OptionsDetail.SmartDetailExpand = false;
            this.gridViewMatrix.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewMatrix.OptionsFilter.AllowFilterEditor = false;
            this.gridViewMatrix.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewMatrix.OptionsHint.ShowCellHints = false;
            this.gridViewMatrix.OptionsHint.ShowColumnHeaderHints = false;
            this.gridViewMatrix.OptionsHint.ShowFooterHints = false;
            this.gridViewMatrix.OptionsLayout.Columns.AddNewColumns = false;
            this.gridViewMatrix.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gridViewMatrix.OptionsLayout.Columns.StoreLayout = false;
            this.gridViewMatrix.OptionsMenu.EnableColumnMenu = false;
            this.gridViewMatrix.OptionsMenu.EnableFooterMenu = false;
            this.gridViewMatrix.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewMatrix.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewMatrix.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewMatrix.OptionsNavigation.AutoMoveRowFocus = false;
            this.gridViewMatrix.OptionsNavigation.UseOfficePageNavigation = false;
            this.gridViewMatrix.OptionsNavigation.UseTabKey = false;
            this.gridViewMatrix.OptionsPrint.AutoWidth = false;
            this.gridViewMatrix.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMatrix.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewMatrix.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewMatrix.OptionsSelection.UseIndicatorForSelection = false;
            this.gridViewMatrix.OptionsView.RowAutoHeight = true;
            this.gridViewMatrix.OptionsView.ShowColumnHeaders = false;
            this.gridViewMatrix.OptionsView.ShowDetailButtons = false;
            this.gridViewMatrix.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewMatrix.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridViewMatrix.OptionsView.ShowGroupPanel = false;
            this.gridViewMatrix.OptionsView.ShowIndicator = false;
            this.gridViewMatrix.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMatrix.RowHeight = 5;
            this.gridViewMatrix.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridViewMatrix.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridViewMatrix.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewMatrix_CustomDrawCell);
            this.gridViewMatrix.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gridViewMatrix_MouseWheel);
            // 
            // MatrixGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlMatrix);
            this.Name = "MatrixGridControl";
            this.Size = new System.Drawing.Size(441, 394);
            this.Resize += new System.EventHandler(this.MatrixGridControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMatrix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMatrix)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl gridControlMatrix;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMatrix;
    }
}
