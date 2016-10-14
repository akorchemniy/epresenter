using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Aspose.Word;

namespace ProductiveAdvantage
{
	public class TemplateStruct
	{
		private System.Collections.Hashtable _Fields;
		private TemplateStruct linkedTS;
		
		private Guid _ID;
		private string _Name;
		private string _Location;
		private string _Description;
		private string _FilePath;
		private TreeNode _TreeNode;
		private bool _NodeChecked;
		private bool _TemplateValid;
		private string _SavedPath;
		private ArrayList _DocumentNames;
		private MasterDS.ChildKeyValueDataTable _DataTable;
		private MasterDS _SourceDataSet;

        public TemplateStruct RootTemplate
        {
            get {
                return linkedTS;
            }
        }

		public System.Collections.Hashtable Fields
		{
			get
			{
					if (_Fields == null)
					{
						_Fields = new Hashtable();
						PopulateFields();
					}
					return _Fields;
			}
			set{_Fields = value;}
		}

		public Guid ID
		{
			get{return linkedTS == null ? _ID : linkedTS.ID;}
			set{if (linkedTS == null) this._ID = value; else linkedTS.ID = value;}
		}
		public string Name
		{
			get{return _Name;}
			set{this._Name = value;}
		}
		public string Location
		{
			get{return linkedTS == null ? _Location : linkedTS.Location;}
			set{if (linkedTS == null) this._Location = value; else linkedTS.Location = value;}
		}
		public string Description
		{
			get{return linkedTS == null ? _Description : linkedTS.Description;}
			set{if (linkedTS == null) this._Description = value; else linkedTS.Description = value;}
		}
		public bool MultipleDocuments
		{
			get{return linkedTS == null ? (_DocumentNames.Count > 0) : linkedTS.MultipleDocuments; }
		}
		public bool IsMultiDocRoot
		{
			get
			{
				if (MultipleDocuments == false)
					return false; // is not multidocumenut
				else if (linkedTS == null) // is it root or child
					return true;
				else
					return false;
			}
		}
		public bool IsMultiDocChild
		{
			get{return linkedTS == null ? false : true;}
		}
		public int NumberOfDocuments
		{
			get{return linkedTS == null ? _DocumentNames.Count : linkedTS.DocumentNames.Count;}
		}
		public ArrayList DocumentNames
		{
			get{return linkedTS == null ? _DocumentNames : linkedTS.DocumentNames;}
		}
		public string FilePath
		{
            get { return _FilePath; }
			set{this._FilePath = value;}
		}
		public TreeNode TreeNode
		{
			get{return _TreeNode;}
			set{this._TreeNode = value;}
		}
		public TreeNode MainTreeNode
		{
			get{return linkedTS == null ? _TreeNode : linkedTS.TreeNode;}
			set{if (linkedTS == null) this._TreeNode = value; else linkedTS.TreeNode = value;}
		}
		public bool NodeChecked
		{
			get{return linkedTS == null ? _NodeChecked : linkedTS.NodeChecked;}
			set{if (linkedTS == null) this._NodeChecked = value; else linkedTS.NodeChecked = value;}
		}
		public bool TemplateValid
		{
			get{return _TemplateValid;}
			set{this._TemplateValid = value;}
		}
		public string SavedPath
		{
			get{return _SavedPath;}
			set{this._SavedPath = value;}
		}
		public MasterDS SourceDataSet
		{
			get{return this._SourceDataSet;}
			set{this._SourceDataSet = value;}
		}
		public MasterDS.ChildKeyValueDataTable DataTable
		{
			get
            {
                if (this.Fields == null)
                    PopulateFields();

                return this._DataTable;
            }
			set{this._DataTable = value;}
		}

		public TemplateStruct(Guid id, string name, string location, string description)
		{
			this._ID = id;
			this._Name = name;
			this._Location = location;
			this._Description = description;
			this._FilePath = "";
			this._DocumentNames = new ArrayList();
			this._TemplateValid = true;
			this._TreeNode = null;
			this._DataTable = null;
			this._SourceDataSet = null;
			this._NodeChecked = true;
			this._SavedPath = "";
			this._Fields = new System.Collections.Hashtable();
			this.linkedTS = null;
		}

		public TemplateStruct(TemplateStruct linkedTemplateStruct, string name, MasterDS ds)
		{
			linkedTS = linkedTemplateStruct;
			this.Name = name;
            this.FilePath = linkedTemplateStruct.FilePath;
			this.SavedPath = "";
			this._SourceDataSet = ds;
			this.InitDataTable();
			this.TreeNode = null;
		}

