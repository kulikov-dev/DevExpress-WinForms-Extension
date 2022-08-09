using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Container;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
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
    /// Extension for a GridView component
    /// </summary>
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(DevExpress.XtraGrid.Views.Grid.GridView))]
    [Description("Extension for a GridView component")]
    public partial class GridViewDev : DevExpress.XtraGrid.Views.Grid.GridView
    {
        /// <summary>
        /// Binding of columns to format rules
        /// </summary>
        /// <remarks> Used to save format rules after DataSource recreation </remarks>
        private readonly Dictionary<string, GridFormatRule> conditionalRules = new Dictionary<string, GridFormatRule>();

        /// <summary>
        /// Histogram data cache
        /// </summary>
        private readonly Dictionary<int, HistogramData> histogrammsCache = new Dictionary<int, HistogramData>();

        /// <summary>
        /// Conditional rules updating depth
        /// </summary>
        private byte isConditionalRulesUpdating = 0;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public GridViewDev()
        {
            InitializeComponent();
            RegisterEvents();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="grid"> GridControl </param>
        public GridViewDev(GridControl grid)
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
        /// Draw histogram
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Draw histogram")]
        public bool IsHistogramDraw = false;

        /// <summary>
        /// Cell histogram font
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Cell histogram font")]
        public Font HistogramFont = new Font("Times New Roman", 7);

        /// <summary>
        /// Cell histogram drawing color
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Cell histogram drawing color")]
        public Color HistogramColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        /// Cell histogram empty color
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Cell histogram empty color")]
        public Color HistogramEmptyColor { get; set; } = Color.DarkBlue;

        /// <summary>
        /// Custom GridView name for a registrator
        /// </summary>
        protected override string ViewName
        {
            get
            {
                return typeof(GridViewDev).Name;
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

            OptionsView.ShowIndicator = false;
            OptionsView.BestFitMode = GridBestFitMode.Fast;
            OptionsView.AllowHtmlDrawGroups = true;
            OptionsView.AllowHtmlDrawHeaders = true;
            OptionsView.EnableAppearanceEvenRow = true;
            OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.SmartTag;
            OptionsView.GroupFooterShowMode = GroupFooterShowMode.Hidden;
            OptionsView.ShowGroupPanel = false;

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
                    for (var i = conditionalRules.Keys.Count - 1; i >= 0; i--)
                    {
                        var key = conditionalRules.Keys.ElementAt(i);
                        var column = Columns[key];
                        if (column == null)
                        {
                            conditionalRules.Remove(key);
                        }
                        else
                        {
                            conditionalRules[key].Column = column;
                        }
                    }

                    foreach (var rule in conditionalRules)
                    {
                        rule.Value.Column = Columns[rule.Key];
                    }

                    FormatRules.AddRange(conditionalRules.Values);
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
                    }
                }
            }

            return base.PostEditor(causeValidation);
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
        private DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, FixedStyle style, Image image, EventHandler checkedChanged)
        {
            var item = new DXMenuCheckItem(caption, column.Fixed == style, image, checkedChanged)
            {
                Tag = new FixedColumnMenuInfo(column, style)
            };
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
                conditionalRules.Add(rule.Column.FieldName, rule);
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
            var column = item.Tag as GridColumn;
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
            var column = item.Tag as GridColumn;

            var colorControl = new ColorPickEditDev { Color = column.AppearanceHeader.BackColor, Width = 150 };
            if (XtraDialog.Show(colorControl, "Column header color", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            column.AppearanceCell.BackColor = colorControl.Color;
            column.AppearanceHeader.BackColor = colorControl.Color;
        }

        /// <summary>
        /// Event on fix column state changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void OnFixedClick(object sender, EventArgs e)
        {
            var item = sender as DXMenuItem;
            if (!(item.Tag is FixedColumnMenuInfo info))
            {
                return;
            }

            info.Column.Fixed = info.Style;
            if (info.Style == FixedStyle.None)
            {
                info.Column.VisibleIndex = info.Column.AbsoluteIndex;
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
                if ((menu != null) && (menu.Column != null))
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

                    if (OptionsBehavior.AllowFixedGroups == DefaultBoolean.True)
                    {
                        if (menu.Column.Fixed == FixedStyle.None)
                        {
                            menu.Items.Add(CreateCheckItem("Lock column", menu.Column, FixedStyle.Left, Properties.Resources.Lock_16, OnFixedClick));
                        }
                        else
                        {
                            menu.Items.Add(CreateCheckItem("Unlock column", menu.Column, FixedStyle.None, Properties.Resources.Unlock_16, OnFixedClick));
                        }
                    }

                    if (AllowColumnFormatSettings && menu.Column.DisplayFormat.FormatType == FormatType.Numeric)
                    {
                        var valueFormatGroup = new DXSubMenuItem("Number format");
                        var originalMenuItem = CreateCheckItem("As per data", menu.Column, FixedStyle.Left, null, OnOriginalDigitCheckedChanged);
                        originalMenuItem.Checked = menu.Column.DisplayFormat.FormatString.ToLower().Contains(GridHelper.GeneralFormatSpecifier);
                        var significantMenuItem = CreateCheckItem("Only significant", menu.Column, FixedStyle.Left, null, OnSignificantDigitCheckedChanged);
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

                OnBeforePopupMenuShowing?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Event on custom row cell style
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        /// <remarks> DevExpress can't process event/odd style for merged cells </remarks>
        private void GridViewRowCellStyle(object sender, RowCellStyleEventArgs e)
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
        /// Event on custom cell display text
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
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
        /// Event on custom draw cell
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void GridViewCustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (IsHistogramDraw)
            {
                if (e.Column.Tag != null && e.Column.Tag.ToString().Equals(GridHelper.IsHistogramColumn))
                {
                    var hash = e.Column.GetHashCode() + GetRow(e.RowHandle).GetHashCode();
                    if (!histogrammsCache.ContainsKey(hash))
                    {
                        var data = GridHelper.GetHistogrammPoints(e.CellValue as IEnumerable<double>);
                        histogrammsCache.Add(hash, data);
                    }

                    GridHelper.DrawCellHistogram(histogrammsCache[hash], HistogramFont, HistogramColor, HistogramEmptyColor, e);
                }
            }
        }

        /// <summary>
        /// Event on data source changing
        /// </summary>
        protected override void OnDataSourceChanging()
        {
            base.OnDataSourceChanging();
            histogrammsCache.Clear();
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

            var args = new UpdateEditorInfoArgs(GetColumnReadOnly(cell.ColumnInfo.Column), bounds, appearance, cell.CellValue, ElementsLookAndFeel, cell.ViewInfo.ErrorIconText, cell.ViewInfo.ErrorIcon);
            UpdateEditor(cellEdit, args);
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
            RowCellStyle += GridViewRowCellStyle;
            FormatRules.CollectionChanged += FormatRulesCollectionChanged;
            CustomColumnDisplayText += GridViewCustomColumnDisplayText;
            CustomDrawCell += GridViewCustomDrawCell;
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
            public FixedColumnMenuInfo(GridColumn column, FixedStyle style)
            {
                Column = column;
                Style = style;
            }

            /// <summary>
            /// Column
            /// </summary>
            public GridColumn Column { get; set; }

            /// <summary>
            /// Fixed type
            /// </summary>
            public FixedStyle Style { get; set; }
        }
    }
}