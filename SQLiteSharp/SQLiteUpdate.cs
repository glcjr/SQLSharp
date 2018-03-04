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
    public class SQLiteUpdate
    {
        private bool Success = false;
        private string CommandPerformed = "";
        private string SQLPerformed = "";

        public SQLiteUpdate(SQLiteConnection sqlcon, SQLUpdateTable table)
        {
            DoUpdateCommand(sqlcon, table);//.GetSQL());
        }
        public SQLiteUpdate(SQLiteConnection sqlcon, string TableName, SQLVarsVals Fields, SQLWhereVars WhereFields)
        {
            SQLUpdateTable table = new SQLUpdateTable(TableName, Fields, WhereFields);
            DoUpdateCommand(sqlcon, table);
        }
        public SQLiteUpdate(SQLiteConnection sqlcon, string TableName, SQLVarVal Field, SQLWhereVar WhereFields)
        {
            SQLUpdateTable table = new SQLUpdateTable(TableName, Field, WhereFields);
            DoUpdateCommand(sqlcon, table);
        }
        public SQLiteUpdate(SQLiteConnection sqlcon, string sqlcommand)
        {
            DoUpdateCommand(sqlcon, sqlcommand);
        }
        private void DoUpdateCommand(SQLiteConnection sqlcon, SQLUpdateTable table)
        {
            CommandPerformed = $"{table.GetSQLWithParameters()}{Environment.NewLine}{table.GetParamList().ToString()}";
            SQLPerformed = table.GetSQL();
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                using (SqliteCommand command = db.CreateCommand())
                {
                    command.CommandText = table.GetSQLWithParameters();
                    List<SqliteParameter> Params = GetCommandParameters(table.GetParamList());
                    if (Params.Count > 0)
                    {
                        foreach (var p in Params)
                        {
                            command.Parameters.Add(p);
                        }
                    }
                    command.ExecuteNonQuery();
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
        }
        private void DoUpdateCommand(SQLiteConnection sqlcon, string commandtext)
        {
            CommandPerformed = commandtext;
            SQLPerformed = commandtext;
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                using (var command = db.CreateCommand())
                {
                    command.CommandText = commandtext;
                    command.ExecuteNonQuery();
                }
                Success = true;
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
