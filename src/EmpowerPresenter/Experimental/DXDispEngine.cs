/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
    public class DXDispEngine : IDisplayer
    {
        /////////////////////////////////////////////////////////////////////////////
        public DXDispEngine()
        {
        }

        private void RefreshDisplay()
        {

        }

        #region IDisplayer Members

        private IProject currentProj;

        public void AttachProject(IProject proj)
        {
            DetachProject();        // Detach other if attached
            currentProj = proj;
            currentProj.Refresh += new EventHandler(currentProj_Refresh);
        }
        public void DetachProject()
        {
            if (currentProj != null)
            {
                currentProj.Refresh -= new EventHandler(currentProj_Refresh);
                currentProj = null;
            }
        }
        public void ShowDisplay()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void HideDisplay()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void CloseDisplay()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public bool ExclusiveDisplay()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public bool ResidentDisplay()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void currentProj_Refresh(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        #endregion
    }
}
