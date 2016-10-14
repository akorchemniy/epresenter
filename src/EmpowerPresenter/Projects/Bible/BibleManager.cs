/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
	public class BibleManager
	{
		//////////////////////////////////////////////////////////////////////////////
		public BibleManager()
		{
		}
		
		public List<ePresenterBible> ListInstalled()
		{
			// TODO
			return null;
		}
		public void Remove(ePresenterBible bib)
		{
			// TODO
		}
	}
	public class ePresenterBible
	{
		public Guid ID;
		public string name;
		public string title;
		internal string location;

		//////////////////////////////////////////////////////////////////////////////
		public ePresenterBible()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
