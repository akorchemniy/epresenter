/* ePresenter is licensed under the GPLV3 -- see the 'COPYING' file details.
   Copyright (C) 2006 Alex Korchemniy */
using System;
using System.Windows.Forms;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.IO;

namespace EmpowerPresenter
{
    public class Firebird
    {
        public static string ConnectionString
        {
            get
            {
                string path2db = Path.GetDirectoryName(Application.ExecutablePath) + "\\content\\BIBDATA.FDB";
                return "ServerType=1;Database=" + path2db + ";User=SYSDBA;Password=masterkey;Charset=WIN1251";
            }
        }
    }

    public class FBirdTask : IDisposable
    {
        private FbConnection connection;
        private FbCommand command;

        public FBirdTask()
        {
        }

        public FbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new FbConnection(Firebird.ConnectionString);
                return connection;
            }
            set
            {
                if (value != null)
                    connection = value;
            }
        }
        public FbCommand Command
        {
            get
            {
                if (command == null)
                    command = new FbCommand("", Connection);

                return command;
            }
        }
        public FbParameterCollection Parameters
        {
            get
            {
                return Command.Parameters;
            }
        }
        public string CommandText
        {
            get
            {
                return this.Command.CommandText;
            }
            set
            {
                this.Command.CommandText = value;
            }
        }

        // Parameters
        public void AddParameter(string name, bool value)
        {
            Command.Parameters.Add(name, FbDbType.Integer);
            Command.Parameters[name].Value = value ? 1 : 0;
        }
        public void AddParameter(string name, int value)
        {
            Command.Parameters.Add(name, FbDbType.Integer);
            Command.Parameters[name].Value = value;
        }
        public void AddParameter(string name, int length, string value)
        {
            Command.Parameters.Add(name, FbDbType.VarChar, length);
            if (value.Length > length)
                value = value.Substring(0, length);
            Command.Parameters[name].Value = value;
        }

        // Execution
        public FbDataReader ExecuteReader()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                // TODO: Remove this hack later
                this.Command.CommandText = this.Command.CommandText.Replace("[", "\"").Replace("]", "\"");

                dataReader = this.Command.ExecuteReader();
            }
            catch(FbException ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            return dataReader;
        }
        public int ExecuteNonQuery()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                // TODO: Remove this hack later
                this.Command.CommandText = this.Command.CommandText.Replace("[", "\"").Replace("]", "\"");

                return Command.ExecuteNonQuery();
            }
            catch(FbException ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            return 0;
        }
        public object ExecuteScalar()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                // TODO: Remove this hack later
                this.Command.CommandText = this.Command.CommandText.Replace("[", "\"").Replace("]", "\"");

                return Command.ExecuteScalar();
            }
            catch(FbException ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            return null;
        }
        private FbDataReader dataReader;
        public FbDataReader DR
        {
            get
            {
                return dataReader;
            }
        }

        public string GetString(int column)
        {
            if (DR.IsDBNull(column))
                return "";
            else
                return DR.GetValue(column).ToString();
        }
        public int GetInt32(int column)
        {
            if (DR.IsDBNull(column))
                return 0;
            else
                return DR.GetInt32(column);
        }
        public bool GetBoolean(int column)
        {
            if (DR.IsDBNull(column))
                return false;
            else
                return DR.GetInt32(column) == 1;
        }
        public System.Drawing.Color GetColor(int column, System.Drawing.Color defaultColor)
        {
            if (DR.IsDBNull(column))
                return defaultColor;
            else
                return System.Drawing.Color.FromArgb(DR.GetInt32(column));
        }

        #region IDisposable Members
        public void Dispose()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }

                if (command != null)
                    command.Dispose();
            }
            catch{System.Diagnostics.Trace.WriteLine("Dispose error in FBirdTask");}
        }
        #endregion
    }
}