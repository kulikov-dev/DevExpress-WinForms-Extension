
using DevExpressWinFormsExtension.DataControls.ColorPickEdit;
using DevExpressWinFormsExtension.DataControls.LookUp;

namespace DevExpressWinFormsExtension.Samples
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnShowInputBox = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnShowProgressManagerControl = new DevExpress.XtraEditors.SimpleButton();
            this.maskTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpDev = new DevExpressWinFormsExtension.DataControls.LookUp.LookUpDev();
            this.treeListSearchable = new DevExpressWinFormsExtension.DataControls.TreeList.TreeListSearchable();
            this.groupControlCheckedDev = new DevExpressWinFormsExtension.DataControls.GroupControlCheckedDev();
            this.btnDarkSkin = new DevExpress.XtraEditors.SimpleButton();
            this.btnOfficeSkin = new DevExpress.XtraEditors.SimpleButton();
            this.btnBlueSkin = new DevExpress.XtraEditors.SimpleButton();
            this.passwordTextEditDev = new DevExpressWinFormsExtension.DataControls.PasswordTextEditDev();
            this.dateDoubleTrackBarControlDev = new DevExpressWinFormsExtension.DataControls.DateDoubleTrackBarControlDev();
            this.colorPickEditDev2 = new DevExpressWinFormsExtension.DataControls.ColorPickEdit.ColorPickEditDev();
            this.colorPickEditDev1 = new DevExpressWinFormsExtension.DataControls.ColorPickEdit.ColorPickEditDev();
            this.cbDateType = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDev.Properties)).BeginInit();
            this.treeListSearchable.SuspendLayout();
            this.groupControlCheckedDev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextEditDev.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEditDev2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEditDev1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDateType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(265, 92);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Size = new System.Drawing.Size(120, 20);
            this.dateEdit.TabIndex = 11;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Color_16.png");
            // 
            // btnShowInputBox
            // 
            this.btnShowInputBox.Location = new System.Drawing.Point(131, 144);
            this.btnShowInputBox.Name = "btnShowInputBox";
            this.btnShowInputBox.Size = new System.Drawing.Size(75, 23);
            this.btnShowInputBox.TabIndex = 8;
            this.btnShowInputBox.Text = "Show";
            this.btnShowInputBox.Click += new System.EventHandler(this.btnShowInputBox_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(109, 13);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "PasswordTextEditDev:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 69);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(84, 13);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "ColorPickEditDev:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 95);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(92, 13);
            this.labelControl5.TabIndex = 20;
            this.labelControl5.Text = "DateEditExtension:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 13);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "LookUpDev:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 149);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(109, 13);
            this.labelControl6.TabIndex = 22;
            this.labelControl6.Text = "InputBoxValidableDev:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 178);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(91, 13);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "Progress manager:";
            // 
            // btnShowProgressManagerControl
            // 
            this.btnShowProgressManagerControl.Location = new System.Drawing.Point(131, 173);
            this.btnShowProgressManagerControl.Name = "btnShowProgressManagerControl";
            this.btnShowProgressManagerControl.Size = new System.Drawing.Size(75, 23);
            this.btnShowProgressManagerControl.TabIndex = 23;
            this.btnShowProgressManagerControl.Text = "Show";
            this.btnShowProgressManagerControl.Click += new System.EventHandler(this.btnShowProgressManagerControl_Click);
            // 
            // maskTextEdit
            // 
            this.maskTextEdit.Location = new System.Drawing.Point(131, 118);
            this.maskTextEdit.Name = "maskTextEdit";
            this.maskTextEdit.Size = new System.Drawing.Size(254, 20);
            this.maskTextEdit.TabIndex = 25;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(12, 121);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(90, 13);
            this.labelControl7.TabIndex = 26;
            this.labelControl7.Text = "RegexMaskHelper:";
            // 
            // lookUpDev
            // 
            this.lookUpDev.DescriptionField = "";
            this.lookUpDev.Location = new System.Drawing.Point(131, 12);
            this.lookUpDev.Name = "lookUpDev";
            this.lookUpDev.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpDev.Properties.DescriptionField = "";
            this.lookUpDev.Properties.ShowHeader = false;
            this.lookUpDev.Size = new System.Drawing.Size(254, 20);
            this.lookUpDev.TabIndex = 10;
            // 
            // treeListSearchable
            // 
            this.treeListSearchable.IsSearchable = true;
            this.treeListSearchable.Location = new System.Drawing.Point(421, 79);
            this.treeListSearchable.Name = "treeListSearchable";
            this.treeListSearchable.Size = new System.Drawing.Size(253, 180);
            this.treeListSearchable.TabIndex = 12;
            // 
            // groupControlCheckedDev
            // 
            this.groupControlCheckedDev.Checked = true;
            this.groupControlCheckedDev.CheckStyle = DevExpress.XtraEditors.Controls.CheckBoxStyle.CheckBox;
            this.groupControlCheckedDev.Controls.Add(this.btnDarkSkin);
            this.groupControlCheckedDev.Controls.Add(this.btnOfficeSkin);
            this.groupControlCheckedDev.Controls.Add(this.btnBlueSkin);
            this.groupControlCheckedDev.Location = new System.Drawing.Point(145, 202);
            this.groupControlCheckedDev.Name = "groupControlCheckedDev";
            this.groupControlCheckedDev.Size = new System.Drawing.Size(216, 65);
            this.groupControlCheckedDev.TabIndex = 9;
            this.groupControlCheckedDev.Title = "Sample skin images";
            // 
            // btnDarkSkin
            // 
            this.btnDarkSkin.Location = new System.Drawing.Point(18, 25);
            this.btnDarkSkin.Name = "btnDarkSkin";
            this.btnDarkSkin.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnDarkSkin.ShowToolTips = false;
            this.btnDarkSkin.Size = new System.Drawing.Size(56, 32);
            this.btnDarkSkin.TabIndex = 14;
            // 
            // btnOfficeSkin
            // 
            this.btnOfficeSkin.Location = new System.Drawing.Point(80, 25);
            this.btnOfficeSkin.Name = "btnOfficeSkin";
            this.btnOfficeSkin.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnOfficeSkin.ShowToolTips = false;
            this.btnOfficeSkin.Size = new System.Drawing.Size(56, 32);
            this.btnOfficeSkin.TabIndex = 15;
            // 
            // btnBlueSkin
            // 
            this.btnBlueSkin.Location = new System.Drawing.Point(142, 25);
            this.btnBlueSkin.Name = "btnBlueSkin";
            this.btnBlueSkin.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnBlueSkin.ShowToolTips = false;
            this.btnBlueSkin.Size = new System.Drawing.Size(56, 32);
            this.btnBlueSkin.TabIndex = 16;
            // 
            // passwordTextEditDev
            // 
            this.passwordTextEditDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextEditDev.Location = new System.Drawing.Point(131, 38);
            this.passwordTextEditDev.Name = "passwordTextEditDev";
            editorButtonImageOptions2.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions2.Image")));
            this.passwordTextEditDev.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "Show password", "Visible", null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.passwordTextEditDev.Properties.PasswordChar = '*';
            this.passwordTextEditDev.Size = new System.Drawing.Size(254, 22);
            this.passwordTextEditDev.TabIndex = 7;
            // 
            // dateDoubleTrackBarControlDev
            // 
            this.dateDoubleTrackBarControlDev.AutoMiddleDate = false;
            this.dateDoubleTrackBarControlDev.ButtonSize = new System.Drawing.Size(12, 26);
            this.dateDoubleTrackBarControlDev.LeftDate = new System.DateTime(2021, 8, 4, 0, 0, 0, 0);
            this.dateDoubleTrackBarControlDev.LengthMinimum = 0;
            this.dateDoubleTrackBarControlDev.Location = new System.Drawing.Point(421, 12);
            this.dateDoubleTrackBarControlDev.MaxDateLimit = new System.DateTime(2022, 7, 26, 0, 0, 0, 0);
            this.dateDoubleTrackBarControlDev.MiddleButtonVisibility = false;
            this.dateDoubleTrackBarControlDev.MiddleDate = new System.DateTime(2022, 1, 24, 0, 0, 0, 0);
            this.dateDoubleTrackBarControlDev.MinDateLimit = new System.DateTime(2021, 7, 26, 0, 0, 0, 0);
            this.dateDoubleTrackBarControlDev.Name = "dateDoubleTrackBarControlDev";
            this.dateDoubleTrackBarControlDev.RightDate = new System.DateTime(2022, 7, 26, 0, 0, 0, 0);
            this.dateDoubleTrackBarControlDev.ShowTooltip = false;
            this.dateDoubleTrackBarControlDev.Size = new System.Drawing.Size(253, 61);
            this.dateDoubleTrackBarControlDev.TabIndex = 6;
            // 
            // colorPickEditDev2
            // 
            this.colorPickEditDev2.EditValue = System.Drawing.Color.Empty;
            this.colorPickEditDev2.Location = new System.Drawing.Point(265, 66);
            this.colorPickEditDev2.Name = "colorPickEditDev2";
            this.colorPickEditDev2.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.colorPickEditDev2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickEditDev2.Properties.ColorDialogOptions.ShowMakeWebSafeButton = false;
            this.colorPickEditDev2.Size = new System.Drawing.Size(120, 20);
            this.colorPickEditDev2.TabIndex = 3;
            // 
            // colorPickEditDev1
            // 
            this.colorPickEditDev1.EditValue = System.Drawing.Color.Empty;
            this.colorPickEditDev1.Location = new System.Drawing.Point(131, 66);
            this.colorPickEditDev1.Name = "colorPickEditDev1";
            this.colorPickEditDev1.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.colorPickEditDev1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickEditDev1.Properties.ColorDialogOptions.ShowMakeWebSafeButton = false;
            this.colorPickEditDev1.Size = new System.Drawing.Size(120, 20);
            this.colorPickEditDev1.TabIndex = 2;
            // 
            // cbDateType
            // 
            this.cbDateType.Location = new System.Drawing.Point(131, 92);
            this.cbDateType.Name = "cbDateType";
            this.cbDateType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbDateType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbDateType.Size = new System.Drawing.Size(120, 20);
            this.cbDateType.TabIndex = 27;
            this.cbDateType.SelectedIndexChanged += new System.EventHandler(this.cbDateType_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 265);
            this.Controls.Add(this.cbDateType);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.maskTextEdit);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.btnShowProgressManagerControl);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnShowInputBox);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lookUpDev);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.treeListSearchable);
            this.Controls.Add(this.dateEdit);
            this.Controls.Add(this.groupControlCheckedDev);
            this.Controls.Add(this.passwordTextEditDev);
            this.Controls.Add(this.dateDoubleTrackBarControlDev);
            this.Controls.Add(this.colorPickEditDev2);
            this.Controls.Add(this.colorPickEditDev1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "DevExpressWinFormsExtension.Samples";
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDev.Properties)).EndInit();
            this.treeListSearchable.ResumeLayout(false);
            this.groupControlCheckedDev.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextEditDev.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEditDev2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEditDev1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDateType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ColorPickEditDev colorPickEditDev1;
        private ColorPickEditDev colorPickEditDev2;
        private DataControls.DateDoubleTrackBarControlDev dateDoubleTrackBarControlDev;
        private DataControls.PasswordTextEditDev passwordTextEditDev;
        private DevExpress.XtraEditors.SimpleButton btnShowInputBox;
        private DataControls.GroupControlCheckedDev groupControlCheckedDev;
        private LookUpDev lookUpDev;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private System.Windows.Forms.ImageList imageList1;
        private DataControls.TreeList.TreeListSearchable treeListSearchable;
        private DevExpress.XtraEditors.SimpleButton btnDarkSkin;
        private DevExpress.XtraEditors.SimpleButton btnOfficeSkin;
        private DevExpress.XtraEditors.SimpleButton btnBlueSkin;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnShowProgressManagerControl;
        private DevExpress.XtraEditors.TextEdit maskTextEdit;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit cbDateType;
    }
}

