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
    public class SQLSelectInTable:SQLJoinTable
    {
        SQLVar WhereInField;
        SQLVals WhereINValues = new SQLVals();
        SQLSelectTable SelectInTable;
        private bool usenot = false;
        public SQLSelectInTable(string table):base(table)
        { }
        public SQLSelectInTable(string table, bool unot = false) : base(table)
        {
            usenot = unot;
        }
        public SQLSelectInTable(string maintable, SQLVars selectfields, SQLWhereVars wherefields, bool unot = false) : base(maintable, selectfields, wherefields)
        {
            usenot = unot;
        }
        public void UseNot()
        {
            usenot = true;
        }
        public void UseNoNot()
        {
            usenot = false;
        }
        public void AddWhereInField(SQLVarVal field)
        {
            WhereInField = new SQLVar(field.GetName());
            WhereINValues.Add(new SQLVal(field.GetRawValue(), field.GetIsNum()));
            
        }
        public void AddWhereInField(string field1, string field2, bool isnum = false)
        {
            WhereInField = new SQLVar(field1);
            WhereINValues.Add(new SQLVal(field2, isnum));
        }
        public void AddWhereInField(string field)
        {
            WhereInField = new SQLVar(field);
        }
        public void AddWhereInValue(string value, bool isnum=false)
        {
            WhereINValues.Add(new SQLVal(value, isnum));
        }
        public void AddWhereInSelect(SQLSelectTable table)
        {
            SelectInTable = table;
        }
        public string GetNot()
        {
            if (usenot)
                return "NOT";
            else
                return "";
               
        }
        new protected string GetWhere()
        {
            string command = "";
            if ((WhereFields.Count() > 0) || (WhereINValues.Count() > 0) || SelectInTable != null)
            {
                command += $" WHERE ";
                if (WhereINValues.Count() > 0)
                    command += $"{WhereInField.GetName()} {GetNot()} IN ({WhereINValues.GetFieldValues()}) ";
                else if (SelectInTable != null)
                    command += $"{WhereInField.GetName()} {GetNot()} IN ({SelectInTable.GetSql()}) ";
                if (WhereFields.Count() > 0)
                    command += $"{WhereFields.GetFieldOperandValueasStringorNum()}";
            }
            return command;
        }
        public override string GetSql()
        {
            return $"{GetSelectFrom()}{GetJoinON()}{GetWhere()}{GetGroupBy()}{GetHaving()}{GetOrderBy()};";
        }
        public override string GetMySql()
        {
            return GetSql();
        }
        
        //new public string GetSql()
        //{
        //    string command = $"SELECT {GetDistinct()} {SelectFields.GetFieldNames()} FROM {MainTable}";
        //    if (JoinTables.Count == JoinFields.Count)
        //        for (int index = 0; index < JoinTables.Count; index++)
        //            command += $" {JoinFields[index].GetJoinType()} JOIN {JoinTables[index]} ON {JoinFields[index].GetJoinSQL()}";
        //    if ((WhereFields.Count() > 0) || (WhereINValues.Count() > 0) ||SelectInTable != null)
        //    {
        //        command += $" WHERE ";
        //        if (WhereINValues.Count() > 0)
        //            command += $"{WhereInField.GetName()} IN ({WhereINValues.GetFieldValues()}) ";
        //        else if (SelectInTable != null)
        //            command += $"{WhereInField.GetName()} IN ({SelectInTable.GetSql()}) ";
        //        if (WhereFields.Count() > 0)
        //        command += $"{WhereFields.GetFieldOperandValueasStringorNum()}";
        //    }
        //    if (OrderByFields.Count() > 0)
        //        command += $" ORDER BY {OrderByFields.GetFieldNames()}";
        //    command += ";";
        //    return command;

        //}
    }
}
