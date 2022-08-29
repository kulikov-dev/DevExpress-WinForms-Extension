using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    public static class PropertyGridControlExtension
    {
        /// <summary>
        /// Reorder field rows by a "Display/Order" attribute
        /// </summary>
        /// <param name="control"> PropertyGridControl </param>
        public static void UpdateFieldsOrderByDisplayOrder(this PropertyGridControl control)
        {
            control.BeginUpdate();
            try
            {
                foreach (var row in control.Rows)
                {
                    UpdateRowOrder(row, null);
                }
            }
            finally
            {
                control.EndUpdate();
            }
        }

        /// <summary>
        /// Supported recursive void for ordering 
        /// </summary>
        /// <param name="row"> Current row </param>
        /// <param name="categoryType"> Base category type </param>
        private static void UpdateRowOrder(BaseRow row, Type categoryType)
        {
            foreach (var child in row.ChildRows)
            {
                UpdateRowOrder(child, row.Properties.RowType);
            }

            var type = categoryType ?? row.Properties.RowType;
            var property = type.GetProperty(row.Properties.FieldName);
            if (property?.GetCustomAttributes().FirstOrDefault(a => a is DisplayAttribute) is DisplayAttribute displayAttribute)
            {
                row.SetOrder(int.MaxValue - displayAttribute.Order); // As DevExpress used reversed order for rows
            }
        }
    }
}