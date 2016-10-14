/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
	internal class PerformanceTimer : IDisposable
	{
		private DateTime t1 = DateTime.Now;
		private string name = "";
		public PerformanceTimer(string name) { this.name = name; }
		public void Dispose()
		{
			DateTime t2 = DateTime.Now;
			TimeSpan ts = t2 - t1;
			System.Diagnostics.Trace.WriteLine(name + " completed in seconds: " + ts.TotalSeconds);
		}
	}
}
