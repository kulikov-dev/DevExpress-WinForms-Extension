using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Natural comparer
    /// </summary>
    /// <remarks>
    /// DevExpress has it's own NaturalComparer, however it's not work for my case, such strings:
    /// 'S_505' and 'S_0505' will equal in DevExpress. But it shouldn't be.
    /// </remarks>
    public class NaturalStringComparer : IComparer<string>
    {
        /// <summary>
        /// Flag of ascending sort order
        /// </summary>
        private readonly bool isAscending;

        /// <summary>
        /// Cache
        /// </summary>
        private readonly Dictionary<string, string[]> cache = new Dictionary<string, string[]>();

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="inAscendingOrder"> Flag of ascending sort order </param>
        public NaturalStringComparer(bool inAscendingOrder = true)
        {
            isAscending = inAscendingOrder;
        }

        /// <summary>
        /// Compare two strings
        /// </summary>
        /// <param name="x"> String 1 </param>
        /// <param name="y"> String 2 </param>
        /// <returns> Result of comparing </returns>
        /// <remarks> DO not remove! </remarks>
        public int Compare(string x, string y)
        {
            return (this as IComparer<string>).Compare(x, y);
        }

        /// <summary>
        /// Compare two strings
        /// </summary>
        /// <param name="x"> String 1 </param>
        /// <param name="y"> String 2 </param>
        /// <returns> Result of comparing </returns>
        int IComparer<string>.Compare(string x, string y)
        {
            if (x == y)
            {
                return 0;
            }

            if (!cache.TryGetValue(x, out var x1))
            {
                x1 = Regex.Split(x.Replace(" ", string.Empty), "([0-9]+)");

                cache.Add(x, x1);
            }

            if (!cache.TryGetValue(y, out var y1))
            {
                y1 = Regex.Split(y.Replace(" ", string.Empty), "([0-9]+)");

                cache.Add(y, y1);
            }

            int returnVal;

            for (var i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] == y1[i])
                {
                    continue;
                }

                returnVal = StringPartsCompare(x1[i], y1[i]);

                return isAscending ? returnVal : -returnVal;
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return isAscending ? returnVal : -returnVal;
        }

        /// <summary>
        /// Compare parts of the strings
        /// </summary>
        /// <param name="left"> Part of string 1 </param>
        /// <param name="right"> Part of string 2 </param>
        /// <returns> Result of comparing </returns>
        private static int StringPartsCompare(string left, string right)
        {
            if (!int.TryParse(left, out var x) || left.StartsWith("0"))
            {
                return string.Compare(left, right, StringComparison.Ordinal);
            }

            if (!int.TryParse(right, out var y) || right.StartsWith("0"))
            {
                return string.Compare(left, right, StringComparison.Ordinal);
            }

            return x.CompareTo(y);
        }
    }
}