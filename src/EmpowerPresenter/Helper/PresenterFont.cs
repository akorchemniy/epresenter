/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Xml;

namespace EmpowerPresenter
{
    public class PresenterFont : ICloneable
    {
        private EmpowerPresenter.FontAlignment fontAlignment;
        internal bool shadow;
        internal bool outline;
        internal bool doubleSpace;
        internal bool bold;
        internal bool italic;
        internal string fontName;
        internal int sizeInPoints;
        internal System.Drawing.Color fontColor;
        internal System.Drawing.Color shadowColor;
        internal System.Drawing.Color outlineColor;

        public PresenterFont()
        {
            fontName = "Verdana";
            sizeInPoints = 32;
            fontAlignment = new FontAlignment(HorizontalAlignment.Center, VerticalAlignment.Middle);
#if InternalOnly
            shadow = false;
            outline = false;
            fontColor = Color.Black;
#else
            shadow = true;
            outline = false;
            fontColor = Color.Transparent;
#endif
            doubleSpace = true;
            shadowColor = Color.White;
            outlineColor = Color.Black;
            bold = false;
            italic = false;
        }
        public EmpowerPresenter.FontAlignment FontAlignment
        {
            get{return fontAlignment;}
            set{fontAlignment = value;}
        }
        public EmpowerPresenter.VerticalAlignment VerticalAlignment
        {
            get{return fontAlignment.Vertical;}
            set{fontAlignment.Vertical = value;}
        }
        public System.Drawing.StringAlignment VerticalStringAlignment
        {
            get
            {
                switch(fontAlignment.Vertical)
                {
                    case VerticalAlignment.Top: return StringAlignment.Near; 
                    case VerticalAlignment.Middle: return StringAlignment.Center; 
                    case VerticalAlignment.Bottom: return StringAlignment.Far; 
                    default: return StringAlignment.Center;
                }
            }
        }
        public System.Windows.Forms.HorizontalAlignment HorizontalAlignment
        {
            get{return fontAlignment.Horizontal;}
            set{fontAlignment.Horizontal = value;}
        }
        public System.Drawing.StringAlignment HorizontalStringAlignment
        {
            get
            {
                switch(fontAlignment.Horizontal)
                {
                    case HorizontalAlignment.Left: return StringAlignment.Near; 
                    case HorizontalAlignment.Center: return StringAlignment.Center; 
                    case HorizontalAlignment.Right: return StringAlignment.Far; 
                    default: return StringAlignment.Center;
                }
            }
        }
        public string FontName
        {
            get{return fontName;}
            set{fontName = value;}
        }
        public System.Drawing.FontFamily FontFamily
        {
            get{
                return new System.Drawing.FontFamily(fontName);
            }
        }
        public int SizeInPoints
        {
            get{return sizeInPoints;}
            set{sizeInPoints = value;}
        }
        public bool Shadow
        {
            get{return shadow;}
            set{shadow = value;}
        }
        public bool Outline
        {
            get{return outline;}
            set{outline = value;}
        }
        public bool DoubleSpace
        {
            get{return doubleSpace;}
            set{doubleSpace = value;}
        }
        public bool Bold
        {
            get{return bold;}
            set{bold = value;}
        }
        public bool Italic
        {
            get{return italic;}
            set{italic = value;}
        }
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                bool bold = this.bold;
                bool italic = this.italic;

                FontFamily fm = new FontFamily("Verdana");

                if (bold && fm.IsStyleAvailable(FontStyle.Bold) == false)
                    bold = false;

                if (italic && fm.IsStyleAvailable(FontStyle.Italic) == false)
                    italic = false;

                if (bold && italic)
                    return FontStyle.Bold | FontStyle.Italic;

                if (bold)
                    return FontStyle.Bold;

                if (italic)
                    return FontStyle.Italic;

