using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;

namespace DevExpressWinFormsExtension.DataControls.GridView.Utils
{
    /// <summary>
    /// Custom BandedGridViewDev registrator
    /// </summary>
    public class BandedGridDevInfoRegistrator : BandedGridInfoRegistrator
    {
        /// <summary>
        /// Custom view name
        /// </summary>
        public override string ViewName
        {
            get
            {
                return "BandedGridViewDev";
            }
        }

        /// <summary>
        /// Create custom view
        /// </summary>
        /// <param name="grid"> GridControl </param>
        /// <returns> View </returns>
        public override BaseView CreateView(GridControl grid)
        {
            return new BandedGridViewDev(grid);
        }
    }
}