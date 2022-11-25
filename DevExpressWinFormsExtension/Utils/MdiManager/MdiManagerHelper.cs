using System;
using DevExpress.XtraEditors;

namespace DevExpressWinFormsExtension.Utils.MdiManager
{
    /// <summary>
    /// Helper to process MDI forms
    /// </summary>
    public static class MdiManagerHelper
    {
        /// <summary>
        /// Inititalize MDI children (create or remove depends on chlidrenCount)
        /// </summary>
        /// <param name="mdiParent"> MDI parent form </param>
        /// <param name="chlidrenCount"> Total MDI children count on the MDI parent form </param>
        /// <param name="formType"> Default type for new MDI children </param>
        public static void InitMDIChildren(XtraForm mdiParent, int chlidrenCount, Type formType)
        {
            if (mdiParent.MdiChildren.Length < chlidrenCount)
            {
                for (var i = mdiParent.MdiChildren.Length; i < chlidrenCount; i++)
                {
                    var newMDIChild = (XtraForm)Activator.CreateInstance(formType);
                    newMDIChild.MdiParent = mdiParent;

                    newMDIChild.Show();
                }
            }
            else if (mdiParent.MdiChildren.Length > chlidrenCount)
            {
                for (var i = mdiParent.MdiChildren.Length - 1; i >= chlidrenCount; i--)
                {
                    mdiParent.MdiChildren[i].Close();
                }
            }
        }
    }
}
