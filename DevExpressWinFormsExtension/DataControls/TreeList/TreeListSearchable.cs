using DevExpress.XtraEditors.Controls;
using DevExpressWinFormsExtension.DataControls.TreeList.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace DevExpressWinFormsExtension.DataControls.TreeList
{
    /// <summary>
    /// TreeList component with filtration panel on top
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DevExpress.XtraTreeList.TreeList))]
    [Designer(typeof(TreeListSearchableDesigner))]
    [Description("TreeList with filtration panel on top")]
    public partial class TreeListSearchable : UserControlDev
    {
        /// <summary>
        /// Custom function for filtration
        /// </summary>
        public Action<string> CustomFilterFunction;

        /// <summary>
        /// Flag if filter panel is enabled
        /// </summary>
        private bool isSearchable = true;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TreeListSearchable()
        {
            InitializeComponent();
            beSearch.EditValue = null;
        }

        /// <summary>
        /// TreeList
        /// </summary>
        public TreeListDev TreeList
        {
            get
            {
                return dataTreeList;
            }
        }

        /// <summary>
        /// Flag if filter panel is enabled
        /// </summary>
        [Category("Appearance")]
        [Description("Enable filter panel")]
        public bool IsSearchable
        {
            get
            {
                return isSearchable;
            }

            set
            {
                isSearchable = value;
                beSearch.EditValue = null;
                panelFilter.Visible = isSearchable;
            }
        }

        /// <summary>
        /// Filter condition
        /// </summary>
        public string FilterCondition
        {
            get
            {
                return beSearch.Text == beSearch.Properties.NullText ? string.Empty : beSearch.Text;
            }
        }

        /// <summary>
        ///Event on filter condition changing
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void beSearch_EditValueChanging(object sender, ChangingEventArgs e)
        {
            try
            {
                BeginUpdate();

                if (CustomFilterFunction != null)
                {
                    CustomFilterFunction(e.NewValue == null ? string.Empty : (string)e.NewValue);
                }
                else
                {
                    if (e.NewValue == null || string.IsNullOrWhiteSpace(e.NewValue.ToString()))
                    {
                        TreeList.ClearNodesFilter();
                    }
                    else
                    {
                        TreeList.Filter(((string)e.NewValue).Trim());
                    }
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Event on clear filter
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void beSearch_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            beSearch.EditValue = null;
        }
    }
}