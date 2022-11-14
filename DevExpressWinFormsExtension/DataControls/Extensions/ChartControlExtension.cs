using DevExpress.XtraCharts;
using DevExpressWinFormsExtension.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    /// <summary>
    /// Extension for the ChartControl component
    /// </summary>
    public static class ChartControlExtension
    {
        /// <summary>
        /// Information about lasso processors
        /// </summary>
        private static readonly Dictionary<ChartControl, ChartControlLassoProcessor> _chartControlLassoProcessors = new Dictionary<ChartControl, ChartControlLassoProcessor>();

        /// <summary>
        /// Change the hue for series with the same color
        /// </summary>
        /// <param name="chartControl"> Chart control </param>
        public static void ColorizeSameColoredSeries(this ChartControl chartControl)
        {
            var colorSeries = new Dictionary<Color, List<Series>>();
            var series = chartControl.Series.Where(serie => serie.Visible).ToList();
            foreach (Series serie in series)
            {
                var color = serie.View.Color;
                if (!colorSeries.ContainsKey(color))
                {
                    colorSeries.Add(color, new List<Series>() { serie });
                }
                else
                {
                    colorSeries[color].Add(serie);
                }
            }

            const float colorizeKoef = 200f;
            foreach (Series serie in series)
            {
                var seriesList = colorSeries[serie.View.Color];
                if (seriesList.Count < 2)
                {
                    continue;
                }

                var serieIndex = seriesList.IndexOf(serie);
                var percent = (((serieIndex + 1) * (colorizeKoef / (seriesList.Count + 1))) - 100) / 100f;
                if (percent < 0)
                {
                    serie.View.Color = ColorExtension.Darken(serie.View.Color, Math.Abs(percent));
                }
                else
                {
                    serie.View.Color = ColorExtension.Lighten(serie.View.Color, Math.Abs(percent));
                }
            }
        }

        /// <summary>
        /// Enable lasso selection in a Chart
        /// </summary>
        /// <param name="chartControl"> Chart control </param>
        /// <param name="selectionColor"> Selection lasso color </param>
        /// <param name="deselectionColor"> Deselection lasso color </param>
        /// <param name="penWidth"> Lasso pen width </param>
        public static void EnableLassoSelection(this ChartControl chartControl, Color selectionColor = default, Color deselectionColor = default, float penWidth = 2)
        {
            new ChartControlLassoProcessor(chartControl, selectionColor, deselectionColor, penWidth);
        }

        /// <summary>
        /// Disable lasso selection in a Chart
        /// </summary>
        /// <param name="chartControl"> Chart control </param>
        public static void DisableLassoSelection(this ChartControl chartControl)
        {
            if (!_chartControlLassoProcessors.ContainsKey(chartControl))
            {
                return;
            }

            _chartControlLassoProcessors[chartControl].Dispose();
            _chartControlLassoProcessors.Remove(chartControl);
        }
    }
}