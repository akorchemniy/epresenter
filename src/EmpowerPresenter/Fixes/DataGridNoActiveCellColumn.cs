/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Windows.Forms;

namespace EmpowerPresenter
{
    public class DataGridNoActiveCellColumn : DataGridTextBoxColumn
    {
        private int SelectedRow = -1;
        protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly,string instantText,bool cellIsVisible)
        {
            //make sure previous selection is valid
            if(SelectedRow > -1 && SelectedRow < source.List.Count + 1)
                this.DataGridTableStyle.DataGrid.UnSelect(SelectedRow);
            SelectedRow = rowNum;
            this.DataGridTableStyle.DataGrid.Select(SelectedRow);
        }
    }
}
