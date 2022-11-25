using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils.MdiManager
{
    /// <summary>
    /// Matrix grid control for MDI children size initialization
    /// </summary>
    [ToolboxItem(true)]
    public partial class MatrixGridControl : XtraUserControl
    {
        /// <summary>
        /// DataTable source
        /// </summary>
        private readonly DataTable dataTable = new DataTable();

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MatrixGridControl()
        {
            InitializeComponent();

            SetSize(3, 3);
            gridControlMatrix.DataSource = dataTable;
        }

        /// <summary>
        /// Event on size selection
        /// </summary>
        public event EventHandler OnSizeSelected;

        /// <summary>
        /// User selected size
        /// </summary>
        public Size SelectedSize { get; set; } = new Size(1, 1);

        /// <summary>
        /// Multiply of matrix size
        /// </summary>
        public int TotalSize
        {
            get
            {
                return SelectedSize.Width * SelectedSize.Height;
            }
        }

        /// <summary>
        /// Selection color
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Selection color")]
        public Color SelectionColor { get; set; } = Color.DodgerBlue;

        /// <summary>
        /// Set matrix size
        /// </summary>
        /// <param name="columnsCount"> Matrix columns count </param>
        /// <param name="rowsCount"> Matrix rows count </param>
        public void SetSize(int columnsCount, int rowsCount)
        {
            gridViewMatrix.BeginDataUpdate();
            try
            {
                dataTable.Rows.Clear();
                dataTable.Columns.Clear();
                for (var i = 0; i < columnsCount; i++)
                {
                    dataTable.Columns.Add();
                }

                for (var i = 0; i < rowsCount; i++)
                {
                    dataTable.Rows.Add();
                }

                dataTable.AcceptChanges();
            }
            finally
            {
                gridViewMatrix.EndDataUpdate();
            }

            UpdateRowHeight();
        }

        /// <summary>
        /// Update cells row height depends on rows count
        /// </summary>
        private void UpdateRowHeight()
        {
            if (dataTable.Rows.Count == 0)
            {
                return;
            }

            gridViewMatrix.RowHeight = gridControlMatrix.Height / dataTable.Rows.Count - 1;
        }

        /// <summary>
        /// Event on form resize
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void MatrixGridControl_Resize(object sender, EventArgs e)
        {
            UpdateRowHeight();
        }

        /// <summary>
        /// Event on a grid mouse move
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void gridControlMatrix_MouseMove(object sender, MouseEventArgs e)
        {
            var hitInfo = gridViewMatrix.CalcHitInfo(e.X, e.Y);
            if (hitInfo.InRowCell)
            {
                var width = hitInfo.Column.AbsoluteIndex + 1;
                var height = gridViewMatrix.GetDataSourceRowIndex(hitInfo.RowHandle) + 1;

                var tempSize = new Size(width, height);
                if (tempSize != SelectedSize)
                {
                    SelectedSize = tempSize;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Event on a grid custom draw cell
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void gridViewMatrix_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.AbsoluteIndex <= SelectedSize.Width - 1 && gridViewMatrix.GetDataSourceRowIndex(e.RowHandle) <= SelectedSize.Height - 1)
            {
                e.Appearance.BackColor = SelectionColor;
            }
            else
            {
                e.Appearance.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Event on a grid click
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void gridControlMatrix_Click(object sender, EventArgs e)
        {
            OnSizeSelected?.Invoke(this, new EventArgs());
            gridViewMatrix.TopRowIndex = 0;
        }

        /// <summary>
        /// Event on a grid mouse wheel
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void gridViewMatrix_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as DXMouseEventArgs).Handled = true;
        }
    }
}
