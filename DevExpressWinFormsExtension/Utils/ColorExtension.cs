using System;
using System.Drawing;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Extension for Color class
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Tints the color by the given percent
        /// </summary>
        /// <param name="color"> The color being tinted </param>
        /// <param name="percent"> The percent to tint. Ex: 0.1 will make the color 10% lighter </param>
        /// <returns> The new tinted color </returns>
        public static Color Lighten(this Color color, float percent)
        {
            var lighting = color.GetBrightness();
            lighting += lighting * percent;
            if (lighting > 1.0)
            {
                lighting = 1;
            }
            else if (lighting <= 0)
            {
                lighting = 0.1f;
            }

            return FromHsl(color.A, color.GetHue(), color.GetSaturation(), lighting);
        }

        /// <summary>
        /// Tints the color by the given percent
        /// </summary>
        /// <param name="color"> The color being tinted </param>
        /// <param name="percent"> The percent to tint. Ex: 0.1 will make the color 10% darker </param>
        /// <returns> The new tinted color </returns>
        public static Color Darken(this Color color, float percent)
        {
            var lighting = color.GetBrightness();
            lighting -= lighting * percent;
            if (lighting > 1.0)
            {
                lighting = 1;
            }
            else if (lighting <= 0)
            {
                lighting = 0;
            }

            return FromHsl(color.A, color.GetHue(), color.GetSaturation(), lighting);
        }

        /// <summary>
        /// Converts the HSL values to a Color
        /// </summary>
        /// <param name="alpha"> The alpha </param>
        /// <param name="hue"> The hue </param>
        /// <param name="saturation"> The saturation </param>
        /// <param name="lighting"> The lighting </param>
        /// <returns> Changed color </returns>
        private static Color FromHsl(int alpha, float hue, float saturation, float lighting)
        {
            if (saturation == 0)
            {
                return Color.FromArgb(alpha, Convert.ToInt32(lighting * 255), Convert.ToInt32(lighting * 255), Convert.ToInt32(lighting * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (lighting > 0.5)
            {
                fMax = lighting - (lighting * saturation) + saturation;
                fMin = lighting + (lighting * saturation) - saturation;
            }
            else
            {
                fMax = lighting + (lighting * saturation);
                fMin = lighting - (lighting * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (hue >= 300f)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor((iSextant + 1f) % 6f / 2f);
            if (iSextant % 2 == 0)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }
    }
}