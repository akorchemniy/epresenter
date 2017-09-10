/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpowerPresenter
{
    public class BibleVerse
    {
        public string RefVersion = "";
        public string RefBook = "";
        public int RefChapter = -1;
        public int RefVerse = -1;
        public string Text = "";

        public string SecondaryVersion = "";
        public string SecondaryBook = "";
        public int SecondaryChapter = -1;
        public int SecondaryVerse = -1;
        public string SecondaryText = "";

        // PENDING - refactor to avoid code repetition
        public string TertiaryVersion = "";
        public string TertiaryBook = "";
        public int TertiaryChapter = -1;
        public int TertiaryVerse = -1;
        public string TertiaryText = "";

        public override string ToString()
        {
            return ReferenceForTranslation(1);
        }
        public string ReferenceForTranslation(int transNum)
        {
            // PENDING - refactor to avoid duplication
            if (transNum == 1)
            {
                if (RefVersion == "")
                    RefVersion = Program.ConfigHelper.BiblePrimaryTranslation;
                string primary = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(RefVersion, RefBook).DisplayBook;
                return primary + " " + RefChapter + ": " + RefVerse;
            }
            if (transNum == 2)
            {
                if (SecondaryVersion == "")
                    SecondaryVersion = Program.ConfigHelper.BibleSecondaryTranslation;
                string secondary = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(SecondaryVersion, SecondaryBook).DisplayBook;
                return secondary + " " + SecondaryChapter + ": " + SecondaryVerse;
            }
            if (transNum == 3)
            {
                if (TertiaryVersion == "")
                    TertiaryVersion = Program.ConfigHelper.BibleTertiaryTranslation;
                string tertiary = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(TertiaryVersion, TertiaryBook).DisplayBook;
                return tertiary + " " + TertiaryChapter + ": " + TertiaryVerse;
            }
            return "";
        }
        public List<string> ReferenceList()
        {
            List<string> ret = new List<string>();
            ret.Add(ReferenceForTranslation(1));
            if (!string.IsNullOrEmpty(SecondaryVersion))
                ret.Add(ReferenceForTranslation(2));
            if (!string.IsNullOrEmpty(TertiaryVersion))
                ret.Add(ReferenceForTranslation(3));
            return ret;
        }
    }
    public enum BibleRenderingFormat
    {
        SingleVerse,
        DoubleVerse,
        MultiTranslation,
        FontFit,
        FontFitMultiTranslation
    }
}
