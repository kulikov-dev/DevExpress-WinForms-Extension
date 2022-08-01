using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpressWinFormsExtension.DataControls.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevExpressWinFormsExtension.DataControls
{
    /// <summary>
    /// GroupControl with checkbox in header.
    /// </summary>
    /// <remarks> Allows user to disable/enable all controls in the GroupControl </remarks>
    [ToolboxItem(true)]
    [Description("GroupControl with checkbox in header.")]
    [ToolboxBitmap(typeof(GroupControl))]
    [Designer(typeof(ParentControlDesigner))]
    public partial class GroupControlCheckedDev : XtraUserControlDev
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public GroupControlCheckedDev()
        {
            InitializeComponent();
            foreach (Control control in Controls)
            {
                control.Enabled = Checked;
            }

            header.Enabled = true;
        }

        /// <summary>
        /// Event on checked changed
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Event on checked changed")]
        public event EventHandler CheckedChanged;

        /// <summary>
        /// GroupBox title
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("GroupBox title")]
        public string Title
        {
            get
            {
                return checkEdit.Text;
            }

            set
            {
                checkEdit.Text = value;
            }
        }

        /// <summary>
        /// CheckBox state
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("CheckBox state")]
        public bool Checked
        {
            get
            {
                return checkEdit.Checked;
            }

            set
            {
                checkEdit.Checked = value;
            }
        }

        /// <summary>
        /// CheckBox style
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("CheckBox style")]
        public CheckBoxStyle CheckStyle
        {
            get
            {
                return checkEdit.Properties.CheckBoxOptions.Style;
            }

            set
            {
                checkEdit.Properties.CheckBoxOptions.Style = value;
            }
        }

        /// <summary>
        /// Event on checked changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void CheckEditCheckedChanged(object sender, EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
            foreach (Control control in Controls)
            {
                if (control != header)
                {
                    control.Enabled = Checked;
                }
            }
        }

        /// <summary>
        /// Event on new control added
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GroupControlWithCheckBoxControlAdded(object sender, ControlEventArgs e)
        {
            //e.Control.Enabled = Checked;
           // header.SendToBack();
        }
    }
}