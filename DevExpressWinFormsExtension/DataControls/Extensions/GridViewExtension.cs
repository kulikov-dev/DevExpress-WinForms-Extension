using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    /// <summary>
    /// Extension for the GridView/BandedGridView components
    /// </summary>
    public static class GridViewExtension
    {
        /// <summary>
        /// Calc current visible on a view columns width
        /// </summary>
        /// <param name="view"> View </param>
        /// <returns> Visible width </returns>
        public static int CalcViewVisibleColumnsWidth(this DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            var width = 0;
            var info = view.GetViewInfo() as GridViewInfo;
            foreach (GridColumn column in view.Columns)
            {
                if (info.ColumnsInfo.Any(y => y.Column == column))
                {
                    width += column.VisibleWidth;
                }
            }

            return width;
        }

        /// <summary>
        /// Apply popup editor changes
        /// </summary>
        /// <param name="view"> View </param>
        /// <remarks> Recommended to use after a popup EditValueChanged event, to apply changes immediately, without user click </remarks>
        public static void ForcePopupEditor(this DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            view.PostEditor();
            view.UpdateCurrentRow();
        }

        /// <summary>
        /// Can perform check for multiple selected rows
        /// </summary>
        /// <param name="view"> View </param>
        /// <param name="hitInfo"> Hit info </param>
        /// <returns> Flag if multiple checking possible </returns>
        public static bool CanMultipleCheck(this DevExpress.XtraGrid.Views.Grid.GridView view, GridHitInfo hitInfo)
        {
            return (view.SelectedRowsCount > 1) &&
                hitInfo.InRowCell &&
                (hitInfo.Column.ColumnType == typeof(bool)) &&
                view.IsRowSelected(hitInfo.RowHandle);
        }

        /// <summary>
        /// Perform check for multiple selected rows
        /// </summary>
        /// <param name="view"> View </param>
        /// <param name="hitInfo"> Hit info </param>
        public static void PerformMultipleCheck(this DevExpress.XtraGrid.Views.Grid.GridView view, GridHitInfo hitInfo)
        {
            var selectedRows = new List<int>();
            var value = false;
            for (var i = 0; i < view.RowCount; i++)
            {
                if (!view.IsRowSelected(i))
                {
                    continue;
                }

                selectedRows.Add(i);
                if (!value)
                {
                    value = !(bool)view.GetRowCellValue(i, hitInfo.Column);
                }
            }

            foreach (var row in selectedRows)
            {
                view.SetRowCellValue(row, hitInfo.Column, value);
            }
        }

        /// <summary>
        /// Get all bands with children bands
        /// </summary>
        /// <param name="gridView"> Grid view </param>
        /// <returns> Bands </returns>
        public static List<GridBand> GetAllBands(this BandedGridView gridView)
        {
            var result = new List<GridBand>();
            foreach (GridBand band in gridView.Bands)
            {
                result.AddRange(GetBands(band));
            }

            return result;
        }

        /// <summary>
        /// Adjust width by bands captions
        /// </summary>
        /// <param name="bandedGridView"> Grid view </param>
        public static void BestFitBands(this BandedGridView bandedGridView)
        {
            using (var graphics = bandedGridView.GridControl.CreateGraphics())
            {
                var bands = bandedGridView.GetAllBands();
                UpdateMinWidth(bands, graphics);
            }
        }

        /// <summary>
        /// Remove bands and all children columns
        /// </summary>
        /// <param name="gridView"> Grid view </param>
        /// <param name="nonRemovableBands"> Bands which will be keeped </param>
        public static void RemoveBands(this BandedGridView gridView, List<GridBand> nonRemovableBands)
        {
            for (var i = gridView.Bands.Count - 1; i >= 0; --i)
            {
                var band = gridView.Bands[i];
                if (nonRemovableBands != null && nonRemovableBands.Contains(band))
                {
                    continue;
                }

                RemoveBandColumns(gridView, band);

                band.Children.Clear();
                gridView.Bands.RemoveAt(i);
            }
        }

        /// <summary>
        /// Set up best width for bands
        /// </summary>
        /// <param name="bands"> List of bands </param>
        /// <param name="graphics"> Drawing context </param>
        private static void UpdateMinWidth(IEnumerable<GridBand> bands, Graphics graphics)
        {
            foreach (GridBand band in bands)
            {
                if (string.IsNullOrWhiteSpace(band.Caption))
                {
                    band.MinWidth = band.Columns.Sum(column => column.Width);
                }
                else
                {
                    band.MinWidth = (int)graphics.MeasureString(band.Caption, band.AppearanceHeader.Font).Width + 6;
                }

                band.Width = band.MinWidth + 2;
                UpdateMinWidth(band.Children, graphics);
            }
        }

        /// <summary>
        /// Get all bands with children bands
        /// </summary>
        /// <param name="band"> Source band </param>
        /// <returns> Bands </returns>
        private static List<GridBand> GetBands(GridBand band)
        {
            var result = new List<GridBand>
            {
                band
            };

            foreach (var child in band.Children)
            {
                if (child is GridBand)
                {
                    result.AddRange(GetBands(child as GridBand));
                }
            }

            return result;
        }

        /// <summary>
        /// Calc bounds for a merged cells editor
        /// </summary>
        /// <param name="cell"> Cell info </param>
        /// <returns> Bounds </returns>
        public static Rectangle GetMergedEditorBounds(this GridViewInfo viewInfo, GridCellInfo cell)
        {
            var bounds = viewInfo.UpdateFixedRange(cell.CellValueRect, cell.ColumnInfo);
            if (bounds.Right > viewInfo.ViewRects.Rows.Right)
            {
                bounds.Width = viewInfo.ViewRects.Rows.Right - bounds.Left;
            }

            if (bounds.Bottom > viewInfo.ViewRects.Rows.Bottom)
            {
                bounds.Height = viewInfo.ViewRects.Rows.Bottom - bounds.Top;
            }

            if (bounds.Width < 1 || bounds.Height < 1)
            {
                return Rectangle.Empty;
            }

            for (int i = 1; i < cell.MergedCell.MergedCells.Count; i++)
            {
                bounds.Height += cell.MergedCell.MergedCells[i].Bounds.Height;
            }

            return bounds;
        }

        /// <summary>
        /// Remove all columns from band and children bands
        /// </summary>
        /// <param name="gridView"> Grid view </param>
        /// <param name="band"> Source band </param>
        private static void RemoveBandColumns(this BandedGridView gridView, GridBand band)
        {
            for (var j = band.Columns.Count - 1; j >= 0; --j)
            {
                var column = band.Columns[j];
                gridView.Columns.RemoveAt(column.AbsoluteIndex);
            }

            foreach (GridBand childBand in band.Children)
            {
                RemoveBandColumns(gridView, childBand);
            }
        }
    }
}