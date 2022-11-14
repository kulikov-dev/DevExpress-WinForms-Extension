using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Processor to work with lasso on the ChartControl with Point Series
    /// </summary>
    internal class ChartControlLassoProcessor : IDisposable
    {
        /// <summary>
        /// Lasso selection pen
        /// </summary>
        private readonly Pen selectionPen;

        /// <summary>
        /// Lasso deselectoin pen
        /// </summary>
        private readonly Pen deselectionPen;

        /// <summary>
        /// Source ChartControl
        /// </summary>
        private readonly ChartControl chartControl;

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="chartControl"> Link to the ChartControl </param>
        /// <param name="selectionColor"> Selection lasso color </param>
        /// <param name="deselectionColor"> Deselection lasso color </param>
        /// <param name="penWidth"> Lasso pen width </param>
        public ChartControlLassoProcessor(ChartControl chartControl, Color selectionColor = default, Color deselectionColor = default, float penWidth = 2)
        {
            this.chartControl = chartControl;

            LassoPoints = new List<Point>();
            Lasso = LassoMode.None;
            chartControl.SeriesSelectionMode = SeriesSelectionMode.Point;

            selectionColor = Equals(selectionColor, default(Color)) ? Color.Green : selectionColor;
            selectionPen = new Pen(selectionColor, penWidth);
            deselectionColor = Equals(deselectionColor, default(Color)) ? Color.Red : deselectionColor;
            deselectionPen = new Pen(deselectionColor, penWidth);

            chartControl.KeyUp += ChartControl_KeyUp;
            chartControl.CustomPaint += ChartControl_CustomPaint;
            chartControl.MouseDoubleClick += ChartControl_MouseDoubleClick;
            chartControl.MouseDown += ChartControl_MouseDown;
            chartControl.MouseMove += ChartControl_MouseMove;
            chartControl.MouseUp += ChartControl_MouseUp;
        }

        /// <summary>
        /// Selection modifier key
        /// </summary>
        private static Keys LassoSelectKey
        {
            get
            {
                return Keys.Control;
            }
        }

        /// <summary>
        /// Deselection modifier key
        /// </summary>
        private static Keys LassoDeselectKey
        {
            get
            {
                return Keys.Shift;
            }
        }

        private XYDiagram Diagram
        {
            get
            {
                return chartControl.Diagram as XYDiagram;
            }
        }
        /// <summary>
        /// Lasso mode
        /// </summary>
        private LassoMode Lasso { get; set; }

        /// <summary>
        /// Current lasso working points
        /// </summary>
        private List<Point> LassoPoints { get; }

        /// <summary>
        /// Radios of PointSeries 
        /// </summary>
        private int PointsRadius
        {
            get
            {
                var pointSerie = chartControl.Series.FirstOrDefault(serie => serie.SeriesView is PointSeriesView) as PointSeriesView;

                return pointSerie?.PointMarkerOptions.Size ?? 1;
            }
        }

        /// <summary>
        /// Event on disposing
        /// </summary>
        public void Dispose()
        {
            selectionPen?.Dispose();
            deselectionPen?.Dispose();
            if (chartControl == null || chartControl.Disposing || chartControl.IsDisposed)
            {
                return;
            }

            chartControl.KeyUp -= ChartControl_KeyUp;
            chartControl.CustomPaint -= ChartControl_CustomPaint;
            chartControl.MouseDoubleClick -= ChartControl_MouseDoubleClick;
            chartControl.MouseDown -= ChartControl_MouseDown;
            chartControl.MouseMove -= ChartControl_MouseMove;
            chartControl.MouseUp -= ChartControl_MouseUp;
        }

        /// <summary>
        /// Check if point in squared radius of circle
        /// </summary>
        /// <param name="point"> Point </param>
        /// <param name="center"> Circle center </param>
        /// <param name="radius"> Circle radius </param>
        /// <returns> Flag if point in circle </returns>
        private static bool CheckIfPointInCircle(Point point, Point center, double radius)
        {
            return Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2) <= Math.Pow(radius, 2);
        }

        /// <summary>
        /// Check if point in an area of points
        /// </summary>
        /// <param name="areaPoints"> Points of an area </param>
        /// <param name="point"> Point </param>
        /// <returns> Flag if point in area </returns>
        private static bool CheckIfPointInArea(Point point, IList<Point> areaPoints)
        {
            var leftIndex = 0;
            var mainIndex = 0;
            var areaLength = areaPoints.Count - 1;

            if (areaLength < 1)
            {
                return false;
            }

            for (var i = 1; i < areaPoints.Count; ++i)
            {
                if (GetDistanceFromPointToLine(point, areaPoints[i - 1], areaPoints[i]) <= double.Epsilon)
                {
                    return true;
                }
            }

            while (mainIndex < areaPoints.Count && areaPoints[mainIndex].Y == point.Y)
            {
                mainIndex++;
            }

            if (mainIndex == areaPoints.Count)
            {
                return false;
            }

            var firstIndex = mainIndex;
            var secondIndex = (mainIndex + 1) % areaLength;
            var iterations = 0;

            do
            {
                var diffY = (areaPoints[mainIndex].Y - point.Y) * (areaPoints[secondIndex].Y - point.Y);
                iterations++;

                double diffX;
                if (diffY < 0)
                {
                    diffX = areaPoints[mainIndex].X + ((areaPoints[secondIndex].X - areaPoints[mainIndex].X) * (point.Y - areaPoints[mainIndex].Y) / (areaPoints[secondIndex].Y - areaPoints[mainIndex].Y));

                    if (diffX < point.X)
                    {
                        leftIndex++;
                    }
                    else if (diffX == point.X)
                    {
                        return true;
                    }
                }
                else
                {
                    if (diffY == 0)
                    {
                        var areaDiffY = areaPoints[mainIndex].Y - point.Y;
                        var iterationIndex = areaLength;
                        while (areaPoints[secondIndex].Y == point.Y && iterationIndex-- >= 0)
                        {
                            mainIndex = secondIndex;
                            secondIndex = (secondIndex + 1) % areaLength;

                            if (areaPoints[mainIndex].Y == point.Y && areaPoints[secondIndex].Y == point.Y && ((areaPoints[mainIndex].X - point.X) * (areaPoints[secondIndex].X - point.X)) <= 0)
                            {
                                return true;
                            }
                        }

                        diffY = areaDiffY * (areaPoints[secondIndex].Y - point.Y);

                        if (diffY < 0)
                        {
                            diffX = areaPoints[mainIndex].X + ((areaPoints[secondIndex].X - areaPoints[mainIndex].X) * (point.Y - areaPoints[mainIndex].Y) / (areaPoints[secondIndex].Y - areaPoints[mainIndex].Y));

                            if (diffX < point.X)
                            {
                                leftIndex++;
                            }
                            else if (diffX == point.X)
                            {
                                return true;
                            }
                        }
                    }
                }

                mainIndex = secondIndex;
                secondIndex = (secondIndex + 1) % areaLength;
            }
            while ((secondIndex != (firstIndex + 1)) && iterations < areaLength);

            return (leftIndex % 2 == 1) && iterations <= areaLength;
        }

        /// <summary>
        /// Get distance from a point to a line
        /// </summary>
        /// <param name="point"> Point </param>
        /// <param name="lineStart"> Line start </param>
        /// <param name="lineEnd"> Line end </param>
        /// <returns> Distance. Zero if equals </returns>
        private static double GetDistanceFromPointToLine(Point point, Point lineStart, Point lineEnd)
        {
            if (lineStart.Equals(lineEnd))
            {
                return 0;
            }

            var diffEndToStart = new Point(lineEnd.X - lineStart.X, lineEnd.Y - lineStart.Y);
            var diffPointToStart = new Point(point.X - lineStart.X, point.Y - lineStart.Y);

            var mCoef = ((diffEndToStart.X * diffPointToStart.X) + (diffEndToStart.Y * diffPointToStart.Y)) / (Math.Pow(diffEndToStart.X, 2) + Math.Pow(diffEndToStart.Y, 2));
            var diffC = new PointF((float)(lineStart.X + (diffEndToStart.X * mCoef)), (float)(lineStart.Y + (diffEndToStart.Y * mCoef)));

            if (XBetweenAB(diffC.X, lineStart.X, lineEnd.X) && XBetweenAB(diffC.Y, lineStart.Y, lineEnd.Y))
            {
                return Math.Sqrt(Math.Pow(point.X - diffC.X, 2) + Math.Pow(point.Y - diffC.Y, 2));
            }

            var diffPointToEnd = new Point(point.X - lineEnd.X, point.Y - lineEnd.Y);

            return Math.Min(Math.Sqrt(Math.Pow(diffPointToStart.X, 2) + Math.Pow(diffPointToStart.Y, 2)), Math.Sqrt(Math.Pow(diffPointToEnd.X, 2) + Math.Pow(diffPointToEnd.Y, 2)));
        }

        private static bool XBetweenAB(double x, double a, double b)
        {
            if (a < b)
            {
                return (x >= a) && (x <= b);
            }

            return (x <= a) && (x >= b);
        }

        /// <summary>
        /// Event on mouse up
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (Diagram == null)
            {
                return;
            }

            if (Lasso != LassoMode.None)
            {
                ApplyLasso();
                LassoPoints.Clear();

                Lasso = LassoMode.None;
            }
        }

        /// <summary>
        /// Event on mouse move
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Diagram == null)
            {
                return;
            }

            var coords = Diagram.PointToDiagram(e.Location);

            if (coords.IsEmpty)
            {
                return;
            }

            Diagram.ScrollingOptions.UseMouse = false;

            if (Lasso != LassoMode.None)
            {
                LassoPoints.Add(e.Location);
                chartControl.Refresh();
                return;
            }

            Diagram.ScrollingOptions.UseMouse = true;
        }

        /// <summary>
        /// Event on mouse down
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (Diagram == null)
            {
                return;
            }

            var coords = Diagram.PointToDiagram(e.Location);

            if (coords.IsEmpty)
            {
                return;
            }

            if ((e.Button == MouseButtons.Left))
            {
                if (Control.ModifierKeys == LassoSelectKey)
                {
                    Lasso = LassoMode.Select;

                    LassoPoints.Add(e.Location);
                }
                else if (Control.ModifierKeys == LassoDeselectKey)
                {
                    Lasso = LassoMode.Deselect;

                    LassoPoints.Add(e.Location);
                }
            }
        }

        /// <summary>
        /// Event on mouse double click
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (chartControl.SelectedItems.Count != 0)
            {
                chartControl.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// Event on custom paint
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_CustomPaint(object sender, CustomPaintEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            var oldMode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if ((Lasso != LassoMode.None) && (LassoPoints.Count > 1))
            {
                var pen = Control.ModifierKeys == LassoSelectKey ? selectionPen : deselectionPen;

                e.Graphics.DrawLines(pen, LassoPoints.ToArray());
            }

            e.Graphics.SmoothingMode = oldMode;
        }

        /// <summary>
        /// Event on key up
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ChartControl_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == LassoSelectKey) || (e.KeyCode == LassoDeselectKey))
            {
                ApplyLasso();
                LassoPoints.Clear();

                Lasso = LassoMode.None;
            }
        }

        /// <summary>
        /// Apply selection points by lasso
        /// </summary>
        private void ApplyLasso()
        {
            if (!LassoPoints.Any())
            {
                return;
            }

            switch (Lasso)
            {
                case LassoMode.Select:
                    SelectPointsInLasso();
                    break;
                case LassoMode.Deselect:
                    DeselectPointsInLasso();
                    break;
            }

            chartControl.RefreshData();
        }

        /// <summary>
        /// Select chart points by lasso
        /// </summary>
        private void SelectPointsInLasso()
        {
            var points = (chartControl.Series.FirstOrDefault(x => x.SeriesView is PointSeriesView) as Series)?.Points ?? new SeriesPointCollection();
            var selected = new List<SeriesPoint>();

            foreach (SeriesPoint point in points)
            {
                if (chartControl.SelectedItems.Contains(point))
                {
                    selected.Add(point);
                    continue;
                }

                if (!IsPointInLasso(point))
                {
                    continue;
                }

                selected.Add(point);
            }

            chartControl.ReplaceSelectedItems(selected.ToArray());
        }

        /// <summary>
        /// Deselect chart points by lasso
        /// </summary>
        private void DeselectPointsInLasso()
        {
            var needRemove = chartControl.SelectedItems.Cast<SeriesPoint>().Where(IsPointInLasso).ToList();

            foreach (var point in needRemove)
            {
                chartControl.SelectedItems.Remove(point);
            }
        }

        /// <summary>
        /// Check if point located in lasso
        /// </summary>
        /// <param name="seriesPoint"> Series point </param>
        /// <returns> Flag if point in lasso </returns>
        private bool IsPointInLasso(SeriesPoint seriesPoint)
        {
            var viewPoint = Diagram.DiagramToPoint(seriesPoint.Argument, seriesPoint.Values[0]).Point;

            return LassoPoints.Count == 1 ? CheckIfPointInCircle(viewPoint, LassoPoints[0], PointsRadius * 2) : CheckIfPointInArea(viewPoint, LassoPoints);
        }

        /// <summary>
        /// Lasso mode types
        /// </summary>
        private enum LassoMode
        {
            /// <summary>
            /// Selection mode
            /// </summary>
            Select,

            /// <summary>
            /// Deselection mode
            /// </summary>
            Deselect,

            /// <summary>
            /// Lasso off
            /// </summary>
            None
        }
    }
}
