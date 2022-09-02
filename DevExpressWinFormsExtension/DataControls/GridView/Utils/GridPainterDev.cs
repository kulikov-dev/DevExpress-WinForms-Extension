using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpressWinFormsExtension.DataControls.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.DataControls.GridView.Utils
{
    /// <summary>
    /// Custom GridPainter
    /// </summary>
    /// <remarks> 
    /// 1. Used for merging Column and Band captions (band must have empty caption and disabled ShowCaption option)
    /// 2. Use ONLY after DataSource applied
    /// </remarks>
    public class GridPainterDev : GridPainter, IDisposable
    {
        /// <summary>
        /// Cache painters
        /// </summary>
        private static readonly Dictionary<BandedGridView, GridPainterDev> paintersCache = new Dictionary<BandedGridView, GridPainterDev>();

        /// <summary>
        /// Source BandedGridView
        /// </summary>
        private readonly BandedGridView sourceView;

        /// <summary>
        /// Bands, merged with its child column
        /// </summary>
        private readonly List<GridBand> mergedBands;

        /// <summary>
        /// Flag if object is disposed
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="view"> GridView </param>
        public GridPainterDev(BandedGridView view)
            : base(view)
        {
            if (view == null)
            {
                return;
            }

            if (view.DataSource == null)
            {
                throw new ArgumentNullException("Setup the DataSource before using GridPainterDev.");
            }

            DisposePainter(view);

            sourceView = view;
            paintersCache.Add(view, this);
            mergedBands = view.GetAllBands().Where(band => !band.OptionsBand.ShowCaption && string.IsNullOrWhiteSpace(band.Caption)).ToList();

            sourceView.GridControl.PaintEx += GridControl_PaintEx;
            sourceView.CustomDrawBandHeader += new BandHeaderCustomDrawEventHandler(View_CustomDrawBandHeader);
            sourceView.MouseDown += View_MouseDown;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="view"> GridView </param>
        /// <param name="bands"> Bands, merged with its child column </param>
        public GridPainterDev(BandedGridView view, IEnumerable<GridBand> bands)
            : this(view)
        {
            InitMergedBands(bands);
        }

        /// <summary>
        /// Dispose painter, attached to a GridView
        /// </summary>
        /// <param name="view"> GridView </param>
        public static void DisposePainter(BandedGridView view)
        {
            if (view == null || !paintersCache.ContainsKey(view))
            {
                return;
            }

            var painter = paintersCache[view];
            paintersCache.Remove(view);
            painter?.Dispose();
        }

        /// <summary>
        /// Set up bands, merged with its child column
        /// </summary>
        /// <param name="bands"> Bands list </param>
        public void InitMergedBands(IEnumerable<GridBand> bands)
        {
            mergedBands.Clear();
            if (bands != null)
            {
                mergedBands.AddRange(bands);
            }
        }

        /// <summary>
        /// Event on disposing
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Event on disposing
        /// </summary>
        /// <param name="disposing"> Flag is disposing </param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (sourceView != null)
                {
                    if (sourceView.GridControl != null)
                    {
                        sourceView.GridControl.PaintEx -= GridControl_PaintEx;
                    }

                    sourceView.CustomDrawBandHeader -= new BandHeaderCustomDrawEventHandler(View_CustomDrawBandHeader);
                    sourceView.MouseDown -= View_MouseDown;
                }
            }

            isDisposed = true;
        }

        /// <summary>
        /// Event on gridControl painting
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridControl_PaintEx(object sender, PaintExEventArgs e)
        {
            foreach (var band in mergedBands)
            {
                var currentBand = band;
                while (currentBand.Columns.Count == 0 && currentBand.Children.Any())
                {
                    currentBand = currentBand.Children[0];
                }

                if (currentBand.Columns.Count > 0)
                {
                    DrawColumnHeader(e.Cache, currentBand.Columns[0], band);
                }
            }
        }

        /// <summary>
        /// Event on mouse down
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void View_MouseDown(object sender, MouseEventArgs e)
        {
            var hitInfo = sourceView.CalcHitInfo(e.Location);
            if (mergedBands.Contains(hitInfo.Band) && hitInfo.InRowCell == false)
            {
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
        }

        /// <summary>
        /// Custom drawing band headers
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void View_CustomDrawBandHeader(object sender, BandHeaderCustomDrawEventArgs e)
        {
            if (mergedBands.Contains(e.Band))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Custom drawing column headers
        /// </summary>
        /// <param name="cache"> Drawing context </param>
        /// <param name="column"> Column </param>
        /// <param name="band"> Band </param>
        private void DrawColumnHeader(GraphicsCache cache, GridColumn column, GridBand band)
        {
            var viewInfo = sourceView.GetViewInfo() as BandedGridViewInfo;
            var colInfo = viewInfo.ColumnsInfo[column];
            var bandInfo = GetBandInfo(viewInfo.BandsInfo, band);
            if (colInfo == null || bandInfo == null)
            {
                return;
            }

            colInfo.Cache = cache;

            var top = bandInfo.Bounds.Top;
            var rect = colInfo.Bounds;
            var delta = rect.Top - top;
            rect.Y = top;
            rect.Height += delta;
            colInfo.Bounds = rect;
            colInfo.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            var args = colInfo as HeaderObjectInfoArgs;
            if (args.InnerElements.Count > 1)
            {
                foreach (DrawElementInfo element in args.InnerElements)
                {
                    var button = element.ElementInfo as GridFilterButtonInfoArgs;
                    if (button != null)
                    {
                        ElementsPainter.Column.CalcObjectBounds(colInfo);
                        button.Bounds = new Rectangle(button.Bounds.X, colInfo.Bounds.Y + colInfo.Bounds.Height - 18, button.Bounds.Width, button.Bounds.Height);
                        break;
                    }
                }
            }
            else
            {
                ElementsPainter.Column.CalcObjectBounds(colInfo);
            }

            ElementsPainter.Column.DrawObject(colInfo);
        }

        /// <summary>
        /// Get drawing band info
        /// </summary>
        /// <param name="bands"> All bands </param>
        /// <param name="band"> Drawing band </param>
        /// <returns> Band info </returns>
        private GridBandInfoArgs GetBandInfo(GridBandInfoCollection bands, GridBand band)
        {
            var info = bands[band];
            if (info != null)
            {
                return info;
            }

            foreach (GridBandInfoArgs bandInfo in bands)
            {
                if (bandInfo.Children == null)
                {
                    continue;
                }

                info = GetBandInfo(bandInfo.Children, band);
                if (info != null)
                {
                    return info;
                }
            }

            return null;
        }
    }
}