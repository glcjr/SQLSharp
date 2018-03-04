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
    public class SQLSelectExistsTable:SQLJoinTable
    {
        
        private SQLJoinTable WhereTable;

        public SQLSelectExistsTable(string table):base(table)
        {            
        }
        public SQLSelectExistsTable(string table, SQLVars selectfields): base(table, selectfields)
        {            
        }
        public SQLSelectExistsTable(string table, SQLVars selectfields, SQLJoinTable wheretable):this(table, selectfields)
        {                   
            WhereTable = wheretable;
        }
        public SQLSelectExistsTable(string table, SQLVars selectfields, SQLSelectTable wheretable): this(table, selectfields, new SQLJoinTable(wheretable))
        {
        }
        public void SetWhereTable(string table, SQLVars selectfields)
        {
            WhereTable = new SQLJoinTable(table, selectfields);
        }
        public void SetWhereTable(SQLJoinTable table)
        {
            WhereTable = new SQLJoinTable(table);
        }
        public void SetWhereTable(SQLSelectTable table)
        {
            WhereTable = new SQLJoinTable(table);
        }
        new protected string GetWhere()
        {
            return $"WHERE EXISTS ({WhereTable.GetSql()})";
        }
        public override string GetSql()
        {
            return $"{ GetSelectFrom()} {GetJoinON()} {GetWhere()} {GetGroupBy()} {GetHaving()} {GetOrderBy()};";
        }
    }
}
