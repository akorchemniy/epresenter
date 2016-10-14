using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
	public class eSwordBibImporter
	{
		//////////////////////////////////////////////////////
		public eSwordBibImporter()
		{
		}
		
		public List<eSwordBib> LocateBibles()
		{
			// TODO: are there any problems with old version of eSword?
			// Is it possible to locate based on msi data?
			// TODO: fuzzy search
			return null;
		}
		public void UnprotectBible(eSwordBib bib)
		{
			// TODO
		}
		public void Import(eSwordBib bib)
		{
			// TODO:
			// Unprotect
			// Read basic information
			// Create bbl file
			// Copy basic data
			// Create book chap summary
			// Create native index
			// Create entry in main
		}
	}
	public class eSwordBib
	{
		/// TODO:
		/// - Name
		/// - Title
		/// - Location
		/// - Protection status
	}
}
