using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.Utils;

namespace DevExpressWinFormsExtension.DataControls.LookUp
{
    /// <summary>
    /// LookUpEdit component
    /// </summary>
    /// <remarks> Allows to show hint for each element in the editor </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(LookUpEdit))]
    [Description("LookUpEdit component with hints")]
    public class LookUpDev : LookUpEdit
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        static LookUpDev()
        {
            RepositoryItemLookUpDev.RegisterLookUpEditHints();
        }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LookUpDev()
            : base()
        {
            CustomDrawCell += LookUpDev_CustomDrawCell;
        }

        /// <summary>
        /// Event on showing ToolTip
        /// </summary>
        public event EventHandler BeforeShowingTooltip
        {
            add
            {
                Properties.BeforeShowingTooltip += value;
            }
            remove
            {
                Properties.BeforeShowingTooltip -= value;
            }
        }

        /// <summary>
        /// Description field
        /// </summary>
        public string DescriptionField
        {
            set
            {
                Properties.DescriptionField = value;
                OnPropertiesChanged();
            }
            get
            {
                return Properties.DescriptionField;
            }
        }

        /// <summary>
        /// Gets the class name of the current editor.
        /// </summary>
        public override string EditorTypeName
        {
            get
            {
                return RepositoryItemLookUpDev.LookUpEditHintsName;
            }
        }

        /// <summary>
        /// Specifies settings specific to the current editor.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemLookUpDev Properties
        {
            get
            {
                return base.Properties as RepositoryItemLookUpDev;
            }
        }

        /// <summary>
        /// Event on creating a lookup popup form
        /// </summary>
        /// <returns> Popup form </returns>
        protected override DevExpress.XtraEditors.Popup.PopupBaseForm CreatePopupForm()
        {
            return new PopupLookUpEditFormDev(this);
        }

        /// <summary>
        /// Event on custom draw cell
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void LookUpDev_CustomDrawCell(object sender, DevExpress.XtraEditors.Popup.LookUpCustomDrawCellArgs e)
        {
            if (e.Row is ILookUpSplitableItem item && item.IsSplitter)
            {
                e.DefaultDraw();
                using (var pen = new Pen(SkinHelper.TranslateColor(Color.LightGray)))
                {
                    e.Graphics.DrawLine(pen, new Point(e.Bounds.X, e.Bounds.Bottom), new Point(e.Bounds.Right, e.Bounds.Bottom));
                }

                e.Handled = true;
            }
        }
    }
}