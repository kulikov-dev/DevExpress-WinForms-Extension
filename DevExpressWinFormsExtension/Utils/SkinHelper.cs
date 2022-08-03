using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using System;
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

        /// <summary>
        /// Get a default skin preview image
        /// </summary>
        /// <param name="width"> Image width </param>
        /// <param name="height"> Image height </param>
        /// <param name="indent"> Indent for bounds </param>
        /// <returns> Image </returns>
        /// <remarks> Useful, if need to create buttons for user to select an app theme </remarks>
        public static Image GetSkinPreviewImage(int width, int height, int indent = 0)
        {
            var currentSkin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            return GetSkinPreviewImage(currentSkin.Name, width, height, indent);
        }

        /// <summary>
        /// Get a skin preview image
        /// </summary>
        /// <param name="skinName"> Skin name </param>
        /// <param name="width"> Image width </param>
        /// <param name="height"> Image height </param>
        /// <param name="indent"> Indent for bounds </param>
        /// <returns> Image </returns>
        /// <remarks> Useful, if need to create buttons for user to select an app theme </remarks>
        public static Image GetSkinPreviewImage(string skinName, int width, int height, int indent = 0)
        {
            using (var button = new SimpleButton())
            {
                var skin = SkinManager.Default.Skins[skinName];
                if (skin == null)
                {
                    throw new ArgumentException("Skin name not found");
                }

                button.LookAndFeel.SetSkinStyle(skin.SkinName);
                var image = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(image))
                {
                    var info = new StyleObjectInfoArgs(new GraphicsCache(graphics))
                    {
                        Bounds = new Rectangle(0, 0, width, height)
                    };

                    button.LookAndFeel.Painter.GroupPanel.DrawObject(info);
                    button.LookAndFeel.Painter.Border.DrawObject(info);
                    info.Bounds = new Rectangle(indent, indent, width - indent * 2, height - indent * 2);
                    button.LookAndFeel.Painter.Button.DrawObject(info);
                }

                return image;
            }
        }
    }
}