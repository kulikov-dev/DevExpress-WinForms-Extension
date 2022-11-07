namespace DevExpressWinFormsExtension.DataControls.GridView.Utils
{
    /// <summary>
    /// Custom BandedGridViewInfo
    /// </summary>
    internal class BandedGridViewInfoDev : DevExpress.XtraGrid.Views.BandedGrid.ViewInfo.BandedGridViewInfo
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="gridView"> Link to GridView </param>
        public BandedGridViewInfoDev(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gridView)
            : base(gridView)
        {
        }

        /// <summary>
        /// Высота строки-сумматора группы
        /// </summary>
        public override int GroupFooterCellHeight
        {
            get
            {
                var devGridView = DefaultView as BandedGridViewDev;
                var multiplier = devGridView?.GroupFooterCellHeight ?? 1;

                return base.GroupFooterCellHeight * multiplier;
            }
        }
    }
}
