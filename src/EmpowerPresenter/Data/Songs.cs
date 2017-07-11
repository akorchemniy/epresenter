/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Data;

namespace EmpowerPresenter.Data
{
    public class Songs
    {
        private Songs()
        {
            // This class contains only static methods and does not need a public constructor
        }
        public static void AddUpdateSong(PresenterDataset.SongsRow song)
        {
            using (FBirdTask t = new FBirdTask())
            {
                if (song.RowState == DataRowState.Added || song.RowState == DataRowState.Detached)
                {
                    t.CommandText = t.CommandText = "INSERT INTO [Songs] " +
                        "([AutoNumber], [Number], [Title], [Chorus], [Location], [DisplayDefault], [ImageId], [Overlay], [FontId]) VALUES " +
                        "(@AutoNumber, @Number, @Title, @Chorus, @Location, @DisplayDefault, @ImageId, @Overlay, @FontId)";
                }
                else
                {
                    t.CommandText = "UPDATE [Songs] SET " +
                        "[Number] = @Number, " +
                        "[Title] = @Title, " +
                        "[Chorus] = @Chorus, " +
                        "[Location] = @Location, " +
                        "[DisplayDefault] = @DisplayDefault, " +
                        "[ImageId] = @ImageId, " +
                        "[Overlay] = @Overlay WHERE [AutoNumber] = @AutoNumber";
                }

                // HACK: M$ needs to fix up their parameters, regardless of name they are used in order
                if (t.CommandText.StartsWith("INSERT INTO"))
                    t.AddParameter("@AutoNumber", song.AutoNumber);

                t.AddParameter("@Number", song.Number);
                t.AddParameter("@Title", 256, song.Title);
                t.AddParameter("@Chorus", 256, song.Chorus);
                t.AddParameter("@Location", 1024, song.Location);
                t.AddParameter("@DisplayDefault", song.DisplayDefault);
                t.AddParameter("@ImageId", song.Image);
                t.AddParameter("@Overlay", song.Overlay);
                if (t.CommandText.StartsWith("INSERT INTO"))
                    t.AddParameter("@FontId", -2);

                // HACK: M$ needs to fix up their params
                if (t.CommandText.StartsWith("UPDATE"))
                    t.AddParameter("@AutoNumber", song.AutoNumber);

                t.ExecuteNonQuery();
            }
        }
        public static void DeleteSong(int autoNumber)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "DELETE FROM [Songs] WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);

                t.ExecuteNonQuery();
            }
        }
        public static string GetSongCopyright(int autoNumber)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "SELECT [Copyright] FROM [Songs] WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                
                t.ExecuteReader();
                if (t.DR.Read())
                    return t.GetString(0);
                else
                    return "";
            }
        }
        public static void UpdateSongCopyright(int autoNumber, string copyright)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "UPDATE [Songs] SET [Copyright] = @Copyright WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                t.AddParameter("@Copyright", 2048, copyright);
                
                t.ExecuteNonQuery();
            }    
        }
        public static void UpdateSongBackground(int autoNumber, int imageId)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "UPDATE [Songs] SET [ImageId] = @ImageId WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                t.AddParameter("@ImageId", imageId);
                
                t.ExecuteNonQuery();
            }    
        }
        public static int GetNextAutoNumber()
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "SELECT MAX([AutoNumber]) FROM [Songs]";
                object obj = t.ExecuteScalar();
                if (obj == null)
                    return 1;
                else
                    return Convert.ToInt32(obj) + 1;
            }
        }
        public static int GetSongBackground(int autoNumber)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "SELECT [ImageId] FROM [Songs] WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                int r = (int)t.ExecuteScalar();
                if (r == -2)
                    r = Program.ConfigHelper.SongDefaultImage;
                return r;
            }
        }
        public static int GetSongOpacity(int autoNumber)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "SELECT [Overlay] FROM [Songs] WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                return (int)t.ExecuteScalar();
            }
        }
        public static void UpdateSongOpacity(int autoNumber, int opacity)
        {
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "UPDATE [Songs] SET [Overlay] = @Overlay WHERE [AutoNumber] = @AutoNumber";
                t.AddParameter("@AutoNumber", autoNumber);
                t.AddParameter("@Overlay", opacity);
                t.ExecuteNonQuery();
            }
        }
    }
}
