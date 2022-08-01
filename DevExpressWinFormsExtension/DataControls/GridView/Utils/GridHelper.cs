using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System;
using System.Data;

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
    }
}