using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using System.ComponentModel;
using System.Drawing;

namespace DevExpressWinFormsExtension.DataControls.Editors
{
    /// <summary>
    /// The extension for working with passwords, allows user to show/hide input characters.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ButtonEdit))]
    [Description("The extension for working with passwords, allows user to show/hide input characters.")]
    public class PasswordTextEditDev : ButtonEdit
    {
        /// <summary>
        /// Hide password char
        /// </summary>
        private const char passwordChar = '*';

        /// <summary>
        /// Tag for button for changing visibility of the password
        /// </summary>
        private const string buttonVisibilityTag = "Visible";

        /// <summary>
        /// Hint title to show password
        /// </summary>
        private const string showPasswordHint = "Show password";

        /// <summary>
        /// Hint title to hide password
        /// </summary>
        private const string hidePasswordHint = "Hide password";

        /// <summary>
        /// Current state
        /// </summary>
        private PasswordStateEnum passwordState = PasswordStateEnum.Hidden;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public PasswordTextEditDev()
        {
            var button = new EditorButton
            {
                Visible = true,
                ToolTip = showPasswordHint,
                Tag = buttonVisibilityTag,
                Kind = ButtonPredefines.Glyph
            };
            button.ImageOptions.Image = DevExpressWinFormsExtension.Properties.Resources.IsVisible_16;
            Properties.Buttons.Add(button);

            Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Properties.PasswordChar = passwordChar;
        }

        /// <summary>
        /// Enum of password visibility
        /// </summary>
        private enum PasswordStateEnum
        {
            /// <summary>
            /// Hiidden
            /// </summary>
            Hidden,

            /// <summary>
            /// Visible
            /// </summary>
            Visible
        }

        /// <summary>
        /// Event on buttons click
        /// </summary>
        /// <param name="buttonInfo"> Button info </param>
        protected override void OnClickButton(EditorButtonObjectInfoArgs buttonInfo)
        {
            base.OnClickButton(buttonInfo);

            passwordState = passwordState == PasswordStateEnum.Hidden ? PasswordStateEnum.Visible : PasswordStateEnum.Hidden;
            if (buttonInfo.Button.Tag?.ToString() != buttonVisibilityTag)
            {
                return;
            }

            switch (passwordState)
            {
                case PasswordStateEnum.Hidden:
                    buttonInfo.Button.ImageOptions.Image = DevExpressWinFormsExtension.Properties.Resources.IsVisible_16;
                    Properties.PasswordChar = passwordChar;
                    buttonInfo.Button.ToolTip = showPasswordHint;
                    break;
                case PasswordStateEnum.Visible:
                    buttonInfo.Button.ImageOptions.Image = DevExpressWinFormsExtension.Properties.Resources.IsHidden_16;
                    Properties.PasswordChar = '\0';
                    buttonInfo.Button.ToolTip = hidePasswordHint;
                    break;
            }
        }
    }
}