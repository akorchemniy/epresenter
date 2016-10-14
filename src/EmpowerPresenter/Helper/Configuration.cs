/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Configuration;
using System.Windows.Forms;
using System.Management;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Security.Policy;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;

namespace EmpowerPresenter
{
    //public class Configuration
    //{
    //    private XmlSerializer xmlSerializer;
    //    public event EventHandler ConfigFileChanged;

    //    public Configuration()
    //    {
    //        // Ensure proper directories are created
    //        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    //        string vendisoft = Path.Combine(appData, "Vendisoft");
    //        if (!Directory.Exists(vendisoft))
    //            Directory.CreateDirectory(vendisoft);
    //        string epresenter = Path.Combine(vendisoft, "ePresenter 2.0");
    //        if (!Directory.Exists(epresenter))
    //            Directory.CreateDirectory(epresenter);

    //        // Ensure configuration file exists
    //        string currentUsrCfg = Path.Combine(epresenter, "appsettings.config");
    //        if (!File.Exists(currentUsrCfg))
    //            File.Copy(Application.StartupPath + "\\appSettings.config", currentUsrCfg);

    //        // Load configuration file
    //        xmlSerializer = new XmlSerializer(currentUsrCfg);
    //    }
    //    public string GetSetting(string settingPath)
    //    {
    //        return xmlSerializer.GetSetting(settingPath);
    //    }
    //    public void Save()
    //    {
    //        xmlSerializer.Save();
    //        if (ConfigFileChanged != null && !Program.exiting)
    //            ConfigFileChanged(this, null);
    //    }
    //    public void SetSetting(string settingPath, string value)
    //    {
    //        xmlSerializer.SetSetting(settingPath, value);
    //    }
    //    public void SetSetting(string settingPath, object value)
    //    {
    //        xmlSerializer.SetSetting(settingPath, value.ToString());
    //    }
    //}
    public class ConfigHelper
    {
        public event EventHandler BibleFormatChanged;
        public event EventHandler SongDefaultsChanged;

        private void ResetPresentationMonitor()
        {
            if (Screen.AllScreens.Length > 1) 
                Properties.Settings.Default.PresentationMonitor = 1;
            else
                Properties.Settings.Default.PresentationMonitor = 1;

            //Properties.Settings.Default.Save();
        }

