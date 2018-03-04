using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using SQLSharp;
/*********************************************************************************************************************************
Copyright and Licensing Message

This code is copyright 2018 Gary Cole Jr. 

This code is licensed by Gary Cole to others under the GPLv.3 https://opensource.org/licenses/GPL-3.0 
If you find the code useful or just feel generous a donation is appreciated.

Donate with this link: paypal.me/GColeJr
Please choose Friends and Family

Alternative Licensing Options

If you prefer to license under the LGPL for a project, https://opensource.org/licenses/LGPL-3.0
Single Developers working on their own project can do so with a donation of $20 or more. 
Small and medium companies can do so with a donation of $50 or more. 
Corporations can do so with a donation of $1000 or more.


If you prefer to license under the MS-RL for a project, https://opensource.org/licenses/MS-RL
Single Developers working on their own project can do so with a donation of $40 or more. 
Small and medium companies can do so with a donation of $100 or more.
Corporations can do so with a donation of $2000 or more.


if you prefer to license under the MS-PL for a project, https://opensource.org/licenses/MS-PL
Single Developers working on their own project can do so with a donation of $1000 or more. 
Small and medium companies can do so with a donation of $2000 or more.
Corporations can do so with a donation of $10000 or more.


If you use the code in more than one project, a separate license is required for each project.


Any modifications to this code must retain this message. 
*************************************************************************************************************************************/
namespace SQLiteSharp
{
    public class SQLiteDelete
    {
        private bool Success = false;
        private string CommandPerformed = "";
        private string SQLPerformed = "";

        /// <summary>
        /// Constructor that takes the database connection and the delete table that contains the information for the command 
        /// </summary>
        /// <param name="sqlcon"></param>
        /// <param name="table"></param>
        public SQLiteDelete(SQLiteConnection sqlcon, SQLDeleteTable table)
        {
            DoDeleteCommand(sqlcon, table);
        }
        /// <summary>
        /// Constructor with the database connection and a string that contains the SQL statement that needs to be done.
        /// </summary>
        /// <param name="sqlcon"></param>
        /// <param name="command"></param>
        public SQLiteDelete(SQLiteConnection sqlcon, string command)
        {
            DoDeleteCommand(sqlcon, command);
        }
        /// <summary>
        /// Constructor that takes the table name, there where values that are to be used delete the matching records. It can also take a bool value to delete everything
        /// </summary>
        /// <param name="sqlcon"></param>
        /// <param name="TableName"></param>
        /// <param name="WhereFields"></param>
        /// <param name="deleteallrecords"></param>
        public SQLiteDelete(SQLiteConnection sqlcon, string TableName, SQLWhereVars WhereFields, bool deleteallrecords = false)
        {
            SQLDeleteTable table = new SQLDeleteTable(TableName, WhereFields, deleteallrecords);
            DoDeleteCommand(sqlcon, table);
        }
        private void DoDeleteCommand(SQLiteConnection sqlcon, string commandtext)
        {
            CommandPerformed = commandtext;
            SQLPerformed = commandtext;
            try
            {
                if (commandtext != "")
                {
                    SqliteConnection db = sqlcon.GetConnection();
                    using (var command = db.CreateCommand())
                    {
                        command.CommandText = commandtext;
                        command.ExecuteNonQuery();
                    }
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
        }
        private void DoDeleteCommand(SQLiteConnection sqlcon, SQLDeleteTable table)
        {
            CommandPerformed = $"{table.GetSqlWithParameters()}{Environment.NewLine}{table.GetParamList().ToString()}{Environment.NewLine}";
            SQLPerformed = table.GetSql();
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                using (SqliteCommand command = db.CreateCommand())
                {
                    command.CommandText = table.GetSqlWithParameters();
                    List<SqliteParameter> Params = GetCommandParameters(table.GetParamList());
                    if (Params.Count > 0)
                    {
                        foreach (var p in Params)
                        {
                            command.Parameters.Add(p);
                        }
                    }
                    command.ExecuteNonQuery();
                    Success = true;
                }
            }
            catch
            {
                Success = false;
            }
        }
        private List<SqliteParameter> GetCommandParameters(SQLParamList Params)
        {
            List<SqliteParameter> param = new List<SqliteParameter>();
            for (int index = 0; index < Params.Count(); index++)
                param.Add(new SqliteParameter(Params.GetParameter(index), Params.GetValue(index)));
            return param;
        }
        public bool GetSuccess()
        {
            return Success;
        }
        public string GetCommmandPerformed()
        {
            return CommandPerformed;
        }
        public string GetSQLPerfomed()
        {
            return SQLPerformed;
        }
    }
}
