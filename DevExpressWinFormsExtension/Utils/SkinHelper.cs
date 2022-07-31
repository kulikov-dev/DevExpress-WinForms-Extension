using DevExpress.LookAndFeel;
using DevExpress.Skins;
using System.Collections.Generic;
using System.Drawing;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Utils to work with colors considering the App Theme.
    /// </summary>
    public static class SkinHelper
    {
        /// <summary>
        /// Dark theme title
        /// </summary>
        private const string darkThemeTitle = "Dark";

        /// <summary>
        /// Comparison of colors for Theme
        /// </summary>
        /// <remarks> Key: Theme name. Value: source/translated colors </remarks>
        private static readonly Dictionary<string, Dictionary<Color, Color>> editorBackColors = new Dictionary<string, Dictionary<Color, Color>>();

        /// <summary>
        /// Check if current skin is dark skin
        /// </summary>
        public static bool IsDarkSkin
        {
            get
            {
                var currentSkin = CommonSkins.GetSkin(UserLookAndFeel.Default);
                return currentSkin.Name.Contains(darkThemeTitle);
            }
        }

        /// <summary>
        /// Convert color to the current theme color
        /// </summary>
        /// <param name="sourceColor"> Source color </param>
        /// <returns> Converted color </returns>
        public static Color TranslateColor(Color sourceColor)
        {
            var currentSkin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            if (editorBackColors.ContainsKey(currentSkin.Name) && editorBackColors[currentSkin.Name].ContainsKey(sourceColor))
            {
                return editorBackColors[currentSkin.Name][sourceColor];
            }

            var color = currentSkin.TranslateColor(sourceColor);
            if (!editorBackColors.ContainsKey(currentSkin.Name))
            {
                editorBackColors.Add(currentSkin.Name, new Dictionary<Color, Color>());
            }

            editorBackColors[currentSkin.Name].Add(sourceColor, color);
            return color;
        }

        /// <summary>
        /// Get default back color of the editor
        /// </summary>
        /// <returns> Default back color </returns>
        public static Color GetEditorBackColor()
        {
            return TranslateColor(SystemColors.Window);
        }

        /// <summary>
        /// Get warning back color for the editor 
        /// </summary>
        /// <returns> Warning color </returns>
        /// <remarks> For empty or not validated values </remarks>
        public static Color GetEditorWarningBackColor()
        {
            return TranslateColor(Color.MistyRose);
        }

        /// <summary>
        /// Get default text color according to the Theme
        /// </summary>
        /// <returns> Text color </returns>
        public static Color GetTextColor()
        {
            return TranslateColor(SystemColors.ControlText);
        }
    }
}