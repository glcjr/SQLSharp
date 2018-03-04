using System;
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
    public class SQLInsertTableMultiValues
    {
        private string TableName;
        private SQLVars Fields = new SQLVars();
        private List<SQLVals> Values = new List<SQLVals>();

        public SQLInsertTableMultiValues(string tablename)
        {
            TableName = tablename;
        }
        public SQLInsertTableMultiValues(string tablename, SQLVars fields):this(tablename)
        {
            Fields = fields;
        }
        public SQLInsertTableMultiValues(string tablename, SQLVars fields, List<SQLVars> Values):this(tablename, fields)
        {
            foreach (var v in Values)
                if (v.Count() == Fields.Count())
                    Values.Add(v);            
        }
        public void SetFields(SQLVars fields)
        {
            Fields = fields;
        }
        public void SetFields(params SQLVar[] fields)
        {
            foreach (var f in fields)
                Fields.Add(f);
        }
        public void SetFields(params string[] fields)
        {
            foreach (var f in fields)
                Fields.Add(new SQLVar(f));
        }
        public void AddValues(params SQLVal[] values)
        {
            if (Fields.Count() == values.Length)
                Values.Add(new SQLVals(values));
        }
        public void AddValues(params string[] values)
        {
            if (Fields.Count() == values.Length)
            {
                SQLVals temp = new SQLVals();
                temp.Add(values);
                Values.Add(temp);
            }
        }
        public List<SQLVarsVals> GetVariables()
        {
            List<SQLVarsVals> temp = new List<SQLVarsVals>();

            string[] fields = Fields.GetFieldNames().Split(',');
            for (int index = 0; index < Values.Count; index++)
            {
                SQLVarsVals var = new SQLVarsVals();
                for (int index2 = 0; index2 < fields.Length; index2++)
                    var.Add(fields[index2], Values[index].GetFieldValues(index2).GetRawValue());
                temp.Add(var);

            }
            return temp;
        }
        public string GetSql()
        {
            string command = $"INSERT INTO {TableName} ({Fields.GetFieldNames()}) VALUES ";
            foreach (var v in Values)
                command += $"({v.GetFieldValues()}), ";
            command = Utilities.RemoveLastComma(command) + ";";
            return command;
        }
        public string GetSqlwithParameters()
        {
            string fields = Fields.GetFieldNames();
            string pfields = Fields.GetParamFieldNames();
            return $"INSERT INTO {TableName} ({fields}) VALUES ({pfields});";
        }
       
        public List<SQLParamList> GetParams()
        {
            List<SQLParamList> temp = new List<SQLParamList>();
            string[] fields = Fields.GetFieldNames().Split(',');
            for (int index = 0; index < Values.Count; index++)
            {
                SQLParamList var = new SQLParamList();
                for (int index2 = 0; index2 < fields.Length; index2++)
                    var.Add($"@{fields[index2].Trim()}", Values[index].GetFieldValues(index2).GetRawValue());
                temp.Add(var);

            }
            return temp;
        }
    }
}
