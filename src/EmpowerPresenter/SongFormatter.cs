using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;
using FirebirdSql.Data.Firebird;

using Syncfusion.Windows.Forms.Grid;

namespace EmpowerPresenter
{
	public class SongFormatter : System.Windows.Forms.Form
	{
		private ArrayList rez = new ArrayList();

		#region Designer
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtChorus;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private Syncfusion.Windows.Forms.Grid.GridDataBoundGrid gdbVerses;
		private System.Windows.Forms.OpenFileDialog ofdLocation;
		private Syncfusion.Windows.Forms.Grid.GridBoundColumn gbcChorus;
		private Syncfusion.Windows.Forms.Grid.GridBoundColumn gbcVerse;
		private System.Windows.Forms.GroupBox gbSongInformation;
		private System.Windows.Forms.PictureBox2 pbPhoto;
		#endregion
		
		private PhotoInfo currentImage;
		private DS.SongsRow currentRow;
		private System.Data.DataView dataView1;
		private System.Windows.Forms.Button button1;
		private Skybound.VisualStyles.VisualStyleProvider visualStyleProvider1;
		private DS.SongVersesDataTable dt;

		public SongFormatter(DS.SongsRow row)
		{
			InitializeComponent();

			currentRow = row;
			
			// accept changes if not new
			if (row.RowState == DataRowState.Detached)
			{
				this.Text = "Add a new song";
				this.btnSave.Text = "Add";
			}
			else
			{
				this.btnSave.Text = "Save";
				row.AcceptChanges();
			}

			dt = new EmpowerPresenter.DS.SongVersesDataTable();
			// load song verses
			using (FBirdTask t = new FBirdTask())
			{
				t.CommandText = 
					"SELECT \"AutoNumber\", \"IsChorus\", \"Verse\", \"OrderNum\" " +
					"FROM \"SongVerses\" " +
					"WHERE \"AutoNumber\" = @AutoNumber " +
					"ORDER BY \"OrderNum\"";
				t.Parameters.Add("@AutoNumber", FbDbType.Integer);
				t.Parameters["@AutoNumber"].Value = row.AutoNumber;

				DS.SongVersesRow r;
				t.ExecuteReader();
				while(t.DR.Read())
				{
					r = dt.NewSongVersesRow();
					r.AutoNumber = t.DR.GetInt32(0);
					r.IsChorus = t.DR.GetInt16(1) == 1 ? true : false;
					r.Verse = t.DR.GetString(2);
					r.OrderNum = t.DR.GetInt32(3);
					dt.AddSongVersesRow(r);
				}
				dt.AcceptChanges();
			}

			dataView1.BeginInit();
			dataView1.Table = dt;
			dataView1.EndInit();
			// disect song
			SongNumber = currentRow.Number.ToString();
			txtTitle.Text = currentRow.Title;
			txtChorus.Text = currentRow.Chorus;

			currentImage = new PhotoInfo();
			currentImage.ImageId = currentRow.Image;

			button1_Click(null, null);
			gdbVerses.Model.Refresh();
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SongFormatter));
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtChorus = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.gdbVerses = new Syncfusion.Windows.Forms.Grid.GridDataBoundGrid();
			this.gbcChorus = new Syncfusion.Windows.Forms.Grid.GridBoundColumn();
			this.gbcVerse = new Syncfusion.Windows.Forms.Grid.GridBoundColumn();
			this.ofdLocation = new System.Windows.Forms.OpenFileDialog();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.gbSongInformation = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.pbPhoto = new System.Windows.Forms.PictureBox2();
			this.dataView1 = new System.Data.DataView();
			this.visualStyleProvider1 = new Skybound.VisualStyles.VisualStyleProvider();
			((System.ComponentModel.ISupportInitialize)(this.gdbVerses)).BeginInit();
			this.gbSongInformation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataView1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtNumber
			// 
			this.txtNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNumber.Location = new System.Drawing.Point(64, 24);
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(344, 20);
			this.txtNumber.TabIndex = 3;
			this.txtNumber.Text = "";
			this.visualStyleProvider1.SetVisualStyleSupport(this.txtNumber, true);
			this.txtNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtNumber_Validating);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Number:";
			this.visualStyleProvider1.SetVisualStyleSupport(this.label3, true);
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Location = new System.Drawing.Point(64, 48);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(344, 20);
			this.txtTitle.TabIndex = 3;
			this.txtTitle.Text = "";
			this.visualStyleProvider1.SetVisualStyleSupport(this.txtTitle, true);
			this.txtTitle.Validating += new System.ComponentModel.CancelEventHandler(this.txtTitle_Validating);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Title:";
			this.visualStyleProvider1.SetVisualStyleSupport(this.label1, true);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Chorus:";
			this.visualStyleProvider1.SetVisualStyleSupport(this.label2, true);
			// 
			// txtChorus
			// 
			this.txtChorus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtChorus.Location = new System.Drawing.Point(64, 72);
			this.txtChorus.Name = "txtChorus";
			this.txtChorus.Size = new System.Drawing.Size(344, 20);
			this.txtChorus.TabIndex = 3;
			this.txtChorus.Text = "";
			this.visualStyleProvider1.SetVisualStyleSupport(this.txtChorus, true);
			this.txtChorus.Validating += new System.ComponentModel.CancelEventHandler(this.txtChorus_Validating);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnCancel.Location = new System.Drawing.Point(624, 578);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.visualStyleProvider1.SetVisualStyleSupport(this.btnCancel, true);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnSave.Location = new System.Drawing.Point(552, 578);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(64, 24);
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "Close";
			this.visualStyleProvider1.SetVisualStyleSupport(this.btnSave, true);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// gdbVerses
			// 
			this.gdbVerses.AllowDragSelectedCols = true;
			this.gdbVerses.AllowSelection = ((Syncfusion.Windows.Forms.Grid.GridSelectionFlags)((Syncfusion.Windows.Forms.Grid.GridSelectionFlags.Row | Syncfusion.Windows.Forms.Grid.GridSelectionFlags.Cell)));
			this.gdbVerses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gdbVerses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.gdbVerses.ControllerOptions = ((Syncfusion.Windows.Forms.Grid.GridControllerOptions)(((Syncfusion.Windows.Forms.Grid.GridControllerOptions.ClickCells | Syncfusion.Windows.Forms.Grid.GridControllerOptions.SelectCells) 
				| Syncfusion.Windows.Forms.Grid.GridControllerOptions.ResizeCells)));
			this.gdbVerses.DataMember = "";
			this.gdbVerses.DefaultRowHeight = 80;
			this.gdbVerses.GridBoundColumns.AddRange(new Syncfusion.Windows.Forms.Grid.GridBoundColumn[] {
																											 this.gbcChorus,
																											 this.gbcVerse});
			this.gdbVerses.Location = new System.Drawing.Point(8, 208);
			this.gdbVerses.Name = "gdbVerses";
			this.gdbVerses.OptimizeInsertRemoveCells = true;
			this.gdbVerses.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
			this.gdbVerses.Size = new System.Drawing.Size(680, 352);
			this.gdbVerses.SortBehavior = Syncfusion.Windows.Forms.Grid.GridSortBehavior.None;
			this.gdbVerses.TabIndex = 12;
			this.gdbVerses.ThemesEnabled = true;
			this.gdbVerses.UseListChangedEvent = true;
			this.gdbVerses.CurrentCellValidated += new System.EventHandler(this.gdbVerses_CurrentCellValidated);
			this.gdbVerses.CurrentCellKeyDown += new System.Windows.Forms.KeyEventHandler(this.gdbVerses_CurrentCellKeyDown);
			this.gdbVerses.CurrentCellValidating += new System.ComponentModel.CancelEventHandler(this.gdbVerses_CurrentCellValidating);
			// 
			// gbcChorus
			// 
			this.gbcChorus.MappingName = "IsChorus";
			this.gbcChorus.StyleInfo.CellType = "CheckBox";
			this.gbcChorus.StyleInfo.CellValueType = typeof(bool);
			this.gbcChorus.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Center;
			this.gbcChorus.StyleInfo.VerticalAlignment = Syncfusion.Windows.Forms.Grid.GridVerticalAlignment.Middle;
			// 
			// gbcVerse
			// 
			this.gbcVerse.MappingName = "Verse";
			this.gbcVerse.StyleInfo.AllowEnter = true;
			this.gbcVerse.StyleInfo.CellValueType = typeof(string);
			this.gbcVerse.StyleInfo.VerticalScrollbar = true;
			this.gbcVerse.StyleInfo.WrapText = true;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// gbSongInformation
			// 
			this.gbSongInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbSongInformation.Controls.Add(this.button1);
			this.gbSongInformation.Controls.Add(this.label3);
			this.gbSongInformation.Controls.Add(this.txtNumber);
			this.gbSongInformation.Controls.Add(this.txtChorus);
			this.gbSongInformation.Controls.Add(this.label2);
			this.gbSongInformation.Controls.Add(this.label1);
			this.gbSongInformation.Controls.Add(this.txtTitle);
			this.gbSongInformation.Location = new System.Drawing.Point(8, 8);
			this.gbSongInformation.Name = "gbSongInformation";
			this.gbSongInformation.Size = new System.Drawing.Size(424, 184);
			this.gbSongInformation.TabIndex = 13;
			this.gbSongInformation.TabStop = false;
			this.gbSongInformation.Text = "Song Information";
			this.visualStyleProvider1.SetVisualStyleSupport(this.gbSongInformation, true);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 104);
			this.button1.Name = "button1";
			this.button1.TabIndex = 5;
			this.button1.Text = "auto format";
			this.visualStyleProvider1.SetVisualStyleSupport(this.button1, true);
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pbPhoto
			// 
			this.pbPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pbPhoto.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pbPhoto.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbPhoto.Location = new System.Drawing.Point(440, 16);
			this.pbPhoto.Name = "pbPhoto";
			this.pbPhoto.Size = new System.Drawing.Size(240, 176);
			this.pbPhoto.TabIndex = 0;
			this.pbPhoto.TabStop = false;
			this.visualStyleProvider1.SetVisualStyleSupport(this.pbPhoto, true);
			this.pbPhoto.Click += new System.EventHandler(this.pbPhoto_Click);
			// 
			// SongFormatter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnSave;
			this.ClientSize = new System.Drawing.Size(696, 610);
			this.Controls.Add(this.pbPhoto);
			this.Controls.Add(this.gbSongInformation);
			this.Controls.Add(this.gdbVerses);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(704, 639);
			this.Name = "SongFormatter";
			this.Text = "Song formatter";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SongEditorForm_Closing);
			this.Load += new System.EventHandler(this.SongEditorForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.gdbVerses)).EndInit();
			this.gbSongInformation.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataView1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Song Information

		/// <summary></summary>
		private void txtNumber_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				int num = Convert.ToInt32(txtNumber.Text);
				if (num < 0)
					errorProvider1.SetError(txtNumber, "Number cannot be negative");
				else
					errorProvider1.SetError(txtNumber, "");
			}
			catch
			{
				errorProvider1.SetError(txtNumber, "Not a valid number");
			}
		}

		/// <summary></summary>
		private void txtTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(txtTitle.Text.IndexOfAny(new char[] {'*', '"', '%'}) != -1)
			{
				errorProvider1.SetError(txtTitle, "Contains at least one of the invalid characters: * \" %");
			}
			else
				errorProvider1.SetError(txtTitle, "");
		}

		/// <summary></summary>
		private void txtChorus_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(txtChorus.Text.IndexOfAny(new char[] {'*', '"', '%'}) != -1)
			{
				errorProvider1.SetError(txtChorus, "Contains at least one of the invalid characters: * \" %");
			}
			else
				errorProvider1.SetError(txtChorus, "");
		}

		#endregion

		#region Background Settings

		/// <summary></summary>
		private void pbPhoto_Click(object sender, System.EventArgs e)
		{
			string val = gdbVerses.CurrentCell.Renderer.ControlText;
			Aspose.PowerPoint.Presentation template = new Aspose.PowerPoint.Presentation(System.Windows.Forms.Application.StartupPath + "\\PPTTemplate.ppt");
			Aspose.PowerPoint.Presentation ppt = new Aspose.PowerPoint.Presentation("BlankPPT.ppt");
			SortedList temp = new SortedList();
			template.CloneSlide(template.Slides[4], ppt.Slides.Count, ppt, temp);
			DisplayBuilder.SetSlideText_Song(ppt.Slides[ppt.Slides.Count - 1], val, Color.Black);
			Aspose.PowerPoint.Slide s = ppt.Slides[ppt.Slides.Count - 1];
			pbPhoto.Image = s.GetThumbnail(.32, .32);
			pbPhoto.Invalidate();
		}

		/// <summary></summary>
		private void tbOverlay_ValueChanged(object sender, System.EventArgs e)
		{
			this.pbPhoto.Invalidate();
		}

		#endregion

		#region Grid

		/// <summary></summary>
		private void gdbVerses_CurrentCellValidating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			GridCurrentCell cc = gdbVerses.CurrentCell;

			if (cc.ColIndex == 2)
			{
				// validate text
				if (gdbVerses[cc.RowIndex, cc.ColIndex].Text.IndexOfAny(new char[]{'<'}) != -1)
				{
					MessageBox.Show(this, "Verse contains an invalid character: <", "Invalid character");
					e.Cancel = true;
				}
			}
		}

		#endregion

		#region Buttons

		/// <summary></summary>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;

			// update row
			currentRow.Number = Convert.ToInt32(txtNumber.Text);
			currentRow.Title = txtTitle.Text;
			currentRow.Chorus = txtChorus.Text;

			// save changes
			if (currentRow.RowState == DataRowState.Detached)
				Presenter.DefaultDS.Songs.AddSongsRow(currentRow);

			currentRow.AcceptChanges();

			// save datatable changes
			using (FBirdTask t = new FBirdTask())
			{
				DataTable changes = dt.GetChanges();
				if (changes != null)
				{
					t.CommandText = "DELETE FROM \"SongVerses\" WHERE \"AutoNumber\" = @AutoNumber";
					t.Parameters.Add("@AutoNumber", FbDbType.Integer);
					t.Parameters["@AutoNumber"].Value = currentRow.AutoNumber;
					t.ExecuteNonQuery();

					t.CommandText = 
						"INSERT INTO \"SongVerses\" (\"AutoNumber\", \"IsChorus\", \"Verse\", \"OrderNum\") " +
						"VALUES (@AutoNumber, @IsChorus, @Verse, @OrderNum)";
					t.Parameters.Add("@IsChorus", FbDbType.SmallInt);
					t.Parameters.Add("@Verse", FbDbType.VarChar, 2048);
					t.Parameters.Add("@OrderNum", FbDbType.Integer);

					int i = 0;
					foreach (DataRow r in dt.Rows)
					{
						t.Parameters["@OrderNum"].Value = i.ToString();
						t.Parameters["@IsChorus"].Value = (bool)r["IsChorus"] == true ? 1 : 0;
						t.Parameters["@Verse"].Value = r["Verse"];
						t.ExecuteNonQuery();
						i++;
					}
				}
			}
			this.Close();
		}

		/// <summary></summary>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			currentRow.RejectChanges();
			this.Close();
		}

		#endregion

		/// <summary></summary>
		private void SongEditorForm_Load(object sender, System.EventArgs e)
		{
			gdbVerses.DataSource = dataView1;
			this.gdbVerses.Model.ColStyles[2].AutoSize = false;
			this.gdbVerses.Model.ColWidths.SetSize(2, 498);
			this.gdbVerses.Model.ColStyles[2].AutoSize = false;
			this.gdbVerses.Model.ColWidths.SetSize(2, 498);
		}

		/// <summary></summary>
		private void gdbVerses_CurrentCellValidated(object sender, System.EventArgs e)
		{
			if (dataView1[gdbVerses.CurrentCell.RowIndex -1].IsNew == true)
				dataView1[gdbVerses.CurrentCell.RowIndex -1].Row["AutoNumber"] = currentRow.AutoNumber;
		}

		/// <summary></summary>
		private void SongEditorForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MainStaticClass.OnSongEditorFormClosed();
		}

		/// <summary></summary>
		public string SongNumber
		{
			get{return txtNumber.Text;}
			set{txtNumber.Text = value == "-1" ? "" : value;}
		}

		/// <summary></summary>
		private ArrayList errors = new ArrayList();
		/// <summary></summary>
		public void SetError(Control control, string value)
		{
			errorProvider1.SetError(control, value);
			
			if (value != "")
			{
				if (!errors.Contains(control))
					errors.Add(control);
			}
			else
			{
				if (errors.Contains(control))
					errors.Remove(control);
			}
			
			btnSave.Enabled = !HasErrors;
		}

		/// <summary></summary>
		public bool HasErrors
		{
			get{return errors.Count == 0;}
		}

		/// <summary></summary>
		private void gdbVerses_CurrentCellKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				pbPhoto_Click(null, null);
			}
		}

		/// <summary></summary>
		private void button1_Click(object sender, System.EventArgs e)
		{
			rez.Clear();
			System.IO.StreamReader sr = new StreamReader("C:\\rez.txt", System.Text.Encoding.Unicode);
			while(sr.Peek() != -1)
			{
				string s = sr.ReadLine();
				s = s.Replace("\r", ""); s = s.Replace("\n", "");
				rez.Add(s);
			}
			sr.Close();

			foreach(DS.SongVersesRow r in dt.Rows)
			{
				string o = r.Verse;
				string[] ws = o.Split(" ".ToCharArray());
				int i = ws[0].Length + 1;

				// Build split points table
				SortedList sl = new SortedList();
				for(int z = 1; z < ws.Length; z++)
				{
					string w = ws[z];
					w = w.Replace(":", ""); w = w.Replace(";", ""); w = w.Replace(",", "");
					w = w.Replace(".", ""); w = w.Replace("'", ""); w = w.Replace("\"", "");
					w = w.Replace("`", ""); w = w.Replace("!", ""); w = w.Replace("~", "");
					w = w.Replace("-", ""); w = w.Replace("?", ""); 
					if (w.Length > 0 && char.IsUpper(w, 0))
					{
						bool isAfterPunc = ws[z-1].Substring(ws[z-1].Length - 1, 1) == ";";
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
				for(int z = sl.Count - 1; z >= 0; z--)
				{
					int current = (int)sl.GetByIndex(z);
					//int previous = (int)sl.GetByIndex(z-1);
					o = o.Insert(current, "\n");
				}
				r.Verse = o;
			}
		}
	}
}