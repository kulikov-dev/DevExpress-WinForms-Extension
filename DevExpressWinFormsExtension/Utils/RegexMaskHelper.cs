using System.Globalization;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Common regex patterns to use as a Mask in editors/repositories
    /// </summary>
    public static class RegexMaskHelper
    {
        /// <summary>
        /// Double mask
        /// </summary>
        public static string DoubleMask => string.Format("(-?[0-9]+([{0}][0-9]*)?|[{0}][0-9]+)", DecimalSeparator);

        /// <summary>
        /// Double positive mask 
        /// </summary>
        public static string DoublePositiveMask => string.Format("([0-9]+([{0}][0-9]*)?|[{0}][0-9]+)", DecimalSeparator);

        /// <summary>
        /// Double positive mask with precision
        /// </summary>
        public static string DoublePositiveMaskPrecision1 => string.Format("([0-9]+([{0}][0-9]{{0,1}})?|[{0}][0-9]{{0,1}})", DecimalSeparator);

        /// <summary>
        /// Int
        /// </summary>
        public static string IntegerMask => @"-?[1-9]\d*";

        /// <summary>
        /// Positive int
        /// </summary>
        public static string IntegerPositiveMask => @"[1-9]\d*";

        /// <summary>
        /// Decimal separator for a patterns purpose
        /// </summary>
        private static string DecimalSeparator => CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
    }
}