        public int BibleImage
        {
            get
            {
                return Properties.Settings.Default.BibleImage;
            }
            set
            {
                Properties.Settings.Default.BibleImage = value;
                Properties.Settings.Default.Save();
            }
        }
        public int BibleImageOpacity
        {
            get
            {
                return Properties.Settings.Default.BibleImageOpacity;
            }
            set
            {
                Properties.Settings.Default.BibleImageOpacity = value;
                Properties.Settings.Default.Save();
            }
        }
        public int SongDefaultImage
        {
            get
            {
                return Properties.Settings.Default.SongDefaultImage;
            }
            set
            {
                Properties.Settings.Default.SongDefaultImage = value;
                Properties.Settings.Default.Save();
            }
        }
        public int SongDefaultOpacity
        {
            get
            {
                return Properties.Settings.Default.SongDefaultOpacity;
            }
            set
            {
                Properties.Settings.Default.SongDefaultOpacity = value;
                Properties.Settings.Default.Save();
            }
        }
        public string CurrentLanguage
        {
            get
            {
                return Properties.Settings.Default.CurrentLanguage;
            }
            set
            {
                Properties.Settings.Default.CurrentLanguage = value;
                Properties.Settings.Default.Save();
            }
        }
        public string SongDefaultFormat
        {
            get
            {
                return Properties.Settings.Default.SongDefaultFormat;
            }
            set
            {
                Properties.Settings.Default.SongDefaultFormat = value;
                Properties.Settings.Default.Save();
            }
        }
        public int BibleNumVerses
        {
            get
            {
                return Properties.Settings.Default.BibleNumVerses;
            }
            set
            {
                if (BibleNumVerses != value)
                {
                    Properties.Settings.Default.BibleNumVerses = value;
                    Properties.Settings.Default.Save();

                    if (BibleFormatChanged != null)
                        BibleFormatChanged(null, null);
                }
            }
        }
        public int PresentationMonitor
        {
            get
            {
                int ret = 1;
                try
                { ret = Properties.Settings.Default.PresentationMonitor; }
                catch { ret = 1; }

                // Bounds check
                if (ret > (Screen.AllScreens.Length - 1))
                {
                    System.Diagnostics.Trace.WriteLine("Monitor configuration bad");
                    ret = Screen.AllScreens.Length - 1;
                }

                return ret;
            }
            set
            {
                Properties.Settings.Default.PresentationMonitor = value;
                Properties.Settings.Default.Save();
            }
        }
        public string BiblePrimaryTranslation
        {
            get
            {
                return Properties.Settings.Default.BiblePrimaryTranslation;
            }
            set
            {
                if (BiblePrimaryTranslation != value)
                {
                    Properties.Settings.Default.BiblePrimaryTranslation = value;
                    Properties.Settings.Default.Save();

                    if (BibleFormatChanged != null)
                        BibleFormatChanged(null, null);
                }
            }
        }
        public string BibleSecondaryTranslation
        {
            get
            {
                return Properties.Settings.Default.BibleSecondaryTranslation;
            }
            set
            {
                if (BibleSecondaryTranslation != value)
                {
                    Properties.Settings.Default.BibleSecondaryTranslation = value;
                    Properties.Settings.Default.Save();

                    if (BibleFormatChanged != null)
                        BibleFormatChanged(null, null);
                }
            }
        }
        public bool UseBlackBackdrop
        {
            get
            {
                return Properties.Settings.Default.UseBlackBackdrop;
            }
            set
            {
                Properties.Settings.Default.UseBlackBackdrop = value;
                Properties.Settings.Default.Save();
            }
        }
        public bool HideContentOnMin
        {
            get
            {
                return Properties.Settings.Default.HideContentOnMin;
            }
            set
            {
                Properties.Settings.Default.HideContentOnMin = value;
                Properties.Settings.Default.Save();
            }
        }
        public void NotifySongDefaultsChanged()
        {
            if (SongDefaultsChanged != null)
                SongDefaultsChanged(null, null);
        }
    }
    /* Previously used for obfuscation
    public sealed class Base32
    {
        private static string ValidChars = "QAZ2WSX3EDC4RFV5TGB6YHN7UJM8K9LP"; // the valid chars for the encoding

        public static string ToBase32String(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();         // holds the base32 chars
            byte index;
            int hi = 5;
            int currentByte = 0;

            while (currentByte < bytes.Length)
            {
                // do we need to use the next byte?
                if (hi > 8)
                {
                    // get the last piece from the current byte, shift it to the right
                    // and increment the byte counter
                    index = (byte)(bytes[currentByte++] >> (hi - 5));
                    if (currentByte != bytes.Length)
                    {
                        // if we are not at the end, get the first piece from
                        // the next byte, clear it and shift it to the left
                        index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
                    }

                    hi -= 3;
                }
                else if (hi == 8)
                {
                    index = (byte)(bytes[currentByte++] >> 3);
                    hi -= 3;
                }
                else
                {

                    // simply get the stuff from the current byte
                    index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
                    hi += 5;
                }

                sb.Append(ValidChars[index]);
            }

            return sb.ToString();
        }
        public static byte[] FromBase32Str2Bytes(string str)
        {
            int numBytes = str.Length * 5 / 8;
            byte[] bytes = new Byte[numBytes];

            // all UPPERCASE chars
            str = str.ToUpper();

            int bit_buffer;
            int currentCharIndex;
            int bits_in_buffer;

            if (str.Length < 3)
            {
                bytes[0] = (byte)(ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
                return bytes;
            }

            bit_buffer = (ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
            bits_in_buffer = 10;
            currentCharIndex = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)bit_buffer;
                bit_buffer >>= 8;
                bits_in_buffer -= 8;
                while (bits_in_buffer < 8 && currentCharIndex < str.Length)
                {
                    bit_buffer |= ValidChars.IndexOf(str[currentCharIndex++]) << bits_in_buffer;
                    bits_in_buffer += 5;
                }
            }

            return bytes;
        }
        public static string ToBase32String(string s)
        {
            return ToBase32String(ASCIIEncoding.ASCII.GetBytes(s));
        }
        public static string FromBase32Str2Str(string s)
        {
            return ASCIIEncoding.ASCII.GetString(FromBase32Str2Bytes(s));
        }
    }*/
}
