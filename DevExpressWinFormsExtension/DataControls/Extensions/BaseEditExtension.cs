using DevExpress.XtraEditors;
using System;
using DevExpressWinFormsExtension.Helpers;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    /// <summary>
    /// Extension for BaseEdit
    /// </summary>
    public static class BaseEditExtension
    {
        /// <summary>
        /// Check if no set value in the editor. 
        /// </summary>
        /// <param name="editor"> Editor </param>
        /// <param name="setWarningColor"> Flag, if need to set warning back color on empty value </param>
        /// <returns> True, if value empty </returns>
        public static bool IsValueEmpty(this BaseEdit editor, bool setWarningColor = true)
        {
            bool isValid;
            if (editor is TextEdit && !(editor is PopupContainerEdit))
            {
                isValid = !string.IsNullOrWhiteSpace(editor.Text);
            }
            else if (editor is DateEdit)
            {
                var dateEdit = editor as DateEdit;
                isValid = dateEdit.DateTime != DateTime.MinValue && dateEdit.DateTime != DateTime.MaxValue;
            }
            else
            {
                isValid = editor.EditValue != null && !string.IsNullOrWhiteSpace(editor.EditValue.ToString());
            }

            if (setWarningColor)
            {
                editor.BackColor = isValid ? SkinHelper.GetEditorBackColor() : SkinHelper.GetEditorWarningBackColor();
            }

            return !isValid;
        }
    }
}