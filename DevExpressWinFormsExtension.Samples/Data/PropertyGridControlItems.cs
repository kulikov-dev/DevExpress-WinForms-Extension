using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevExpressWinFormsExtension.Samples.Data
{
    /// <summary>
    /// Base row info for a PropertyGridControl demo
    /// </summary>
    public class BaseRowInfo
    {
        /// <summary>
        /// Price
        /// </summary>
        [Category("Level"), Display(Name = "Price", Order = 0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Percent price
        /// </summary>
        [Category("Level"), Display(Name = "Price in percent", Order = 2)]
        public double PercentPrice { get; set; }
    }

    /// <summary>
    /// Extended row info for a PropertyGridControl demo
    /// </summary>
    public sealed class ExtendedRowInfo :BaseRowInfo
    {
        /// <summary>
        /// View in percent
        /// </summary>
        [Category("Level"), Display(Name = "Show in percent", Order = 1)]
        public bool InPercent { get; set; }

        /// <summary>
        /// Is level usable
        /// </summary>
        [Category("Level"), Display(Name = "Use level", Order = 3)]
        public bool Use { get; set; }
    }
}