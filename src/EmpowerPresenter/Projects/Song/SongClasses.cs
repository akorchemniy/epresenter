/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
	public class SongVerse
	{
		public bool IsChorus;
		public string Text;
		public int VerseNumber;
		public SongVerse(bool isChorus, string text, int verseNumber)
		{
			IsChorus = isChorus;
			Text = text;
			VerseNumber = verseNumber;
		}
	}
}
