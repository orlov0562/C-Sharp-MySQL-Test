using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace MySQLTest
{
    class Sql
    {
        private string _host = "localhost";
        private string _port = "3306";
        private string _user = "mysql";
        private string _pass = "mysql";
        private string _dbName = null;

        private bool _testConnectionEstablished = false;

        public bool Connect(string host, int port, string user, string pass, string dbName)
        {
           
            _testConnectionEstablished = false;

            SetConnectionData(host, port, user, pass, dbName);

            string connString = GetConnectionString();
            
            var conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                _testConnectionEstablished = true;
            }
            finally
            {
                conn.Close();
            }

            return _testConnectionEstablished;
        }

        private void SetConnectionData(string host, int port, string user, string pass, string dbName)
        {
            _host = host;
            _port = port.ToString();
            _user = user;
            _pass = pass;
            _dbName = dbName;
        }

        private string GetConnectionString()
        {
            return   "Server="    + _host
                   + ";Port="     + _port
                   + ";Database=" + _dbName
                   + ";Uid="      + _user
                   + ";Password=" + _pass
            ;
        }

        public List<Dictionary<string, string>> Fetch(string query)
        {
            var ret = new List<Dictionary<string, string>>();

            if (!_testConnectionEstablished)
            {
                throw new Exception("Credentials for connection not found, use SQL::connect method first");
            }

            string connString = GetConnectionString();
            var conn = new MySqlConnection(connString);

            try
            {
                conn.Open();

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var dRow = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dRow.Add(reader.GetName(i), reader.GetValue(i).ToString());
                    }
                    ret.Add(dRow);
                }
            }
            finally
            {
                conn.Close();
            }

            return ret;
        }

        public void Query(string query)
        {
            if (!_testConnectionEstablished)
            {
                throw new Exception("Credentials for connection not found, use SQL::connect method first");
            }

            string connString = GetConnectionString();
            var conn = new MySqlConnection(connString);

            try
            {
                conn.Open();

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

        }

        public void PreparedQuery(string preparedQuery, MySqlParameter[] parameters)
        {
            if (!_testConnectionEstablished)
            {
                throw new Exception("Credentials for connection not found, use SQL::connect method first");
            }

            string connString = GetConnectionString();
            var conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = preparedQuery;
                command.Prepare();
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public string Esc(string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate(Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";   
                        case "\b":              // BACKSPACE character
                            return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";
                        case "\t":              // TAB
                            return "\\t";
                        case "\u001A":          // Ctrl-Z
                            return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
        }
    }
}
