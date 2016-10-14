/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Policy;
using System.Collections.Generic;

namespace EmpowerPresenter
{
    /*
	public class XmlSerializer
	{
		private Dictionary<string, string> cache = new Dictionary<string, string>();
		private XmlDocument xd = new XmlDocument();
		private string file;
		public string root;
		public string prez;

		public XmlSerializer(string file)
		{
			this.file = file;
			if (File.Exists(file))
			{
				xd.Load(file);
			}
			else
			{
				xd.LoadXml("<EmpowerPresenter></EmpowerPresenter>");
			}
		}
		public void Save()
		{
			xd.Save(file);
		}

		public void SetSetting(string settingPath, string val)
		{
			cache[settingPath] = val;

			// Update the XML
			string[] path = settingPath.Split(".".ToCharArray());
			XmlElement current;
			//string im = "X" + Base32.ToBase32String(GetDecoded(path[0]));
            string im = path[0];
			if (xd.DocumentElement[im] != null)
			{
				current = xd.DocumentElement[im];
			}
			else
			{
				current = xd.CreateElement(im);
				xd.DocumentElement.AppendChild(current);
			}
			for(int x = 1; x<path.Length; x++)
			{
				//string im2 = "X" + Base32.ToBase32String(GetDecoded(path[x]));
                string im2 = path[x];
				if (current[im2] != null)
				{
					current = current[path[x]];
				}
				else
				{
					current.AppendChild(xd.CreateElement(im2));
					current = current[im2];
				}
			}
			current.InnerText = val;
			return;
		}
		public string GetSetting(string settingPath)
		{
			if (cache.ContainsKey(settingPath))
				return cache[settingPath];

			try
			{
				string[] path = settingPath.Split(".".ToCharArray());
				//string im = "X" + Base32.ToBase32String(GetDecoded(path[0]));
                string im = path[0];
				XmlElement current = xd.DocumentElement[im];
                if (current == null)
                    return "";

				for(int x = 1; x<path.Length; x++)
				{
					//string im2 = "X" + Base32.ToBase32String(GetDecoded(path[x]));
                    string im2 = path[x];
					current = current[im2];
                    if (current == null)
                        return "";
				}

				// cache the result
				if (!cache.ContainsKey(settingPath))
					cache[settingPath] = current.InnerText;

				return current.InnerText;
			}catch{	return "";}
		}
		public XmlDocument GetDocument
		{
			get
			{
				return xd;
			}
		}

		public static string GetDecoded(string source)
		{
			string s = "";
			Assembly assembly = Assembly.GetExecutingAssembly();
			Evidence evidence = assembly.Evidence;
			IEnumerator enumerator = evidence.GetHostEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetType() == typeof(System.Security.Policy.StrongName))
				{
					System.Security.Policy.StrongName strongName = (System.Security.Policy.StrongName)enumerator.Current;
					string hexString = strongName.PublicKey.ToString();

					#region Decode the hex string
					byte[] buffer1;
					if (hexString == null)
						throw new ArgumentNullException("hexString");
					bool flag1 = false;
					int num1 = 0;
					int num2 = hexString.Length;
					if (((num2 >= 2) && (hexString[0] == '0')) && ((hexString[1] == 'x') || (hexString[1] == 'X')))
					{
						num2 = hexString.Length - 2;
						num1 = 2;
					}
					if (((num2 % 2) != 0) && ((num2 % 3) != 2))
						throw new ArgumentException("Argument_InvalidHexFormat");
					if ((num2 >= 3) && (hexString[num1 + 2] == ' '))
					{
						flag1 = true;
						buffer1 = new byte[(num2 / 3) + 1];
					}
					else
						buffer1 = new byte[num2 / 2];
					for (int num5 = 0; num1 < hexString.Length; num5++)
					{
						int num4 = ConvertHexDigit(hexString[num1]);
						int num3 = ConvertHexDigit(hexString[num1 + 1]);
						buffer1[num5] = (byte)(num3 | (num4 << 4));
						if (flag1)
							num1++;
						num1 += 2;
					}
					#endregion

					if (source.Length > buffer1.Length)
						return source;

					// XOR "encrypt"
					for (int i = 0; i < source.Length; i++)
					{
						int k = (buffer1[i] << 24) + (buffer1[i] << 16) + (buffer1[i] << 8) + (buffer1[i]);
						s += (char)((int)source[i] ^ k);
					}
				}
			}
			return s;
		}
		public static int ConvertHexDigit(char val)
		{
			if ((val <= '9') && (val >= '0'))
				return (val - '0');
			if ((val >= 'a') && (val <= 'f'))
				return ((val - 'a') + '\n');
			if ((val < 'A') || (val > 'F'))
				throw new ArgumentException("ArgumentOutOfRange_Index");
			return ((val - 'A') + '\n');
		}
	}*/
}
