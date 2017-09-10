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
            return ToString(false);
        }
        public string ToString(bool secondaryVersion)
        {
            if (secondaryVersion)
            {
                if (SecondaryVersion == "")
                    SecondaryVersion = Program.ConfigHelper.BibleSecondaryTranslation;
                string secondary = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(SecondaryVersion, SecondaryBook).DisplayBook;
                return secondary + " " + SecondaryChapter + ": " + SecondaryVerse;
            }
            else
            {
                if (RefVersion == "")
                    RefVersion = Program.ConfigHelper.BiblePrimaryTranslation;
                string primary = Program.BibleDS.BibleLookUp.FindByVersionIdMappingBook(RefVersion, RefBook).DisplayBook;
                return primary + " " + RefChapter + ": " + RefVerse;
            }
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
