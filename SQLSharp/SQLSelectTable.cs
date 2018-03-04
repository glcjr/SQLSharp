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
    public class SQLSelectTable: ISQLSelectTable
    {
        protected string MainTable;
        protected SQLVars SelectFields = new SQLVars();
        protected SQLWhereVars WhereFields = new SQLWhereVars();
        protected SQLOrderVars OrderByFields = new SQLOrderVars();
        protected SQLVars GroupBy = new SQLVars();
        protected SQLHavingVars HavingFields = new SQLHavingVars();
        protected bool IsDistinct = false;

        public SQLSelectTable(string maintab)
        {
            MainTable = maintab;
        }
        public SQLSelectTable(string maintab, SQLVars selfied) : this(maintab)
        {
            SelectFields = selfied;
        }
        public SQLSelectTable(string maintab, string selectfield):this(maintab)
        {
            string[] args = selectfield.Split(',');
            foreach (var s in args)
                SelectFields.Add(new SQLVar(s.Trim()));
        }
        public SQLSelectTable(string maintab, string selectfield, SQLWhereVars wherefields): this(maintab, selectfield)
        {
         
            WhereFields = wherefields;
        }
        public SQLSelectTable(string maintab, string selectfield, string wherefield, string wherevalue, string operand = "=", string con = ""):this(maintab, selectfield)
        {
            if (wherefield != string.Empty)
                WhereFields.Add(new SQLWhereVar(con,wherefield, wherevalue, operand));
        }
        public SQLSelectTable(string maintab, SQLVars selfield, SQLWhereVars wherefield):this(maintab, selfield)
        {
            WhereFields = wherefield;
        }
        public SQLSelectTable(string maintab, SQLVars selfield, SQLWhereVars wherefield, SQLVars groupby) : this(maintab, selfield, wherefield)
        {
            GroupBy = groupby;
        }
        public SQLSelectTable(string maintab, SQLVars selfield, SQLWhereVars wherefield, SQLVars groupby, SQLOrderVars orderbyfields) : this(maintab, selfield, wherefield, groupby)
        {
            OrderByFields = orderbyfields;
        }
        public SQLSelectTable(SQLSelectTable table):this(table.GetMainTable(), table.GetSelectFields(), table.GetWhereFields(), table.GetGroupByFields(), table.GetOrderByFields())
        {
            IsDistinct = table.GetIsDistinct();
        }
        public void SetMainTable(string maintab)
        {
            MainTable = maintab;
        }
        public void AddSelectField(SQLVars field)
        {
            SelectFields.Add(field);
        }
        public void AddSelectField(string field1)
        {
            SelectFields.Add(new SQLVar(field1));
        }
        public void AddSelectField(params string[] fields)
        {
            foreach(var s in fields)
                 SelectFields.Add(new SQLVar(s));
        }
        public void AddWhereField(SQLWhereVar field)
        {
            WhereFields.Add(field);
        }
        public void AddWhereField(string field1, string field2, string con = "", string operand = " = ", bool isnum = false)
        {
            WhereFields.Add(new SQLWhereVar(con, field1, field2, operand, isnum));
        }
        public void AddOrderByField(string field1, string order)
        {
            OrderByFields.Add(new SQLOrderVar(field1, order));
        }
        public void AddOrderByField(SQLField field1, string order)
        {
            OrderByFields.Add(new SQLOrderVar(field1.GetName(), order));
        }
        public void AddOrderByField(SQLOrderVar field)
        {
            OrderByFields.Add(field);
        }
        public void AddGroupByField(SQLVar field)
        {
            GroupBy.Add(field);
        }
        public void AddGroupByField(SQLField field1)
        {
            GroupBy.Add(new SQLVar(field1.GetName()));
        }
        public void AddGroupByField(string field1)
        {
            GroupBy.Add(new SQLVar(field1));
        }
        public void AddHavingField(string function, SQLWhereVar variable)
        {
            HavingFields.Add(function, variable);
        }
        public void AddHavingField(string function, string con, string columnname, string val, string operand = " = ", bool isnum = false)
        {
            HavingFields.Add(function, con, columnname, val, operand, isnum);
        }
        public void AddHavingField(string function, string con, SQLVarVal field)
        {
            HavingFields.Add(function, con, field);
        }
        public string GetMainTable()
        {
            return MainTable;
        }
        public SQLVars GetSelectFields()
        {
            return SelectFields;
        }
        public SQLOrderVars GetOrderByFields()
        {
            return OrderByFields;
        }
        public bool GetIsDistinct()
        {
            return IsDistinct;
        }
        public SQLWhereVars GetWhereFields()
        {
            return WhereFields;
        }  
        public SQLVars GetGroupByFields()
        {
            return GroupBy;
        }
        public void MakeDistinct()
        {
            IsDistinct = true;
        }
        public void MakeNotDistinct()
        {
            IsDistinct = false;
        }
        protected string GetDistinct()
        {
            if (IsDistinct)
                return "DISTINCT";
            else
                return "";
        }
        protected string GetSelectFrom()
        {
            return $"SELECT {GetDistinct()} {SelectFields.GetFieldNames()} FROM {MainTable}";
        }
        protected string GetWhere()
        {
            if (WhereFields.Count() > 0)
                return $" WHERE {WhereFields.GetFieldOperandValueasStringorNum()}";
            else
                return "";
        }
        protected string GetWhereParam()
        {
            if (WhereFields.Count() > 0)
                return $" WHERE {WhereFields.GetWhereFieldEqualsParameter("@W")}";
            else
                return "";
        }
        protected string GetOrderBy()
        {
            if (OrderByFields.Count() > 0)
                return $" ORDER BY {OrderByFields.GetFieldNames()} ";
            else
                return "";
        }
        protected string GetGroupBy()
        {
            if (GroupBy.Count() > 0)
                return $" GROUP BY {GroupBy.GetFieldNames()}";
            else
                return "";
        }
        protected string GetHaving()
        {
            if (HavingFields.Count() > 0)
                return $" Having {HavingFields.GetHavingStatements()} ";
            else
                return "";
        }
        protected string GetHavingParam()
        {
            if (HavingFields.Count() > 0)
                return $" Having {HavingFields.GetHavingParamStatements("@H")} ";
            else
                return "";
        }
        public virtual string GetSql()
        {
            return $"{ GetSelectFrom()}{GetWhere()}{GetGroupBy()}{GetHaving()}{GetOrderBy()};";
            //string command = $"SELECT {GetDistinct()} {SelectFields.GetFieldNames()} FROM {MainTable}";
            //if (WhereFields.Count() > 0)
            //    command += $" WHERE {WhereFields.GetFieldOperandValueasStringorNum()}";
            //if (OrderByFields.Count() > 0)
            //    command += $" ORDER BY {OrderByFields.GetFieldNames()} ";
            //command += ";";
            //return command;

        }
        public virtual string GetSqlWithParameters()
        {
            return $"{ GetSelectFrom()}{GetWhereParam()}{GetGroupBy()}{GetHavingParam()}{GetOrderBy()};";
        }
        public SQLParamList GetParamList()
        {
            SQLParamList temp = new SQLParamList();
            if (WhereFields.Count() > 0)
                temp.Add(WhereFields.GetParameterList("@W"));
            if (HavingFields.Count() > 0)
                temp.Add(HavingFields.GetParameterList("@H"));
            return temp;
        }
    }
}
