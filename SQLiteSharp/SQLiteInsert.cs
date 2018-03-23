using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public class SQLiteInsert
    {
        private bool Success = false;
        private string CommandPerformed = "";
        private string SQLPerformed = "";
        public SQLiteInsert(SQLiteConnection sqlcon, SQLInsertTable table)
        {
            DoInsertCommand(sqlcon, table);
        }
        public SQLiteInsert(SQLiteConnection sqlcon, SQLInsertTableMultiValues table)
        {
            DoInsertCommand(sqlcon, table);//.GetSqlwithParameters(), table.GetVariables());
        }
        public SQLiteInsert(SQLiteConnection sqlcon, string TableName, SQLVarsVals Fields)
        {
            SQLInsertTable table = new SQLInsertTable(TableName, Fields);
            DoInsertCommand(sqlcon, table);
        }
        public SQLiteInsert(SQLiteConnection sqlcon, string TableName, SQLVars Fields, List<SQLVars> Values)
        {
            SQLInsertTableMultiValues table = new SQLInsertTableMultiValues(TableName, Fields, Values);
            DoInsertCommand(sqlcon, table);//.GetSql());
        }
        public SQLiteInsert(SQLiteConnection sqlcon, string sqlcommand)
        {
            DoInsertCommand(sqlcon, sqlcommand);
        }
        private void DoInsertCommand(SQLiteConnection sqlcon, string commandtext)
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
        private void DoInsertCommand(SQLiteConnection sqlcon, SQLInsertTable table)
        {
            CommandPerformed = $"{table.GetSqlWithParameters()}{Environment.NewLine}{table.GetParamList().ToString()}";
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
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
        }        
        private void DoInsertCommand(SQLiteConnection sqlcon, SQLInsertTableMultiValues table)
        {
            SQLPerformed = table.GetSql();            
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                foreach (var vars in table.GetParams())
                {
                    using (var command = db.CreateCommand())
                    {
                        command.CommandText = table.GetSqlWithParameters();
                        CommandPerformed += $"{table.GetSqlWithParameters()}{Environment.NewLine}{vars.ToString()}\r";
                        List<SqliteParameter> Params = GetCommandParameters(vars);
                        if (Params.Count > 0)
                        {
                            foreach (var p in Params)
                            {
                                command.Parameters.Add(p);
                            }
                        }
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
        //private void DoInsertCommand(SQLiteConnection sqlcon, string commandtext, SQLVarsVals Variables)
        //{
        //    SqliteConnection db = sqlcon.GetConnection();
        //    using (var command = db.CreateCommand())
        //    {
        //        command.CommandText = commandtext;
        //        List<SqliteParameter> Params = GetCommandParameters(Variables);
        //        if (Params.Count > 0)
        //        {
        //            foreach (var p in Params)
        //            {
        //                command.Parameters.Add(p);
        //            }
        //        }
        //        command.ExecuteNonQuery();
        //    }
        //}
        //private void DoInsertCommand(SQLiteConnection sqlcon, string commandtext, List<SQLVarsVals> Variables)
        //{
        //    SqliteConnection db = sqlcon.GetConnection();
        //    foreach (var vars in Variables)
        //    {
        //        using (var command = db.CreateCommand())
        //        {
        //            command.CommandText = commandtext;
        //            List<SqliteParameter> Params = GetCommandParameters(vars);
        //            if (Params.Count > 0)
        //            {
        //                foreach (var p in Params)
        //                {
        //                    command.Parameters.Add(p);
        //                }
        //            }
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}
        //private List<SqliteParameter> GetCommandParameters(SQLVarsVals Variables)
        //{
        //    List<SqliteParameter> param = new List<SqliteParameter>();
        //    for (int index = 0; index < Variables.Count(); index++)
        //        param.Add(new SqliteParameter($"@{Variables.GetName(index).Trim()}", Variables.GetValue(index)));
        //    return param;
        //}
    }
}
