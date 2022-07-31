using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.Utils;
using DevExpressWinFormsExtension.Interfaces;
using DevExpressWinFormsExtension.DataControls.Forms;

namespace DevExpressWinFormsExtension.DataControls
{
    /// <summary>
    /// InputBox with possibility of using custom function for user input validation
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TextEdit))]
    [Description("InputBox with possibility of using custom function for user input validation.")]
    public partial class InputBoxValidableDev : XtraUserControlDev, IValidable
    {
        /// <summary>
        /// Custom function of user input validation
        /// </summary>
        private readonly Func<string, string> customInputValidationFunc;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        private InputBoxValidableDev()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="defaultValue"> Default value in the inputBox </param>
        /// <param name="labelTitle"> Title for the label </param>
        /// <param name="customInputValidationFunc"> Custom function of user input validation </param>
        private InputBoxValidableDev(string defaultValue, string labelTitle, Func<string, string> customInputValidationFunc)
            : this()
        {
            if (!string.IsNullOrWhiteSpace(labelTitle))
            {
                labelText.Text = labelTitle;
                editValue.Location = new Point(labelText.Location.X + labelText.Size.Width + 5, editValue.Location.Y);
                editValue.Size = new Size(Width - labelText.Size.Width - 20, editValue.Size.Height);
            }

            editValue.Text = defaultValue;
            this.customInputValidationFunc = customInputValidationFunc;
            SetControlState(ValidateData());
        }

        /// <summary>
        /// User input value
        /// </summary>
        public string InputValue
        {
            get
            {
                return editValue.Text.Trim();
            }
        }

        /// <summary>
        /// Show InputBox
        /// </summary>
        /// <param name="controlCaption"> Control's title caption </param>
        /// <returns> User input value </returns>
        public static string Show(string controlCaption)
        {
            return Show(string.Empty, controlCaption, string.Empty);
        }

        /// <summary>
        /// Show InputBox
        /// </summary>
        /// <param name="controlCaption"> Control's title caption </param>
        /// <param name="defaultValue"> Default value in the inputBox </param>
        /// <param name="labelTitle"> Title for the label </param>
        /// <returns> User input value </returns>
        public static string Show(string controlCaption, string defaultValue, string labelTitle)
        {
            var result = Show(controlCaption, labelTitle);
            return string.IsNullOrWhiteSpace(result) ? result : defaultValue;
        }

        /// <summary>
        /// Show InputBox
        /// </summary>
        /// <param name="controlCaption"> Control's title caption </param>
        /// <param name="labelTitle"> Title for the label </param>
        /// <param name="customInputValidationFunc"> Custom function of user input validation </param>
        /// <param name="parentControl"> Parent control </param>
        /// <returns> Dialog result </returns>
        public static string Show(string controlCaption, string labelTitle, Func<string, string> customInputValidationFunc = null, Control parentControl = null)
        {
            using (var control = new InputBoxValidableDev(string.Empty, labelTitle, customInputValidationFunc))
            {
                var result = ControlBox.Show(parentControl, control, controlCaption, MessageBoxButtons.OKCancel, 0, canResize: false, control);
                if (result == DialogResult.OK)
                {
                    return control.InputValue;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Setup control's state, depends on validation result
        /// </summary>
        /// <param name="isValid"> Validation result </param>
        public void SetControlState(bool isValid)
        {
            btnOk.Enabled = isValid;
        }

        /// <summary>
        /// Control validation
        /// </summary>
        /// <returns> True, if valid </returns>
        public bool ValidateData()
        {
            var hasError = false;
            if (string.IsNullOrEmpty(InputValue))
            {
                editValue.ToolTipController.ShowHint("Input value can't be empty.", editValue, ToolTipLocation.BottomCenter);
                hasError = true;
            }
            else
            {
                toolTipController.HideHint();
            }

            if (customInputValidationFunc != null)
            {
                var textMessage = customInputValidationFunc(InputValue);
                if (!string.IsNullOrWhiteSpace(textMessage))
                {
                    editValue.ToolTipController.ShowHint(textMessage, editValue, ToolTipLocation.BottomCenter);
                    hasError = true;
                }
                else
                {
                    toolTipController.HideHint();
                }
            }

            editValue.BackColor = hasError ? SkinHelper.GetEditorWarningBackColor() : SkinHelper.GetEditorBackColor();
            return !hasError;
        }

        /// <summary>
        /// Event on input value changing
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void EditValueChanging(object sender, ChangingEventArgs e)
        {
            SetControlState(ValidateData());
        }
    }
}