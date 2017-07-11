/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;

using EmpowerPresenter.Data;
using System.Data.OleDb;
using FirebirdSql.Data.FirebirdClient;

namespace EmpowerPresenter
{
    public class SongEditorForm : System.Windows.Forms.Form
    {
        private PhotoInfo currentImage;
        private PresenterDataset.SongsRow currentRow;
        private System.Data.DataView dataView1;
        private PresenterDataset.SongVersesDataTable dt;
        private int sliderSuspendUpdate = 0;
    
        #region Designer

        private System.Windows.Forms.DataGridWithEnter dataGrid1;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.DataGridMultiLineTextBox dataGridTextBoxColumn1;
        private System.Windows.Forms.DataGridBoolColumn dataGridBoolColumn1;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtChorus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog ofdLocation;
        private System.Windows.Forms.PictureBox pbPhoto;
        private Vendisoft.Controls.ImageButton btnFont;
        private Vendisoft.Controls.ImageButton btnImage;
        private PictureBox pbCopyright;
        private Label lblCopyrightTitle;
        private EmpowerPresenter.Controls.PopupSlider sliderOpacity;
        private IContainer components;
        #endregion

        //////////////////////////////////////////////////////////////////////
        public SongEditorForm(PresenterDataset.SongsRow row)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            InitializeComponent();

            LoadSong(row);

            dataGridTextBoxColumn1.TextBox.AcceptsReturn = true;
            dataGridTextBoxColumn1.TextBox.Multiline = true;
            dataGridTextBoxColumn1.TextBox.ScrollBars = ScrollBars.Vertical;
            dataGrid1.KeyEnter += new ProcessKeyPreviewMsgEventHandler(dataGrid1_KeyEnter);
            dt.SongVersesRowChanged += new EmpowerPresenter.PresenterDataset.SongVersesRowChangeEventHandler(dt_SongVersesRowChanged);
            dt.SongVersesRowDeleted += new EmpowerPresenter.PresenterDataset.SongVersesRowChangeEventHandler(dt_SongVersesRowDeleted);

