/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;

namespace System.Windows.Forms
{
    /// <summary>
    /// Summary description for DataGridWithEnter.
    /// </summary>
    public delegate void ProcessKeyPreviewMsgEventHandler(object sender, ref System.Windows.Forms.Message e);

    public class DataGridWithEnter : DataGrid
    {
        public DataGridWithEnter()
        {
        }
        public const int WM_KEYDOWN = 256;
        public const int WM_KEYUP = 257;

        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;
            if((m.Msg == WM_KEYDOWN) && (keyCode == Keys.Return || keyCode == Keys.Enter))
            {
                onKeyEnter(ref m);
                return false;
            }

            return base.ProcessKeyPreview(ref m);
        }

        public void ColumnStartedEditingExt(System.Drawing.Rectangle r)
        {
            this.ColumnStartedEditing(r);
        }

        public event ProcessKeyPreviewMsgEventHandler KeyEnter;
        public void onKeyEnter(ref System.Windows.Forms.Message m)
        {
            if (KeyEnter != null)
                KeyEnter(this, ref m);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter ||e.KeyCode == Keys.Return)
                return;

            base.OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
                return;

            base.OnKeyPress(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter ||e.KeyCode == Keys.Return)
                return;

            base.OnKeyUp(e);
        }
    }
}