                return FontStyle.Regular;
            }
        }
        public System.Drawing.Color Color
        {
            get{return fontColor;}
            set
            {
                fontColor = value;
                if (fontColor.ToArgb() == Color.Transparent.ToArgb()) // HACKED: transparent.ToArgb() != Color.FromArgb(transparent.ToArgb())
                    fontColor = Color.Transparent;
            }
        }
        public System.Drawing.Color OutlineColor
        {
            get{return outlineColor;}
            set{outlineColor = value;}
        }
        public System.Drawing.Color ShadowColor
        {
            get{return shadowColor;}
            set{shadowColor = value;}
        }
        public Font GdiFont
        {
            get
            {
                return new Font(this.fontName, this.sizeInPoints, this.FontStyle);
            }
        }

        // XML support
        public static FontStyle GetFontStyle(FontFamily family, FontStyle original, FontStyle requested)
        {
            if (family.IsStyleAvailable(requested))
                return requested;

            return original;
        }
        public static string ToXML(PresenterFont font)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml("<PresenterFont></PresenterFont>");
            AppendToXMLNode(xd.DocumentElement, font);
            return xd.DocumentElement.OuterXml;
        }
        public static void AppendToXMLNode(XmlNode xnc, PresenterFont font)
        {
            AddStringNode(xnc, "FontName", font.FontName);
            AddStringNode(xnc, "SizeInPoints", font.SizeInPoints.ToString());
            AddStringNode(xnc, "Color", font.Color.ToArgb().ToString());
            AddStringNode(xnc, "VertAlign", font.VerticalAlignment.ToString());
            AddStringNode(xnc, "HorAlign", font.HorizontalAlignment.ToString());
            AddStringNode(xnc, "Outline", font.Outline.ToString());
            AddStringNode(xnc, "Shadow", font.Shadow.ToString());
            AddStringNode(xnc, "OutlineColor", font.OutlineColor.ToArgb().ToString());
            AddStringNode(xnc, "ShadowColor", font.ShadowColor.ToArgb().ToString());
            AddStringNode(xnc, "Italic", font.Italic.ToString());
            AddStringNode(xnc, "Bold", font.Bold.ToString());
            AddStringNode(xnc, "DoubleSpace", font.DoubleSpace.ToString());
        }
        public static PresenterFont FromXML(string xml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xml);
            XmlNode xnc = xd.DocumentElement;

            return FromXMLNode(xnc);
        }
        public static PresenterFont FromXMLNode(XmlNode xnc)
        {
            PresenterFont font = new PresenterFont();
            if (xnc["FontName"] != null)
                font.FontName = xnc["FontName"].InnerText;
            if (xnc["SizeInPoints"] != null)
                font.SizeInPoints = int.Parse(xnc["SizeInPoints"].InnerText);
            if (xnc["Color"] != null)
                font.Color = Color.FromArgb(int.Parse(xnc["Color"].InnerText));
            if (xnc["VertAlign"] != null)
                font.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), xnc["VertAlign"].InnerText);
            if (xnc["HorAlign"] != null)
                font.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), xnc["HorAlign"].InnerText);
            if (xnc["Outline"] != null)
                font.Outline = bool.Parse(xnc["Outline"].InnerText);
            if (xnc["Shadow"] != null)
                font.Shadow = bool.Parse(xnc["Shadow"].InnerText);
            if (xnc["OutlineColor"] != null)
                font.OutlineColor = Color.FromArgb(int.Parse(xnc["OutlineColor"].InnerText));
            if (xnc["ShadowColor"] != null)
                font.ShadowColor = Color.FromArgb(int.Parse(xnc["ShadowColor"].InnerText));
            if (xnc["Italic"] != null)
                font.Italic = bool.Parse(xnc["Italic"].InnerText);
            if (xnc["Bold"] != null)
                font.Bold = bool.Parse(xnc["Bold"].InnerText);
            if (xnc["DoubleSpace"] != null)
                font.DoubleSpace = bool.Parse(xnc["DoubleSpace"].InnerText);

            return font;
        }
        private static void AddStringNode(XmlNode xnParent, string name, string value)
        {
            XmlNode xnNew = xnParent.OwnerDocument.CreateElement(name);
            xnNew.InnerText = value;
            xnParent.AppendChild(xnNew);
        }

        // Database support
        public static PresenterFont GetFontFromDatabase(int id)
        {
            PresenterFont font = new PresenterFont();
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText =
                    "SELECT [FONTNAME], [SIZEINPOINTS], [COLOR], [VERTICALALIGNMENT], [HORIZONTALALIGNMENT], [OUTLINE], [SHADOW], [OUTLINECOLOR], [SHADOWCOLOR], [ITALIC], [BOLD], [DOUBLESPACE] " +
                    "FROM [PPTFONT] " +
                    "WHERE [AUTONUMBER] = @AUTONUMBER";
                t.AddParameter("@AUTONUMBER", id);

                t.ExecuteReader();
                if (t.DR.Read())
                {
                    font.FontName = t.GetString(0);
                    font.SizeInPoints = t.GetInt32(1);
                    font.Color = t.GetColor(2, Color.Black);
                    switch (t.GetInt32(3))
                    {
                        case 0:
                            font.VerticalAlignment = VerticalAlignment.Top;
                            break;
                        case 1:
                            font.VerticalAlignment = VerticalAlignment.Middle;
                            break;
                        case 2:
                            font.VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        default:
                            break;
                    }
                    switch (t.GetInt32(4))
                    {
                        case 0:
                            font.HorizontalAlignment = HorizontalAlignment.Left;
                            break;
                        case 2:
                            font.HorizontalAlignment = HorizontalAlignment.Center;
                            break;
                        case 1:
                            font.HorizontalAlignment = HorizontalAlignment.Right;
                            break;
                        default:
                            break;
                    }
                    font.Outline = t.GetBoolean(5);
                    font.Shadow = t.GetBoolean(6);
                    font.OutlineColor = t.GetColor(7, Color.White);
                    font.ShadowColor = t.GetColor(8, Color.Gray);
                    font.Italic = t.GetBoolean(9);
                    font.Bold = t.GetBoolean(10);
                    font.DoubleSpace = t.GetBoolean(11);
                }

                return font;
            }
        }
        public static void SaveFontToDatabase(int id, PresenterFont font)
        {
            using (FBirdTask t = new FBirdTask())
            {
                // check is font saved
                t.CommandText = "SELECT COUNT(*) FROM [PPTFONT] WHERE [AUTONUMBER] = @AUTONUMBER";
                t.AddParameter("@AUTONUMBER", id);
                if ((int)t.ExecuteScalar() == 0)
                {
                    // insert
                    t.CommandText =
                        "INSERT INTO [PPTFONT] ([AUTONUMBER], [FONTNAME], [SIZEINPOINTS], [COLOR], [VERTICALALIGNMENT], [HORIZONTALALIGNMENT], [OUTLINE], [SHADOW], [OUTLINECOLOR], [SHADOWCOLOR], [ITALIC], [BOLD], [DOUBLESPACE]) " +
                        "VALUES (@AUTONUMBER, @FONTNAME, @SIZEINPOINTS, @COLOR, @VERTICALALIGNMENT, @HORIZONTALALIGNMENT, @OUTLINE, @SHADOW, @OUTLINECOLOR, @SHADOWCOLOR, @ITALIC, @BOLD, @DOUBLESPACE)";
                }
                else
                {
                    // update
                    t.CommandText =
                        "UPDATE [PPTFONT] SET " +
                            "[FONTNAME] = @FONTNAME, " +
                            "[SIZEINPOINTS] = @SIZEINPOINTS," +
                            "[COLOR] = @COLOR," +
                            "[VERTICALALIGNMENT] = @VERTICALALIGNMENT," +
                            "[HORIZONTALALIGNMENT] = @HORIZONTALALIGNMENT," +
                            "[OUTLINE] = @OUTLINE," +
                            "[SHADOW] = @SHADOW," +
                            "[ITALIC] = @ITALIC," +
                            "[BOLD] = @BOLD, " +
                            "[OUTLINECOLOR] = @OUTLINECOLOR, " +
                            "[SHADOWCOLOR] = @SHADOWCOLOR, " +
                            "[DOUBLESPACE] = @DOUBLESPACE " +
                        "WHERE [AUTONUMBER] = @AUTONUMBER";
                }

                // add parameters
                t.AddParameter("@FONTNAME", 512, font.FontName);
                t.AddParameter("@SIZEINPOINTS", font.SizeInPoints);
                t.AddParameter("@COLOR", font.Color.ToArgb());
                t.AddParameter("@VERTICALALIGNMENT", (int)font.VerticalAlignment);
                t.AddParameter("@HORIZONTALALIGNMENT", (int)font.HorizontalAlignment);
                t.AddParameter("@OUTLINE", font.Outline);
                t.AddParameter("@SHADOW", font.Shadow);
                t.AddParameter("@ITALIC", font.Italic);
                t.AddParameter("@BOLD", font.Bold);
                t.AddParameter("@OUTLINECOLOR", font.OutlineColor.ToArgb());
                t.AddParameter("@SHADOWCOLOR", font.ShadowColor.ToArgb());
                t.AddParameter("@DOUBLESPACE", font.DoubleSpace);

                // save to db
                t.ExecuteNonQuery();
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            PresenterFont font = new PresenterFont();
            font.Bold = this.Bold;
            font.Color = this.Color;
            font.Outline = this.Outline;
            font.Shadow = this.Shadow;
            font.OutlineColor = this.OutlineColor;
            font.ShadowColor = this.ShadowColor;

            font.FontAlignment = this.FontAlignment;
            font.FontName = this.FontName;
            font.Italic = this.Italic;
            font.SizeInPoints = this.SizeInPoints;
            return font;
        }

        #endregion
    }
    public struct FontAlignment
    {
        public HorizontalAlignment Horizontal;
        public VerticalAlignment Vertical;
        public FontAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        public override bool Equals(object obj)
        {
            if (obj is FontAlignment)
            {
                FontAlignment fa = (FontAlignment)obj;
                return (this.Horizontal == fa.Horizontal && this.Vertical == fa.Vertical);
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public enum VerticalAlignment
    {
        Top = 0,
        Middle,
        Bottom
    }
}
