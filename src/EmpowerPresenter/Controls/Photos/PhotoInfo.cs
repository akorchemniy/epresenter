/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Generic;

namespace EmpowerPresenter
{
    public class PhotoInfo
    {
        private Image preview;
        private int imgId;

        public static int AddImage(Image largeImage)
        {
            #region Scan for next image name
            int nextid = 0;
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\previews");
            if (!di.Exists)
                di.Create();
            foreach(FileInfo fi in di.GetFiles("*.w"))
            {
                int num = int.Parse(Path.GetFileNameWithoutExtension(fi.FullName)) + 1;
                string newpath = di.FullName + "\\" + num + ".w";
                if (!File.Exists(newpath))
                {
                    nextid = num;
                    break;
                }
            }
            if (nextid == 0)nextid=1;
            #endregion

            #region Generate 800x600 image

            // Get paths
            string path1 = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\fullsize\\" + nextid + ".w";
            string path1temp = Path.GetTempFileName();

            // Save temp copy
            Image i = new Bitmap(800, 600);
            Graphics g = Graphics.FromImage(i);
            g.DrawImage(largeImage, new Rectangle(0,0,800,600), 0,0,largeImage.Width,largeImage.Height, GraphicsUnit.Pixel);
            i.Save(path1, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Perform XOR
            System.IO.FileStream fs1 = System.IO.File.OpenRead(path1temp);
            byte[] b1 = new byte[fs1.Length];
            fs1.Read(b1, 0, b1.Length);
            fs1.Close();
            for(int j = 0; j < b1.Length; j++)
            {b1[j] = (byte)(b1[j]^0xff);}
            File.Delete(path1temp);

            // Save XOR copy
            FileStream fsw1 = new FileStream(path1, FileMode.CreateNew);
            fsw1.Write(b1, 0, b1.Length);
            fsw1.Flush();
            fsw1.Close();

            #endregion

            #region Generate 211x158 image

            // Get paths
            string path2 = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\previews\\" + nextid + ".w";
            string path2temp = Path.GetTempFileName();

            // Save temp copy
            Image i2 = new Bitmap(211, 158);
            Graphics g2 = Graphics.FromImage(i2);
            g2.DrawImage(largeImage, new Rectangle(0,0,211,158), 0,0,largeImage.Width,largeImage.Height, GraphicsUnit.Pixel);
            i2.Save(path2, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Perform XOR
            System.IO.FileStream fs2 = System.IO.File.OpenRead(path2temp);
            byte[] b2 = new byte[fs2.Length];
            fs2.Read(b2, 0, b2.Length);
            fs2.Close();
            for(int j = 0; j < b2.Length; j++)
            {b2[j] = (byte)(b2[j]^0xff);}
            File.Delete(path2temp);

            // Save XOR copy
            FileStream fsw2 = new FileStream(path2, FileMode.CreateNew);
            fsw2.Write(b2, 0, b2.Length);
            fsw2.Flush();
            fsw2.Close();

            #endregion

            return nextid;
        }
        public static void RemoveImage(int id)
        {
            #region Remove the 800x600 image
            string path1 = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\fullsize\\" + id + ".w";
            if (File.Exists(path1))
            {
                try
                {
                    File.Delete(path1);
                }catch(Exception ex){System.Diagnostics.Trace.WriteLine(ex.ToString());}
            }
            #endregion

            #region Remove the thumbnail
            string path2 = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\previews\\" + id + ".w";
            if (File.Exists(path2))
            {
                try
                {
                    File.Delete(path2);
                }catch(Exception ex){System.Diagnostics.Trace.WriteLine(ex.ToString());}
            }
            #endregion

            #region Remove from the categories
            string pathXML = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(pathXML))
                return;

            XmlDocument xd = new XmlDocument();
            xd.Load(pathXML);

            bool dirty = false;
            foreach(XmlNode xn in xd.DocumentElement.ChildNodes)
            {
                if (xn.Name == "Category")
                {
                    foreach(XmlNode xn1 in xn.ChildNodes)
                    {
                        if (xn1.Attributes["id"] != null && int.Parse(xn1.Attributes["id"].InnerText) == id)
                        {
                            xn.RemoveChild(xn1);
                            dirty = true;
                        }
                    }
                }
            }
            if (dirty)
                xd.Save(pathXML);
            #endregion

            MyCache.Instance.Remove(id);
        }
        public static bool AddCategory(string cat)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return false;

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Check if it already exists
            foreach(XmlNode xn in xd.DocumentElement.ChildNodes)
            {
                if (xn.Attributes["name"] != null && xn.Attributes["name"].InnerText == cat)
                    return false;
            }

            // Create the cat element
            XmlElement xe = xd.CreateElement("Category");
            XmlAttribute xa = xd.CreateAttribute("name");
            xa.Value = cat;
            xe.Attributes.Append(xa);
            xd.DocumentElement.AppendChild(xe);
            xd.Save(path);
            return true;
        }
        public static bool RemoveCategory(string cat)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return false;

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Scan for
            foreach(XmlNode xn in xd.DocumentElement.ChildNodes)
            {
                if (xn.Attributes["name"] != null && xn.Attributes["name"].InnerText == cat)
                {
                    xd.DocumentElement.RemoveChild(xn);
                    xd.Save(path);
                    return true;
                }
            }

