using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.Utils.MdiManager;
using System;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Samples.Forms
{
    /// <summary>
    /// Parent MDI form
    /// </summary>
    public partial class ParentMDIForm : XtraForm
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ParentMDIForm()
        {
            InitializeComponent();

            IsMdiContainer = true;
        }

        /// <summary>
        /// Event on user MDI children size selected
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void matrixGridControl_OnSizeSelected(object sender, EventArgs e)
        {
            popupContainerControlMatrix.OwnerEdit.ClosePopup();



            MdiManagerHelper.InitMDIChildren(this, matrixGridControl.TotalSize, typeof(EmptyMDIForm));
            UpdateMDILayout();
        }

        /// <summary>
        /// Event on form's resize
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ParentMDIForm_ClientSizeChanged(object sender, EventArgs e)
        {
            UpdateMDILayout();
        }

        /// <summary>
        /// Event on custom display text for popupMatrix
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void repositoryItemPopupMatrix_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            e.DisplayText = matrixGridControl.TotalSize.ToString();
        }

        /// <summary>
        /// Update MDI children layout
        /// </summary>
        private void UpdateMDILayout()
        {
            LayoutMdi(matrixGridControl.SelectedSize.Width > matrixGridControl.SelectedSize.Height ? MdiLayout.TileVertical : MdiLayout.TileHorizontal);
        }
    }
}
