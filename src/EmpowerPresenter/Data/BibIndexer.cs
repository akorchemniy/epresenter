using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace EmpowerPresenter
{
	/// <summary>
	/// This class has a very inefficient implementation
	/// </summary>
	public class BibIndexer
	{
		internal static Dictionary<int, string> reverseBook;
		internal static Dictionary<string, int> wordOffsetTable;
		internal static MemoryStream msIndex;

		public BibIndexer()
		{
		}
		public void IndexAllVersions()
		{
			/////////////////////////////////////////////////////////////////
			// Go through the bible and build a forward index
			Dictionary<string, List<int>> forwardIndex = new Dictionary<string, List<int>>();
			string badchars = "',.?!:;`-\"()[]";
			using (PerformanceTimer pt = new PerformanceTimer("Analyze"))
			using (FBirdTask t = new FBirdTask())
			{
				t.CommandText = "SELECT \"BibleLookUp\".\"OrderNum\", BIBLEVERSES.REFCHAPTER, BIBLEVERSES.REFVERSE, BIBLEVERSES.DATA, Version " +
					"FROM BIBLEVERSES " +
					"INNER JOIN \"BibleLookUp\" ON (BIBLEVERSES.REFBOOK=\"BibleLookUp\".\"RefBook\") " +
					"WHERE (Version = 'KJV')";
				using (PerformanceTimer pt2 = new PerformanceTimer("Analyze - Initial exec"))
					t.ExecuteReader();

				using (PerformanceTimer pt2 = new PerformanceTimer("Analyze - Data read"))
					while (t.DR.Read())
					{
						string s = t.DR.GetString(3).ToLower(); // get raw
						foreach (char c in badchars) // strip bad chars
							s = s.Replace(c.ToString(), " ");
						string[] words = s.Split(" ".ToCharArray());

						// Count
						List<string> localWords = new List<string>();
						foreach (string word in words)
						{
							if (word.Length > 0)
							{
								// Check local
								if (!localWords.Contains(word))
								{
									localWords.Add(word);

									// Ensure list
									if (!forwardIndex.ContainsKey(word))
										forwardIndex[word] = new List<int>();

									int z = ((byte)t.DR.GetInt16(2))
										+ ((byte)t.DR.GetInt16(1) << 8)
										+ ((byte)t.DR.GetInt16(0) << 16);

									// Adjust for version
									if (t.DR.GetString(4) == "RST")
										z += ((byte)1 << 24);
									else
										z += ((byte)2 << 24);

									forwardIndex[word].Add(z);
								}
							}
						}
					}
			}

			/////////////////////////////////////////////////////////////////
			// Sort words
			List<string> sortedWords = new List<string>();
			foreach (string s in forwardIndex.Keys)
				sortedWords.Add(s);
			sortedWords.Sort();

			/////////////////////////////////////////////////////////////////
			// Write indexs to file
			Dictionary<string, int> wordIndex = new Dictionary<string,int>();
			string pthIndex = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bibndx.bin");
			FileStream fs = new FileStream(pthIndex, FileMode.Create, FileAccess.Write);
			foreach (string key in sortedWords)
			{
				// Store the current word offset into word map
				wordIndex.Add(key, (int)fs.Position);

				// Write the count of results
				byte[] bInt = BitConverter.GetBytes((int)forwardIndex[key].Count);
				fs.Write(bInt, 0, 4);

				// Write out all the result pointers
				foreach (int i in forwardIndex[key])
				{
					bInt = BitConverter.GetBytes(i);
					fs.Write(bInt, 0, 4);
				}
			}
			fs.Flush();
			fs.Close();

			/////////////////////////////////////////////////////////////////
			// Write word list to file
			string pthWords = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bibwds.bin");
			FileStream fs2 = new FileStream(pthWords, FileMode.Create, FileAccess.Write, FileShare.None);
			foreach (string s in wordIndex.Keys)
			{
				byte[] bCurWord = UnicodeEncoding.Unicode.GetBytes(s);
				fs2.WriteByte((byte)bCurWord.Length);
				fs2.Write(bCurWord, 0, bCurWord.Length);
				byte[] bInt = BitConverter.GetBytes((int)wordIndex[s]);
				fs2.Write(bInt, 0, 4);
			}
			fs2.Flush();
			fs2.Close();
		}
		public List<int> QueryWordFrags(string[] wordFrags)
		{
			/// For each word, expand word list
			/// For each word, build a primary list by using all the words in expanded list

			List<int> primarys = new List<int>();
			Dictionary<string, List<int>> fragPrimaries = new Dictionary<string, List<int>>();

			// Ensure init
			if (wordOffsetTable == null)
				InitWordOffsetTable();
			
			// Match all the partial words
			Dictionary<string, List<string>> expandedWords = new Dictionary<string, List<string>>(); // original, expanded
			foreach (string wordFrag in wordFrags)
			{
				foreach (string s in wordOffsetTable.Keys)
				{
					if (!expandedWords.ContainsKey(wordFrag))
						expandedWords.Add(wordFrag, new List<string>());
					if (s.Contains(wordFrag) && !expandedWords[wordFrag].Contains(s))
						expandedWords[wordFrag].Add(s);
				}
			}

			// Load file into memory
			if (msIndex == null)
			{
				string pthIndex = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bibndx.bin");
				FileStream fs = new FileStream(pthIndex, FileMode.Open, FileAccess.Read);
				byte[] bytes = new byte[fs.Length];
				fs.Read(bytes, 0, (int)fs.Length);
				fs.Close();
				msIndex = new MemoryStream(bytes);
			}

			// Get a list of keys
			byte[] keyCount = new byte[4];
			using (PerformanceTimer pt = new PerformanceTimer("Main"))
			foreach (string word in expandedWords.Keys)
			{
				if (!fragPrimaries.ContainsKey(word))
					fragPrimaries.Add(word, new List<int>());

				foreach (string exWord in expandedWords[word])
				{
					msIndex.Position = wordOffsetTable[exWord]; // Seek to position

					// Get count of results
					msIndex.Read(keyCount, 0, 4);
					int count = BitConverter.ToInt32(keyCount, 0);

					// Loop through all the kyes
					byte[] bytes = new byte[4];
					for (int i = 0; i < count; i++)
					{
						msIndex.Read(bytes, 0, 4);
						int primary = BitConverter.ToInt32(bytes, 0);
						if (!fragPrimaries[word].Contains(primary))
							fragPrimaries[word].Add(primary);
					}
				}
			}
			

			// Filter to only overlapping
			bool first = true;
			using (PerformanceTimer pt = new PerformanceTimer("Filter"))
			foreach (List<int> li in fragPrimaries.Values)
			{
				List<int> j = new List<int>();
				foreach(int i in li)
				{
					if (first || primarys.Contains(i))
						j.Add(i);
				}
				primarys = j;
				first = false;
			}

			return primarys;
		}
		private void InitWordOffsetTable()
		{
			if (wordOffsetTable != null)
				return;

			wordOffsetTable = new Dictionary<string, int>();
			string pthWords = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "bibwds.bin");
			FileStream fs2 = new FileStream(pthWords, FileMode.Open, FileAccess.Read);
			byte[] bOffset = new byte[4];
			while(fs2.Position < fs2.Length)
			{
				int wordLen = fs2.ReadByte();
				byte[] word = new byte[wordLen];
				fs2.Read(word, 0, wordLen);

				fs2.Read(bOffset, 0, 4);
				int offset = BitConverter.ToInt32(bOffset, 0);

				wordOffsetTable.Add(UnicodeEncoding.Unicode.GetString(word), offset);
			}
			fs2.Close();
		}
		public string BookFromId(int id)
		{
			if (reverseBook == null)
			{
				reverseBook = new Dictionary<int, string>();
				using (FBirdTask t = new FBirdTask())
				{
					t.CommandText = "SELECT \"OrderNum\", \"RefBook\" FROM \"BibleLookUp\"";
					t.ExecuteReader();
					while (t.DR.Read())
						if (!reverseBook.ContainsKey(t.DR.GetInt32(0)))
							reverseBook.Add(t.DR.GetInt32(0), t.DR.GetString(1));
				}
			}

			return reverseBook[id];
		}
		public List<BibleVerse> VersesFromIds(List<int> ids)
		{
			List<BibleVerse> bl = new List<BibleVerse>();

			// Convert to BibleVerse
			foreach (int ikey in ids)
			{
				byte[] bytes = BitConverter.GetBytes(ikey);

				BibleVerse bv = new BibleVerse();
				bv.RefVersion = bytes[3] == 1 ? "RST" : "KJV";
				bv.RefBook = BookFromId((int)bytes[2]);
				bv.RefChapter = bytes[1];
				bv.RefVerse = bytes[0];
				bl.Add(bv);
			}

			return bl;
		}
	}
}
