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
    public class SQLInsertIntoSelectTable
    {
        string IntoTableName;
        SQLVars Fields;
        SQLJoinTable SelectTable;

        public SQLInsertIntoSelectTable(string nm)
        {
            IntoTableName = nm;
        }
        public SQLInsertIntoSelectTable(string nm, SQLVars fields):this(nm)
        {
            Fields = fields;
        }
        public SQLInsertIntoSelectTable(string nm, SQLVars fields, SQLJoinTable selectable): this(nm, fields)
        {
            SelectTable = selectable;
        }
        public SQLInsertIntoSelectTable(string nm, SQLVars fields, SQLSelectTable selectable) : this(nm, fields)
        {
            SelectTable = new SQLJoinTable(selectable);
        }
        public void AddField(params SQLVar[] vars)
        {
            foreach (var v in vars)
                Fields.Add(v);
        }
        public void AddField(string field)
        {
            Fields.Add(new SQLVar(field));
        }
        public void AddField(params SQLVarVal[] vars)
        {
            foreach (var v in vars)
                Fields.Add(v.GetName());
        }
        public void AddField(SQLVars vars)
        {
            Fields.Add(vars);
        }
        public void AddSelectTable(SQLSelectTable table)
        {
            SelectTable = new SQLJoinTable(table);
        }
        public void AddSelectTable(SQLJoinTable table)
        {
            SelectTable = new SQLJoinTable(table);
        }
        public void AddSelectTable(string maintable, SQLVars selectfields, SQLWhereVars wherefields)
        {
            SelectTable = new SQLJoinTable(maintable, selectfields, wherefields);
        }
        public void AddSelectTable(string maintable, List<string> jointables, List<SQLJoinFields> joinfields, SQLVars selectfields, SQLWhereVars wherefields)
        {
            SelectTable = new SQLJoinTable(maintable, jointables, joinfields, selectfields, wherefields);
        }
        public void MakeSelectTableDistinct()
        {
            SelectTable.MakeDistinct();
        }
        public void MakeSelectTableNotDistinct()
        {
            SelectTable.MakeNotDistinct();
        }
        public void AddJoinTable(string jt, SQLJoinFields jf)
        {
            SelectTable.AddJoinTable(jt, jf);
        }
        public void AddJoinTable(string jt, string field1, string field2, string operand = " = ")
        {
            SelectTable.AddJoinTable(jt, field1, field2, operand);
        }
            protected string GetInsert()
        {
            return $"INSERT INTO {IntoTableName} ({Fields.GetFieldNames()})";
        }
        public string GetSql()
        {
            return $"{GetInsert()} {SelectTable.GetSql()}";
        }
    }
}
