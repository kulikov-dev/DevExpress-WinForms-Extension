using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.DataControls.Editors
{
    /// <summary>
    /// CheckedListBoxControl component
    /// </summary>
    /// <remarks> The extension for the standard component supports hotkeys for fast check/uncheck of items. Ctrl+A: check all, Ctrl+D: uncheck all, Ctrl+I: invert checking. </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(CheckedListBoxControl))]
    [Description("CheckedListBoxControl with hotkeys")]
    public class CheckedListBoxControlDev : CheckedListBoxControl
    {
        /// <summary>
        /// Event on KeyDown
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Control | Keys.D:
                    UnCheckAll();
                    break;
                case Keys.Control | Keys.A:
                    CheckAll();
                    break;
                case Keys.Control | Keys.I:
                    {
                        InvertSelection();
                        break;
                    }
            }
        }

        /// <summary>
        /// Inverse checkings
        /// </summary>
        private void InvertSelection()
        {
            BeginItemsCheck();
            try
            {
                foreach (CheckedListBoxItem item in Items)
                {
                    item.CheckState = item.CheckState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
                }
            }
            finally
            {
                EndItemsCheck();
            }
        }
    }
}