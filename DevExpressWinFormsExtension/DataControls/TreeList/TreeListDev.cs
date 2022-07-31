using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;
using DevExpressWinFormsExtension.DataControls.Extensions;

namespace DevExpressWinFormsExtension.DataControls.TreeList
{
    /// <summary>
    /// TreeList component
    /// </summary>
    /// <remarks> The extension for the standard component supports hotkeys for fast check/uncheck of items. Ctrl+A: check all, Ctrl+D: uncheck all, Ctrl+I: invert checking. </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DevExpress.XtraTreeList.TreeList))]
    [Description("TreeList")]
    public class TreeListDev : DevExpress.XtraTreeList.TreeList
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TreeListDev()
        {
            BeforeCheckNode += TreeListEx_BeforeCheckNode;
            AfterCheckNode += TreeListEx_AfterCheckNode;
        }

        /// <summary>
        /// Finalizes an instance of the TreeListDev class.
        /// </summary>
        ~TreeListDev()
        {
            BeforeCheckNode -= TreeListEx_BeforeCheckNode;
            AfterCheckNode -= TreeListEx_AfterCheckNode;
        }

        /// <summary>
        /// Flag if TreeListNode has custom images in tags and draws them itself
        /// </summary>
        [Category("Appearance")]
        [Description("Has custom nodes image")]
        public bool HasNodeImageInTag { get; set; }

        /// <summary>
        /// Create ViewInfo
        /// </summary>
        /// <returns> ViewInfo </returns>
        protected override TreeListViewInfo CreateViewInfo()
        {
            return new TreeListViewInfoDev(this, hasNodeImageInTag: HasNodeImageInTag);
        }

        /// <summary>
        /// Event on mouse double click
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            var hitInfo = CalcHitInfo(e.Location);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (hitInfo.HitInfoType != HitInfoType.Button && Selection.Count > 0)
                    {
                        BeginUpdate();
                        try
                        {
                            var state = Selection[0].CheckState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
                            foreach (TreeListNode node in Nodes)
                            {
                                if (!node.Visible)
                                {
                                    continue;
                                }

                                node.CheckState = state;
                                node.UpdateChildrenCheckState();
                            }
                        }
                        finally
                        {
                            EndUpdate();
                        }
                    }

                    break;
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Event on key down
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            BeginUpdate();
            try
            {
                switch (e.KeyData)
                {
                    case Keys.Control | Keys.D:
                        {
                            foreach (TreeListNode node in Nodes)
                            {
                                if (!node.Visible)
                                {
                                    continue;
                                }

                                node.Checked = false;
                                node.UpdateChildrenCheckState();
                            }
                        }

                        break;
                    case Keys.Control | Keys.A:
                        {
                            foreach (TreeListNode node in Nodes)
                            {
                                if (!node.Visible)
                                {
                                    continue;
                                }

                                node.Checked = true;
                                node.UpdateChildrenCheckState();
                            }
                        }

                        break;
                    case Keys.Control | Keys.E:
                        ExpandAll();
                        break;
                    case Keys.Control | Keys.G:
                        CollapseAll();
                        break;
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Create new node
        /// </summary>
        /// <param name="value"> Node value </param>
        /// <param name="columnIndex"> Column index </param>
        /// <returns> New node</returns>
        public TreeListNode CreateNode(object value, TreeListNode parentNode = null, int columnIndex = 0)
        {
            var node = AppendNode(null, parentNode);
            node.SetValue(columnIndex, value);
            return node;
        }

        /// <summary>
        /// Clear nodes filter and make all nodes visible
        /// </summary>
        public void ClearNodesFilter()
        {
            BeginSort();
            try
            {
                foreach (TreeListNode node in Nodes)
                {
                    node.ChangeVisibility(isVisible: true);
                }
            }
            finally
            {
                EndSort();
            }
        }

        /// <summary>
        /// Filter node by condition
        /// </summary>
        /// <param name="condition"> Condition </param>
        /// <param name="columnIndex"> Column index according to which filtering </param>
        public void Filter(string condition, int columnIndex = 0)
        {
            if (Columns.Count == 0)
            {
                return;
            }

            BeginUpdate();
            BeginExpandCollapse();
            try
            {
                if (string.IsNullOrWhiteSpace(condition))
                {
                    ClearNodesFilter();
                }
                else
                {
                    foreach (TreeListNode node in Nodes)
                    {
                        node.Filter(condition);
                    }
                }
            }
            finally
            {
                EndExpandCollapse();
                EndUpdate();
            }
        }

        /// <summary>
        /// Event before check node
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void TreeListEx_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            if (!Selection.Contains(e.Node))
            {
                Selection.Clear();
            }

            e.State = e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
            if (OptionsSelection.MultiSelect)
            {
                Selection.ForEach(node => node.CheckState = e.State);
            }
        }

        /// <summary>
        /// Event after check node
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void TreeListEx_AfterCheckNode(object sender, NodeEventArgs e)
        {
            if (IsLockUpdate)
            {
                return;
            }

            BeginUpdate();
            try
            {
                if (e.Node.HasChildren)
                {
                    e.Node.UpdateChildrenCheckState();
                }

                e.Node.ParentNode?.UpdateParentsCheckState();
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}