		public void InitDataTable()
		{
			this._DataTable = new ProductiveAdvantage.MasterDS.ChildKeyValueDataTable();
			this._DataTable.BeginInit();

			// fill with fields
			foreach (DictionaryEntry entry in this.Fields)
			{
				this._DataTable.AddChildKeyValueRow(entry.Key.ToString(), "", true, "", true);
			}

			// fill with names
			foreach (MasterDS.ChildKeyValueRow row in this._DataTable)
			{
                row.KeyName = DatabaseOptimizer.GetKeyName(row.Key);
			}

			this._DataTable.EndInit();
			this._DataTable.AcceptChanges();
		}

		public void UpdateDataTable()
		{
			this._DataTable.BeginLoadData();

			foreach (MasterDS.ChildKeyValueRow row in this._DataTable)
			{
				if (row.UseShared == false)
					continue;

                if (this._SourceDataSet.KeyValue.FindByKey(row.Key) != null)
				    row.ValueText = this._SourceDataSet.KeyValue.FindByKey(row.Key).ValueText;
			}

			this._DataTable.EndLoadData();
		}

		public string FileName
		{
			get
			{
//				if (FilePath.IndexOf('\\') == -1)
//					return FilePath;

				// expecting FilePath to be in C:\folder\document.doc format
				string name = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
				return name.Remove(name.Length - 4, 4);
			}
		}

		public void PopulateTemplate(bool isTemplate)
		{
			if (isTemplate)
				this.FilePath = GetTemplateFilePath();

			this.TemplateValid = this.FilePath != "";
			if (TemplateValid)
				this.TemplateValid = System.IO.File.Exists(this.FilePath);
			if (!this.TemplateValid)
			{
                if (this.SavedPath != "")
                    this.TemplateValid = System.IO.File.Exists(this.SavedPath);
			}

			this._Fields = null;
		}

		public void PopulateFields()
		{
			// fill hashTable
			Fields.Clear();
			Aspose.Word.Document doc;
			string filePath = this.SavedPath == "" ? this.FilePath : this.SavedPath;

			if (File.Exists(filePath) == false)
				return;

			using (FileStreamManager fs = new FileStreamManager(filePath, true))
			{
				fs.OpenFileStream();
				if (fs.Abort == true)
				{
					TemplateValid = false;
					return;
				}

                doc = null;
                try
                {
                    doc = new Aspose.Word.Document(fs.FileStream);
                }
                catch (Exception ex)
                {
                    ErrorManager.WriteTraceInfo("TemplateStruct.PopulateFields().openDocument", ex, true, String.Format("The following document failed to open...\n{0}", filePath));
                    TemplateValid = false;
                    return;
                }
			}

			using (JetTask t = new JetTask())
			{
				object str;
				foreach(DocumentProperty property in doc.CustomDocumentProperties)
				{
					if (property.Name.StartsWith("pA_"))
					{
						t.CommandText = "SELECT [Default] FROM FieldNames WHERE FieldKey = '" + property.Name + "'";
						str = t.ExecuteScalar();
						if (str == null)
							str = "";

						this.Fields.Add(property.Name, str);
					}
				}
			}

            // sync dataTable
            if (this.DataTable != null)
            {
                Hashtable ht = new Hashtable();
                foreach (DictionaryEntry entry in this.Fields)
                {
                    ht.Add(entry.Key, entry.Value);

                    // check to see if its already in the datatable
                    if (this.DataTable != null && this.DataTable.IsInitialized)
                    {
                        if (this.DataTable.FindByKey(entry.Key.ToString()) == null)
                        {
                            this.DataTable.AddChildKeyValueRow(entry.Key.ToString(), DatabaseOptimizer.GetKeyName(entry.Key.ToString()), true, "", true);
                        }
                    }
                }

                // update visibility
                if (this.DataTable.Count > 0)
                {
                    foreach (MasterDS.ChildKeyValueRow row in DataTable)
                    {
                        row.IsVisible = ht.ContainsKey(row.Key);
                        row.KeyName = DatabaseOptimizer.GetKeyName(row.Key);
                    }
                }
            }
		}

		public string GetTemplateFilePath()
		{
			using (JetTask t = new JetTask())
			{
				t.CommandText = "SELECT FilePath FROM Templates WHERE TemplateID = @TemplateID";
				t.AddParameter("@TemplateID", ID);
				object obj = t.ExecuteScalar();

				if (obj == null)
					return "";

				if (Path.IsPathRooted(obj.ToString()) == false)
					obj = Path.Combine(Program.Configuration.NetworkPath, obj.ToString());

				return obj.ToString();
			}
		}

		public TemplateStruct CreateLinked(string name, MasterDS ds)
		{
			return new TemplateStruct(this, name, ds);
		}

        public bool ReValidate()
        {
            if (this.FilePath == "")
                this.PopulateTemplate(true);
            else
                this.PopulateTemplate(false);

            return TemplateValid;
        }
	}
}
