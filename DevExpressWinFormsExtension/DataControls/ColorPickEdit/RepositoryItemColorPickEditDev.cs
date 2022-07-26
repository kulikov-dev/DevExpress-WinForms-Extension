using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using System;
using System.Reflection;

namespace DevExpressWinFormsExtension.DataControls.Editors.ColorPickEdit
{
    /// <summary>
    /// RepositoryItemColorPickEdit component
    /// </summary>
    /// <remarks> The extension for the standard components allows to save custom user colors during the program, so each ColorPickEdit and RepositoryItemColorPickEdit will have the same, actual user colors. </remarks>
    public partial class RepositoryItemColorPickEditDev : DevExpress.XtraEditors.Repository.RepositoryItemColorPickEdit
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RepositoryItemColorPickEditDev()
        {
            InitializeComponent();
            ColorPickDialogClosed += Properties_ColorPickDialogClosed;
            BeforePopup += RepositoryItemColorEdit_BeforePopup;
            Popup += RepositoryItemColorEdit_Popup;
        }

        /// <summary>
        /// Event before a dialog popup
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void RepositoryItemColorEdit_Popup(object sender, EventArgs e)
        {
            var popupFormProperty = typeof(DevExpress.XtraEditors.ColorPickEdit).GetProperty("PopupForm", BindingFlags.Instance | BindingFlags.NonPublic);
            if (popupFormProperty?.GetValue(this, null) is PopupColorEditForm popupForm)
            {
                popupForm.TabControl.SelectedTabPageIndex = 0;
            }
        }

        /// <summary>
        /// Event before a dialog popup
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void RepositoryItemColorEdit_BeforePopup(object sender, EventArgs e)
        {
            ColorPickEditUtils.InitializeCustomColors(RecentColors);
        }

        /// <summary>
        /// Event on dialog closed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void Properties_ColorPickDialogClosed(object sender, ColorPickDialogClosedEventArgs e)
        {
            ColorPickEditUtils.SaveCustomUserColors(RecentColors);
        }
    }
}