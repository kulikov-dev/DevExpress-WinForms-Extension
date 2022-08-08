using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.DataControls.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Helper to work with XtraUserControl.
    /// </summary>
    /// <remarks> Allows to show a control is float/dialog modes. </remarks>
    public static class XtraUserControlHelper
    {
        /// <summary>
        /// Form buttons margin
        /// </summary>
        private const int formButtonsMargin = 4;

        /// <summary>
        /// Show a control in a float mode
        /// </summary>
        /// <param name="ownerForm"> An owner </param>
        /// <param name="control"> The control to show </param>
        /// <param name="title"> Control title </param>
        /// <param name="isSizable"> Is control sizable </param>
        /// <returns> Created form with the control on it </returns>
        /// <remarks> Auto process all sub control with a public modifier </remarks>
        public static Form ShowFloatControl(XtraForm ownerForm, Control control, string title, bool isSizable)
        {
            var parentForm = CreateParentForm(control, ShowingControlModeEnum.Float, title, isSizable);
            parentForm.Owner = ownerForm;
            parentForm.ShowInTaskbar = true;

            SetupControl(control, parentForm);
            PutControlsOnFormFooter(parentForm, control, null, 0);

            if (ownerForm != null)
            {
                parentForm.Show(ownerForm);
            }
            else
            {
                parentForm.Show();
            }

            return parentForm;
        }

        /// <summary>
        /// Show a control in a dialog mode
        /// </summary>
        /// <param name="ownerForm"> An owner </param>
        /// <param name="control"> The control to show </param>
        /// <param name="title"> Control title </param>
        /// <param name="defaultButtons"> Default buttons for the control </param>
        /// <param name="isSizable"> Is control sizable </param>
        /// <param name="acceptButtonIndex"> The button on the form that is clicked when the user presses the ENTER key </param>
        /// <param name="focusedChild"> Focused child control </param>
        /// <returns> DialogResult </returns>
        public static DialogResult ShowControl(Control ownerForm, Control control, string title, MessageBoxButtons? defaultButtons, bool isSizable, int acceptButtonIndex = -1, Control focusedChild = null)
        {
            var parentForm = CreateParentForm(control, ShowingControlModeEnum.DialogBox, title, isSizable, focusedChild);
            PutControlsOnFormFooter(parentForm, control, defaultButtons, acceptButtonIndex);
            SetupControl(control, parentForm);

            return ownerForm != null ? parentForm.ShowDialog(ownerForm) : parentForm.ShowDialog();
        }

        /// <summary>
        /// Show a control in a dialog mode
        /// </summary>
        /// <param name="ownerForm"> An owner </param>
        /// <param name="control"> The control to show </param>
        /// <param name="title"> Control title </param>
        /// <param name="isSizable"> Is a control sizable </param>
        /// <param name="focusedChild"></param>
        /// <returns></returns>
        public static DialogResult ShowControl(Control ownerForm, Control control, string title, bool isSizable, Control focusedChild = null)
        {
            return ShowControl(ownerForm, control, title, null, isSizable, -1, focusedChild);
        }

        /// <summary>
        /// Create parent form
        /// </summary>
        /// <param name="control"> The control to show </param>
        /// <param name="showingMode"> Showing mode </param>
        /// <param name="title"> Control title </param>
        /// <param name="isSizable"> Is a control sizable </param>
        /// <param name="focusedChild"> Focused child control </param>
        /// <returns> The parent form </returns>
        private static ShowControlParentForm CreateParentForm(Control control, ShowingControlModeEnum showingMode, string title, bool isSizable, Control focusedChild = null)
        {
            int defaultButtonHeight;
            using (var button = new SimpleButton())
            {
                defaultButtonHeight = button.Height;
            }

            var parentForm = new ShowControlParentForm(focusedChild, showingMode)
            {
                Text = title,
                FormBorderStyle = isSizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedToolWindow,
                ShowIcon = false,
                MaximizeBox = isSizable,
                StartPosition = FormStartPosition.CenterScreen,
                KeyPreview = true,
                ClientSize = new Size(control.Width + (2 * formButtonsMargin), control.Height + defaultButtonHeight + (4 * formButtonsMargin))
            };

            if (!control.MinimumSize.IsEmpty)
            {
                parentForm.MinimumSize = new Size(control.MinimumSize.Width + (2 * formButtonsMargin), control.MinimumSize.Height + defaultButtonHeight + (3 * formButtonsMargin)) + (parentForm.Size - parentForm.ClientSize);
            }

            if (!control.MaximumSize.IsEmpty)
            {
                parentForm.MaximumSize = new Size(control.MaximumSize.Width + (2 * formButtonsMargin), control.MaximumSize.Height + defaultButtonHeight + (3 * formButtonsMargin)) + (parentForm.Size - parentForm.ClientSize);
            }

            return parentForm;
        }

        /// <summary>
        /// The control initialization
        /// </summary>
        /// <param name="control"> A control </param>
        /// <param name="parentForm"> A parent form </param>
        private static void SetupControl(Control control, XtraForm parentForm)
        {
            control.Location = new Point(formButtonsMargin, formButtonsMargin);
            control.Parent = parentForm;
            if (control.Dock != DockStyle.Fill)
            {
                control.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
        }

        /// <summary>
        /// Get children controls with a public modifier
        /// </summary>
        /// <param name="parentConrol"> Control </param>
        /// <returns> List of children controls </returns>
        /// <remarks> All these controls will placed on a form footer </remarks>
        private static List<BaseControl> GetPublicChildren(Control parentConrol)
        {
            var result = new List<BaseControl>();
            var publicFields = parentConrol.GetType().GetFields().Where(field => field.IsPublic);
            foreach (var field in publicFields)
            {
                if (field.GetValue(parentConrol) is BaseControl button)
                {
                    result.Add(button);
                }
            }

            return result;
        }

        /// <summary>
        /// Put all children controls with public modifier to the form footer
        /// </summary>
        /// <param name="parentForm"> Parent form </param>
        /// <param name="control"> Showed control </param>
        /// <param name="buttons"> Default buttons </param>
        /// <param name="acceptButtonIndex"> The button on the form that is clicked when the user presses the ENTER key </param>
        private static void PutControlsOnFormFooter(ShowControlParentForm parentForm, Control control, MessageBoxButtons? buttons, int acceptButtonIndex)
        {
            var controlElements = GetPublicChildren(control);
            var okButton = default(SimpleButton);
            foreach (var element in controlElements)
            {
                if (!(element is SimpleButton button))
                {
                    continue;
                }

                switch (button.DialogResult)
                {
                    case DialogResult.OK:
                        okButton = button;
                        if (buttons == null)
                        {
                            parentForm.AcceptButton = button;
                        }

                        break;
                    case DialogResult.Cancel:
                        if (buttons == null)
                        {
                            parentForm.CancelButton = button;
                        }

                        break;
                }
            }

            if (buttons != null)
            {
                AppendDefaultButtons(parentForm, ref controlElements, buttons.Value);
            }

            PutControlsOnFormFooter(parentForm, controlElements, acceptButtonIndex);
        }

        /// <summary>
        /// Put all children controls with public modifier to the form footer
        /// </summary>
        /// <param name="parentForm"> Parent form </param>
        /// <param name="childrenControls"> Children controls </param>
        /// <param name="acceptButtonIndex"> The button on the form that is clicked when the user presses the ENTER key </param>
        private static void PutControlsOnFormFooter(ShowControlParentForm parentForm, List<BaseControl> childrenControls, int acceptButtonIndex)
        {
            var orderedControls = (from button in childrenControls
                                   orderby (button.Tag != null && double.TryParse(button.Tag.ToString(), out double order) ? double.Parse(button.Tag.ToString()) : 1)
                                   select button).ToList();

            var footerMargin = parentForm.FormBorderStyle == FormBorderStyle.Sizable || parentForm.FormBorderStyle == FormBorderStyle.SizableToolWindow ? 10 : 0;
            var leftPosition = parentForm.ClientSize.Width - formButtonsMargin - footerMargin;
            var rightPosition = parentForm.Left + formButtonsMargin + footerMargin;
            var reversePos = 0;
            for (var i = orderedControls.Count - 1; i >= 0; i--)
            {
                var control = orderedControls[i];
                var order = control.Tag != null && double.TryParse(control.Tag.ToString(), out double orderValue) && orderValue < 0 ? orderValue : 0;
                if (order < 0)
                {
                    control.Location = new Point(rightPosition, parentForm.ClientSize.Height - control.Height - formButtonsMargin);
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                    control.Parent = parentForm;
                    rightPosition += control.Width + formButtonsMargin;
                }
                else
                {
                    control.Location = new Point(leftPosition - control.Width, parentForm.ClientSize.Height - control.Height - formButtonsMargin);

                    control.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    control.Parent = parentForm;
                    leftPosition -= control.Width + formButtonsMargin;
                }

                orderedControls[reversePos].TabIndex = reversePos + 1;
                ++reversePos;
            }

            if (acceptButtonIndex >= 0 && acceptButtonIndex < orderedControls.Count)
            {
                var acceptableButton = orderedControls[acceptButtonIndex] as SimpleButton;
                if (acceptableButton?.DialogResult != DialogResult.None)
                {
                    parentForm.AcceptButton = orderedControls[acceptButtonIndex] as SimpleButton;
                }
            }

            parentForm.ClientSize = new Size(Math.Max(parentForm.ClientSize.Width - leftPosition + (2 * formButtonsMargin), parentForm.ClientSize.Width), parentForm.ClientSize.Height);
        }

        /// <summary>
        /// Appen default buttons
        /// </summary>
        /// <param name="parentForm"> Parent form</param>
        /// <param name="childrenControls"> Children controls </param>
        /// <param name="buttons"> Default buttons </param>
        /// <returns> List of default buttons </returns>
        private static void AppendDefaultButtons(ShowControlParentForm parentForm, ref List<BaseControl> childrenControls, MessageBoxButtons buttons)
        {
            var dialogResult = DialogResult.Cancel;
            switch (buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    AppendButton(ref childrenControls, "Abort", DialogResult.Abort, 1);
                    AppendButton(ref childrenControls, "Retry", DialogResult.Retry, 2);
                    AppendButton(ref childrenControls, "Ignore", DialogResult.Ignore, 3);
                    dialogResult = DialogResult.Ignore;
                    break;
                case MessageBoxButtons.OK:
                    AppendButton(ref childrenControls, "OK", DialogResult.OK, 1);
                    break;
                case MessageBoxButtons.OKCancel:
                    AppendButton(ref childrenControls, "OK", DialogResult.OK, 1);
                    AppendButton(ref childrenControls, "Cancel", DialogResult.Cancel, 2);
                    break;
                case MessageBoxButtons.RetryCancel:
                    AppendButton(ref childrenControls, "Retry", DialogResult.Retry, 1);
                    AppendButton(ref childrenControls, "Cancel", DialogResult.Cancel, 2);
                    break;
                case MessageBoxButtons.YesNo:
                    AppendButton(ref childrenControls, "Yes", DialogResult.Yes, 1);
                    AppendButton(ref childrenControls, "No", DialogResult.No, 2);
                    dialogResult = DialogResult.No;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AppendButton(ref childrenControls, "Yes", DialogResult.Yes, 1);
                    AppendButton(ref childrenControls, "No", DialogResult.No, 2);
                    AppendButton(ref childrenControls, "Cancel", DialogResult.Cancel, 3);
                    break;
            }

            if (childrenControls.Count == 0)
            {
                childrenControls.Add(new SimpleButton() { Text = "Close", DialogResult = DialogResult.Cancel });
                parentForm.CancelButton = childrenControls[0] as SimpleButton;
            }

            var cancelButton = childrenControls.FirstOrDefault(x => (x as SimpleButton)?.DialogResult == dialogResult) as SimpleButton;
            if (cancelButton != null)
            {
                parentForm.CancelButton = cancelButton;
                cancelButton.Click += OnParentFormDispose;
            }
        }

        /// <summary>
        /// Add a button to a form
        /// </summary>
        /// <param name="childrens"> List of public child controls </param>
        /// <param name="text"> Button text </param>
        /// <param name="dialogResult"> Dialog result of the button </param>
        /// <param name="order"> Order </param>
        private static void AppendButton(ref List<BaseControl> childrens, string text, DialogResult dialogResult, double order)
        {
            if (!childrens.Exists(btn => (btn as SimpleButton)?.DialogResult == dialogResult))
            {
                childrens.Add(new SimpleButton() { Text = text, DialogResult = DialogResult.Cancel, Tag = order });
            }
        }

        /// <summary>
        /// Event on closing dialog parent form
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private static void OnParentFormDispose(object sender, EventArgs e)
        {
            var parentForm = (sender as SimpleButton)?.Parent ?? sender as ShowControlParentForm;
            parentForm?.Dispose();
        }

        /// <summary>
        /// Showing mode enum
        /// </summary>
        internal enum ShowingControlModeEnum
        {
            DialogBox,
            Float
        }
    }
}