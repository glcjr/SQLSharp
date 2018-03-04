using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
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
    public class SQLiteSelect
    {
        private SqliteDataReader dread;
        private SQLiteConnection sqlcon;

        private bool Success = false;
        private string CommandPerformed = "";
        private string SQLPerformed = "";

        /// <summary>
        /// Constructor that creates a SQLiteSelect variable with a connection to an SQLite db
        /// </summary>
        /// <param name="con">The connection to the SQLite db</param>
        public SQLiteSelect(SQLiteConnection con)
        {
            sqlcon = con;
        }
        /// <summary>
        /// Method that returns the DbDataReader that contains the object that results from the SQL Select statement that is run
        /// </summary>
        /// <returns></returns>
        public SqliteDataReader GetReader()
        {
            return dread;
        }
        /// <summary>
        /// Method that returns a datatable with the results of the SQL Select statement that was run
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            
            DataTable dt = new DataTable();
            SqliteDataReader dr = GetReader();
            for (int index = 0; index < dr.FieldCount; index++)
                dt.Columns.Add(dr.GetName(index));
           
            
            while (dr.Read())
                dt.Rows.Add(dr);
            return dt;
            
        }
        //Not currently supported in Net Standard 2.0. Should be eventually based on what I've read but do not expect it to work for now.
        /// <summary>
        /// Returns the Schema of the data in the form of a table scheme
        /// </summary>
        /// <returns></returns>
        //public DataTable GetSchemaTable()
        //{
        //    SqliteDataReader dr = GetReader();
        //    return dr.GetSchemaTable();
        //}
        /// <summary>
        /// Returns the first item that the select statement producted as a DataRow
        /// </summary>
        /// <returns></returns>
        public DataRow GetRow()
        {            
            return GetTable().Rows[0];
        }
        public int Count()
        {
            int count = 0;
            while (dread.Read())
                count++;
            return count;
        }
        public string GetFields()
        {
            string temp = "";
            SqliteDataReader dr = GetReader();
            for (int index = 0; index < dr.FieldCount; index++)
                temp += $"{dr.GetName(index)},";
            return Utilities.RemoveLastComma(temp);
        }
        
        /// <summary>
        /// The methods that takes a SQL Select statement and performs the select statement on the db
        /// It stores the result in the DbDataReader
        /// </summary>
        /// <param name="sqlcommand">A standard SQl Select statement to be performed on the db</param>
        public void DoQuery(string sqlcommand)
        {            
            DoSelectCommand(sqlcommand);
        }
        public void DoQuery(SQLSpecialSelectTable table)
        {
            DoSelectCommand(table);
        }
        public void DoQuery(SQLSelectInTable table)
        {
            DoSelectCommand(table.GetSql());
        }
        /// <summary>
        /// The method builds a SQL Select statement to perform on the SQLite DB
        /// </summary>
        /// <param name="TableName">The table name to be queried</param>
        /// <param name="SelectField">The field to select in the query</param>
        /// <param name="Variables">Contains the variables and values for the WHERE portion of the query</param>
        /// <param name="separator">In case a different separator is wanted for the query than a comma</param>
        public void DoQuery(string TableName, string SelectField, SQLWhereVars Variables, string separator=", ")
        {
            SQLSelectTable table = new SQLSelectTable(TableName, SelectField, Variables);

            //string command = "SELECT " + SelectField + " FROM " + TableName;
            //command += " WHERE ";
            //command += Variables.GetWhereFieldEqualsValue();
            //command += ";";
            DoSelectCommand(table);// command, Variables);
        }
        /// <summary>
        /// This is the method for any select statements that include a join.
        /// It uses the SQLiteJoinTable class which holds all the information for the join
        /// </summary>
        /// <param name="JoinTable">This holds all the information about the Join and provides the SQL select statement for
        /// it. </param>
        public void DoQuery(SQLJoinTable JoinTable)
        {
            // DoSelectCommand(JoinTable.GetSql(), JoinTable.GetWhereFields());
            DoSelectCommand(JoinTable);
        }
        public void DoQuery(SQLSelectTable SelectTable)
        {
            // DoSelectCommand(SelectTable.GetSql(), SelectTable.GetWhereFields());
            DoSelectCommand(SelectTable);
        }
        public void DoQuery(SQLSelectBetweenTable BetweenTable)
        {
            DoSelectCommand(BetweenTable.GetSql());
        }
        /// <summary>
        /// This method builds a SQL Select statement to perfrom on the SQLite DB
        /// </summary>
        /// <param name="TableName">The name of the table to be queried</param>
        /// <param name="SelectFields">A variable that contains multiple fields to select from the table</param>
        /// <param name="Variables">Contains the variables and values for the WHERE portion of the query</param>
        /// <param name="separator">In case a different separator is wanted for the query than a comma</param>
        public void DoQuery(string TableName, SQLVars SelectFields, SQLWhereVars Variables, string separator=", ")
        {
            SQLSelectTable table = new SQLSelectTable(TableName, SelectFields, Variables);
            DoSelectCommand(table);//.GetSql(), table.GetWhereFields());
            //string fields = SelectFields.GetFieldNames(separator);
            //DoQuery(TableName, fields, Variables, separator);
        }
        /// <summary>
        /// This method builds a SQL Select statment to perform on the SQLite DB
        /// </summary>
        /// <param name="TableName">The name of the table to be queried</param>
        /// <param name="SelectField">The fields to be selected in the table</param>
        /// <param name="column">Field to be used in a WHERE statement</param>
        /// <param name="columnvalue">Value to be looked for</param>
        public void DoQuery(string TableName, string SelectField, string column = "", string columnvalue = "", string operand = " = ")
        {
            SQLSelectTable table = new SQLSelectTable(TableName, SelectField, column, columnvalue, operand);
            DoSelectCommand(table);
            //string commandtext = "SELECT " + SelectField + " FROM " + TableName;
            //if ((column != String.Empty) && (columnvalue != String.Empty))
            //    commandtext += " WHERE " + column + operand + "@" + column;
            //commandtext += ";";
            //if ((column != String.Empty) && (columnvalue != String.Empty))
            //{
            //    SQLVarsVals Vars = new SQLVarsVals(column, columnvalue);                
            //    DoSelectCommand(commandtext, Vars);
            //}
            //else
            //    DoSelectCommand(commandtext);
        }
        private void DoSelectCommand(ISQLSelectTable table)
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
                    dread = command.ExecuteReader();
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
        }
        private void DoSelectCommand(string commandtext, SQLVarsVals Variables)
        {
            CommandPerformed = commandtext;
            SQLPerformed = commandtext;
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                using (SqliteCommand command = db.CreateCommand())
                {
                    command.CommandText = commandtext;
                    List<SqliteParameter> Params = GetCommandParameters(Variables);
                    if (Params.Count > 0)
                    {
                        foreach (var p in Params)
                        {
                            command.Parameters.Add(p);
                        }
                    }
                    dread = command.ExecuteReader();
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
        }
        private void DoSelectCommand(string commandtext, SQLWhereVars Variables)
        {
            DoSelectCommand(commandtext, Variables.GetVarsVals());

        }
        /// <summary>
        /// Creates the Parameter list to be added to a query
        /// </summary>
        /// <returns></returns>
        private List<SqliteParameter> GetCommandParameters(SQLVarsVals Variables)
        {
            List<SqliteParameter> param = new List<SqliteParameter>();
            for (int index = 0; index < Variables.Count(); index++)
                param.Add(new SqliteParameter("@" + Variables.GetName(index), Variables.GetValue(index)));
            return param;
        }
        private List<SqliteParameter> GetCommandParameters(SQLParamList Params)
        {
            List<SqliteParameter> param = new List<SqliteParameter>();
            for (int index = 0; index < Params.Count(); index++)
                param.Add(new SqliteParameter(Params.GetParameter(index), Params.GetValue(index)));
            return param;
        }
        private void DoSelectCommand(string commandtext)
        {
            CommandPerformed = commandtext;
            SQLPerformed = commandtext;
            try
            {
                SqliteConnection db = sqlcon.GetConnection();
                using (var command = db.CreateCommand())
                {
                    command.CommandText = commandtext;
                    dread = command.ExecuteReader();
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
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
        //public void doquery(string TableName, List<string> SelectField, List<string> columns, List<string> columnvalues, string separator = ", ")
        //{
        //    doquery(TableName, unpacklist(SelectField, separator), columns, columnvalues, separator);
        //}
        //public void doquery(string TableName, List<string> SelectField, string column = "", string columnvalue = "", string separator=", ")
        //{
        //    doquery(TableName, unpacklist(SelectField, separator), column, columnvalue);
        //}
        //public void doquery(string TableName, string SelectField, List<string> columns, List<string> columnvalues, string separator = ", ")
        //{
        //    string command = "SELECT " + SelectField + " FROM " + TableName;
        //    command += " WHERE ";
        //    command += unpacklists(columns, columnvalues, separator);
        //    command += ";";
        //    doselectcommand(command, columns, columnvalues);
        //}
        //private void doselectcommand(string commandtext, List<string> columns, List<string> columnvalues)
        //{
        //    DbConnection db = sqlcon.getconnection();
        //    using (DbCommand command = db.CreateCommand())
        //    {
        //        command.CommandText = commandtext;
        //        for (int index = 0; index < columns.Count; index++)
        //        {
        //            command.Parameters.Add(new SqliteParameter("@" + columns[index], columnvalues[index]));
        //        }
        //        dread = command.ExecuteReader();
        //    }
        //}
        //private string unpacklists(List<string> fields, List<string> values, string separator)
        //{
        //    if ((fields.Count > 0) && (fields.Count == values.Count))
        //    {
        //        string com = fields[0] + " = @" + fields[0];
        //        for (int index = 1; index < fields.Count; index++)
        //            if ((fields[index] != String.Empty) && (values[index] != String.Empty))
        //                com += separator + fields[index] + " = @" + fields[index];
        //        return com;
        //    }
        //    else
        //        return "";
        //}
        //private static string unpacklist(List<string> fields, string separator)
        //{
        //    if (fields.Count > 0)
        //    {
        //        string com = fields[0];
        //        for (int index = 1; index < fields.Count; index++)
        //            if (fields[index] != String.Empty)
        //                com += separator + fields[index];
        //        return com;
        //    }
        //    else
        //        return "";
        //}
    }
}
