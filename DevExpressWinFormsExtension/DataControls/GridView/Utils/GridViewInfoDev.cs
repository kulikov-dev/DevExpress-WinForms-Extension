namespace DevExpressWinFormsExtension.DataControls.GridView.Utils
{
    /// <summary>
    /// Custom GridViewInfo
    /// </summary>
    internal class GridViewInfoDev : DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="gridView"> Link to GridView </param>
        public GridViewInfoDev(DevExpress.XtraGrid.Views.Grid.GridView gridView) : base(gridView)
        {
        }

        /// <summary>
        /// Высота строки-сумматора группы
        /// </summary>
        public override int GroupFooterCellHeight
        {
            get
            {
                var devGridView = DefaultView as GridViewDev;
                var multiplier = devGridView?.GroupFooterCellHeight ?? 1;

                return base.GroupFooterCellHeight * multiplier;
            }
        }
    }
}
