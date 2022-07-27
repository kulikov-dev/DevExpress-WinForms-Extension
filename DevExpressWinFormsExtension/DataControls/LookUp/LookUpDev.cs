using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors;

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
    }
}