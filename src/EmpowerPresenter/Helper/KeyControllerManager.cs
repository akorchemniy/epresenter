using System;
using System.Collections;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	/// TODO:
	/// - redo this things so that the key controller is in charge of all keys
	/// - key controller knows is attached to project and on key press activates the project

	public interface IKeyController
	{
		void EnableController();
	}
	public class KeyControllerManager
	{
		private IKeyController kc;
		private bool hasControl = false;
		private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
		public event EventHandler ControlLostControl;
		public event EventHandler ControlGotControl;

		public KeyControllerManager()
		{
			t.Interval = 400;
			t.Tick += new EventHandler(t_Tick);
		}
		public void AssignController(IKeyController controller)
		{
			this.kc = controller;
		}
		public void EnableController()
		{
			if (kc != null)
				kc.EnableController();
		}
		public void NotifyControllerLostControl()
		{
			t.Start();
			hasControl = false;
		}
		private void t_Tick(object sender, EventArgs e)
		{
			t.Stop();
			if (ControlLostControl != null && !hasControl)
				ControlLostControl(this, null);
		}
		public void NotifyControllerGotControl()
		{
			hasControl = true;

			t.Stop();

			if (ControlGotControl != null)
				ControlGotControl(this, null);
		}
		public bool ControllerHasControl
		{get{return hasControl;}}
	}
}