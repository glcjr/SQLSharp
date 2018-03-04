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
    public class SQLSpecialSelectTable: SQLSelectTable, ISQLSelectTable
    {
        string SpecialAction = "COUNT";
        public SQLSpecialSelectTable(string MainTable):base(MainTable)
        { }
        public void MakeCount()
        {
            SpecialAction = "COUNT";
        }
        public void MakeAverage()
        {
            SpecialAction = "AVG";
        }
        public void MakeSUM()
        {
            SpecialAction = "SUM";
        }
        public void MakeMIN()
        {
            SpecialAction = "MIN";
        }
        public void MakeMAX()
        {
            SpecialAction = "MAX";
        }
        new public void AddSelectField(SQLVars field)
        {
            SelectFields = new SQLVars();
            SelectFields.Add(field);
        }
        new public void AddSelectField(string field1)
        {
            SelectFields = new SQLVars();
            SelectFields.Add(new SQLVar(field1));
        }
        [Obsolete("This method only adds the first item because Special Select can only have one Select Field")]
        new public void AddSelectField(params string[] fields)
        {
            SelectFields = new SQLVars();
            SelectFields.Add(new SQLVar(fields[0]));
        }
        new protected string GetSelectFrom()
        {
            return $"SELECT {GetDistinct()} {SpecialAction}({SelectFields.GetFieldNames()}) FROM {MainTable}";
        }
        new public virtual string GetSql()
        {
            return $"{ GetSelectFrom()} { GetWhere()} {GetGroupBy()} {GetHaving()} {GetOrderBy()};";
            //string command = $"SELECT {GetDistinct()} {SpecialAction}({SelectFields.GetFieldNames()}) FROM {MainTable}";
            //if (WhereFields.Count() > 0)
            //    command += $" WHERE {WhereFields.GetFieldOperandValueasStringorNum()}";
            //if (OrderByFields.Count() > 0)
            //    command += $" ORDER BY {OrderByFields.GetFieldNames()} ";
            //command += ";";
            //return command;

        }
        public override string GetSqlWithParameters()
        {
            return $"{ GetSelectFrom()}{GetWhereParam()}{GetGroupBy()}{GetHavingParam()}{GetOrderBy()};";
        }
    }
}
