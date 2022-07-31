using System;
using System.ComponentModel;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace DevExpressWinFormsExtension.DataControls.LookUp
{
    /// <summary>
    /// RepositoryItemLookUpEdit component
    /// </summary>
    /// <remarks> Allows to show hint for each element in the editor </remarks>
    [ToolboxItem(false)]
    [UserRepositoryItem("RepositoryItemLookUpDev")]
    public class RepositoryItemLookUpDev : RepositoryItemLookUpEdit
    {
        /// <summary>
        /// Description field
        /// </summary>
        private string descriptionField = string.Empty;

        /// <summary>
        /// Handler before showing ToolTip
        /// </summary>
        private static readonly object beforeShowingTooltipHandler = new object();

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        static RepositoryItemLookUpDev()
        {
            RegisterLookUpEditHints();
        }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RepositoryItemLookUpDev()
            : base()
        {
        }

        /// <summary>
        /// Custom lookup control name
        /// </summary>
        public const string LookUpEditHintsName = "LookUpEditHints";

        /// <summary>
        /// Custom lookup control name
        /// </summary>
        public override string EditorTypeName
        {
            get
            {
                return LookUpEditHintsName;
            }
        }

        /// <summary>
        /// Custom editor registration
        /// </summary>
        public static void RegisterLookUpEditHints()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
                LookUpEditHintsName, typeof(LookUpDev), typeof(RepositoryItemLookUpDev),
                typeof(LookUpEditViewInfo), new ButtonEditPainter(), true));
        }

        /// <summary>
        /// Description field
        /// </summary>
        public string DescriptionField
        {
            set
            {
                descriptionField = value;
                OnPropertiesChanged();
            }
            get
            {
                return descriptionField;
            }
        }

        /// <summary>
        /// Event before showing tooltip
        /// </summary>
        public event EventHandler BeforeShowingTooltip
        {
            add
            {
                Events.AddHandler(beforeShowingTooltipHandler, value);
            }
            remove
            {
                Events.RemoveHandler(beforeShowingTooltipHandler, value);
            }
        }

        /// <summary>
        /// Event before showing tooltip
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected internal virtual void OnBeforeShowingTooltip(EventArgs e)
        {
            var handler = (EventHandler)Events[beforeShowingTooltipHandler];
            handler?.Invoke(GetEventSender(), e);
        }

        /// <summary>
        /// Copies properties of the source repository item to the current object.
        /// </summary>
        /// <param name="item"> The source repository item object. </param>
        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                if (!(item is RepositoryItemLookUpDev source))
                {
                    return;
                }

                DescriptionField = source.DescriptionField;
                Events.AddHandler(beforeShowingTooltipHandler, source.Events[beforeShowingTooltipHandler]);
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}