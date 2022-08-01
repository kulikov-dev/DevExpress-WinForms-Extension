using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;
using DevExpressWinFormsExtension.DataControls.GridView.Utils;
using System.ComponentModel;
using System.Drawing;

namespace DevExpressWinFormsExtension.DataControls.GridView
{
    /// <summary>
    /// GridControl with registration of GridView/BandedGridView views.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GridControl))]
    [Description("GridControl")]
    public partial class GridControlDev : GridControl
    {
        /// <summary>
        /// Create default view
        /// </summary>
        /// <returns> View </returns>
        protected override BaseView CreateDefaultView()
        {
            return CreateView(typeof(GridViewDev).Name);
        }

        /// <summary>
        /// Register custom views
        /// </summary>
        /// <param name="collection"> Views list </param>
        protected override void RegisterAvailableViewsCore(InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new GridViewDevInfoRegistrator());
            collection.Add(new BandedGridDevInfoRegistrator());
        }

        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="name"> View name </param>
        /// <returns> View </returns>
        /// <returns> View </returns>
        public override BaseView CreateView(string name)
        {
            if (name == typeof(DevExpress.XtraGrid.Views.Grid.GridView).Name)
            {
                name = typeof(GridViewDev).Name;
            }
            else if (name == typeof(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView).Name)
            {
                name = typeof(BandedGridViewDev).Name;
            }

            return base.CreateView(name);
        }
    }
}