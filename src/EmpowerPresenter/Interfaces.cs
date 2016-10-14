/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EmpowerPresenter
{
	/// The architecture idea
	/// 1. User select the type of project to start
	/// 2. A new project is created
	/// 3. The new project is queried for the type of controller, a new controller is created, and 
	/// the project is attached to the controller
	/// 4. When requested, a the project is queried for the type of displayer, a new displayer is created, and 
	/// the project is attached to the displayer
	/// 
	/// Everything is a pull model. All components of this applications rely on events to work

	public enum ProjectType
	{
		Bible,Song,PPT,Video,Image,Scroller,Anouncement
	}
	public interface IProject
	{
		event EventHandler Refresh;
		event EventHandler RequestActivate; // This method is handeled by Main and allow memory saving by sharing controllers
		event EventHandler RequestDeactivate; // This method is handled by Main

		string GetName();
		ProjectType GetProjectType();
		void Activate(); // This will prepare data and fire the RequestActivate event
		void CloseProject(); // This will close down and fire the RequestDeactivate event
		Type GetControllerUIType();
		Type GetDisplayerType();
	}
	public interface IController
	{
		void AttachProject(IProject proj);
		void DetachProject();
		void DoEditSettings();
	}
	public interface ISlideShow
	{
		void GoNextSlide();
		void GoPrevSlide();
	}
	public interface IDisplayer
	{
		void AttachProject(IProject proj);
		void DetachProject();
		void ShowDisplay();
		void HideDisplay();
		void CloseDisplay();
		bool ExclusiveDisplay();
		bool ResidentDisplay();
	}
	public interface IDisplayManager
	{
		// Notice that only one displayer can be active at any time

		void RemoveDisplayer(Type t); // Call this functions when the display exits
		void DeactiveDisplayer(); // Call this function when display is no long active
	}
	public interface IPopupManager
	{
		void ShowPopupWindow(IPopup c, Point ptScreen);
		void UnregisterPopupWindow(IPopup c);
	}
	public interface IPopup
	{
		void Deactivate();
	}
	public interface IDragDropClient
	{
		bool DragDropHitTest(Point ptScreen, IDataObject dataObj);
		void OnDragDrop(IDataObject dataObj);
	}
	public interface IKeyClient
	{
		void ProccesKeys(Keys c, bool exOwner);
	}
	public interface ISearchPanel
	{
		void TryPrepare();
		void TryDisplay();
	}
	public interface ISupportGfxCtx
	{
		GfxContext GetCurrentGfxContext();
	}
}