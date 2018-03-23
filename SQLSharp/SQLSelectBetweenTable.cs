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
    public class SQLSelectBetweenTable : SQLJoinTable
    {
        private SQLVar BetweenField;
        private SQLVal Between1;
        private SQLVal Between2;
        private bool usenot = false;

        public SQLSelectBetweenTable(string tablename) : base(tablename)
        {
        }

        public SQLSelectBetweenTable(string maintable, SQLVars selectfields, SQLWhereVars wherefields, bool unot = false) : base(maintable, selectfields, wherefields)
        {
            usenot = unot;
        }
        public void AddBetweenField(string field)
        {
            BetweenField = new SQLVar(field);
        }
        public void AddBetweenValue1(string value, bool isnum = false)
        {
            Between1 = new SQLVal(value, isnum);
        }
        public void AddBetweenValue2(string value, bool isnum = false)
        {
            Between2 = new SQLVal(value, isnum);
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
            if ((WhereFields.Count() > 0) || (BetweenField != null))
            {
                command += $" WHERE ";
                if (BetweenField != null)
                    command += $"{BetweenField.GetName()} {GetNot()} BETWEEN ({Between1.GetValue()} AND {Between2.GetValue()}";
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
        
    }
}
