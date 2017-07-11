/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Drawing;
using System.Reflection;
using System.Collections;

namespace System.Windows.Forms
{
    /// <summary>
    /// Datagrid column that displays text in a multiline, word-wrapped textbox
    /// </summary>
    public class DataGridMultiLineTextBox : DataGridTextBoxColumn
    {
        StringFormat sf;
        public DataGridMultiLineTextBox()
        {
            sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.LineLimit;
        }
        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, 
            int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            // clear the cell
            g.FillRectangle(backBrush, bounds);

            // draw the value
            String s = this.GetColumnValueAtRow(source, rowNum).ToString();
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            r.Inflate(0, -1);

            g.DrawString(s, base.TextBox.Font, foreBrush, r, sf);
        }
    }
}