using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
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
    /// <summary>
    /// Class for easily creating a SQLite Table 
    /// </summary>
    public class SQLiteCreate
    {
        private bool Success = false;
        private string CommandPerformed = "";
        private string SQLPerformed = "";

        /// <summary>
        /// This constructor creates a table with the connection that is passed to it. The name of the table is passed into it
        /// A long with a List of Fields, their type and whether or not the fields are nullable, the primary key and if they should
        /// be auto incredmented. In addition the length that is to be saved for each field for those fieldtypes that need it.
        /// All the lists need to be the same size.
        /// </summary>
        /// <param name="sqlcon">This is the connection to the SQLite DB</param>
        /// <param name="tablename">The name of the table to create</param>
        /// <param name="Fields">A list of fields to create</param>
        /// <param name="FieldType">A list of the types the fields are</param>
        /// <param name="NotNullable">A list of whether the field is nullable</param>
        /// <param name="Length">A list of the length of the different fields</param>
        /// <param name="IsPrimaryKey">A list of whether the field is a primary key</param>
        /// <param name="AutoIncrement">A list of whether a field should be autoincremented</param>
        public SQLiteCreate(SQLiteConnection sqlcon, string tablename, List<string> Fields, List<string> FieldType, 
            List<bool> NotNullable, List<string> Length, List<bool> IsPrimaryKey, List<bool> AutoIncrement )
        {
            try
            {
                SQLCreateTable table = new SQLCreateTable(tablename);
                for (int index = 0; index < Fields.Count; index++)
                    table.AddField(Fields[index], FieldType[index], Int32.Parse(Length[index]), NotNullable[index], 
                        AutoIncrement[index], IsPrimaryKey[index]);
                DoCreateCommand(sqlcon, table.GetCreateSql());
            }
            catch
            { }
        }
        /// <summary>
        /// This constructor creates a table with the connection with the table name and also a list of the fields with their 
        /// values.
        /// The SQLiteField name contains all the information you need for the Fields. Such as their type, nullable, length,
        /// is it the primary key, and to autoincrement them
        /// </summary>
        /// <param name="sqlcon">This is the connection to the SQLite DB</param>
        /// <param name="tablename">The name of the table to create</param>
        /// <param name="Fields">A variable that contains a class that contains all the information about the fields to be 
        /// created</param>
        public SQLiteCreate(SQLiteConnection sqlcon, string tablename, List<SQLField> Fields)
        {
            SQLCreateTable table = new SQLCreateTable(tablename, Fields);
            DoCreateCommand(sqlcon, table.GetCreateSql());            
        }
        /// <summary>
        /// This is the standard constructor that takes a Create Table Sql statement and applies it to the Sqlite database. 
        /// It will create the table using standard SQL.
        /// </summary>
        /// <param name="sqlcon">This is the connection to the SQLite db</param>
        /// <param name="sqlcommand">A string with a standard SQL statement for creating a table</param>
        public SQLiteCreate(SQLiteConnection sqlcon, string sqlcommand)
        {
            if (sqlcommand.ToUpper().Contains("CREATE TABLE"))            
                DoCreateCommand(sqlcon, sqlcommand);             
        }
        /// <summary>
        /// This constructor creates a table that has been set up with the SQLiteTable variable. It contains all the information
        /// including the table name, the fields and their types.
        /// </summary>
        /// <param name="sqlcon">This is the connection to the SQlite DB</param>
        /// <param name="table">This contains a class of everything concerning the table to create</param>
        public SQLiteCreate(SQLiteConnection sqlcon, SQLCreateTable table)
        {
            DoCreateCommand(sqlcon, table.GetCreateSql());
        }
        private void DoCreateCommand(SQLiteConnection sqlcon, string commandtext)
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
        public bool GetSuccess()
        {
            return Success;
        }
        public string GetCommmandPerformed()
        {
            return CommandPerformed;
        }
        public string GetSQLPerformed()
        {
            return SQLPerformed;
        }
    }
}