            Program.Presenter.RegisterExKeyOwnerForm(this);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongEditorForm));
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChorus = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ofdLocation = new System.Windows.Forms.OpenFileDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.lblCopyrightTitle = new System.Windows.Forms.Label();
            this.pbCopyright = new System.Windows.Forms.PictureBox();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.sliderOpacity = new EmpowerPresenter.Controls.PopupSlider();
            this.btnFont = new Vendisoft.Controls.ImageButton();
            this.btnImage = new Vendisoft.Controls.ImageButton();
            this.dataGrid1 = new System.Windows.Forms.DataGridWithEnter();
            this.dataView1 = new System.Data.DataView();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridBoolColumn1 = new System.Windows.Forms.DataGridBoolColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridMultiLineTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNumber
            // 
            resources.ApplyResources(this.txtNumber, "txtNumber");
            this.errorProvider1.SetIconAlignment(this.txtNumber, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtNumber.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.txtNumber, ((int)(resources.GetObject("txtNumber.IconPadding"))));
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtNumber_Validating);
            // 
            // label3
            // 
            this.errorProvider1.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtTitle
            // 
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.errorProvider1.SetIconAlignment(this.txtTitle, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtTitle.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.txtTitle, ((int)(resources.GetObject("txtTitle.IconPadding"))));
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Validating += new System.ComponentModel.CancelEventHandler(this.txtTitle_Validating);
            // 
            // label1
            // 
            this.errorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.errorProvider1.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtChorus
            // 
            resources.ApplyResources(this.txtChorus, "txtChorus");
            this.errorProvider1.SetIconAlignment(this.txtChorus, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtChorus.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.txtChorus, ((int)(resources.GetObject("txtChorus.IconPadding"))));
            this.txtChorus.Name = "txtChorus";
            this.txtChorus.Validating += new System.ComponentModel.CancelEventHandler(this.txtChorus_Validating);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.errorProvider1.SetIconAlignment(this.btnCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCancel.IconAlignment"))));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.errorProvider1.SetIconAlignment(this.btnSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnSave.IconAlignment"))));
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // txtCopyright
            // 
            resources.ApplyResources(this.txtCopyright, "txtCopyright");
            this.errorProvider1.SetIconAlignment(this.txtCopyright, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtCopyright.IconAlignment"))));
            this.txtCopyright.Name = "txtCopyright";
            // 
            // lblCopyrightTitle
            // 
            resources.ApplyResources(this.lblCopyrightTitle, "lblCopyrightTitle");
            this.errorProvider1.SetIconAlignment(this.lblCopyrightTitle, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblCopyrightTitle.IconAlignment"))));
            this.lblCopyrightTitle.Name = "lblCopyrightTitle";
            // 
            // pbCopyright
            // 
            this.errorProvider1.SetIconAlignment(this.pbCopyright, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pbCopyright.IconAlignment"))));
            this.pbCopyright.Image = global::EmpowerPresenter.Properties.Resources.copyright;
            resources.ApplyResources(this.pbCopyright, "pbCopyright");
            this.pbCopyright.Name = "pbCopyright";
            this.pbCopyright.TabStop = false;
            // 
            // pbPhoto
            // 
            resources.ApplyResources(this.pbPhoto, "pbPhoto");
            this.errorProvider1.SetIconAlignment(this.pbPhoto, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pbPhoto.IconAlignment"))));
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.TabStop = false;
            this.pbPhoto.Click += new System.EventHandler(this.pbPhoto_Click);
            this.pbPhoto.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPhoto_Paint);
            this.pbPhoto.DoubleClick += new System.EventHandler(this.pbPhoto_Click);
            // 
            // sliderOpacity
            // 
            resources.ApplyResources(this.sliderOpacity, "sliderOpacity");
            this.sliderOpacity.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider1.SetIconAlignment(this.sliderOpacity, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sliderOpacity.IconAlignment"))));
            this.sliderOpacity.ImedUpdate = false;
            this.sliderOpacity.MaximumSize = new System.Drawing.Size(27, 95);
            this.sliderOpacity.MinimumSize = new System.Drawing.Size(27, 95);
            this.sliderOpacity.Name = "sliderOpacity";
            this.sliderOpacity.Value = 100;
            this.sliderOpacity.ValueChanged += new System.EventHandler(this.sliderOpacity_ValueChanged);
            // 
            // btnFont
            // 
            resources.ApplyResources(this.btnFont, "btnFont");
            this.btnFont.DisabledImg = global::EmpowerPresenter.Properties.Resources.font_d;
            this.errorProvider1.SetIconAlignment(this.btnFont, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnFont.IconAlignment"))));
            this.btnFont.IsHighlighted = false;
            this.btnFont.IsPressed = false;
            this.btnFont.IsSelected = false;
            this.btnFont.Name = "btnFont";
            this.btnFont.NormalImg = global::EmpowerPresenter.Properties.Resources.font_n;
            this.btnFont.OverImg = global::EmpowerPresenter.Properties.Resources.font_n;
            this.btnFont.PressedImg = global::EmpowerPresenter.Properties.Resources.font_n;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // btnImage
            // 
            resources.ApplyResources(this.btnImage, "btnImage");
            this.btnImage.DisabledImg = global::EmpowerPresenter.Properties.Resources.images_d;
            this.errorProvider1.SetIconAlignment(this.btnImage, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnImage.IconAlignment"))));
            this.btnImage.IsHighlighted = false;
            this.btnImage.IsPressed = false;
            this.btnImage.IsSelected = false;
            this.btnImage.Name = "btnImage";
            this.btnImage.NormalImg = global::EmpowerPresenter.Properties.Resources.images;
            this.btnImage.OverImg = global::EmpowerPresenter.Properties.Resources.images;
            this.btnImage.PressedImg = global::EmpowerPresenter.Properties.Resources.images;
            this.btnImage.Click += new System.EventHandler(this.pbPhoto_Click);
            // 
            // dataGrid1
            // 
            resources.ApplyResources(this.dataGrid1, "dataGrid1");
            this.dataGrid1.DataMember = "";
            this.dataGrid1.DataSource = this.dataView1;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.errorProvider1.SetIconAlignment(this.dataGrid1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("dataGrid1.IconAlignment"))));
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.dataGrid1;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridBoolColumn1,
            this.dataGridTextBoxColumn1});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.dataGridTableStyle1, "dataGridTableStyle1");
            // 
            // dataGridBoolColumn1
            // 
            resources.ApplyResources(this.dataGridBoolColumn1, "dataGridBoolColumn1");
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn1, "dataGridTextBoxColumn1");
            // 
            // SongEditorForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.sliderOpacity);
            this.Controls.Add(this.lblCopyrightTitle);
            this.Controls.Add(this.pbCopyright);
            this.Controls.Add(this.txtCopyright);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.btnImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbPhoto);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.txtChorus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "SongEditorForm";
            this.Load += new System.EventHandler(this.SongEditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void SongEditorForm_Load(object sender, System.EventArgs e)
        {
            dataGrid1.DataSource = dataView1;
        }

        // Song Information
        public void LoadSong(PresenterDataset.SongsRow row)
        {
            if (row == null)
            {
                this.Close();
                return;
            }

            currentRow = row;

            // Accept changes if not new
            if (row.RowState == DataRowState.Detached)
            {
                this.Text = Loc.Get("Add a new song");
                this.btnSave.Text = Loc.Get("Add");

                // setup datatable
                InternalLoadSongFromDB(row, true);
            }
            else
            {
                this.btnSave.Text = Loc.Get("Save");
                row.AcceptChanges();
                
                // Load song verses from database
                InternalLoadSongFromDB(row, false);
            }

            

            // Set textboxes
            if (currentRow.Number != -1)
                txtNumber.Text = currentRow.Number.ToString();
            else
                txtNumber.Text = "";
            txtTitle.Text = currentRow.Title;
            txtChorus.Text = currentRow.Chorus;

            // Image and properties
            currentImage = new PhotoInfo();
            if (currentRow.Image == -2)
                currentImage.ImageId = Program.ConfigHelper.SongDefaultImage;
            else
                currentImage.ImageId = currentRow.Image;
            sliderSuspendUpdate++;
            if (currentRow.Overlay == 777)
                sliderOpacity.Value = 50;
            else
                sliderOpacity.Value = (int)(((double)currentRow.Overlay + 255) * 100 / 512);
            sliderSuspendUpdate--;

            txtCopyright.Text = Data.Songs.GetSongCopyright(currentRow.AutoNumber);
        }
        private void InternalLoadSongFromDB(PresenterDataset.SongsRow row, bool isNew)
        {
            dt = new PresenterDataset.SongVersesDataTable();
            dt.Columns["AutoNumber"].DefaultValue = row.AutoNumber;
            if (!isNew)
            {
                using (FBirdTask t = new FBirdTask())
                {
                    t.CommandText =
                        "SELECT [AutoNumber], [IsChorus], [Verse], [OrderNum] " +
                        "FROM [SongVerses] " +
                        "WHERE [AutoNumber] = @AutoNumber " +
                        "ORDER BY [OrderNum]";
                    t.Parameters.Add("@AutoNumber", FbDbType.Integer);
                    t.Parameters["@AutoNumber"].Value = row.AutoNumber;

                    PresenterDataset.SongVersesRow r;
                    t.ExecuteReader();
                    while (t.DR.Read())
                    {
                        r = dt.NewSongVersesRow();
                        r.AutoNumber = t.GetInt32(0);
                        r.IsChorus = t.GetInt32(1) == 1 ? true : false;
                        r.Verse = t.GetString(2).Replace("\r\n", "\n").Replace("\n", "\r\n");
                        r.OrderNum = t.GetInt32(3);
                        dt.AddSongVersesRow(r);
                    }
                }
            }

            dt.AcceptChanges();

            if (dt.Count == 0)
                btnSave.Enabled = false;

            dataView1.BeginInit();
            dataView1.Table = dt;
            dataView1.EndInit();
            dataGrid1.TableStyles[0].MappingName = dt.TableName;
        }
        private void txtNumber_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int num = 0;
            if (int.TryParse(txtNumber.Text, out num))
            {
                if (num < 0)
                    errorProvider1.SetError(txtNumber, Loc.Get("Number cannot be negative"));
                else
                    errorProvider1.SetError(txtNumber, "");
            }
            else
                errorProvider1.SetError(txtNumber, Loc.Get("Not a valid number"));
        }
        private void txtTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(txtTitle.Text.IndexOfAny(new char[] {'*', '"', '%'}) != -1)
            {
                errorProvider1.SetError(txtTitle, Loc.Get("Contains at least one of the invalid characters:") + " * \" %");
            }
            else
                errorProvider1.SetError(txtTitle, "");
        }
        private void txtChorus_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(txtChorus.Text.IndexOfAny(new char[] {'*', '"', '%'}) != -1)
            {
                errorProvider1.SetError(txtChorus, Loc.Get("Contains at least one of the invalid characters:") + " * \" %");
            }
            else
                errorProvider1.SetError(txtChorus, "");
        }
        private void dataGrid1_KeyEnter(object sender, ref Message e)
        {
            if (dataGrid1.CurrentCell.RowNumber > -1 && dataGrid1.CurrentCell.ColumnNumber == 1)
            {
                if (dataGridTextBoxColumn1.TextBox.ContainsFocus)
                {
                    SendKeys.Send(" \b"); // A simple return will not mark the cell as modified
                    dataGridTextBoxColumn1.TextBox.Modified = true;
                    dataGridTextBoxColumn1.TextBox.SelectedText = "\r\n";
                    dataGrid1.ColumnStartedEditingExt(dataGridTextBoxColumn1.TextBox.Bounds);
                }
            }
        }
        private void dt_SongVersesRowChanged(object sender, PresenterDataset.SongVersesRowChangeEvent e)
        {
            if (dt.Count > 0 || e.Action == DataRowAction.Add)
                btnSave.Enabled = true;
        }
        private void dt_SongVersesRowDeleted(object sender, PresenterDataset.SongVersesRowChangeEvent e)
        {
            if (dt.Count == 0)
                btnSave.Enabled = false;
        }

        // Display Settings
        private void llAttachPPT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentRow == null)
                return;

            OpenFileDialog f = new OpenFileDialog();
            Program.Presenter.BeginExKeyOwner();
            f.Title = Loc.Get("Select PowerPoint presentation");
            f.Filter = Loc.Get("PowerPoint presentation") + " (*.ppt;*.pps;*.pot)| *.ppt;*.pps;*.pot";
            if (f.ShowDialog() == DialogResult.Yes && File.Exists(f.FileName))
            {
                currentRow.DisplayDefault = false;
                currentRow.Location = f.FileName;
            }
            Program.Presenter.EndExKeyOwner();
        }
        private void llDetach_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currentRow.DisplayDefault = true;
            currentRow.Location = "";
        }

        // Background Settings and font
        private void pbPhoto_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (currentImage != null)
                e.Graphics.DrawImage(currentImage.Preview, 0,0,pbPhoto.Width,pbPhoto.Height);

            int v = currentRow.Overlay;
            if (v == 777)
                v = Program.ConfigHelper.SongDefaultOpacity;
            if (v > 0)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(v, Color.White)), 0, 0, pbPhoto.Width, pbPhoto.Height);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(-v, Color.Black)), 0, 0, pbPhoto.Width, pbPhoto.Height);
        }
        private void pbPhoto_Click(object sender, System.EventArgs e)
        {
            ChangeBackgroundImage();
        }
        private void ChangeBackgroundImage()
        {
#if DEMO
            new DemoVersionOnly("Changing background").ShowDialog();
#else
            ImageSelection images = new ImageSelection();
            if (DialogResult.OK == images.ShowDialog())
            {
                currentImage = images.SelectedItem;
                this.pbPhoto.Invalidate();
            }
#endif
        }
        private void sliderOpacity_ValueChanged(object sender, EventArgs e)
        {
            if (sliderSuspendUpdate == 0)
            {
                int v = sliderOpacity.Value;
                if (v > 98) // math fix
                    v = 98;
                if (v < 3)
                    v = 3;
                int opacityval = (int)((double)v * 512 / 100) - 255;
                currentRow.Overlay = opacityval;
                this.pbPhoto.Invalidate();
            }
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
#if DEMO
            new DemoVersionOnly("Changing font").ShowDialog();
#else
            FontSelection fsForm = new FontSelection();
            currentRow.FontId = currentRow.AutoNumber;
            fsForm.LoadFont(PresenterFont.GetFontFromDatabase(currentRow.FontId));
            if (fsForm.ShowDialog() == DialogResult.OK)
                PresenterFont.SaveFontToDatabase(currentRow.FontId, fsForm.PresenterFont);
#endif
        }

        // Buttons
        private void save()
        {
#if DEMO
            new DemoVersionOnly("Modifying song data").ShowDialog();
#else
            // update row
            currentRow.Number = Convert.ToInt32(txtNumber.Text);
            currentRow.Title = txtTitle.Text;
            currentRow.Chorus = txtChorus.Text;
            currentRow.Image = currentImage.ImageId;

            // save changes
            if (currentRow.RowState == DataRowState.Detached)
                Program.SongsDS.Songs.AddSongsRow(currentRow);

            Data.Songs.AddUpdateSong(currentRow);
            currentRow.AcceptChanges();

            #region Save datatable changes
            Data.Songs.UpdateSongCopyright(currentRow.AutoNumber, txtCopyright.Text);
            using (FBirdTask t = new FBirdTask())
            {
                DataTable changes = dt.GetChanges();
                if (changes != null)
                {
                    t.CommandText = "DELETE FROM [SongVerses] WHERE [AutoNumber] = @AutoNumber";
                    t.Parameters.Add("@AutoNumber", FbDbType.Integer);
                    t.Parameters["@AutoNumber"].Value = currentRow.AutoNumber;
                    t.ExecuteNonQuery();

                    t.CommandText = 
                        "INSERT INTO [SongVerses] ([AutoNumber], [IsChorus], [Verse], [OrderNum]) " +
                        "VALUES (@AutoNumber, @IsChorus, @Verse, @OrderNum)";
                    t.Parameters.Add("@IsChorus", FbDbType.SmallInt);
                    t.Parameters.Add("@Verse", FbDbType.VarChar, 0);
                    t.Parameters.Add("@OrderNum", FbDbType.Integer);

                    int i = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted)
                            continue;

                        t.Parameters["@OrderNum"].Value = i.ToString();
                        t.Parameters["@IsChorus"].Value = (bool)r["IsChorus"] == true ? 1 : 0;
                        t.Parameters["@Verse"].Value = r["Verse"];
                        t.ExecuteNonQuery();
                        i++;
                    }
                }
            }
            #endregion
