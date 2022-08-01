using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Container;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpressWinFormsExtension.DataControls.ColorPickEdit;
using DevExpressWinFormsExtension.DataControls.Extensions;
using DevExpressWinFormsExtension.DataControls.GridView.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode;

namespace DevExpressWinFormsExtension.DataControls.GridView
{
    /// <summary>
    /// Extension for a BandedGridView component
    /// </summary>
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(BandedGridView))]
    [Description("Extension for a BandedGridView component")]
    public partial class BandedGridViewDev : BandedGridView
    {
        /// <summary>
        /// Column-fixed info
        /// </summary>
        private readonly Dictionary<BandedGridColumn, FixedBandMenuInfo> fixedColumnInfo = new Dictionary<BandedGridColumn, FixedBandMenuInfo>();

        /// <summary>
        /// Binding of columns to format rules
        /// </summary>
        /// <remarks> Used to save format rules after DataSource recreation </remarks>
        private readonly Dictionary<GridFormatRule, int> conditionalRules = new Dictionary<GridFormatRule, int>();

        /// <summary>
        /// Band-absolute index
        /// </summary>
        private readonly Dictionary<GridBand, int> fixedBandInfo = new Dictionary<GridBand, int>();

        /// <summary>
        /// Conditional rules updating depth
        /// </summary>
        private byte isConditionalRulesUpdating = 0;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public BandedGridViewDev()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="grid"> GridControl </param>
        public BandedGridViewDev(GridControl grid)
            : base(grid)
        {
        }

        /// <summary>
        /// Event before show popup menu
        /// </summary>
        public event PopupMenuShowingEventHandler OnBeforePopupMenuShowing;

        /// <summary>
        /// Event on accuracy changed
        /// </summary>
        public event ChangingEventHandler ColumnFormatChanged;

        /// <summary>
        /// Allow user set up format settings
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Allow user set up format settings")]
        public bool AllowColumnFormatSettings { get; set; }

        /// <summary>
        /// Custom GridView name for a registrator
        /// </summary>
        protected override string ViewName
        {
            get
            {
                return "BandedGridViewDev";
            }
        }

        /// <summary>
        /// Flag if conditional rules is updating
        /// </summary>
        private bool IsConditionalRulesUpdating
        {
            get
            {
                return isConditionalRulesUpdating > 0;
            }
        }

        /// <summary>
        /// Initialize default GridView settings
        /// </summary>
        public void InitializeDefaultSettings()
        {
            barManager.Form = GridControl.Parent;

            OptionsView.ShowIndicator = true;
            OptionsView.BestFitMode = GridBestFitMode.Fast;
            OptionsView.AllowHtmlDrawGroups = true;
            OptionsView.AllowHtmlDrawHeaders = true;
            OptionsView.EnableAppearanceEvenRow = true;
            OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.SmartTag;
            OptionsView.GroupFooterShowMode = GroupFooterShowMode.Hidden;

            OptionsMenu.EnableColumnMenu = true;
            OptionsMenu.ShowConditionalFormattingItem = true;
            OptionsMenu.ShowGroupSortSummaryItems = false;
            OptionsMenu.ShowSplitItem = false;
            OptionsMenu.DialogFormBorderEffect = FormBorderEffect.Shadow;

            OptionsClipboard.AllowCopy = DefaultBoolean.True;
            OptionsClipboard.AllowCsvFormat = DefaultBoolean.True;
            OptionsClipboard.AllowTxtFormat = DefaultBoolean.True;
            OptionsClipboard.AllowExcelFormat = DefaultBoolean.True;
            OptionsClipboard.ClipboardMode = ClipboardMode.Formatted;
            OptionsClipboard.CopyColumnHeaders = DefaultBoolean.True;

            OptionsCustomization.AllowColumnMoving = false;
            OptionsCustomization.AllowQuickHideColumns = false;
            OptionsCustomization.AllowGroup = false;

            OptionsDetail.ShowEmbeddedDetailIndent = DefaultBoolean.True;

            OptionsFilter.AllowColumnMRUFilterList = false;
            OptionsFilter.ColumnFilterPopupMode = ColumnFilterPopupMode.Excel;

            OptionsBehavior.AllowFixedGroups = DefaultBoolean.True;
            OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
            OptionsBehavior.AllowPixelScrolling = DefaultBoolean.False;
            OptionsBehavior.AutoExpandAllGroups = true;

            OptionsLayout.StoreFormatRules = true;

            foreach (GridBand band in Bands)
            {
                fixedBandInfo.Add(band, band.VisibleIndex);
            }

            RegisterEvents();
        }

        /// <summary>
        /// Begin conditional rules update
        /// </summary>
        protected void BeginConditionalRulesUpdate()
        {
            ++isConditionalRulesUpdating;
        }

        /// <summary>
        /// End conditional rules update
        /// </summary>
        protected void EndConditionalRulesUpdate()
        {
            if (isConditionalRulesUpdating > 0)
            {
                isConditionalRulesUpdating--;
            }
        }

        /// <summary>
        /// Event on end update
        /// </summary>
        protected override void OnEndUpdate()
        {
            if (!IsConditionalRulesUpdating)
            {
                BeginConditionalRulesUpdate();
                try
                {
                    FormatRules.Clear();

                    foreach (var rule in conditionalRules)
                    {
                        if (rule.Value < Columns.Count)
                        {
                            rule.Key.Column = Columns[rule.Value];
                        }
                    }

                    FormatRules.AddRange(conditionalRules.Keys);
                }
                finally
                {
                    EndConditionalRulesUpdate();
                }
            }

            base.OnEndUpdate();
        }

        /// <summary>
        /// Event on an editor activation
        /// </summary>
        /// <param name="cell"> Cell info </param>
        protected override void ActivateEditor(GridCellInfo cell)
        {
            if (cell.MergedCell == null)
            {
                base.ActivateEditor(cell);
            }
            else
            {
                ActivateMergedCellEditor(cell);
            }
        }

        /// <summary>
        /// Event on post editor
        /// </summary>
        /// <param name="causeValidation"> Flag on validation </param>
        /// <returns> True, if changes applied </returns>
        /// <remarks> Process merged cells </remarks>
        protected override bool PostEditor(bool causeValidation)
        {
            if (IsEditing)
            {
                if (fEditingCell.MergedCell != null)
                {
                    var currentValue = ExtractEditingValue(fEditingCell.ColumnInfo.Column, EditingValue);
                    for (int i = 0; i < fEditingCell.MergedCell.MergedCells.Count; i++)
                    {
                        SetRowCellValue(fEditingCell.RowHandle + i, fEditingCell.Column, currentValue);
                        if (fEditingCell?.MergedCell == null)
                        {
                            break;
                        }
                    }
                }
            }

            return base.PostEditor(causeValidation);
        }

        /// <summary>
        /// Create check button for the popup menu
        /// </summary>
        /// <param name="caption"> Button caption </param>
        /// <param name="band"> Parent band </param>
        /// <param name="style"> Fixed type </param>
        /// <param name="image"> Button image </param>
        /// <param name="checkedChanged"> Checked changed event </param>
        /// <returns> Check button </returns>
        private DXMenuCheckItem CreateCheckItem(string caption, GridBand band, FixedStyle style, Image image, EventHandler checkedChanged)
        {
            var item = new DXMenuCheckItem(caption, band.Fixed == style, image, checkedChanged);
            item.Tag = new FixedBandMenuInfo(band, style);
            return item;
        }

        /// <summary>
        /// Create check button for the popup menu
        /// </summary>
        /// <param name="caption"> Button caption </param>
        /// <param name="column"> Parent column </param>
        /// <param name="style"> Fixed type </param>
        /// <param name="image"> Button image </param>
        /// <param name="checkedChanged"> Checked changed event </param>
        /// <returns> Check button </returns>
        private DXMenuCheckItem CreateCheckItem(string caption, BandedGridColumn column, FixedStyle style, Image image, EventHandler checkedChanged)
        {
            var item = new DXMenuCheckItem(caption, column.Fixed == style, image, checkedChanged);
            item.Tag = new FixedColumnMenuInfo(column, style);
            return item;
        }

        /// <summary>
        /// Event on format rules changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void FormatRulesCollectionChanged(object sender, FormatConditionCollectionChangedEventArgs e)
        {
            if (IsConditionalRulesUpdating)
            {
                return;
            }

            conditionalRules.Clear();
            foreach (var rule in FormatRules)
            {
                conditionalRules.Add(rule, rule.Column.AbsoluteIndex);
            }
        }

        /// <summary>
        /// Event on clear a column header color
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnClearColorClick(object sender, EventArgs e)
        {
            var item = sender as DXMenuItem;
            var column = item.Tag as BandedGridColumn;
            column.AppearanceCell.BackColor = Color.Empty;
            column.AppearanceHeader.BackColor = Color.Empty;
        }

        /// <summary>
        /// Event on set up a column header color
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnColorClick(object sender, EventArgs e)
        {
            var item = sender as DXMenuItem;
            var column = item.Tag as BandedGridColumn;

            var colorControl = new ColorPickEditDev { Color = column.AppearanceHeader.BackColor, Width = 150 };
            if (XtraDialog.Show(colorControl, "Column header color", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            column.AppearanceCell.BackColor = colorControl.Color;
            column.AppearanceHeader.BackColor = colorControl.Color;
        }

        /// <summary>
        /// Event on fix band state changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnFixedBandClicked(object sender, EventArgs e)
        {
            var item = sender as DXMenuItem;
            var info = item.Tag as FixedBandMenuInfo;
            if (info == null)
            {
                return;
            }

            if (info.Style == FixedStyle.None)
            {
                info.Band.Fixed = info.Style;
                var oldPosition = fixedBandInfo.ContainsKey(info.Band) ? fixedBandInfo[info.Band] + 1 : 0;
                Bands.MoveTo(oldPosition, info.Band);
            }
            else
            {
                if (!fixedBandInfo.ContainsKey(info.Band))
                {
                    fixedBandInfo.Add(info.Band, info.Band.Index);
                }

                info.Band.Fixed = info.Style;
            }
        }

        /// <summary>
        /// Event on fix column state changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnFixedColumnClicked(object sender, EventArgs e)
        {
            var item = sender as DXMenuItem;
            var info = item.Tag as FixedColumnMenuInfo;
            if (info == null)
            {
                return;
            }

            info.Column.Fixed = info.Style;
            if (info.Style == FixedStyle.None)
            {
                if (info.Column.OwnerBand.Columns.Count == 1)
                {
                    Bands.Remove(info.Column.OwnerBand);
                }

                var infoBands = fixedColumnInfo[info.Column];
                infoBands.Band.Columns.Insert(infoBands.Index, info.Column);
                fixedColumnInfo.Remove(info.Column);
            }
            else
            {
                if (!fixedColumnInfo.ContainsKey(info.Column))
                {
                    var bandInfo = new FixedBandMenuInfo(info.Column.OwnerBand, info.Column.OwnerBand.Columns.IndexOf(info.Column));
                    fixedColumnInfo.Add(info.Column, bandInfo);
                }

                var newBandName = info.Column.OwnerBand.Name + "_fixed";
                var newBand = Bands.FirstOrDefault(x => x.Name.Equals(newBandName));
                if (newBand == null)
                {
                    newBand = new GridBand { Name = newBandName, Caption = info.Column.OwnerBand.Caption };
                    newBand.AppearanceHeader.Assign(info.Column.OwnerBand.AppearanceHeader);
                    newBand.Fixed = FixedStyle.Left;
                    newBand.OptionsBand.ShowCaption = info.Column.OwnerBand.OptionsBand.ShowCaption;
                    Bands.Add(newBand);
                }

                newBand.Columns.Add(info.Column);
            }
        }

        /// <summary>
        /// Event on set up accuracy
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnSignificantDigitCheckedChanged(object sender, EventArgs e)
        {
            var item = sender as DXMenuCheckItem;
            var info = item.Tag as FixedColumnMenuInfo;
            if (info == null)
            {
                return;
            }

            var significantDigit = item.Checked;
            var formatStirng = info.Column.DisplayFormat.FormatString;
            formatStirng = significantDigit ? formatStirng.Replace(GridHelper.SubstituteZeroChar, GridHelper.SubstituteNumberChar) : formatStirng.Replace(GridHelper.SubstituteNumberChar, GridHelper.SubstituteZeroChar);
            info.Column.DisplayFormat.FormatString = formatStirng;

            ColumnFormatChanged?.Invoke(info.Column, null);
        }

        /// <summary>
        /// Event on set up format as stored
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnOriginalDigitCheckedChanged(object sender, EventArgs e)
        {
            var item = sender as DXMenuCheckItem;
            var info = item.Tag as FixedColumnMenuInfo;
            if (info == null)
            {
                return;
            }

            var originalDigit = item.Checked;

            info.Column.DisplayFormat.FormatString = originalDigit ? GridHelper.GeneralFormatSpecifier.ToString() : GridHelper.ZeroFormatSpecifier;

            ColumnFormatChanged?.Invoke(info.Column, null);
        }

        /// <summary>
        /// Event on custom row cell style
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        /// <remarks> DevExpress can't process event/odd style for merged cells </remarks>
        private void BandedGridViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (OptionsView.AllowCellMerge && OptionsView.EnableAppearanceEvenRow)
            {
                var cellInfo = ViewInfo.GetGridCellInfo(e.RowHandle, e.Column);
                if ((e.Column.OptionsColumn.AllowMerge == DefaultBoolean.True) || (cellInfo == null) || cellInfo.IsMerged || (e.Appearance.BackColor != Color.White))
                {
                    return;
                }

                var res = e.RowHandle % 2;
                var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (res == 0)
                {
                    e.Appearance.Combine(view.PaintAppearance.EvenRow);
                }
                else
                {
                    e.Appearance.Combine(view.PaintAppearance.OddRow);
                }
            }
        }

        /// <summary>
        /// Event on draw column header
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewCustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if ((e.Column != null) && (e.Column.AppearanceHeader.BackColor != Color.Empty))
            {
                e.Info.AllowColoring = true;
            }
        }

        /// <summary>
        /// Event on key down
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (GroupCount > 0)
                {
                    if (e.KeyCode == Keys.E)
                    {
                        ExpandAllGroups();
                    }
                    else if (e.KeyCode == Keys.G)
                    {
                        CollapseAllGroups();
                    }
                }

                if (e.KeyCode == Keys.C)
                {
                    OptionsClipboard.CopyColumnHeaders = DefaultBoolean.True;
                }
                else if (e.KeyCode == Keys.Insert)
                {
                    OptionsClipboard.CopyColumnHeaders = DefaultBoolean.False;
                }
            }
        }

        /// <summary>
        /// Event on mouse down
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hitInfo = CalcHitInfo(e.Location);
                var view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;
                if (view.CanMultipleCheck(hitInfo))
                {
                    view.PerformMultipleCheck(hitInfo);
                    ((DXMouseEventArgs)e).Handled = true;
                }
            }
        }

        /// <summary>
        /// Event on showing popup
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == GridMenuType.Column)
            {
                var menu = e.Menu as GridViewColumnMenu;
                if (menu == null)
                {
                    return;
                }

                var hi = e.HitInfo as BandedGridHitInfo;
                if (menu.Column != null)
                {
                    var formatGroup = new DXSubMenuItem("Appearance");
                    formatGroup.ImageOptions.Image = Properties.Resources.ImageBrush_16;
                    e.Menu.Items.Add(formatGroup);
                    var dxx = new DXMenuItem("Column header color", OnColorClick, Properties.Resources.Color_16) { Tag = menu.Column };
                    formatGroup.Items.Add(dxx);
                    if (menu.Column.AppearanceHeader.BackColor != Color.Empty)
                    {
                        dxx = new DXMenuItem("Clear color", OnClearColorClick, Properties.Resources.Clear_16) { Tag = menu.Column, BeginGroup = true };
                        formatGroup.Items.Add(dxx);
                    }

                    var bandedColumn = menu.Column as BandedGridColumn;
                    if (OptionsBehavior.AllowFixedGroups == DefaultBoolean.True)
                    {
                        if (bandedColumn.OwnerBand.Fixed == FixedStyle.None)
                        {
                            menu.Items.Add(CreateCheckItem("Lock column", bandedColumn, FixedStyle.Left, Properties.Resources.Lock_16, OnFixedColumnClicked));
                        }
                        else if (fixedColumnInfo.ContainsKey(bandedColumn))
                        {
                            menu.Items.Add(CreateCheckItem("Unlock column", bandedColumn, FixedStyle.None, Properties.Resources.Unlock_16, OnFixedColumnClicked));
                        }
                    }

                    if (AllowColumnFormatSettings && menu.Column.DisplayFormat.FormatType == FormatType.Numeric)
                    {
                        var valueFormatGroup = new DXSubMenuItem("Number format");
                        var originalMenuItem = CreateCheckItem("As per data", bandedColumn, FixedStyle.Left, null, OnOriginalDigitCheckedChanged);
                        originalMenuItem.Checked = menu.Column.DisplayFormat.FormatString.ToLower().Contains(GridHelper.GeneralFormatSpecifier);
                        var significantMenuItem = CreateCheckItem("Only significant", bandedColumn, FixedStyle.Left, null, OnSignificantDigitCheckedChanged);
                        significantMenuItem.Checked = menu.Column.DisplayFormat.FormatString.Contains(GridHelper.SubstituteNumberChar);
                        significantMenuItem.Enabled = !originalMenuItem.Checked;

                        var accuracyMenuItem = new DXEditMenuItem("Accuracy", accuracySpin, Properties.Resources.Decimal_16);
                        accuracyMenuItem.Edit.Tag = menu.Column;
                        int accuracy;
                        if (menu.Column.DisplayFormat.FormatString.Contains(GridHelper.DelimiterChar))
                        {
                            accuracy = menu.Column.DisplayFormat.FormatString.Substring(menu.Column.DisplayFormat.FormatString.IndexOf(GridHelper.DelimiterChar) + 1).Count();
                        }
                        else
                        {
                            accuracy = int.Parse(menu.Column.DisplayFormat.FormatString.Remove(0, 1));
                        }

                        accuracyMenuItem.EditValue = accuracy;
                        accuracyMenuItem.Enabled = !originalMenuItem.Checked;

                        valueFormatGroup.Items.Add(originalMenuItem);
                        valueFormatGroup.Items.Add(significantMenuItem);
                        valueFormatGroup.Items.Add(accuracyMenuItem);
                        e.Menu.Items.Add(valueFormatGroup);
                    }
                }
                else if ((hi.Band != null) && (hi.Band.BandLevel == 0))
                {
                    if ((OptionsBehavior.AllowFixedGroups == DefaultBoolean.True) && !hi.Band.Name.Contains("_fixed"))
                    {
                        if (hi.Band.Fixed == FixedStyle.None)
                        {
                            menu.Items.Add(CreateCheckItem("Lock bands", hi.Band, FixedStyle.Left, Properties.Resources.Lock_16, OnFixedBandClicked));
                        }
                        else
                        {
                            menu.Items.Add(CreateCheckItem("Unlock bands", hi.Band, FixedStyle.None, Properties.Resources.Unlock_16, OnFixedBandClicked));
                        }
                    }
                }

                OnBeforePopupMenuShowing?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Event on custom cell display text
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void BandedGridView_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.DisplayFormat.FormatType == FormatType.Numeric)
            {
                var isParsed = double.TryParse(e.Value?.ToString() ?? string.Empty, out double value);
                if (!isParsed || double.IsNaN(value) || double.IsInfinity(value))
                {
                    e.DisplayText = string.Empty;
                }
            }
            else if (e.Column.DisplayFormat.FormatType == FormatType.DateTime)
            {
                var isParsed = DateTime.TryParse(e.Value?.ToString() ?? string.Empty, out DateTime value);
                if (isParsed || value == DateTime.MinValue || value == DateTime.MaxValue)
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        /// <summary>
        /// Event on accuracy changing (by user)
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void AccuracySpin_EditValueChanging(object sender, ChangingEventArgs e)
        {
            var column = accuracySpin.Tag as GridColumn;
            var accuracy = int.Parse(e.NewValue?.ToString() ?? string.Empty);
            var formatString = column.DisplayFormat.FormatString;
            var containsNumberChar = formatString.Contains(GridHelper.SubstituteNumberChar);
            var formatChar = containsNumberChar ? GridHelper.SubstituteNumberChar : GridHelper.SubstituteZeroChar;
            var formatSpecifier = containsNumberChar ? GridHelper.NumberFormatSpecifier : GridHelper.ZeroFormatSpecifier;
            column.DisplayFormat.FormatString = formatSpecifier + string.Join(string.Empty, Enumerable.Repeat(formatChar, accuracy));

            ColumnFormatChanged?.Invoke(column, e);
        }

        /// <summary>
        /// Activate an editor for the merged cells
        /// </summary>
        /// <param name="cell"> Cell info </param>
        private void ActivateMergedCellEditor(GridCellInfo cell)
        {
            if (cell == null)
            {
                return;
            }

            cell = cell.MergedCell.MergedCells[0];
            fEditingCell = cell;
            Rectangle bounds = ViewInfo.GetMergedEditorBounds(cell);
            if (bounds.IsEmpty)
            {
                return;
            }

            var cellEdit = RequestCellEditor(cell);
            ViewInfo.UpdateCellAppearance(cell);
            ViewInfo.RequestCellEditViewInfo(cell);
            var appearance = new AppearanceObject();
            AppearanceHelper.Combine(appearance, new AppearanceObject[] { GetEditorAppearance(), ViewInfo.PaintAppearance.Row, cell.Appearance });
            if (cellEdit != cell.Editor && cellEdit.DefaultAlignment != HorzAlignment.Default)
            {
                appearance.TextOptions.HAlignment = cellEdit.DefaultAlignment;
            }

            UpdateEditor(cellEdit, new UpdateEditorInfoArgs(GetColumnReadOnly(cell.ColumnInfo.Column), bounds, appearance, cell.CellValue, ElementsLookAndFeel, cell.ViewInfo.ErrorIconText, cell.ViewInfo.ErrorIcon));
            ViewInfo.UpdateCellAppearance(cell);
            if (cell != null)
            {
                InvalidateRow(cell.RowHandle);
            }
        }

        /// <summary>
        /// Subscribe on events
        /// </summary>
        private void RegisterEvents()
        {
            PopupMenuShowing += GridViewPopupMenuShowing;
            CustomDrawColumnHeader += GridViewCustomDrawColumnHeader;
            KeyDown += GridViewKeyDown;
            MouseDown += GridViewMouseDown;
            RowCellStyle += BandedGridViewRowCellStyle;
            FormatRules.CollectionChanged += FormatRulesCollectionChanged;
            CustomColumnDisplayText += BandedGridView_CustomColumnDisplayText;
        }

        /// <summary>
        /// Info about band fixation
        /// </summary>
        private class FixedBandMenuInfo
        {
            /// <summary>
            /// Constructor with parameters
            /// </summary>
            /// <param name="band"> Band </param>
            /// <param name="index"> Index group </param>
            public FixedBandMenuInfo(GridBand band, int index)
            {
                Band = band;
                Style = FixedStyle.None;
                Index = index;
            }

            /// <summary>
            /// Constructor with parameters
            /// </summary>
            /// <param name="band"> Band </param>
            /// <param name="style"> Fixed type </param>
            public FixedBandMenuInfo(GridBand band, FixedStyle style)
            {
                Band = band;
                Style = style;
                Index = -1;
            }

            /// <summary>
            /// Band
            /// </summary>
            public GridBand Band { get; set; }

            /// <summary>
            /// Index group
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// Fixed type
            /// </summary>
            public FixedStyle Style { get; set; }
        }

        /// <summary>
        /// Info about column fixation
        /// </summary>
        private class FixedColumnMenuInfo
        {
            /// <summary>
            /// Constructor with parameters
            /// </summary>
            /// <param name="column"> Column </param>
            /// <param name="style"> Fixed type </param>
            public FixedColumnMenuInfo(BandedGridColumn column, FixedStyle style)
            {
                Column = column;
                Style = style;
            }

            /// <summary>
            /// Column
            /// </summary>
            public BandedGridColumn Column { get; set; }

            /// <summary>
            /// Fixed type
            /// </summary>
            public FixedStyle Style { get; set; }
        }
    }
}