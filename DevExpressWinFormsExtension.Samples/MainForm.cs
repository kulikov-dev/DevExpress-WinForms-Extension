using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpressWinFormsExtension.Utils;
using DevExpressWinFormsExtension.DataControls.Extensions;
using DevExpressWinFormsExtension.DataControls;
using DevExpressWinFormsExtension.Samples.Data;
using DevExpressWinFormsExtension.Helpers;
using DevExpressWinFormsExtension.Samples.Enums;
using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.DataControls.GridView.Utils;
using Bogus;
using System.Linq;

namespace DevExpressWinFormsExtension.Samples
{
    /// <summary>
    /// Main form of the application
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            InitLookUp();
            InitTreeList();
            InitGrid();
            InitSkins();

            maskTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            maskTextEdit.Properties.Mask.EditMask = RegexMaskHelper.DoublePositiveMaskPrecision1;

            EnumExtension.FillComboBox<DateViewEnum>(cbDateType);
            cbDateType.SelectedIndex = 0;

            passwordTextEditDev.IsValueEmpty();

            Application.ApplicationExit += Application_ApplicationExit;
        }

        /// <summary>
        /// GridView initilization
        /// </summary>
        private void InitGrid()
        {
            bandedGridViewDev.InitializeDefaultSettings();
            bandedGridViewDev.IsHistogramDraw = true;
            colHistogram.Tag = GridHelper.IsHistogramColumn;

            var source = new List<PersonGridRowInfo>();
            var testFaker = new Faker();
            for (var i = 0; i < 25; ++i)
            {
                var item = new PersonGridRowInfo()
                {
                    Name = testFaker.Name.FullName(),
                    AverageSalary = testFaker.Random.Double(1000, 15000),
                    IsValid = testFaker.Random.Bool(),
                    Histogram = Enumerable.Repeat(0, 1000).Select(x=> testFaker.Random.Double(0, 1000)).ToList()
                };

                source.Add(item);
            }

            gridControlDev.DataSource = source;

            //// For merging a band and a caption titles
            new GridPainterDev(bandedGridViewDev);
            bandedGridViewDev.BestFitBands();
        }

        /// <summary>
        /// TreeList initialization
        /// </summary>
        private void InitTreeList()
        {
            treeListSearchable.TreeList.StateImageList = imageList1;
            treeListSearchable.TreeList.OptionsView.ShowColumns = false;
            treeListSearchable.TreeList.OptionsView.CheckBoxStyle = DevExpress.XtraTreeList.DefaultNodeCheckBoxStyle.Check;

            var column = treeListSearchable.TreeList.Columns.Add();
            column.Visible = true;
            var node = treeListSearchable.TreeList.CreateNode("Test record 1");
            node.StateImageIndex = 0;

            treeListSearchable.TreeList.CreateNode("Test record 2");
            var parentNode = treeListSearchable.TreeList.CreateNode("Parent");
            parentNode.StateImageIndex = 0;

            node = treeListSearchable.TreeList.CreateNode("Test record 3", parentNode);
            node.StateImageIndex = 0;

            node = treeListSearchable.TreeList.CreateNode("Test record 4", parentNode);
            node.StateImageIndex = 0;
        }

        /// <summary>
        /// Lookup initialization
        /// </summary>
        private void InitLookUp()
        {
            var source = new List<MusicStyleDataItem>
            {
                new MusicStyleDataItem("Rock", "Rock music is a broad genre of popular music that originated as \"rock and roll\" in the United States in the late 1940s and early 1950s"),
                new MusicStyleDataItem("Pop", "Pop music is a genre of popular music that originated in its modern form during the mid-1950s in the United States and the United Kingdom."),
                new MusicStyleDataItem("Funk", "Funk is a music genre that originated within the African-American communities in the mid-1960s, when musicians created a rhythmic, danceable, new form of music influenced by Jazz, R&B, Soul music")
            };

            lookUpDev.Properties.DataSource = source;
            lookUpDev.Properties.DisplayMember = "Name";
            lookUpDev.Properties.DescriptionField = "Description";

            lookUpDev.Properties.Columns.Clear();
            var column = new LookUpColumnInfo("Name", "Name", 70);
            lookUpDev.Properties.Columns.Add(column);
            column = new LookUpColumnInfo("Description", "Description", 70);
            lookUpDev.Properties.Columns.Add(column);
            lookUpDev.Properties.Columns["Description"].Visible = false;
        }

        /// <summary>
        /// Skins initialization
        /// </summary>
        private void InitSkins()
        {
            btnDarkSkin.ImageOptions.Image = SkinHelper.GetSkinPreviewImage("DevExpress Dark Style", 50, 28, 2);
            btnOfficeSkin.ImageOptions.Image = SkinHelper.GetSkinPreviewImage("Office 2019 Colorful", 50, 28, 2);
            btnBlueSkin.ImageOptions.Image = SkinHelper.GetSkinPreviewImage("Visual Studio 2013 Blue", 50, 28, 2);
        }

        /// <summary>
        /// Event on show input box
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void btnShowInputBox_Click(object sender, EventArgs e)
        {
            InputBoxValidableDev.Show("New user", "Input your username:", CustomInputBoxValidator);
        }

        /// <summary>
        /// Custom validation logic for the user input
        /// </summary>
        /// <param name="value"> Input value </param>
        /// <returns> Error message, if not valid </returns>
        private string CustomInputBoxValidator(string value)
        {
            return "User already exists!";
        }

        /// <summary>
        /// Event on show progress manager
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void btnShowProgressManagerControl_Click(object sender, EventArgs e)
        {
            using (var control = new CalcProgressControl())
            {
                XtraUserControlHelper.ShowControl(this, control, "Calculator", isSizable: true);
            }
        }

        /// <summary>
        /// Event on datetype selected index changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void cbDateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (cbDateType.SelectedItem as EnumEditorItem<DateViewEnum>).EnumValue;
            dateEdit.UpdateView(item == DateViewEnum.Month ? VistaCalendarInitialViewStyle.MonthView : VistaCalendarInitialViewStyle.YearView);
        }

        /// <summary>
        /// Event on application exit
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            SolidBrushesCache.Dispose();
        }

        /// <summary>
        /// Event on closing the form
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GridPainterDev.DisposePainter(bandedGridViewDev);
        }
    }
}