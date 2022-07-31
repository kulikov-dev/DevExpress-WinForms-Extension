using Caching;
using System;
using System.Drawing;
using System.Reflection;

namespace DevExpressWinFormsExtension
{
    /// <summary>
    /// Class for caching solid brushes
    /// </summary>
    /// <remarks> Don't dispose brushes after using! Use Dispose on Application.Exit </remarks>
    public static class SolidBrushesCache
    {
        /// <summary>
        /// Amount of brushes followed by cleaning
        /// </summary>
        private const int cacheSize = 1000;

        /// <summary>
        /// Cache of used brushes
        /// </summary>
        private static readonly LRUCache<int, SolidBrush> brushes = new LRUCache<int, SolidBrush>(cacheSize);

        /// <summary>
        /// Get SolidBrush by color
        /// </summary>
        /// <param name="color"> Color </param>
        /// <returns> SolidBrush </returns>
        public static SolidBrush GetBrushByColor(Color color)
        {
            var colorValue = color.GetHashCode();
            brushes.TryGetValue(colorValue, out SolidBrush brush);
            if (brush != null)
            {
                //// Check if brush was disposed accidently
                var field = typeof(Brush).GetField("nativeBrush", BindingFlags.NonPublic | BindingFlags.Instance);
                var hbrush = (IntPtr)field?.GetValue(brush);
                if (hbrush != IntPtr.Zero)
                {
                    return brush;
                }

                brushes.Remove(colorValue);
            }

            var result = new SolidBrush(color);
            brushes.Add(colorValue, result);

            return result;
        }

        /// <summary>
        /// Dispose all brushes in the cache
        /// </summary>
        /// <remarks> Needs to be called on Application.Exit </remarks>
        public static void Dispose()
        {
            brushes.Clear();
        }
    }
}