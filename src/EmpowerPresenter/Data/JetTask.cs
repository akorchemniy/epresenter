using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;

namespace EmpowerPresenter
{
    public delegate void ExceptionHandler(Exception ex);

    public class JetTask : IDisposable
    {
        private OleDbConnection connection;
        private OleDbCommand command;
        public event ExceptionHandler errorOccured;

        public static string ConnectionString
        {
            get
            {
                string path2db = Path.Combine(Application.StartupPath, "BIBDATA.MDB");

                //				return 
                //					@"Jet OLEDB:Global Partial Bulk Ops=2;"+
                //					@"Jet OLEDB:Registry Path=;"+
                //					@"Jet OLEDB:Database Locking Mode=1;"+
                //					@"Jet OLEDB:Database Password=;"+
                //					@"Data Source=""" + path2db + @""";"+
                //					@"Password=;"+
                //					@"Jet OLEDB:Engine Type=5;"+
                //					@"Jet OLEDB:Global Bulk Transactions=1;"+
                //					@"Provider=""Microsoft.Jet.OLEDB.4.0"";"+
                //					@"Jet OLEDB:System database=;"+
                //					@"Jet OLEDB:SFP=False;"+
                //					@"Extended Properties=;"+
                //					@"Mode=Share Deny None;"+
                //					@"Jet OLEDB:New Database Password=;"+
                //					@"Jet OLEDB:Create System Database=False;"+
                //					@"Jet OLEDB:Don't Copy Locale on Compact=False;"+
                //					@"Jet OLEDB:Compact Without Replica Repair=False;"+
                //					@"User ID=Admin;"+
                //					@"Jet OLEDB:Encrypt Database=False";

                return @"Jet OLEDB:Database Password=;" +
                        @"Data Source=""" + path2db + @""";" +
                        @"Password=;" +
                        @"Jet OLEDB:Engine Type=5;" +
                        @"Provider=""Microsoft.Jet.OLEDB.4.0"";" +
                        @"Jet OLEDB:Database Locking Mode=1;" + // this is row level locking
                        @"Jet OLEDB:New Database Password=;" +
                        @"User ID=Admin";
            }
        }

        public OleDbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new OleDbConnection(ConnectionString);
                return connection;
            }
        }

        public OleDbCommand Command
        {
            get
            {
                if (command == null)
                    command = new OleDbCommand("", Connection);

                return command;
            }
        }

        public OleDbParameterCollection Parameters
        {
            get
            {
                return Command.Parameters;
            }
        }

        public void AddParameter(string name, bool value)
        {
            Command.Parameters.Add(name, OleDbType.Integer);
            Command.Parameters[name].Value = value ? 1 : 0;
        }

        public void AddParameter(string name, int value)
        {
            Command.Parameters.Add(name, OleDbType.Integer);
            Command.Parameters[name].Value = value;
        }

        public void AddParameter(string name, string value)
        {
            Command.Parameters.Add(name, OleDbType.VarWChar, 0);
            Command.Parameters[name].Value = value;
        }

        public void AddParameter(string name, Guid value)
        {
            Command.Parameters.Add(name, OleDbType.Guid, 0);
            Command.Parameters[name].Value = value;
        }

        public void AddParameter(string name, int length, string value)
        {
            // text inside mdb can only be a maximum of 255 chars
            if (length > 255)
                length = 0;

            Command.Parameters.Add(name, OleDbType.VarWChar, length);

            // length 0 means unlimited length
            if (length != 0 && value.Length > length)
                value = value.Substring(0, length);

            Command.Parameters[name].Value = value;
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

        public OleDbDataReader ExecuteReader()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (dataReader != null)
                    dataReader.Close();

                dataReader = this.Command.ExecuteReader();
            }
            catch (OleDbException ex)
            {
                if (errorOccured != null)
                    errorOccured(ex);

                Trace.WriteLine("JetTask.ExecuteReader " + ex.ToString());
            }

            return dataReader;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                return Command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                if (errorOccured != null)
                    errorOccured(ex);

                if (dataReader != null)
                    dataReader.Close();

                Trace.WriteLine("JetTask.ExecuteNonQuery " + ex.ToString());
            }
            return 0;
        }

        public object ExecuteScalar()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (dataReader != null)
                    dataReader.Close();

                return Command.ExecuteScalar();
            }
            catch (OleDbException ex)
            {
                if (errorOccured != null)
                    errorOccured(ex);

                Trace.WriteLine("JetTask.ExecuteScalar ", ex.ToString());
            }
            return null;
        }

        private OleDbDataReader dataReader;
        public OleDbDataReader DR
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

        public Guid GetGuid(int column)
        {
            if (DR.IsDBNull(column))
                return Guid.Empty;
            else
                return DR.GetGuid(column);
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
            else if (DR[column] is int)
                return DR.GetInt32(column) == 1 ? true : false;
            else
                return DR.GetBoolean(column);
        }

        public System.Drawing.Color GetColor(int column, System.Drawing.Color color)
        {
            if (DR.IsDBNull(column))
                return color;
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
                    command = null;

                if (errorOccured != null)
                    errorOccured = null;
            }
            catch (Exception ex)
            { Trace.WriteLine("JetTask.Dispose " + ex.ToString()); }
        }

        #endregion
    }
}