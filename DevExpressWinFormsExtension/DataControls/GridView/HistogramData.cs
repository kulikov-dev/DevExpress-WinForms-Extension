using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpressWinFormsExtension.DataControls.GridView
{
    /// <summary>
    /// Data for the histogram
    /// </summary>
    public class HistogramData
    {
        /// <summary>
        /// Contructor with parameters
        /// </summary>
        /// <param name="points"> Pre-calced points </param>
        /// <param name="min"> Min value </param>
        /// <param name="max"> Max value </param>
        public HistogramData(IEnumerable<PointF> points, double min, double max)
        {
            Points = points;
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Histogram pre-calced points
        /// </summary>
        public IEnumerable<PointF> Points { get; private set; }

        /// <summary>
        /// Min value
        /// </summary>
        public double Min { get; private set; }

        /// <summary>
        /// Max value
        /// </summary>
        public double Max { get; private set; }
    }
}