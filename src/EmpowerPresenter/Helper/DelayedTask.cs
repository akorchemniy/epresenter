/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public class DelayedTask
    {
        private Timer t;
        private EventHandler ehFinished;
        public DelayedTask(int interval, EventHandler ehFinished)
        {
            this.ehFinished = ehFinished;
            t = new Timer();
            t.Interval = interval;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }
        void t_Tick(object sender, EventArgs e)
        {
            t.Stop();
            if (this.ehFinished != null)
                ehFinished(null, null);
        }
    }
}
