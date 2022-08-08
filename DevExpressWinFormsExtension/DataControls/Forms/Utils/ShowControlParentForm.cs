using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static DevExpressWinFormsExtension.Utils.XtraUserControlHelper;

namespace DevExpressWinFormsExtension.DataControls.Forms.Utils
{
    /// <summary>
    /// A form to show XtraUserControl's in dialog/float modes
    /// </summary>
    internal partial class ShowControlParentForm : XtraForm
    {
        /// <summary>
        /// Showing mode
        /// </summary>
        private readonly ShowingControlModeEnum showingMode;

        /// <summary>
        /// Focused child on the control
        /// </summary>
        private readonly Control focusedChild;

        /// <summary>
        /// Contructor with parameters
        /// </summary>
        /// <param name="focusedChild"> Focused child </param>
        /// <param name="showingMode"> Showing mode </param>
        public ShowControlParentForm(Control focusedChild, ShowingControlModeEnum showingMode)
        {
            this.focusedChild = focusedChild;
            this.showingMode = showingMode;
        }

        /// <summary>
        /// Event on form closed
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (showingMode == ShowingControlModeEnum.Float)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Event on closing
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != DialogResult.OK && DialogResult != DialogResult.Yes)
            {
                var allowClose = true;
                foreach (var control in Controls)
                {
                    if (control is IClosableControl closableControl)
                    {
                        allowClose &= closableControl.AllowClose();
                    }
                }

                e.Cancel = !allowClose;
                if (e.Cancel)
                {
                    return;
                }
            }

            foreach (Control control in Controls)
            {
                control.Dispose();
            }
        }

        /// <summary>
        /// Event on preprocess keys
        /// </summary>
        /// <param name="msg"> Message </param>
        /// <param name="keyData"> Key </param>
        /// <returns> True if success </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Event on key down
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        /// <summary>
        /// Event on form shown
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void frmControlBox_Shown(object sender, EventArgs e)
        {
            if (focusedChild == null)
            {
                var control = Controls.Cast<Control>().FirstOrDefault(c => !(c is SimpleButton));
                control?.Focus();
                return;
            }

            focusedChild.Focus();
        }
    }
}