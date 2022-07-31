using DevExpress.XtraEditors.ColorPickEditControl;
using System.Collections.Generic;
using System.Drawing;

namespace DevExpressWinFormsExtension.DataControls.ColorPickEdit
{
    /// <summary>
    /// Utils for ColorPickUpEdit components
    /// </summary>
    internal static class ColorPickEditUtils
    {
        /// <summary>
        /// All user-defiended colors, to apply for all pickup components in a program
        /// </summary>
        /// <remarks> If necessary - can save them to xml. </remarks>
        internal static List<Color> CustomUserColors = new List<Color>();

        /// <summary>
        /// Initialize user custom colors in editors
        /// </summary>
        /// <param name="editorColorsMatrix"> An editor color matrix </param>
        internal static void InitializeCustomColors(Matrix editorColorsMatrix)
        {
            if (CustomUserColors.Count == 0)
            {
                return;
            }

            editorColorsMatrix.Clear();
            for (var i = 0; i < CustomUserColors.Count; ++i)
            {
                var color = CustomUserColors[i];
                editorColorsMatrix[0, i] = color;
            }
        }

        /// <summary>
        /// Save user defined colors
        /// </summary>
        /// <param name="editorColorsMatrix"> Editor colors matrix </param>
        internal static void SaveCustomUserColors(Matrix editorColorsMatrix)
        {
            CustomUserColors.Clear();
            for (var i = 0; i < editorColorsMatrix.ColumnCount; i++)
            {
                var color = editorColorsMatrix[0, i];
                if (color.IsEmpty || color.A == 0)
                {
                    continue;
                }

                CustomUserColors.Add(color);
            }
        }
    }
}