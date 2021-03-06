﻿using System;
using System.Collections.Generic;
using System.Text;

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
namespace SQLSharp
{
    public class SQLDeleteTable
    {
        private string TableName;
        private SQLWhereVars WhereFields = new SQLWhereVars();
        bool AllFields = false;

        public SQLDeleteTable(string table)
        {
            TableName = table;
        }
        public SQLDeleteTable(string table, SQLWhereVars where):this(table)
        {
            WhereFields = where;
        }
        public SQLDeleteTable(string table, SQLWhereVars where, bool allfields): this(table, where)
        {
            AllFields = allfields;
        }
        public SQLDeleteTable(string table, string field, string value, string operand = " = ", bool isnum = false):this(table, new SQLWhereVars(field, value, operand, isnum))
        {
        }
        public void DeleteAllRecords(bool deleteall)
        {
            AllFields = deleteall;
        }
        public void AddWhereField(SQLVarVal field)
        {
            WhereFields.Add(field);
        }
        public void AddWhereField(string field1, string field2, string con = "", string operand = " = ", bool isnum = false)
        {
            WhereFields.Add(new SQLWhereVar(con, field1, field2, operand, isnum));
        }
        private string DeleteAll()
        {
            if (AllFields)
                return " * ";
            else
                return "";
        }
        public string GetSql()
        {
            string command = "";
            if ((WhereFields.Count() > 0) || (AllFields))
            {
                command = $"DELETE {DeleteAll()} FROM {TableName}";
                if (WhereFields.Count() > 0)
                    command += $"WHERE {Utilities.RemoveLastComma(WhereFields.GetFieldOperandValueasStringorNum())};";
            }
            return command;
        }
        public string GetSqlWithParameters()
        {
            string command = "";
            if ((WhereFields.Count() > 0) || (AllFields))
            {
                command = $"DELETE {DeleteAll()} FROM {TableName}";
                if (WhereFields.Count() > 0)
                    command += $"WHERE {Utilities.RemoveLastComma(WhereFields.GetWhereFieldEqualsParameter())};";
            }
            return command;
        }
        public SQLParamList GetParamList()
        {
            SQLParamList temp = new SQLParamList();
            if (WhereFields.Count() > 0)
                temp.Add(WhereFields.GetParameterList());           
            return temp;
        }
        public string GetMySql()
        {
            return GetSql();
        }
        public string GetMySqlWithParameters()
        {
            return GetSqlWithParameters();
        }        
    }
}
