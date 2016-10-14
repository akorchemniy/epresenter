/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EmpowerPresenter.Data;
using System.Data.OleDb;
using FirebirdSql.Data.FirebirdClient;

namespace EmpowerPresenter
{
    public partial class SongNotesForm : System.Windows.Forms.Form
    {
        private System.Data.DataView SNdataView;
        private PresenterDataset.SongNotesDataTable dt;        
        private PresenterDataset.SongNotesRow sRow;
        private int SNumber;
       
        private int rowCount; 
        public SongNotesForm(int songNumber){         
            InitializeComponent();            
            LoadSongNote(songNumber);
            SNumber = songNumber;
            Program.Presenter.RegisterExKeyOwnerForm(this);
        }     
        
        private void LoadSongNote(int songNumber){
            dt=new PresenterDataset.SongNotesDataTable();
            SNdataView = new DataView();                              
            this.Controls.Add(SndataGrid);
            BindingSource source = new BindingSource();
            source.DataSource = SNdataView;
            SndataGrid.DataSource = SNdataView;                        
            int SongId=5;
            using (FBirdTask t = new FBirdTask())
            {
                   t.CommandText = "SELECT [SongNotes].[Number], [SongNotes].[Note], [SongNotes].[IdSong] FROM [SongNotes]" +
                        " where [SongNotes].[IdSong]=" + songNumber;
                    t.ExecuteReader();

                    if (t.DR != null)
                    {
                        int rowNumber = 1;
                        while (t.DR.Read())
                        {
                            sRow = dt.NewSongNotesRow();
                            sRow.Number = t.GetInt32(0);// rowNumber;
                            sRow.Note = t.GetString(1);
                            // sRow.SongNumber= t.GetInt32(2);
                            dt.AddSongNotesRow(sRow);
                            rowNumber++;
                        }

                        t.DR.Close();
                        SNdataView.BeginInit();
                        SNdataView.Table = dt;
                        SNdataView.EndInit();
                    }
                    else {
                        sRow = dt.NewSongNotesRow();
                        sRow.Note = "";
                        dt.AddSongNotesRow(sRow);
                    }
                   rowCount=dt.Rows.Count;                  
               
        }            
        }

        private void newNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sRow = dt.NewSongNotesRow();            
            sRow.Note = "";            
            dt.AddSongNotesRow(sRow);
            SndataGrid.ReadOnly = false;
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.save();
            this.Close();   
            
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;           
            this.Close();
        }

        private void save() 
        {
            int rNum = dt.Rows.Count;
            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "INSERT INTO [SongNotes] ([Note],[IdSong]) " +
                        "VALUES (@Note, @IdSong)";
                t.Parameters.Add("@Note", FbDbType.VarChar);
                t.Parameters.Add("@IdSong", FbDbType.Integer);            
                for (int i = rowCount; i < rNum; i++)
                {
                    t.Parameters["@Note"].Value = sRow.Note;
                    t.Parameters["@IdSong"].Value = SNumber;
                    t.ExecuteNonQuery();

                }
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index;
            index = SndataGrid.CurrentCell.RowNumber;
            int rNumber=sRow.Number;


            using (FBirdTask t = new FBirdTask())
            {
                t.CommandText = "DELETE FROM [SongNotes] WHERE [Number] = @NNumber";
                t.AddParameter("@Number", rNumber);

                t.ExecuteNonQuery();
            }
            SNdataView.Delete(index);
        }
    }
}
