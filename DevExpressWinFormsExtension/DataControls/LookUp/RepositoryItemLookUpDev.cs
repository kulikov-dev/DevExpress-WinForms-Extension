using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpressWinFormsExtension.Utils;

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
        /// Custom lookup control name
        /// </summary>
        internal const string LookUpEditHintsName = "LookUpEditHints";

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
        /// Custom editor registration
        /// </summary>
        public static void RegisterLookUpEditHints()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
                LookUpEditHintsName, typeof(LookUpDev), typeof(RepositoryItemLookUpDev),
                typeof(LookUpEditViewInfo), new ButtonEditPainter(), true));
        }

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
        /// Event on custom draw cell
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        protected override void RaiseCustomDrawCell(LookUpCustomDrawCellArgs e)
        {
            base.RaiseCustomDrawCell(e);
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