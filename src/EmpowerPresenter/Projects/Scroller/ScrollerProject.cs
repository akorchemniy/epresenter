/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public class ScrollerProject : IProject
    {
        internal string name = "";
        private ScrollerPosition pos = ScrollerPosition.Bottom;
        internal PresenterFont font;
        private string message = "";
        private string leftMessage = "";
        private string rightMessage = "";
        internal bool bSaved = false; // UI flag - show the save dlg or no
        internal bool dirty = false;

        ////////////////////////////////////////////////////////////////////////////////////////
        public ScrollerProject()
        {
            font = new PresenterFont();
            font.SizeInPoints = 12;
        }

        public void RefreshUI()
        {
            if (Refresh != null)
                Refresh(null, null);
        }
        public void LoadData(ScrollerData data)
        {
            bSaved = true;
            this.name = data.name;
            this.font = data.font;
            UpdateMessage(data.message, data.leftMessage, data.rightMessage);
        }
        public void UpdateMessage(string newMessage)
        {
            message = newMessage;
            RefreshUI();
        }
        public void UpdateMessage(string newMessage, string leftMessage, string rightMessage)
        {
            this.message = newMessage;
            this.leftMessage = leftMessage;
            this.rightMessage = rightMessage;
            RefreshUI();
        }

        public string Message
        {
            get { return message; }
            set { message = value; RefreshUI(); }
        }
        public string LeftMessage
        {
            get { return leftMessage; }
            set { leftMessage = value; RefreshUI(); }
        }
        public string RightMessage
        {
            get { return rightMessage; }
            set { rightMessage = value; RefreshUI(); }
        }

        #region IProject Members

        public event EventHandler Refresh;
        public event EventHandler RequestActivate;
        public event EventHandler RequestDeactivate;

        public string GetName()
        {
            if (name != "")
                return "Scroller: " + name;
            else
                return "Scroller: " + message;
        }
        public ProjectType GetProjectType()
        {
            return ProjectType.Scroller;
        }
        public void Activate()
        {
            if (RequestActivate != null)
                RequestActivate(this, null);
            RefreshUI();
        }
        public void CloseProject()
        {
            if (RequestDeactivate != null)
                RequestDeactivate(this, null);

            // Close display manually (because we are not exclusive)
            Type ts = typeof(ScrollerDisplay);
            if (Program.Presenter.displayers.ContainsKey(ts))
            {
                ScrollerDisplay sd = (ScrollerDisplay)Program.Presenter.displayers[ts];
                sd.CloseDisplay();
            }
        }
        public Type GetControllerUIType()
        {
            return typeof(ScrollerProjectView);
        }
        public Type GetDisplayerType()
        {
            return typeof(ScrollerDisplay);
        }
        public bool IsPrimaryDisplay(){return false;}

        #endregion
    }
    public enum ScrollerPosition
    {
        Top,Bottom
    }
    public class ScrollerData
    {
        public string name = "";
        public string leftMessage = "";
        public string message = "";
        public string rightMessage = "";
        public PresenterFont font = null;
    }
    public class ScrollerStore
    {
        private string loc;
        private XmlDocument xd = new XmlDocument();

        public ScrollerStore()
        {
            loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "content\\scrollerproj.xml");

            if (File.Exists(loc))
                xd.Load(loc);
            else
                xd.LoadXml("<Projects></Projects>");
        }
        public void Save()
        {
            xd.Save(loc);
        }

        public Dictionary<string, ScrollerData> GetProjectList()
        {
            #if DEMO
            return new Dictionary<string, ScrollerData>();
            #else
            Dictionary<string, ScrollerData> anouncements = new Dictionary<string, ScrollerData>();
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
            {
                if (xn.Name == "Project")
                {
                    ScrollerData d = new ScrollerData();
                    d.name = xn.Attributes["Name"].InnerText;
                    d.leftMessage = xn["LeftMessage"].InnerText;
                    d.message = xn["Message"].InnerText;
                    d.rightMessage = xn["RightMessage"].InnerText;

                    // Get font
                    if (xn["FontName"] == null)
                    {
                        d.font = new PresenterFont();
                        d.font.SizeInPoints = 12;
                    }
                    else
                        d.font = PresenterFont.FromXMLNode(xn);
                    
                    anouncements.Add(d.name, d);
                }
            }
            return anouncements;
            #endif
        }
        public bool NameExists(string name)
        {
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Project" && xn.Attributes["Name"].InnerText == name)
                    return true;
            return false;
        }
        public void SaveProject(ScrollerData sd)
        {
            // Search and remove the previous version
            XmlNode nodeToRemove = null;
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Project" && xn.Attributes["Name"].InnerText == sd.name)
                    nodeToRemove = xn;
            if (nodeToRemove != null)
                xd.DocumentElement.RemoveChild(nodeToRemove);

            // Write the new version
            XmlNode an = xd.CreateElement("Project");
            XmlAttribute xaName = xd.CreateAttribute("Name");
            xaName.InnerText = sd.name;
            an.Attributes.Append(xaName);
            AddStringNode(an, "LeftMessage", sd.leftMessage);
            AddStringNode(an, "Message", sd.message);
            AddStringNode(an, "RightMessage", sd.rightMessage);

            if (sd.font == null)
            {
                sd.font = new PresenterFont();
                sd.font.SizeInPoints = 12;
            }
            PresenterFont.AppendToXMLNode(an, sd.font);

            // Finish and save
            xd.DocumentElement.AppendChild(an);
            Save();
        }
        public void DeleteAnouncement(string name)
        {
            XmlNode nodeToRemove = null;
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Project" && xn.Attributes["Name"].InnerText == name)
                    nodeToRemove = xn;
            if (nodeToRemove != null)
                xd.DocumentElement.RemoveChild(nodeToRemove);
            Save();
        }
        private void AddStringNode(XmlNode xnParent, string name, string value)
        {
            XmlNode xnNew = xnParent.OwnerDocument.CreateElement(name);
            xnNew.InnerText = value;
            xnParent.AppendChild(xnNew);
        }
    }
}
