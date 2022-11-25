using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpressWinFormsExtension.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.DataControls.GridView.Utils
{
    /// <summary>
    /// Utils to work with DevExpress xtraGrid
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// Substitue zero char
        /// </summary>
        public const char SubstituteZeroChar = '0';

        /// <summary>
        /// Substitute number char
        /// </summary>
        public const char SubstituteNumberChar = '#';

        /// <summary>
        /// Delimiter char
        /// </summary>
        public const char DelimiterChar = '.';

        /// <summary>
        /// General format specifier
        /// </summary>
        public const char GeneralFormatSpecifier = 'g';

        /// <summary>
        /// Format with a zero specifier
        /// </summary>
        public static string ZeroFormatSpecifier => $"{SubstituteZeroChar}{DelimiterChar}";

        /// <summary>
        /// Format with a number specifier
        /// </summary>
        public static string NumberFormatSpecifier => $"{SubstituteNumberChar}{DelimiterChar}";

        public static string IsHistogramColumn = "HistogramColumn";

        /// <summary>
        /// Height of a zero level of a histogram
        /// </summary>
        private const float histogrammZeroLevelHeight = 0.5f;

        /// <summary>
        /// Intitialize DataTable with columns from a GridView
        /// </summary>
        /// <param name="view"> Source view </param>
        /// <param name="table"> Destination DataTable </param>
        /// <param name="boundTypes"> Flag, if need to bound data types to the DataTable </param>
        public static void InitializeDataTable(DevExpress.XtraGrid.Views.Grid.GridView view, out DataTable table, bool boundTypes = false)
        {
            table = new DataTable();

            foreach (GridColumn column in view.Columns)
            {
                DataColumn dataColumn;

                if (boundTypes)
                {
                    switch (column.UnboundType)
                    {
                        case UnboundColumnType.DateTime:

                            dataColumn = new DataColumn(column.FieldName, typeof(DateTime));

                            break;
                        case UnboundColumnType.Decimal:

                            dataColumn = new DataColumn(column.FieldName, typeof(double));

                            break;
                        case UnboundColumnType.Integer:

                            dataColumn = new DataColumn(column.FieldName, typeof(int));

                            break;
                        case UnboundColumnType.String:

                            dataColumn = new DataColumn(column.FieldName, typeof(string));

                            break;
                        default:

                            dataColumn = new DataColumn(column.FieldName, typeof(object));

                            break;
                    }
                }
                else
                {
                    dataColumn = new DataColumn(column.FieldName, typeof(object));
                }

                table.Columns.Add(dataColumn);
            }
        }

        /// <summary>
        /// Get real GridView size (not visible)
        /// </summary>
        /// <param name="gridView"> GridView </param>
        /// <returns> Size </returns>
        public static Rectangle GetActualViewSize(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            var viewInfo = (GridViewInfo)gridView.GetViewInfo();
            var width = viewInfo.ViewRects.IndicatorWidth;

            foreach (GridColumn column in gridView.Columns)
            {
                if (viewInfo.GetColumnLeftCoord(column) < viewInfo.ViewRects.ColumnPanelWidth)
                {
                    gridView.LeftCoord = width;
                }

                width += viewInfo.ColumnsInfo[column].Bounds.Width;
            }

            const int gridMaximumHeight = 10000;        // unfortunately workaround to allow devexpress calc the height
            var height = viewInfo.CalcRealViewHeight(new Rectangle(0, 0, width, gridMaximumHeight));

            return new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Get GridView as image
        /// </summary>
        /// <param name="gridView"> GridView </param>
        /// <returns> GridView as Image </returns>
        /// <remarks> Capture all table, not only visible part. </remarks>
        public static Bitmap GetAsImage(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            var printingSystem = new PrintingSystem();
            var printableComponentLink = new PrintableComponentLink(printingSystem);

            printingSystem.Links.AddRange(new object[] { printableComponentLink });

            printableComponentLink.Component = gridView.GridControl;

            var image = default(Bitmap);

            using (var memoryStream = new MemoryStream())
            {
                printableComponentLink.ExportToImage(memoryStream);
                image = new Bitmap(memoryStream);
            }

            const int borderSize = 5;

            using (var memoryStream = new MemoryStream())
            {
                var oldDock = gridView.GridControl.Dock;
                var oldHorzVisibility = gridView.HorzScrollVisibility;
                var oldVertVisibility = gridView.VertScrollVisibility;
                var oldIndicator = gridView.OptionsView.ShowIndicator;

                gridView.GridControl.Dock = DockStyle.None;
                gridView.HorzScrollVisibility = ScrollVisibility.Never;
                gridView.VertScrollVisibility = ScrollVisibility.Never;
                gridView.OptionsView.ShowIndicator = false;

                var size = GetActualViewSize(gridView);
                var bounds = new Rectangle(0, 0, size.Width + borderSize, size.Height + borderSize);

                var gridBitmap = new Bitmap(bounds.Width, bounds.Height);
                gridView.GridControl.Size = new Size(bounds.Width, bounds.Height);
                gridView.GridControl.RenderToBitmap(gridBitmap, bounds);

                gridView.GridControl.Dock = oldDock;
                gridView.HorzScrollVisibility = oldHorzVisibility;
                gridView.VertScrollVisibility = oldVertVisibility;
                gridView.OptionsView.ShowIndicator = oldIndicator;

                return image;
            }
        }

        /// <summary>
        /// Get values dictribution for the histogram
        /// </summary>
        /// <param name="values"> Values </param>
        /// <returns> Histogram data </returns>
        internal static HistogramData GetHistogrammPoints(IEnumerable<double> values)
        {
            if (values == null)
            {
                return default;
            }

            var max = double.MinValue;
            var min = double.MaxValue;

            foreach (var value in values)
            {
                max = Math.Max(max, value);
                min = Math.Min(min, value);
            }

            var valuesCount = values.Count();
            var rangesCount = valuesCount == 1 ? 1 : (int)(4 * Math.Log(valuesCount));
            var step = (max - min) / rangesCount;
            var ranges = new int[rangesCount];

            foreach (var value in values)
            {
                if (Math.Abs(value - max) < 0.01)
                {
                    ranges[rangesCount - 1]++;
                }
                else
                {
                    var index = (int)((value - min) / step);

                    ranges[index]++;
                }
            }

            var maxRange = ranges.Max(range => range);
            var points = rangesCount == 1 ? new[] { new PointF(0, 1) } : ranges.Select((range, index) => new PointF((float)index / (rangesCount - 1), (float)range / maxRange));
            var result = new HistogramData(points, min, max);

            return result;
        }

        /// <summary>
        /// Draw histogram in the cell
        /// </summary>
        /// <param name="data"> Histogram data </param>
        /// <param name="font"> Histogram font </param>
        /// <param name="color"> Histogram color </param>
        /// <param name="emptyColor"> Histogram empty color </param>
        /// <param name="e"> Drawing parameters </param>
        internal static void DrawCellHistogram(HistogramData data, Font font, Color color, Color emptyColor, RowCellCustomDrawEventArgs e)
        {
            if (data == null)
            {
                return;
            }

            var minText = data.Min.ToString("0.#");
            var maxText = data.Max.ToString("0.#");
            var minTextSize = e.Cache.Graphics.MeasureString(minText, font);
            var maxTextSize = e.Cache.Graphics.MeasureString(maxText, font);
            var leftHistogrammBorder = e.Bounds.Left + (int)minTextSize.Width;
            var rightHistogrammBorder = e.Bounds.Right - (int)maxTextSize.Width;
            var textHeight = (int)(minTextSize.Height + 1);
            var textTop = e.Bounds.Bottom - textHeight;
            var minTextBounds = new Rectangle(e.Bounds.Left, textTop, e.Bounds.Width, textHeight);
            var maxTextBounds = new Rectangle(rightHistogrammBorder, textTop, e.Bounds.Width, textHeight);
            var histogrammBounds = new RectangleF(leftHistogrammBorder, e.Bounds.Top, rightHistogrammBorder - leftHistogrammBorder, e.Bounds.Height - histogrammZeroLevelHeight);
            var zeroLevelBounds = new RectangleF(histogrammBounds.X, histogrammBounds.Bottom - histogrammZeroLevelHeight, histogrammBounds.Width, histogrammZeroLevelHeight);
            var histogrammPoints = data.Points.ToArray();
            var histogramm = new PointF[histogrammPoints.Length + 2];

            for (var i = 0; i < histogrammPoints.Length; i++)
            {
                var point = histogrammPoints[i];
                histogramm[i + 1] = new PointF(histogrammBounds.X + (point.X * histogrammBounds.Width), histogrammBounds.Bottom - (point.Y * histogrammBounds.Height));
            }

            histogramm[0] = new PointF(histogramm[1].X, histogrammBounds.Bottom);
            histogramm[histogramm.Length - 1] = new PointF(histogramm[histogramm.Length - 2].X, histogrammBounds.Bottom);

            e.Cache.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.SetClip(histogrammBounds);
            e.Cache.Graphics.FillClosedCurve(SolidBrushesCache.GetBrushByColor(color), histogramm);

            e.Cache.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            e.Graphics.ResetClip();
            e.Graphics.FillRectangle(SolidBrushesCache.GetBrushByColor(emptyColor), zeroLevelBounds);
            e.Appearance.DrawString(e.Cache, minText, minTextBounds, font, color, StringFormat.GenericDefault);
            e.Appearance.DrawString(e.Cache, maxText, maxTextBounds, font, color, StringFormat.GenericDefault);

            e.Handled = true;
        }

        /// <summary>
        /// Calc BandedGridView bands depth
        /// </summary>
        /// <param name="gridView"> GridView </param>
        /// <returns> Bands depth </returns>
        internal static int GetBandsDepth(BandedGridView gridView)
        {
            var headerRows = 1;

            foreach (BandedGridColumn column in gridView.VisibleColumns)
            {
                var tempHeaderRows = 1;
                var band = column.OwnerBand;

                while (band != null)
                {
                    ++tempHeaderRows;

                    band = band.ParentBand;
                }

                headerRows = Math.Max(tempHeaderRows, headerRows);
            }

            return headerRows;
        }
    }
}