#endif
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            // Verify that we have at least one verse
            int rows = 0;
            foreach (DataRow r in dt.Rows)
                if (r.RowState != DataRowState.Deleted)
                    rows++;
            if (rows == 0)
            {
                MessageBox.Show(Loc.Get("Please add at least one verse for the song"));
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.save();
            this.Close();
        }
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            if (currentRow.RowState != DataRowState.Detached)
                currentRow.RejectChanges();
            this.Close();
        }

        #region Internal testing funcions
        #if InternalOnly
        private void SongEditorForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PPTFont f;
            switch (e.KeyCode)
            {
                case Keys.F1:
                    breakVerses(3);
                    break;
                case Keys.F2:
                    breakVerses(4);
                    break;
                case Keys.F3:
                    button2.PerformClick();
                    break;
                case Keys.F4:
                    btnSave.PerformClick();
                    break;
                case Keys.F5:
                    MainStaticClass.MainForm.StartSongSlideShow(currentRow);
                    MainStaticClass.MainForm.BringToFront();
                    break;
                case Keys.F6:
                    Application.CurrentInputLanguage = InputLanguage.InstalledInputLanguages[0];
                    break;
                case Keys.F7:
                    Application.CurrentInputLanguage = InputLanguage.InstalledInputLanguages[1];
                    break;
                case Keys.F8:
                    f = FireBirdPPTFont.GetFontFromDatabase(this.currentRow.AutoNumber);
                    f.SizeInPoints += 2;
                    FireBirdPPTFont.SaveFontToDatabase(this.currentRow.AutoNumber, f);
                    btnSave.PerformClick();
                    break;
                case Keys.F9:
                    f = FireBirdPPTFont.GetFontFromDatabase(this.currentRow.AutoNumber);
                    f.SizeInPoints -= 2;
                    FireBirdPPTFont.SaveFontToDatabase(this.currentRow.AutoNumber, f);
                    btnSave.PerformClick();
                    break;
                case Keys.F10:
                    f = FireBirdPPTFont.GetFontFromDatabase(this.currentRow.AutoNumber);
                    f.Bold = !f.Bold;
                    FireBirdPPTFont.SaveFontToDatabase(this.currentRow.AutoNumber, f);
                    btnSave.PerformClick();
                    break;
                case Keys.F11:
                    MainStaticClass.SlideView.GoToLast();
                    break;
                case Keys.F12:
                    splitVerses();
                    break;
                default:
                    break;
            }
        }

        // Verse splitting
        ArrayList rez = new ArrayList();
        private void breakVerses(int verses)
        {
            int position;
            foreach (BibleDataset.SongVersesRow r in dt.Rows)
            {
                position = 0;
                for (int i = 0; i < verses; i++)
                {
                    position = r.Verse.IndexOf("\r\n", position);
                    if (position == -1)
                        break;
                    position += 2;
                }

                if (position != -1)
                {
                    r.Verse = r.Verse.Substring(0, position) + "<br/>\r\n" + r.Verse.Substring(position);
                }
            }

            btnSave.PerformClick();
        }
        private void splitVerses()
        {
            rez.Clear();
            System.IO.StreamReader sr = new StreamReader("C:\\rez.txt", System.Text.Encoding.Unicode);
            while (sr.Peek() != -1)
            {
                string s = sr.ReadLine();
                s = s.Replace("\r", ""); s = s.Replace("\n", "");
                rez.Add(s);
            }
            sr.Close();

            foreach (BibleDataset.SongVersesRow r in dt.Rows)
            {
                string o = r.Verse;
                string[] ws = o.Split(" ".ToCharArray());
                int i = ws[0].Length + 1;

                // Build split points table
                SortedList sl = new SortedList();
                for (int z = 1; z < ws.Length; z++)
                {
                    string w = ws[z];
                    w = w.Replace(":", ""); w = w.Replace(";", ""); w = w.Replace(",", "");
                    w = w.Replace(".", ""); w = w.Replace("'", ""); w = w.Replace("\"", "");
                    w = w.Replace("`", ""); w = w.Replace("!", ""); w = w.Replace("~", "");
                    w = w.Replace("-", ""); w = w.Replace("?", "");
                    if (w.Length > 0 && char.IsUpper(w, 0))
                    {
                        bool isAfterPunc = ws[z - 1].Substring(ws[z - 1].Length - 1, 1) == ";";
                        bool inExcludeList = rez.Contains(w);
                        if (!inExcludeList)
                        {
                            sl.Add(i, i);
                        }
                        else
                        {
                            if (isAfterPunc)
                            {
                                sl.Add(i, i);
                            }
                        }
                    }
                    i += ws[z].Length + 1;
                }
                for (int z = sl.Count - 1; z >= 0; z--)
                {
                    int current = (int)sl.GetByIndex(z);
                    //int previous = (int)sl.GetByIndex(z-1);
                    o = o.Insert(current, "\r\n");
                }
                r.Verse = o;
            }
            btnSave.PerformClick();
        }
        private void button2_Click(object sender, System.EventArgs e)
        {
            this.save();
            refreshSongRow(Presenter.DefaultDS.Songs.FindByAutoNumber(currentRow.AutoNumber + 1));
        }
#endif
        #endregion
    }
}