using DevExpress.XtraEditors;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace DevExpressWinFormsExtension.DataControls.Forms
{
    /// <summary>
    /// XtraForm with the update mechanism
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(XtraForm))]
    [Designer(typeof(ParentControlDesigner))]
    [Description("XtraForm with the update mechanism")]
    public class XtraFormDev : XtraForm
    {
        /// <summary>
        /// Update depth
        /// </summary>
        private byte updateDepth;

        /// <summary>
        /// If form is updating
        /// </summary>
        protected bool IsLockUpdating
        {
            get
            {
                return updateDepth > 0;
            }
        }

        /// <summary>
        /// Begin update
        /// </summary>
        protected void BeginUpdate()
        {
            OnBeginUpdate();
        }

        /// <summary>
        /// End update
        /// </summary>
        protected void EndUpdate()
        {
            OnEndUpdate();
        }

        /// <summary>
        /// Event on begin update
        /// </summary>
        protected virtual void OnBeginUpdate()
        {
            updateDepth++;
        }

        /// <summary>
        /// Event on end update
        /// </summary>
        protected virtual void OnEndUpdate()
        {
            if (updateDepth > 0)
            {
                updateDepth--;
            }
        }
    }
}