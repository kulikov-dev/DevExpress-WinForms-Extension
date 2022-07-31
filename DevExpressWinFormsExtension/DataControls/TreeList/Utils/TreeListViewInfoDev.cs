using DevExpress.XtraTreeList.ViewInfo;
using System.Drawing;

namespace DevExpressWinFormsExtension.DataControls.TreeList.Utils
{
    /// <summary>
    /// Custom TreeListViewInfo
    /// </summary>
    /// <remarks> Remove empty spaces for nodes without StateImage </remarks>
    internal class TreeListViewInfoDev : TreeListViewInfo
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="treeList"> TreeList </param>
        /// <param name="hasNodeImageInTag"> Flag if TreeListNode has custom images in tags and draws them itself </param>
        public TreeListViewInfoDev(DevExpress.XtraTreeList.TreeList treeList, bool hasNodeImageInTag)
            : base(treeList)
        {
            HasNodeImageInTag = hasNodeImageInTag;
        }

        /// <summary>
        /// Flag if TreeListNode has custom images in tags and draws them itself
        /// </summary>
        private bool HasNodeImageInTag { get; set; }

        /// <summary>s
        /// Calc select image bounds
        /// </summary>
        /// <param name="rInfo"> Row info </param>
        /// <param name="indentBounds"> Indents </param>
        protected override void CalcSelectImageBounds(RowInfo rInfo, Rectangle indentBounds)
        {
            base.CalcSelectImageBounds(rInfo, indentBounds);
            if (rInfo.StateImageIndex == -1 && (!HasNodeImageInTag || rInfo.Node.Tag != null))
            {
                rInfo.SelectImageBounds = Rectangle.Empty;
            }
        }

        /// <summary>
        /// Calc state image bounds
        /// </summary>
        /// <param name="rInfo"> Row info </param>
        /// <param name="indentBounds"> Indents </param>
        protected override void CalcStateImageBounds(RowInfo rInfo, Rectangle indentBounds)
        {
            base.CalcStateImageBounds(rInfo, indentBounds);
            if (rInfo.StateImageIndex == - 1 && (!HasNodeImageInTag || rInfo.Node.Tag != null))
            {
                rInfo.StateImageBounds = Rectangle.Empty;
            }
        }
    }
}