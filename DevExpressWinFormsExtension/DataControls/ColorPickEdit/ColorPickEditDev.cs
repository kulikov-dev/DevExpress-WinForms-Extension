using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace DevExpressWinFormsExtension.DataControls.ColorPickEdit
{
    /// <summary>
    /// ColorPickEdit component
    /// </summary>
    /// <remarks> The extension for the standard components allows to save custom user colors during the program, so each ColorPickEdit and RepositoryItemColorPickEdit will have the same, actual user colors. </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DevExpress.XtraEditors.ColorPickEdit))]
    [Description("ColorPickEdit component")]
    public partial class ColorPickEditDev : DevExpress.XtraEditors.ColorPickEdit
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ColorPickEditDev()
        {
            InitializeComponent();
            Properties.ColorPickDialogClosed += Properties_ColorPickDialogClosed;
        }

        /// <summary>
        /// Event on dialog closed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void Properties_ColorPickDialogClosed(object sender, ColorPickDialogClosedEventArgs e)
        {
            ColorPickEditUtils.SaveCustomUserColors(Properties.RecentColors);
        }

        /// <summary>
        /// Event on showing popup dialog
        /// </summary>
        public override void ShowPopup()
        {
            ColorPickEditUtils.InitializeCustomColors(Properties.RecentColors);
            var popupFormProperty = typeof(DevExpress.XtraEditors.ColorPickEdit).GetProperty("PopupForm", BindingFlags.Instance | BindingFlags.NonPublic);
            if (popupFormProperty?.GetValue(this, null) is PopupColorEditForm popupForm)
            {
                popupForm.TabControl.SelectedTabPageIndex = 0;
            }

            base.ShowPopup();
        }
    }
}