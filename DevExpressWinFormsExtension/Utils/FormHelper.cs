using DevExpressWinFormsExtension.DataControls.Forms;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Helper to work with forms
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        /// Check if there are any shown form with a specific title
        /// </summary>
        /// <param name="title"> Title to compare </param>
        /// <param name="tagData"> Tag data to compare </param>
        /// <param name="needFocus"> Flag if need focus on opened form </param>
        /// <returns> Flag if the form showed </returns>
        public static bool IsFormShown(string title, object tagData = null, bool needFocus = true)
        {
            return GetShownForm(title, tagData, needFocus) != null;
        }

        /// <summary>
        /// Get shown form with a specific title
        /// </summary>
        /// <param name="title"> Title to compare </param>
        /// <param name="tagData"> Tag data to compare </param>
        /// <param name="needFocus"> Flag if need focus on opened form </param>
        /// <returns> Shown form </returns>
        public static XtraFormDev GetShownForm(string title, object tagData = null, bool needFocus = true)
        {
            XtraFormDev openForm = null;

            foreach (var form in Application.OpenForms)
            {
                if (form is XtraFormDev devForm && (devForm.Text == title) && (tagData == null || tagData == devForm.Tag))
                {
                    openForm = devForm;

                    break;
                }
            }

            if ((openForm != null) && needFocus)
            {
                openForm.Focus();
            }

            return openForm;
        }
    }
}
