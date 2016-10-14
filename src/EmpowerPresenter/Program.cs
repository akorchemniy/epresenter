/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

using EmpowerPresenter.Data;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Data.OleDb;
using FirebirdSql.Data.FirebirdClient;

namespace EmpowerPresenter
{
	public class Program
	{
		public static Main Presenter;
		public static ConfigHelper ConfigHelper;
		public static PresenterDataset SongsDS;
        public static PresenterDataset SongNotesDS;
		public static PresenterDataset BibleDS;
		public static bool exiting = false;
		private static Mutex onlyOne;

		[STAThread]
		static void Main()
		{
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			Application.ThreadExit += new EventHandler(Application_ThreadExit);

			// Single instance only
			Program.onlyOne = new Mutex(true, "SingleInstanceEPresenter");
			if (!Program.onlyOne.WaitOne(0, false))
			{
				MessageBox.Show(Loc.Get("ePresenter is already running"), "ePresenter");
				try
				{
					Process[] p = System.Diagnostics.Process.GetProcessesByName("LaunchEP");
					if (p.Length > 0)
						p[0].Kill();
				}
				catch { /*System.Diagnostics.Trace.WriteLine("Failed to close the splash");*/ }
				return;
			}

			Program.ConfigHelper = new ConfigHelper();
            global::EmpowerPresenter.Properties.Resources.Culture = new System.Globalization.CultureInfo(Program.ConfigHelper.CurrentLanguage);
			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Program.ConfigHelper.CurrentLanguage);
			Loc.culture = Program.ConfigHelper.CurrentLanguage;

			Application.EnableVisualStyles();
			Program.Presenter = new Main();
			Application.Run(Program.Presenter);
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			if (e.Exception is LicenseException)
			{
				// Close the native exe for splash
				try { System.Diagnostics.Process.GetProcessesByName("LaunchEP")[0].Kill(); }
				catch { System.Diagnostics.Trace.WriteLine("Failed to close the splash"); }

				Environment.Exit(0);
			}

			string err = e.Exception.Message + " " + e.Exception.ToString();
			System.Diagnostics.Trace.WriteLine(err);
			System.Diagnostics.Trace.Flush();

			ErrorDlg f = new ErrorDlg(err);
			f.TopMost = true;
			f.Show();
		}
		private static void Application_ThreadExit(object sender, EventArgs e)
		{
			Program.exiting = true;

			if (!Program.Presenter.songsInitComplete)
			{

				try
				{
					if (Program.Presenter.bwSongsInit.IsBusy)
						Program.Presenter.bwSongsInit.CancelAsync();
				}
				catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.ToString()); }
				return;
			}

			/*
			 * Save all the unsaved data
			 */
            using (FBirdTask t = new FBirdTask())
			{
				t.CommandText = "INSERT INTO [Songs] " +
					"([AutoNumber], [Number], [Title], [Chorus], [Location], [DisplayDefault], [ImageId], [Overlay], [FontId], [Settings]) VALUES " +
					"(@AutoNumber, @Number, @Title, @Chorus, @Location, @DisplayDefault, @ImageId, @Overlay, @FontId, @Settings)";
                t.Parameters.Add("@AutoNumber", FbDbType.Integer);
                t.Parameters.Add("@Number", FbDbType.Integer);
                t.Parameters.Add("@Title", FbDbType.VarChar, 0);
                t.Parameters.Add("@Chorus", FbDbType.VarChar, 0);
                t.Parameters.Add("@Location", FbDbType.VarChar, 0);
                t.Parameters.Add("@DisplayDefault", FbDbType.SmallInt);
                t.Parameters.Add("@ImageId", FbDbType.Integer);
                t.Parameters.Add("@Overlay", FbDbType.Integer);
                t.Parameters.Add("@FontId", FbDbType.Integer);
                t.Parameters.Add("@Settings", FbDbType.VarChar, 0);

				DataTable dt = Program.SongsDS.Songs.GetChanges(DataRowState.Added);
				if (dt != null)
				{
					foreach (DataRow row in dt.Rows)
					{
						t.Parameters["@AutoNumber"].Value = row["AutoNumber"];
						t.Parameters["@Number"].Value = row["Number"];
						t.Parameters["@Title"].Value = row["Title"];
						t.Parameters["@Chorus"].Value = row["Chorus"];
						t.Parameters["@Location"].Value = row["Location"];
						t.Parameters["@DisplayDefault"].Value = (bool)row["DisplayDefault"] == true ? 1 : 0;
						t.Parameters["@ImageId"].Value = row["Image"];
						t.Parameters["@Overlay"].Value = row["Overlay"];
						t.Parameters["@FontId"].Value = row["FontId"];
						t.Parameters["@Settings"].Value = row["Settings"];

						t.ExecuteNonQuery();
					}
				}

				dt = Program.SongsDS.Songs.GetChanges(DataRowState.Modified);
				if (dt != null)
				{
					t.CommandText = "UPDATE [Songs] SET " +
						"[Number] = @Number, " +
						"[Title] = @Title, " +
						"[Chorus] = @Chorus, " +
						"[Location] = @Location, " +
						"[DisplayDefault] = @DisplayDefault, " +
						"[ImageId] = @ImageId, " +
						"[Overlay] = @Overlay, " +
						"[FontId] = @FontId, " +
						"[Settings] = @Settings WHERE [AutoNumber] = @AutoNumber";

					foreach (DataRow row in dt.Rows)
					{
						t.Parameters["@AutoNumber"].Value = row["AutoNumber"];
						t.Parameters["@Number"].Value = row["Number"];
						t.Parameters["@Title"].Value = row["Title"];
						t.Parameters["@Chorus"].Value = row["Chorus"];
						t.Parameters["@Location"].Value = row["Location"];
						t.Parameters["@DisplayDefault"].Value = (bool)row["DisplayDefault"] == true ? 1 : 0;
						t.Parameters["@ImageId"].Value = row["Image"];
						t.Parameters["@Overlay"].Value = row["Overlay"];
						t.Parameters["@FontId"].Value = row["FontId"];
						t.Parameters["@Settings"].Value = row["Settings"];

						t.ExecuteNonQuery();
					}
				}

				dt = Program.SongsDS.Songs.GetChanges(DataRowState.Deleted);
				if (dt != null)
				{
					t.Parameters.Clear();
                    t.Parameters.Add("@AutoNumber", FbDbType.Integer);
					t.CommandText = "DELETE FROM [Songs] WHERE [AutoNumber] = @AutoNumber";

					foreach (DataRow row in dt.Rows)
					{
						t.Parameters["@AutoNumber"].Value = row["AutoNumber"];
						t.ExecuteNonQuery();
					}
				}
			}//*/
		}
	}
}
