/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	public class AssignToCategory : System.Windows.Forms.Form
	{
		private Hashtable categories;
		private int imageId;
		private bool newImg;
		internal bool needCategoryRefresh = false;

		#region Designer
		private EmpowerPresenter.Controls.Photos.PhotoPreviewItem photoPreviewItem1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Button btnAddCat;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlBottomDiv;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.CheckedListBox _categories;
		#endregion

		public AssignToCategory(int imageId, bool newImg)
		{
			this.imageId = imageId;
			this.newImg = newImg;
			
			InitializeComponent();
			photoPreviewItem1.PhotoInfo = new PhotoInfo();
			photoPreviewItem1.PhotoInfo.ImageId = imageId;

			BuildCategoryList();
			Program.Presenter.RegisterExKeyOwnerForm(this);
		}
		private void BuildCategoryList()
		{
			_categories.Items.Clear();
			categories = PhotoInfo.GetCatListById(imageId);
			foreach(string cat in categories.Keys)
			{
				if (cat == "All")
					continue; // Do not allow the user to remove the image from the built in "All" category
				_categories.Items.Add(cat, (bool)categories[cat]);
			}
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignToCategory));
			this.photoPreviewItem1 = new EmpowerPresenter.Controls.Photos.PhotoPreviewItem();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnAddCat = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlBottomDiv = new System.Windows.Forms.Panel();
			this._categories = new System.Windows.Forms.CheckedListBox();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// photoPreviewItem1
			// 
			this.photoPreviewItem1.AccessibleDescription = null;
			this.photoPreviewItem1.AccessibleName = null;
			resources.ApplyResources(this.photoPreviewItem1, "photoPreviewItem1");
			this.photoPreviewItem1.BackgroundImage = null;
			this.photoPreviewItem1.Font = null;
			this.photoPreviewItem1.Name = "photoPreviewItem1";
			this.photoPreviewItem1.PhotoInfo = null;
			this.photoPreviewItem1.Selected = false;
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// pnlBottom
			// 
			this.pnlBottom.AccessibleDescription = null;
			this.pnlBottom.AccessibleName = null;
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			this.pnlBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.pnlBottom.BackgroundImage = null;
			this.pnlBottom.Controls.Add(this.btnSave);
			this.pnlBottom.Controls.Add(this.btnAddCat);
			this.pnlBottom.Controls.Add(this.btnCancel);
			this.pnlBottom.Controls.Add(this.pnlBottomDiv);
			this.pnlBottom.Font = null;
			this.pnlBottom.Name = "pnlBottom";
			// 
			// btnSave
			// 
			this.btnSave.AccessibleDescription = null;
			this.btnSave.AccessibleName = null;
			resources.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.BackColor = System.Drawing.SystemColors.Control;
			this.btnSave.BackgroundImage = null;
			this.btnSave.Font = null;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnAddCat
			// 
			this.btnAddCat.AccessibleDescription = null;
			this.btnAddCat.AccessibleName = null;
			resources.ApplyResources(this.btnAddCat, "btnAddCat");
			this.btnAddCat.BackColor = System.Drawing.SystemColors.Control;
			this.btnAddCat.BackgroundImage = null;
			this.btnAddCat.Font = null;
			this.btnAddCat.Name = "btnAddCat";
			this.btnAddCat.UseVisualStyleBackColor = false;
			this.btnAddCat.Click += new System.EventHandler(this.btnAddCat_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleDescription = null;
			this.btnCancel.AccessibleName = null;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.BackgroundImage = null;
			this.btnCancel.Font = null;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pnlBottomDiv
			// 
			this.pnlBottomDiv.AccessibleDescription = null;
			this.pnlBottomDiv.AccessibleName = null;
			resources.ApplyResources(this.pnlBottomDiv, "pnlBottomDiv");
			this.pnlBottomDiv.BackColor = System.Drawing.Color.Gray;
			this.pnlBottomDiv.BackgroundImage = null;
			this.pnlBottomDiv.Font = null;
			this.pnlBottomDiv.Name = "pnlBottomDiv";
			// 
			// _categories
			// 
			this._categories.AccessibleDescription = null;
			this._categories.AccessibleName = null;
			resources.ApplyResources(this._categories, "_categories");
			this._categories.BackgroundImage = null;
			this._categories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._categories.CheckOnClick = true;
			this._categories.Font = null;
			this._categories.Name = "_categories";
			this._categories.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._categories_ItemCheck);
			// 
			// AssignToCategory
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.BackgroundImage = null;
			this.Controls.Add(this._categories);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.photoPreviewItem1);
			this.Font = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AssignToCategory";
			this.pnlBottom.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAddCat_Click(object sender, System.EventArgs e)
		{
			AddCategoryPrompt f = new AddCategoryPrompt();
			if (f.ShowDialog() == DialogResult.OK)
			{
				BuildCategoryList();
				needCategoryRefresh = true;
			}
		}
		private void _categories_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			#region Ensure that the at least one category is selected
			if (e.NewValue == CheckState.Checked)
				btnSave.Enabled = true;
			else
			{
				bool enable = false;
				for(int i = 0; i < _categories.Items.Count; i++)
				{
					if (_categories.GetItemChecked(i))
					{
						btnSave.Enabled = true;
						enable = true;
					}
				}
				if (!enable)
					btnSave.Enabled = false;
			}
			#endregion
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			// Add to the main category "All"
			PhotoInfo.AddImage2Cat(imageId, "All");

			// Add to all other categories
			for(int i = 0; i < _categories.Items.Count; i++)
			{
				string cat = _categories.Items[i].ToString();
				bool isChecked = _categories.GetItemChecked(i);
				bool isAdded = (bool)categories[cat];
				if (isAdded != isChecked)
				{
					if (isAdded)
						PhotoInfo.RemoveImageFromCat(imageId, cat);
					else
						PhotoInfo.AddImage2Cat(imageId, cat);
				}
			}
			DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			if (newImg)
			{
				PhotoInfo.RemoveImage(imageId);
			}
			this.Close();
		}
	}
}
