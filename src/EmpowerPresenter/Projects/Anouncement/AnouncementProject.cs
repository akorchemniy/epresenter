/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Drawing2D;

namespace EmpowerPresenter
{
    public class AnouncementProject : IProject, ISupportGfxCtx
    {
        internal AnouncementData data;
        private int imageId = -1;
        GfxContext ctx = new GfxContext();
        internal bool bSaved = false;
        internal bool dirty = false;

        //////////////////////////////////////////////////////////////////////////////
        public AnouncementProject()
        {
            data = new AnouncementData();
            UpdateBackground(Program.ConfigHelper.BibleImage);
        }
        public void LoadData(AnouncementData ad)
        {
            bSaved = true;
            data = ad;
            
            // Update image
            PhotoInfo pi = new PhotoInfo();
            pi.ImageId = imageId;
            ctx.img = pi.FullSizeImage;
            ctx.opacity = ad.opacity;
        }

        public void RefreshUI()
        {
            if (Refresh != null)
                Refresh(null, null);
        }
        public void UpdateBackground(int imageId)
        {
            this.imageId = imageId;
            PhotoInfo pi = new PhotoInfo();
            pi.ImageId = imageId;
            UpdateBackground(pi.FullSizeImage);
        }
        public void UpdateBackground(Image newBackground)
        {
            ctx.img = newBackground;
            RefreshUI();
        }
        public GfxContext GetCurrentGfxContext()
        {
            if (ctx.img == null)
                return null;

            ctx.opacity = data.opacity;
            ctx.destSize = DisplayEngine.NativeResolution.Size;
            ctx.textRegions = this.data.lTextRegions;
            ctx.supportAnimation = false;
            return ctx.Clone();
        }
        public int Opacity
        {
            get { return ctx.opacity; }
            set { if (ctx.opacity != value) { data.opacity = ctx.opacity = value; this.RefreshUI(); } }
        }

        #region IProject Members

        public event EventHandler Refresh;
        public event EventHandler RequestActivate;
        public event EventHandler RequestDeactivate;

        public string GetName()
        {
            return "Anouncement: " + data.ToString();
        }
        public ProjectType GetProjectType()
        {
            return ProjectType.Anouncement;
        }
        public void Activate()
        {
            if (RequestActivate != null)
                RequestActivate(this, null);
        }
        public void CloseProject()
        {
            if (RequestDeactivate != null)
                RequestDeactivate(this, null);
        }
        public Type GetControllerUIType()
        {
            return typeof(AnouncementProjectView);
        }
        public Type GetDisplayerType()
        {
            return typeof(DisplayEngine);
        }
        public bool IsPrimaryDisplay() { return true; }

        #endregion
    }
    
    public class AnouncementData
    {
        public string name = "";
        public int imageId;
        public int opacity = 0;
        public List<GfxTextRegion> lTextRegions = new List<GfxTextRegion>();
        public override string ToString()
        {
            if (name != "")
                return name;
            else
            {
                if (lTextRegions == null)
                    return "Anouncement";
                StringBuilder sb = new StringBuilder();
                foreach (GfxTextRegion tr in lTextRegions)
                {
                    sb.Append(tr.message);
                    sb.Append(" ");
                }
                return sb.ToString();
            }
        }
    }
    public class AnouncementStore
    {
        private string loc;
        private XmlDocument xd = new XmlDocument();

        public AnouncementStore()
        {
            loc = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "content\\anouncements.xml");

            if (File.Exists(loc))
                xd.Load(loc);
            else
                xd.LoadXml("<Anouncements></Anouncements>");
        }
        public void Save()
        {
            xd.Save(loc);
        }

        public Dictionary<string, AnouncementData> GetAnouncements()
        {
            #if DEMO
            return new Dictionary<string, AnouncementData>();
            #else
            Dictionary<string, AnouncementData> anouncements = new Dictionary<string, AnouncementData>();
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
            {
                if (xn.Name == "Anouncement")
                {
                    AnouncementData d = new AnouncementData();
                    d.name = xn.Attributes["Name"].InnerText;
                    d.imageId = int.Parse(xn["ImageId"].InnerText);
                    if (xn["Opacity"] != null)
                        d.opacity = int.Parse(xn["Opacity"].InnerText);

                    // Load all text regions
                    foreach (XmlNode xnc in xn.ChildNodes)
                    {
                        if (xnc.Name == "TextRegion")
                        {
                            GfxTextRegion textRegion = new GfxTextRegion();
                            string[] parts = xnc["Bounds"].InnerText.Split(",".ToCharArray());
                            if (parts.Length != 4)
                                continue;
                            textRegion.bounds = new RectangleF(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                            textRegion.message = xnc["Message"].InnerText;
                            textRegion.font = PresenterFont.FromXMLNode(xnc);

                            d.lTextRegions.Add(textRegion);
                        }
                    }

                    anouncements.Add(d.name, d);
                }
            }
            return anouncements;
            #endif
        }
        public bool NameExists(string name)
        {
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Anouncement" && xn.Attributes["Name"].InnerText == name)
                    return true;
            return false;
        }
        public void SaveAnouncement(AnouncementData ad)
        {
            // Search and remove the previous version
            XmlNode nodeToRemove = null;
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Anouncement" && xn.Attributes["Name"].InnerText == ad.name)
                    nodeToRemove = xn;
            if (nodeToRemove != null)
                xd.DocumentElement.RemoveChild(nodeToRemove);

            // Write the new version
            XmlNode an = xd.CreateElement("Anouncement");
            XmlAttribute xaName = xd.CreateAttribute("Name");
            xaName.InnerText = ad.name;
            an.Attributes.Append(xaName);
            AddStringNode(an, "ImageId", ad.imageId.ToString());
            AddStringNode(an, "Opacity", ad.opacity.ToString());

            // Write out each region
            foreach (GfxTextRegion tr in ad.lTextRegions)
            {
                XmlNode xnTr = xd.CreateElement("TextRegion");
                
                // Save basics
                string bounds = tr.bounds.X.ToString() + "," + tr.bounds.Y.ToString() + "," +
                    tr.bounds.Width.ToString() + "," + tr.bounds.Height.ToString();
                AddStringNode(xnTr, "Bounds", bounds);
                AddStringNode(xnTr, "Message", tr.message);

                // Save the font
                PresenterFont.AppendToXMLNode(xnTr, tr.font);

                an.AppendChild(xnTr);
            }

            // Finish and save
            xd.DocumentElement.AppendChild(an);
            Save();
        }
        public void DeleteAnouncement(string name)
        {
            XmlNode nodeToRemove = null;
            foreach (XmlNode xn in xd.DocumentElement.ChildNodes)
                if (xn.Name == "Anouncement" && xn.Attributes["Name"].InnerText == name)
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