            return false;
        }
        public static bool AddImage2Cat(int id, string cat)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return false;

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Checks
            XmlNode catnode = null;
            foreach(XmlNode x in xd.DocumentElement.ChildNodes)
            {
                if (x.Name == "Category"  && x.Attributes["name"] != null && x.Attributes["name"].InnerText == cat)
                {
                    catnode = x;
                    foreach(XmlNode xn in x.ChildNodes)
                    {
                        if (xn.Attributes["id"] != null && int.Parse(xn.Attributes["id"].InnerText) == id)
                            return false;
                    }
                    break;
                }
            }

            // Add
            XmlElement xe = xd.CreateElement("Image");
            XmlAttribute xa = xd.CreateAttribute("id");
            xa.InnerText = id.ToString();
            xe.Attributes.Append(xa);
            catnode.AppendChild(xe);
            xd.Save(path);
            return true;
        }
        public static bool RemoveImageFromCat(int id, string cat)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return false;

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Checks
            XmlNode catnode = null;
            foreach(XmlNode x in xd.DocumentElement.ChildNodes)
            {
                if (x.Name == "Category"  && x.Attributes["name"] != null && x.Attributes["name"].InnerText == cat)
                {
                    catnode = x;
                    foreach(XmlNode xn in x.ChildNodes)
                    {
                        if (xn.Attributes["id"] != null && int.Parse(xn.Attributes["id"].InnerText) == id)
                        {
                            catnode.RemoveChild(xn);
                            xd.Save(path);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void RenameCategory(string cat, string newname)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return;

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            XmlNode catnode = null;
            foreach(XmlNode x in xd.DocumentElement.ChildNodes)
            {
                if (x.Name == "Category"  && x.Attributes["name"] != null && x.Attributes["name"].InnerText == cat)
                {
                    catnode = x;
                    break;
                }
            }
            if (catnode != null)
            {
                catnode.Attributes["name"].InnerXml = newname;
                xd.Save(path);
            }
        }
        public static String[] GetCatList()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return new string[0];

            ArrayList al = new ArrayList();
            al.Add("All");

            // Read categories
            SortedList sl = new SortedList(); // Sort the rest of the items
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            foreach(XmlNode x in xd.DocumentElement.ChildNodes)
            {
                if (x.Name == "Category"  && x.Attributes["name"] != null)
                {
                    string name = x.Attributes["name"].InnerXml;
                    if (name != "All")
                        sl.Add(name, name);
                }
            }

            // put them back into the arraylist
            foreach(string key in sl.Keys)
                al.Add(key);
            return (string[])al.ToArray(typeof(string));
        }
        public static Dictionary<int, int> GetPhotoFrequency()
        {
            Dictionary<int, int> f = new Dictionary<int, int>();
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return null;

            // Load document
            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Load frequency node
            XmlNode fr = xd.DocumentElement["Frequency"];
            if (fr == null)
            {
                fr = xd.CreateElement("Frequency");
                xd.DocumentElement.AppendChild(fr);
            }

            // Get all the image frequencies
            foreach (XmlNode x in fr.ChildNodes)
            {
                if (x.Name == "Image")
                {
                    int id = int.Parse(x.Attributes["id"].InnerText);
                    int freq = int.Parse(x.Attributes["frequency"].InnerText);
                    f.Add(id, freq); // If there is a dupe then something went wrong (not our fault)
                }
            }

            return f;
        }
        public static void LogPhotoHit(int id)
        {
            Dictionary<int, int> f = new Dictionary<int, int>();
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return;

            // Load document
            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            // Load frequency node
            XmlNode fr = xd.DocumentElement["Frequency"];
            if (fr == null)
            {
                fr = xd.CreateElement("Frequency");
                xd.DocumentElement.AppendChild(fr);
            }

            // Load the node of photo & update the data
            bool found = false;
            foreach (XmlNode x in fr.ChildNodes)
            {
                if (x.Name == "Image" && int.Parse(x.Attributes["id"].InnerText) == id)
                {
                    found = true;
                    int newf = int.Parse(x.Attributes["frequency"].InnerText) + 1;
                    x.Attributes["frequency"].InnerText = newf.ToString();
                }
            }
            if (!found)
            {
                XmlNode photo = xd.CreateElement("Image");
                XmlAttribute xaid = xd.CreateAttribute("id");
                xaid.InnerText = id.ToString();
                photo.Attributes.Append(xaid);
                XmlAttribute xaf = xd.CreateAttribute("frequency");
                xaf.InnerText = "1";
                photo.Attributes.Append(xaf);
                fr.AppendChild(photo);
            }

            xd.Save(path);
        }

        public static Hashtable GetCatListById(int id)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\PhotoCategories.xml";
            if (!File.Exists(path))
                return null;

            Hashtable h = new Hashtable();
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            foreach(XmlNode x in xd.DocumentElement.ChildNodes)
            {
                bool found = false;
                if (x.Name == "Category"  && x.Attributes["name"] != null)
                {
                    string catname = x.Attributes["name"].InnerXml;
                    foreach(XmlNode xn in x.ChildNodes)
                    {
                        if (xn.Attributes["id"] != null && int.Parse(xn.Attributes["id"].InnerText) == id)
                        {
                            found = true;
                            break;
                        }
                    }
                    h.Add(catname, found);
                }
            }
            return h;
        }

        public PhotoInfo(){}

        public void Clear()
        {
            MyCache.Instance.Remove(imgId); // force a remove
            if (preview != null)
                preview.Dispose();
        }
        public Image Preview
        {
            get
            {
                if (preview == null)
                    preview = ImageSelection.GetPreviewImage(imgId);
                return preview;
            }
            set
            {
                preview = value;
            }
        }
        public int ImageId
        {
            get{return imgId;}
            set
            {
                imgId = value;
            }
        }
        public Image FullSizeImage
        {
            get
            {
                if(!MyCache.Instance.ContainsKey(imgId))
                {
                    string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\fullsize\\" + imgId + ".w";
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                            byte[] bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, (int)fs.Length);
                            fs.Close();

                            // Previously used for Obfuscation
                            // Perform XOR
                            for (int j = 0; j < bytes.Length; j++)
                            { bytes[j] = (byte)(bytes[j] ^ 0xff); }

                            MemoryStream ms = new MemoryStream(bytes);
                            Image img = Image.FromStream(ms);
                            ms.Close();
                            return img;
                        }
                        catch(Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine("Failed to load image: " + ex.ToString());
                            return Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\default.full");
                        }
                    }
                    else
                        return Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\default.full");
                }
                
                ImageKeepAlive ret = ((ImageKeepAlive)MyCache.Instance[imgId]);
                return ret.i;
            }
        }

        //[System.Runtime.InteropServices.DllImport("Helper.dll", EntryPoint="GetBitmap")]
        //private static extern IntPtr GetBitmap(string path, ref int length);
    }

    /// <summary>
    /// This class is a hack to fix the way images are handled when they are created from a stream
    /// </summary>
    internal class ImageKeepAlive : IDisposable
    {
        internal System.IO.MemoryStream ms;
        internal Image i;

        internal ImageKeepAlive(System.IO.MemoryStream ms, Image i)
        {
            this.ms = ms;
            this.i = i;
        }
        #region IDisposable Members

        public void Dispose()
        {
            if (i != null)
            {
                i.Dispose();
            }
            if (ms != null)
            {
                ms.Close();
            }
        }

        #endregion
    }

    internal class MyCache
    {
        private static volatile MyCache instance = null;
        private static object syncRoot = new object();

        private Hashtable timestamps;
        private Hashtable lifetimes;
        private Hashtable objects;
        private Timer t;

        private MyCache()
        {
            timestamps = new Hashtable();
            lifetimes = new Hashtable();
            objects = new Hashtable();
            t = new Timer();
            t.Interval = 10 * 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }
        public static MyCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                            instance = new MyCache();
                    }
                }
                return instance;
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            ArrayList trashlist = new ArrayList();
            foreach(object key in objects.Keys) // Run a clean up
            {
                System.Diagnostics.Trace.Assert(timestamps.Contains(key), "object does not have a timestamp!");
                System.Diagnostics.Trace.Assert(lifetimes.Contains(key), "object does not have a lifetime!");

                TimeSpan difference = DateTime.Now - (DateTime)timestamps[key];
                if ((double)lifetimes[key] < difference.TotalMilliseconds) // lifetime expired
                {
                    trashlist.Add(key); // hashtable cannot be modified while indexing
                }
            }
            foreach(object key in trashlist)
            {
                lock(syncRoot) // make sure we get everthing removed before someone tries to acccess something
                {
                    try
                    {
                        if (objects[key] is IDisposable)
                            ((IDisposable)objects[key]).Dispose();
                    }
                    finally
                    {
                        objects.Remove(key);
                        lifetimes.Remove(key);
                        timestamps.Remove(key);

                        //System.Diagnostics.Trace.WriteLine("Removing dead object from cache: " + key.ToString());
                    }
                }
            }
            trashlist.Clear();
        }
        public bool ContainsKey(object key)
        {
            return objects.ContainsKey(key);
        }
        public object this[object key]
        {
            get
            {
                if (objects.ContainsKey(key))
                {
                    timestamps[key] = DateTime.Now; // update the last access time
                    return objects[key];
                }
                else
                    return null;
            }
        }
        /// <summary>
        /// Adds an object to the cache. Object will be garbage collected after give milliseconds. Give 10 seconds.
        /// </summary>
        public void Add(object key, object val, double lifetimeMilliseconds)
        {
            if (key == null || val == null || lifetimeMilliseconds < 10000)
                throw new ArgumentNullException("All arguments are required. lifetime must be at least 10 seconds");
            objects.Add(key, val);
            lifetimes.Add(key, lifetimeMilliseconds);
            timestamps.Add(key, DateTime.Now);

            //System.Diagnostics.Trace.WriteLine("Adding to cache: " + key.ToString());
        }
        /// <summary>
        /// Adds an object to the cache. Object will be garbage collected after 30 seconds. Give 10 seconds.
        /// </summary>
        public void Add(object key, object val)
        {
            Add(key, val, 30000);
        }
        /// <summary>
        /// Removes if the object exists. Calls dispose if appropriate
        /// </summary>
        public void Remove(object key)
        {
            if (objects.ContainsKey(key))
            {
                lock(syncRoot) // make sure we get everthing removed before someone tries to acccess something
                {
                    try
                    {
                        if (objects[key] is IDisposable)
                            ((IDisposable)objects[key]).Dispose();
                    }
                    finally
                    {
                        objects.Remove(key);
                        lifetimes.Remove(key);
                        timestamps.Remove(key);

                        //System.Diagnostics.Trace.WriteLine("Removing object (forced): " + key.ToString());
                    }
                }
            }
        }
    }
}