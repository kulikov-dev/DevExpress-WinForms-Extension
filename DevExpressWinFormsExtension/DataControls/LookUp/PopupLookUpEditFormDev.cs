using System.ComponentModel;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraEditors.Popup;

namespace DevExpressWinFormsExtension.DataControls.LookUp
{
    /// <summary>
    /// Popup lookupform
    /// </summary>
    [ToolboxItem(false)]
    internal class PopupLookUpEditFormDev : PopupLookUpEditForm
    {
        /// <summary>
        /// Previous point
        /// </summary>
        private Point prevPoint = Point.Empty;

        /// <summary>
        /// ToolTipController
        /// </summary>
        private ToolTipController toolTipController;

        /// <summary>
        /// Previous selected row index
        /// </summary>
        private int prevRowIndex = -1;

        /// <summary>
        /// Contructor with parameters
        /// </summary>
        /// <param name="ownerEdit"> LookUpEdit owner </param>
        public PopupLookUpEditFormDev(LookUpDev ownerEdit)
            : base(ownerEdit)
        {
            if (OwnerEdit.ToolTipController == null)
            {
                toolTipController = ToolTipController.DefaultController;
            }

            toolTipController.BeforeShow += ToolTipControllerBeforeShow;
        }

        /// <summary>
        /// Event befor tooltip showing
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ToolTipControllerBeforeShow(object sender, ToolTipControllerShowEventArgs e)
        {
            if (OwnerEdit is LookUpDev lookUpEdit)
            {
                lookUpEdit.Properties.OnBeforeShowingTooltip(e);
            }
        }

        /// <summary>
        /// Event on show popup form
        /// </summary>
        public override void ShowPopupForm()
        {
            SetToolTipController();
            base.ShowPopupForm();
        }

        /// <summary>
        /// Check mouse cursor for showing tooltip
        /// </summary>
        /// <param name="hitTest"> HitTest info </param>
        protected override void CheckMouseCursor(LookUpPopupHitTest hitTest)
        {
            if (prevPoint.X != hitTest.Point.X || prevPoint.Y != hitTest.Point.Y)
            {
                prevPoint = hitTest.Point;

                if (!(OwnerEdit is LookUpDev lookUp) || string.IsNullOrEmpty(lookUp.DescriptionField))
                {
                    return;
                }

                if (hitTest.HitType == LookUpPopupHitType.Row)
                {
                    if (hitTest.Index != prevRowIndex)
                    {
                        toolTipController.HideHint();
                        prevRowIndex = hitTest.Index;
                    }

                    var value = lookUp.Properties.GetDataSourceValue(lookUp.Properties.DescriptionField, hitTest.Index);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        toolTipController.ShowHint(value.ToString());
                    }
                }
            }

            base.CheckMouseCursor(hitTest);
        }

        /// <summary>
        /// Attach tooltipcontroller to the LookUpEdit
        /// </summary>
        private void SetToolTipController()
        {
            if (OwnerEdit.ToolTipController != null && OwnerEdit.ToolTipController != toolTipController)
            {
                toolTipController = OwnerEdit.ToolTipController;
                toolTipController.BeforeShow += ToolTipControllerBeforeShow;
            }
        }
    }
}