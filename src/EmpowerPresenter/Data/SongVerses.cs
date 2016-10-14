/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Data;

namespace EmpowerPresenter.Data
{
	public class SongVerses
	{
		private SongVerses()
		{
			// This class contains only static methods and does not need a public constructor
		}
        //public static void AddoSngVerses(PresenterDataset.SongVersesRow songVerse)
        //{
        //    using (FBirdTask t = new FBirdTask())
        //    {
        //        t.CommandText = 
        //            "INSERT INTO [SongVerses] ([AutoNumber], [IsChorus], [Verse], [OrderNum]) " +
        //            "VALUES (@AutoNumber, @IsChorus, @Verse, @OrderNum)";

        //        t.AddParameter("@AutoNumber", songVerse.AutoNumber);
        //        t.AddParameter("@IsChorus", songVerse.IsChorus);
        //        t.AddParameter("@Verse", 2048, songVerse.Verse);
        //        t.AddParameter("@OrderNum", songVerse.OrderNum);

        //        t.ExecuteNonQuery();
        //    }
        //}
        //public static void DeleteSongVerses(int autoNumber)
        //{
        //    using (FBirdTask t = new FBirdTask())
        //    {
        //        t.CommandText = "DELETE FROM [SongVerses] WHERE [AutoNumber] = @AutoNumber";
        //        t.AddParameter("@AutoNumber", autoNumber);
        //        t.ExecuteNonQuery();
        //    }
        //}
        //public static string GetFirstVerse(int autoNumber)
        //{
        //    using (FBirdTask t = new FBirdTask())
        //    {
        //        t.CommandText = "SELECT [Verse] FROM [SongVerses] WHERE [AutoNumber] = @AutoNumber ORDER BY [OrderNum]";
        //        t.AddParameter("@AutoNumber", autoNumber);
        //        t.ExecuteReader();

        //        if (t.DR.Read())
        //            return t.GetString(0);
        //        else
        //            return "";
        //    }
        //}
	}
}
