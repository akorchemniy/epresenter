/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace EmpowerPresenter
{
    public class SearchHelper
    {
        public static List<string> BreakSearchTerms(string str)
        {
            // Remove punctuation
            string p = ",./<>?[]\\{}|!@#$%^&*()-=_+:;'\"";
            foreach (char c in p)
                str = str.Replace(c, ' ');

            // Split to parts
            string[] parts = str.Split(" ".ToCharArray());

            // Rebuilt list without empty strings
            List<string> searchTerms = new List<string>();
            foreach (string s in parts)
                if (s.Trim().Length != 0)
                    searchTerms.Add(s);

            return searchTerms;
        }
        public static int GetScore(string a, List<string> searchTerms)
        {
            return InternalGetNodeCost(a.ToLower(), 0, 0, 0, searchTerms);
        }
        private static int InternalGetNodeCost(string a, int costSoFar, int searchTermIx, int startIndex, List<string> searchTerms)
        {
            #if !DEMO
            /// Function summary
            /// - This function calculates the score of a match by measuring the distance between occurances of the 
            /// search terms. This algorithm is a post-order tree traversal. It starts with the first occurance
            /// of the first search terms and calculates the cost of the arc to to the occurances to the next search 
            /// term recursively. The cost of each node on the tree is the equal to its distance to the parent +
            /// the lowest child arc cost. Therefore the score of the search result is based on the lowest cost treespan. 

            /// For tree branches that end prematurely there is a penalty. There is also a penalty for partial matches
            /// with suffixes and prefixes

            int ixOfNext = -1;
            int ci = startIndex;
            int minCostChild = int.MaxValue;

            // Check if we finished the search normally
            if (searchTerms.Count == searchTermIx)
                return costSoFar;

            // Get next search word
            string nextSearchTerm = searchTerms[searchTermIx].ToLower();

            // Check if we ended prematurely
            if (a.IndexOf(nextSearchTerm, startIndex) == -1)
                return costSoFar + 500 * (searchTerms.Count - searchTermIx);

            // Loop through all the children
            while (true)
            {
                ixOfNext = a.IndexOf(nextSearchTerm, ci);
                if (ixOfNext == -1) // Done searching return lowest cost
                    return minCostChild;
                ci = ixOfNext + 1; // adjust next start
                int costToChild = ixOfNext - startIndex;
                if (searchTermIx == 0) // No cost for first node
                    costToChild = 0;

                // Check partial match and add penalty
                if (ixOfNext > 0 && char.IsLetter(a[ixOfNext - 1]))
                    costToChild += 35; // higher penalty if there is a prefix
                if (ixOfNext + nextSearchTerm.Length < a.Length 
                    && char.IsLetter(a[ixOfNext + nextSearchTerm.Length]))
                    costToChild += 10; // lower penalty if there is a suffix

                // Check the brance
                int childCost = InternalGetNodeCost(a, costSoFar + costToChild, searchTermIx + 1, ci, searchTerms);
                if (childCost < minCostChild)
                    minCostChild = childCost;
            }
            #else
            return 0;
            #endif
        }
        public static CharacterRange[] FindCharacterRanges(string source, List<string> searchTerms)
        {
            List<CharacterRange> ranges = new List<CharacterRange>();
            foreach (string s in searchTerms)
            {
                int ix = -1;
                do
                {
                    ix = source.ToLower().IndexOf(s.ToLower(), ix + 1);
                    if (ix != -1 && ranges.Count < 32)
                        ranges.Add(new CharacterRange(ix, s.Length));
                } while (ix != -1 && ranges.Count < 32);

                if (ranges.Count >= 32)
                    break;
            }
            return ranges.ToArray();
        }
    }
#if !DEMO
    public class BibleSearchHelper
    {
        public static List<BibleSearchResult> Search(List<string> searchTerms)
        {
            if (searchTerms.Count < 1)
                throw (new ApplicationException("No search terms"));

            // Double check sql
            string p = ",./<>?[]\\{}|!@#$%^&*()-=_+:;'\"";
            foreach (string str in searchTerms)
                if (str.IndexOfAny(p.ToCharArray()) != -1)
                    throw (new ApplicationException("Malformed sql passed to Search"));

            // Build the query
            string query = "SELECT \"VERSION\", \"REFBOOK\", \"REFCHAPTER\", \"REFVERSE\", \"DATA\", \"BOOK\", \"CHAPTER\", \"VERSE\" FROM \"BIBLEVERSES\" WHERE ";
            foreach(string s in searchTerms)
                query += "(\"DATA\" LIKE '%" + s + "%') AND ";
            query = query.Remove(query.Length - 4);

            // Query the database
            List<BibleSearchResult> lResults = new List<BibleSearchResult>();
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = query;
                t.ExecuteReader();
                int cutoff = 250;

                while (t.DR.Read() && cutoff > 0)
                {
                    cutoff--;

                    BibleSearchResult r = new BibleSearchResult();
                    BibleVerse bv = new BibleVerse();
                    bv.RefVersion = t.GetString(0);
                    bv.RefBook = t.GetString(1);
                    bv.RefChapter = t.GetInt32(2);
                    bv.RefVerse = t.GetInt32(3);
                    bv.Text = t.GetString(4);
                    bv.SecondaryBook = t.GetString(5);
                    bv.SecondaryChapter = t.GetInt32(6);
                    bv.SecondaryVerse = t.GetInt32(7);
                    r.bibVerse = bv;
                    r.searchResult = bv.Text;
                    lResults.Add(r);
                }
            }

            return lResults;
        }
        internal static void InternalDebugPrintVerses(List<BibleSearchResult> lResults)
        {
            foreach (BibleSearchResult r in lResults)
            {
                BibleVerse bv = r.bibVerse;
                System.Diagnostics.Trace.WriteLine(r.score + " " + bv.RefBook + " " + bv.RefChapter + ", " + bv.RefVerse + ": " + bv.Text);
            }
        }
        public static void ScoreResults(List<BibleSearchResult> results, List<string> searchTerms)
        {
            foreach (BibleSearchResult r in results)
                r.score = SearchHelper.GetScore(r.searchResult, searchTerms);
        }
    }
    public class SongSearchHelper
    {
        public static List<SongSearchResult> Search(List<string> searchTerms)
        {
            if (searchTerms.Count < 1)
                throw (new ApplicationException("No search terms"));

            // Double check sql
            string p = ",./<>?[]\\{}|!@#$%^&*()-=_+:;'\"";
            foreach (string str in searchTerms)
                if (str.IndexOfAny(p.ToCharArray()) != -1)
                    throw (new ApplicationException("Malformed sql passed to Search"));

            // Build the query
            string query = "SELECT \"SongVerses\".\"AutoNumber\", \"Songs\".\"Number\", \"SongVerses\".\"Verse\", \"SongVerses\".\"OrderNum\", \"SongVerses\".\"IsChorus\" " +
                "FROM \"SongVerses\" INNER JOIN \"Songs\" ON \"SongVerses\".\"AutoNumber\" = \"Songs\".\"AutoNumber\" WHERE ";
            foreach (string s in searchTerms)
                query += "(\"Verse\" LIKE '%" + s + "%') AND ";
            query = query.Remove(query.Length - 4);
            query += "ORDER BY \"Number\"";
            
            // Query the database
            List<SongSearchResult> lResult = new List<SongSearchResult>();
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = query;
                t.ExecuteReader();
                int cutoff = 100;
                while (t.DR.Read() && cutoff > 0)
                {
                    cutoff--;
                    SongSearchResult ssr = new SongSearchResult();
                    ssr.autoNumber = t.GetInt32(0);
                    ssr.songNumber = t.GetInt32(1);
                    ssr.verseData = t.GetString(2);
                    ssr.isAtStart = t.GetInt32(3) == 0;
                    ssr.isChorus = t.GetBoolean(4);
                    ssr.verseData = SongProject.RemoveVerseFormatting(ssr.verseData);
                    ssr.searchResult = ssr.verseData;
                    lResult.Add(ssr);
                }
            }

            return lResult;
        }
        public static void ScoreResults(List<SongSearchResult> results, List<string> searchTerms)
        {
            foreach (SongSearchResult r in results)
                r.score = SearchHelper.GetScore(r.searchResult, searchTerms);

            // Add small penalty for non-chorus, to boost chorus matches
            foreach (SongSearchResult r in results)
                if (!r.isChorus)
                    r.score += 3; // very small penalty
        }
    }

    internal class SearchRelevancyComparer : IComparer<SongSearchResult>, IComparer<BibleSearchResult>
    {
        public int Compare(SongSearchResult x, SongSearchResult y)
        {
            return x.score.CompareTo(y.score);
        }
        public int Compare(BibleSearchResult x, BibleSearchResult y)
        {
            if (x.score == y.score)
                return new BibleVerseComparer().Compare(x, y);
            else
                return x.score.CompareTo(y.score);
        }
    }
    internal class BibleVerseComparer : IComparer<BibleSearchResult>
    {
        public int Compare(BibleSearchResult a, BibleSearchResult b)
        {
            BibleVerse x = a.bibVerse;
            BibleVerse y = b.bibVerse;
            int ixa = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(x.RefVersion, x.RefBook).OrderNum;
            int ixb = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(x.RefVersion, y.RefBook).OrderNum;
            if (ixa == ixb)
            {
                if (x.RefChapter == y.RefChapter)
                {
                    return x.RefVerse.CompareTo(y.RefVerse);
                }
                else
                    return x.RefChapter.CompareTo(y.RefChapter);
            }
            else
                return ixa.CompareTo(ixb);
        }
    }
    internal class SongNumberComparer : IComparer<SongSearchResult>
    {
        public int Compare(SongSearchResult x, SongSearchResult y)
        {
            return x.songNumber.CompareTo(y.songNumber);
        }
    }
#endif

    public class BibleSearchResult
    {
        public BibleVerse bibVerse;
        public int score = -1;
        public string searchResult = "";

        public override string ToString()
        {
            return bibVerse.ToString() + " " + bibVerse.Text;
        }
    }
    public class SongSearchResult
    {
        public int autoNumber = 0;
        public int songNumber = 0;
        public string verseData = "";
        public bool isAtStart = false;
        public bool isChorus = false;
        public int score = -1;
        public string searchResult = "";
    }
}
