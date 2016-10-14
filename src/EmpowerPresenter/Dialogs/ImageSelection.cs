/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

using EmpowerPresenter.Controls.Photos;
using System.IO;
using System.Collections.Generic;

namespace EmpowerPresenter
{
	public partial class ImageSelection : System.Windows.Forms.Form
	{
		private Font lbFont = new Font("Arial", 10F, FontStyle.Regular);
		private PhotoPreviewItem ppiCurrent;
		private PhotoInfo selectedItem;
		private int ixItemSelected;
		private bool supressCatChanged = false;

		#region Designer
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox _categories;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlBottomDiv;
		private Vendisoft.Controls.ImageButton btnRemove;
		private Vendisoft.Controls.ImageButton btnAdd;
		private Vendisoft.Controls.ImageButton btnRenameCat;
		private Vendisoft.Controls.ImageButton btnRemoveCat;
		private Vendisoft.Controls.ImageButton btnAddCat;
		private Button btnSelect;
		private Vendisoft.Controls.ImageButton btnLink;
		private Vendisoft.Controls.ImageButton btnUnlink;
		private Label lblInfo;
		private Button button1;
		private EmpowerPresenter.Controls.Photos.PhotoPreviewContainer photoPreviewContainer1;
		#endregion

		/////////////////////////////////////////////////////////////////////////////////
		public ImageSelection()
		{
#if !DEMO
			InitializeComponent();
			LoadCategories();
			Program.Presenter.RegisterExKeyOwnerForm(this);
#endif
		}
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageSelection));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this._categories = new System.Windows.Forms.ListBox();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.lblInfo = new System.Windows.Forms.Label();
			this.btnUnlink = new Vendisoft.Controls.ImageButton();
			this.btnLink = new Vendisoft.Controls.ImageButton();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnRenameCat = new Vendisoft.Controls.ImageButton();
			this.btnRemoveCat = new Vendisoft.Controls.ImageButton();
			this.btnAddCat = new Vendisoft.Controls.ImageButton();
			this.btnRemove = new Vendisoft.Controls.ImageButton();
			this.btnAdd = new Vendisoft.Controls.ImageButton();
			this.btnClose = new System.Windows.Forms.Button();
			this.pnlBottomDiv = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.photoPreviewContainer1 = new EmpowerPresenter.Controls.Photos.PhotoPreviewContainer();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// _categories
			// 
			resources.ApplyResources(this._categories, "_categories");
			this._categories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._categories.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this._categories.Items.AddRange(new object[] {
            resources.GetString("_categories.Items"),
            resources.GetString("_categories.Items1"),
            resources.GetString("_categories.Items2"),
            resources.GetString("_categories.Items3")});
			this._categories.Name = "_categories";
			this._categories.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._categories_DrawItem);
			this._categories.SelectedIndexChanged += new System.EventHandler(this._categories_SelectedIndexChanged);
			// 
			// pnlBottom
			// 
			this.pnlBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pnlBottom.Controls.Add(this.lblInfo);
			this.pnlBottom.Controls.Add(this.btnUnlink);
			this.pnlBottom.Controls.Add(this.btnLink);
			this.pnlBottom.Controls.Add(this.btnSelect);
			this.pnlBottom.Controls.Add(this.btnRenameCat);
			this.pnlBottom.Controls.Add(this.btnRemoveCat);
			this.pnlBottom.Controls.Add(this.btnAddCat);
			this.pnlBottom.Controls.Add(this.btnRemove);
			this.pnlBottom.Controls.Add(this.btnAdd);
			this.pnlBottom.Controls.Add(this.btnClose);
			this.pnlBottom.Controls.Add(this.pnlBottomDiv);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			this.pnlBottom.Name = "pnlBottom";
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// btnUnlink
			// 
			this.btnUnlink.BackColor = System.Drawing.Color.Transparent;
			this.btnUnlink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnUnlink.DisabledImg = global::EmpowerPresenter.Properties.Resources.unlinkfolder_dis;
			this.btnUnlink.IsHighlighted = false;
			this.btnUnlink.IsPressed = false;
			this.btnUnlink.IsSelected = false;
			resources.ApplyResources(this.btnUnlink, "btnUnlink");
			this.btnUnlink.Name = "btnUnlink";
			this.btnUnlink.NormalImg = global::EmpowerPresenter.Properties.Resources.unlinkfolder;
			this.btnUnlink.OverImg = global::EmpowerPresenter.Properties.Resources.unlinkfolder;
			this.btnUnlink.PressedImg = global::EmpowerPresenter.Properties.Resources.unlinkfolder;
			this.btnUnlink.Tag = "Remove image from current category";
			this.btnUnlink.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnUnlink.Click += new System.EventHandler(this.btnUnlink_Click);
			this.btnUnlink.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnLink
			// 
			this.btnLink.BackColor = System.Drawing.Color.Transparent;
			this.btnLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnLink.DisabledImg = ((System.Drawing.Image)(resources.GetObject("btnLink.DisabledImg")));
			this.btnLink.IsHighlighted = false;
			this.btnLink.IsPressed = false;
			this.btnLink.IsSelected = false;
			resources.ApplyResources(this.btnLink, "btnLink");
			this.btnLink.Name = "btnLink";
			this.btnLink.NormalImg = global::EmpowerPresenter.Properties.Resources.linkfolder;
			this.btnLink.OverImg = global::EmpowerPresenter.Properties.Resources.linkfolder;
			this.btnLink.PressedImg = global::EmpowerPresenter.Properties.Resources.linkfolder;
			this.btnLink.Tag = "Assign category";
			this.btnLink.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
			this.btnLink.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnSelect
			// 
			resources.ApplyResources(this.btnSelect, "btnSelect");
			this.btnSelect.BackColor = System.Drawing.SystemColors.Control;
			this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.UseVisualStyleBackColor = false;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnRenameCat
			// 
			this.btnRenameCat.BackColor = System.Drawing.Color.Transparent;
			this.btnRenameCat.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRenameCat.DisabledImg = global::EmpowerPresenter.Properties.Resources.edit_d;
			this.btnRenameCat.IsHighlighted = false;
			this.btnRenameCat.IsPressed = false;
			this.btnRenameCat.IsSelected = false;
			resources.ApplyResources(this.btnRenameCat, "btnRenameCat");
			this.btnRenameCat.Name = "btnRenameCat";
			this.btnRenameCat.NormalImg = global::EmpowerPresenter.Properties.Resources.editfolder;
			this.btnRenameCat.OverImg = global::EmpowerPresenter.Properties.Resources.editfolder;
			this.btnRenameCat.PressedImg = global::EmpowerPresenter.Properties.Resources.editfolder;
			this.btnRenameCat.Tag = "Rename category";
			this.btnRenameCat.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnRenameCat.Click += new System.EventHandler(this.btnRenameCat_Click);
			this.btnRenameCat.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnRemoveCat
			// 
			this.btnRemoveCat.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveCat.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRemoveCat.DisabledImg = global::EmpowerPresenter.Properties.Resources.removefolderdis;
			this.btnRemoveCat.IsHighlighted = false;
			this.btnRemoveCat.IsPressed = false;
			this.btnRemoveCat.IsSelected = false;
			resources.ApplyResources(this.btnRemoveCat, "btnRemoveCat");
			this.btnRemoveCat.Name = "btnRemoveCat";
			this.btnRemoveCat.NormalImg = global::EmpowerPresenter.Properties.Resources.removefolder;
			this.btnRemoveCat.OverImg = global::EmpowerPresenter.Properties.Resources.removefolder;
			this.btnRemoveCat.PressedImg = global::EmpowerPresenter.Properties.Resources.removefolder;
			this.btnRemoveCat.Tag = "Remove current category";
			this.btnRemoveCat.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnRemoveCat.Click += new System.EventHandler(this.btnRemoveCat_Click);
			this.btnRemoveCat.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnAddCat
			// 
			this.btnAddCat.BackColor = System.Drawing.Color.Transparent;
			this.btnAddCat.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddCat.DisabledImg = global::EmpowerPresenter.Properties.Resources.addfolder;
			this.btnAddCat.IsHighlighted = false;
			this.btnAddCat.IsPressed = false;
			this.btnAddCat.IsSelected = false;
			resources.ApplyResources(this.btnAddCat, "btnAddCat");
			this.btnAddCat.Name = "btnAddCat";
			this.btnAddCat.NormalImg = global::EmpowerPresenter.Properties.Resources.addfolder;
			this.btnAddCat.OverImg = global::EmpowerPresenter.Properties.Resources.addfolder;
			this.btnAddCat.PressedImg = global::EmpowerPresenter.Properties.Resources.addfolder;
			this.btnAddCat.Tag = "Adding a new category";
			this.btnAddCat.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnAddCat.Click += new System.EventHandler(this.btnAddCategory_Click);
			this.btnAddCat.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnRemove
			// 
			this.btnRemove.BackColor = System.Drawing.Color.Transparent;
			this.btnRemove.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRemove.DisabledImg = ((System.Drawing.Image)(resources.GetObject("btnRemove.DisabledImg")));
			this.btnRemove.IsHighlighted = false;
			this.btnRemove.IsPressed = false;
			this.btnRemove.IsSelected = false;
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.NormalImg = global::EmpowerPresenter.Properties.Resources.removeimage;
			this.btnRemove.OverImg = global::EmpowerPresenter.Properties.Resources.removeimage;
			this.btnRemove.PressedImg = global::EmpowerPresenter.Properties.Resources.removeimage;
			this.btnRemove.Tag = "Remove image";
			this.btnRemove.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			this.btnRemove.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.Transparent;
			this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAdd.DisabledImg = global::EmpowerPresenter.Properties.Resources.addimage;
			this.btnAdd.IsHighlighted = false;
			this.btnAdd.IsPressed = false;
			this.btnAdd.IsSelected = false;
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.NormalImg = global::EmpowerPresenter.Properties.Resources.addimage;
			this.btnAdd.OverImg = global::EmpowerPresenter.Properties.Resources.addimage;
			this.btnAdd.PressedImg = global::EmpowerPresenter.Properties.Resources.addimage;
			this.btnAdd.Tag = "Add image";
			this.btnAdd.MouseLeave += new System.EventHandler(this.btnSmall_MouseLeave);
			this.btnAdd.Click += new System.EventHandler(this.btnAddImage_Click);
			this.btnAdd.MouseEnter += new System.EventHandler(this.btnSmall_MouseEnter);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this._cancel_Click);
			// 
			// pnlBottomDiv
			// 
			this.pnlBottomDiv.BackColor = System.Drawing.Color.Gray;
			resources.ApplyResources(this.pnlBottomDiv, "pnlBottomDiv");
			this.pnlBottomDiv.Name = "pnlBottomDiv";
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			// 
			// photoPreviewContainer1
			// 
			resources.ApplyResources(this.photoPreviewContainer1, "photoPreviewContainer1");
			this.photoPreviewContainer1.BackColor = System.Drawing.Color.White;
			this.photoPreviewContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.photoPreviewContainer1.CurrentCategory = "";
			this.photoPreviewContainer1.Name = "photoPreviewContainer1";
			this.photoPreviewContainer1.ItemDoubleClicked += new System.EventHandler(this.photoPreviewContainer1_ItemDoubleClicked);
			this.photoPreviewContainer1.ItemClicked += new System.EventHandler(this.photoPreviewContainer1_ItemClicked);
			// 
			// ImageSelection
			// 
			this.AcceptButton = this.btnSelect;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnClose;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.photoPreviewContainer1);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this._categories);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Name = "ImageSelection";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadCategories()
		{
			InternalLoadCategories();
			_categories.SelectedIndex = 0;

			UpdateCategoryButtonStates();
			UpdateImagesButtonStates();
		}
		private void InternalLoadCategories()
		{
			_categories.Items.Clear();

			// Load al the categories
			foreach (string name in PhotoInfo.GetCatList())
				_categories.Items.Add(name);
		}
		private void LoadPreviews(string category)
		{
			ArrayList al = new ArrayList();
			
			// Load document
			string pathPreview = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
			XmlDocument xd = new XmlDocument();
			xd.Load(pathPreview);

			// Read all the image for this category
			foreach(XmlNode xn in xd.DocumentElement.ChildNodes)
			{
				if (xn.Name == "Category" && xn.Attributes["name"] != null && xn.Attributes["name"].InnerText == category)
				{
					foreach(XmlNode xn1 in xn.ChildNodes)
					{
						PhotoInfo pi = new PhotoInfo();
						pi.ImageId = int.Parse(xn1.Attributes["id"].InnerText);
						al.Add(pi);
					}
				}
			}

			// Sort the list based on frequency
			al.Sort(new ImageFreqComparer());
			
			photoPreviewContainer1.LoadImages(al, category);
		}
		private void UpdateCategoryButtonStates()
		{
			btnRemoveCat.Enabled = photoPreviewContainer1.CurrentCategory != "All";
			btnRemoveCat.Refresh();
			btnRenameCat.Enabled = btnRemoveCat.Enabled;
			btnRenameCat.Refresh();
		}
		private void UpdateImagesButtonStates()
		{
			bool catAll = photoPreviewContainer1.CurrentCategory != "All";
			bool imageSelected = ppiCurrent != null;

			btnUnlink.Enabled = catAll && imageSelected; btnUnlink.Refresh();
			btnLink.Enabled = imageSelected; btnLink.Refresh();
			btnRemove.Enabled = imageSelected; btnRemove.Refresh();
		}
		
		public PhotoInfo SelectedItem
		{
			get 
			{
				if (ppiCurrent != null)
					return ppiCurrent.PhotoInfo;
				else
					return null;
			}
		}

		// Categories
		private void _categories_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (supressCatChanged || _categories.SelectedIndex == -1 || _categories.SelectedIndex >= _categories.Items.Count)
				return;

			ixItemSelected = _categories.SelectedIndex;
			LoadPreviews(_categories.SelectedItem.ToString());
			photoPreviewContainer1.CurrentCategory = _categories.SelectedItem.ToString();

			UpdateCategoryButtonStates();
			UpdateImagesButtonStates();

			ppiCurrent = null;
			btnSelect.Enabled = false;
		}
		private void _categories_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index > _categories.Items.Count)
				return;

			string s = _categories.Items[e.Index].ToString();

			// Draw bg
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(Brushes.SteelBlue, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1);
			}

			// Draw text
			e.Graphics.DrawString(s, lbFont, Brushes.White, e.Bounds.X + 5, e.Bounds.Y + 6);
		}
		private void btnAddCategory_Click(object sender, System.EventArgs e)
		{
			AddCategoryPrompt f = new AddCategoryPrompt();
			if (f.ShowDialog() == DialogResult.OK)
			{
				LoadCategories(); // reload the categories to reflect the new change
			}
		}
		private void btnRemoveCat_Click(object sender, EventArgs e)
		{
			
			if (ixItemSelected < 1)
				return;
			string catname = _categories.Items[ixItemSelected].ToString();
			string txt = Loc.Get("Are you sure you want to remove the selected category:") + " ";
			if (MessageBox.Show(this, txt + catname + "?", Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				PhotoInfo.RemoveCategory(catname);
				LoadCategories(); // refresh the list
			}
		}
		private void btnRenameCat_Click(object sender, EventArgs e)
		{
			if (ixItemSelected < 1)
				return;
			string catname = _categories.Items[ixItemSelected].ToString();
			Prompt p = new Prompt();
			p.Text = Loc.Get("Rename category");
			p.lblPrompt.Text = Loc.Get("Please enter a new name for the category:");
			if (p.ShowDialog() == DialogResult.OK)
			{
				string newname = p._input.Text;
				if (newname != "")
				{
					if (_categories.Items.Contains(newname))
					{
						MessageBox.Show(Loc.Get("Name is already in use"), Loc.Get("Error"));
						return;
					}

					PhotoInfo.RenameCategory(catname, newname);
					LoadCategories(); // refresh the list

					// update the current category for the photo prevew container
					photoPreviewContainer1.CurrentCategory = newname;
				}
			}
		}

		// Photos
		private void photoPreviewContainer1_ItemClicked(object sender, System.EventArgs e)
		{
			if (ppiCurrent != null)
				ppiCurrent.Selected = false;

			ppiCurrent = (PhotoPreviewItem)sender;
			ppiCurrent.Selected = true;
			btnSelect.Enabled = true;
			UpdateImagesButtonStates();
		}
		private void photoPreviewContainer1_ItemDoubleClicked(object sender, EventArgs e)
		{
			if (ppiCurrent != null)
				btnSelect_Click(null, null); // Open the image
		}
		private void btnAddImage_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			Program.Presenter.BeginExKeyOwner();
			f.Title = Loc.Get("Select image to add");
			f.Filter = Loc.Get("Supported formats") + " | *.jpeg;*.jpg;*.png;*.bmp;*.gif | JPG | *.jpg | JPEG | *.jpeg | GIF | *.gif | PNG | *.png | BMP | *.bmp";
			f.Multiselect = false;
			if (f.ShowDialog() == DialogResult.OK)
			{
				try
				{
					int id = PhotoInfo.AddImage(Image.FromFile(f.FileName));
					AssignToCategory a2c = new AssignToCategory(id, true);
					if (a2c.ShowDialog() == DialogResult.OK)
					{
						LoadCategories();
						photoPreviewContainer1.Controls.Clear();
					}
				}
				catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }
			}
			Program.Presenter.EndExKeyOwner();
		}
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (ppiCurrent == null)
				return;

			string txt = Loc.Get("Are you sure you want to remove the image") + "?";
			if (MessageBox.Show(this, txt, Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				photoPreviewContainer1.Controls.Remove(ppiCurrent);
				PhotoInfo.RemoveImage(ppiCurrent.PhotoInfo.ImageId);
			}
		}
		private void btnLink_Click(object sender, EventArgs e)
		{
			if (ppiCurrent == null)
				return;

			AssignToCategory f = new AssignToCategory(ppiCurrent.PhotoInfo.ImageId, false);
			if (f.ShowDialog() == DialogResult.OK)
			{
				// Check if the current image got removed from the current category
				System.Collections.Hashtable h = PhotoInfo.GetCatListById(ppiCurrent.PhotoInfo.ImageId);
				if (!(bool)h[photoPreviewContainer1.CurrentCategory])
					photoPreviewContainer1.Controls.Remove(ppiCurrent);

				// Check if category list needs to be refreshed
				if (f.needCategoryRefresh)
				{
					supressCatChanged = true;
					object selectedItem = _categories.SelectedItem;
					InternalLoadCategories();
					_categories.SelectedItem = selectedItem;
					supressCatChanged = false;
				}
			}
		}
		private void btnUnlink_Click(object sender, EventArgs e)
		{
			if (ppiCurrent == null)
				return;

			string txt = Loc.Get("Are you sure you want to remove the image from the current category") + "?";
			if (MessageBox.Show(this, txt, Loc.Get("Warning"), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				PhotoInfo.RemoveImageFromCat(ppiCurrent.PhotoInfo.ImageId, photoPreviewContainer1.CurrentCategory);
				photoPreviewContainer1.Controls.Remove(ppiCurrent);
			}
		}

		// Buttons
		private void _cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void btnSmall_MouseEnter(object sender, EventArgs e)
		{
			lblInfo.Text = Loc.Get(((Control)sender).Tag.ToString());
		}
		private void btnSmall_MouseLeave(object sender, EventArgs e)
		{
			lblInfo.Text = "";
		}
		private void btnSelect_Click(object sender, EventArgs e)
		{
			PhotoInfo.LogPhotoHit(SelectedItem.ImageId);

			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
	public partial class ImageSelection
	{
		public static Image GetPreviewImage(int id)
		{
			string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\Previews\\" + id + ".w";
			if (System.IO.File.Exists(path))
			{
				try
				{
					FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
					byte[] bytes = new byte[fs.Length];
					fs.Read(bytes, 0, (int)fs.Length);
					fs.Close();

					// Perform XOR
					//for (int j = 0; j < bytes.Length; j++)
					//{ bytes[j] = (byte)(bytes[j] ^ 0xff); }

					MemoryStream ms = new MemoryStream(bytes);
					Image img = Image.FromStream(ms);
					ms.Close();
					return img;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLine(ex.ToString());
                    return Image.FromFile(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\default.preview");
				}
			}
            return Image.FromFile(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\default.preview");
		}
	}
	public class ImageFreqComparer: IComparer
	{
		private Dictionary<int, int> frequencies = null;
		public ImageFreqComparer()
		{
			frequencies = PhotoInfo.GetPhotoFrequency();
		}

		#region IComparer Members
		public int Compare(object x, object y)
		{
			PhotoInfo _x = (PhotoInfo)x;
			PhotoInfo _y = (PhotoInfo)y;
			bool xfound = frequencies.ContainsKey(_x.ImageId);
			bool yfound = frequencies.ContainsKey(_y.ImageId);

			if (!xfound && !yfound)
				return _x.ImageId.CompareTo(_y.ImageId);
			else if (xfound && !yfound)
				return 1;
			else if (yfound && !xfound)
				return -1;
			else
				return frequencies[_x.ImageId].CompareTo(frequencies[_y.ImageId]);
		}
		#endregion
	}
}
