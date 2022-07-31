using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    /// <summary>
    /// Extension for the TreeListNode class
    /// </summary>
    public static class TreeListNodeExtension
    {
        /// <summary>
        /// Update CheckState for all children nodes according to CheckState of the parentNode
        /// </summary>
        /// <param name="parentNode"> Parent node </param>
        /// <remarks> If the parent is checked - all children should be checked too. And vice versa. </remarks>
        public static void UpdateChildrenCheckState(this TreeListNode parentNode)
        {
            foreach (TreeListNode child in parentNode.Nodes)
            {
                if (!child.Visible)
                {
                    continue;
                }

                child.CheckState = parentNode.CheckState;
                UpdateChildrenCheckState(child);
            }
        }

        /// <summary>
        /// Update CheckState of the parent node according to all children nodes
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="check"></param>
        /// <remarks> So, if all children checked - than parent state checked too. If one checked/one no - state = Intermediate </remarks>
        public static void UpdateParentsCheckState(this TreeListNode parentNode)
        {
            if (parentNode == null)
            {
                return;
            }

            if (parentNode.Nodes.Count > 0)
            {
                var hasDifferentStates = false;
                var defaultCheck = parentNode.Nodes[0].CheckState;
                for (var i = 0; i < parentNode.Nodes.Count; i++)
                {
                    var state = parentNode.Nodes[i].CheckState;
                    if (!defaultCheck.Equals(state))
                    {
                        hasDifferentStates = true;
                        break;
                    }
                }

                parentNode.CheckState = hasDifferentStates ? CheckState.Indeterminate : defaultCheck;
            }

            UpdateParentsCheckState(parentNode.ParentNode);
        }

        /// <summary>
        /// Update node and all children visibility
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="isVisible"> Visibility state </param>
        public static void ChangeVisibility(this TreeListNode node, bool isVisible)
        {
            node.Visible = isVisible;
            foreach (TreeListNode child in node.Nodes)
            {
                ChangeVisibility(child, isVisible);
            }
        }

        /// <summary>
        /// Filter node by condition
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="condition"> Condition </param>
        /// <param name="columnIndices"> Columns indices according to which filtering </param>
        public static void Filter(this TreeListNode node, string condition, IEnumerable<int> columnIndices)
        {
            if (node == null || !columnIndices.Any())
            {
                return;
            }

            condition = condition.Trim();
            if (string.IsNullOrWhiteSpace(condition))
            {
                node.ChangeVisibility(isVisible: true);
            }
            else
            {
                var isFiltered = false;
                foreach (var index in columnIndices)
                {
                    var value = node.GetValue(index);
                    var nodeValue = value == null ? string.Empty : value.ToString();

                    if (nodeValue.IndexOf(condition, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        isFiltered = true;
                        break;
                    }
                }

                node.SetFiltered(isFiltered);
            }

            if (node.Visible)
            {
                return;
            }

            foreach (TreeListNode child in node.Nodes)
            {
                Filter(child, condition, columnIndices);
            }
        }

        /// <summary>
        /// Filter node by condition
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="condition"> Condition </param>
        /// <param name="columnIndex"> Column index according to which filtering </param>
        public static void Filter(this TreeListNode node, string condition, int columnIndex = 0)
        {
            Filter(node, condition, new List<int>() { columnIndex });
        }

        /// <summary>
        /// Set node filtered state
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="isFiltered"> Node passes the filter </param>
        public static void SetFiltered(this TreeListNode node, bool isFiltered)
        {
            if (isFiltered)
            {
                node.Visible = true;
                node.Expanded = false;
                var parentNode = node.ParentNode;

                //// Apply changed to all parents
                while (parentNode != null)
                {
                    parentNode.Visible = true;
                    parentNode.Expanded = true;
                    parentNode = parentNode.ParentNode;
                }

                // Apply changes to all children
                node.ChangeVisibility(isVisible: true);
            }
            else
            {
                node.Expanded = false;
                node.Visible = false;
            }
        }
    }
}