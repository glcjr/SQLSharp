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
    public class SQLCreateIndex
    {
        private string IndexName;
        private string TableName;
        private SQLVars Columns;
        private bool unique = false;
        public SQLCreateIndex(string index, string table, bool isunique = false)
        {
            IndexName = index;
            TableName = table;
            unique = isunique;
        }
        public SQLCreateIndex(string index, string table, string column, bool isunique = false) : this(index, table, new SQLVar(column), isunique)
        {

        }
        public SQLCreateIndex(string index, string table, SQLVar column, bool isunique = false) : this(index, table, isunique)
        {

            Columns.Add(column);
        }
        public SQLCreateIndex(string index, string table, SQLVars columns, bool isunique = false) : this(index, table, isunique)
        {
            Columns.Add(columns);
        }
        private string GetUniquePhrase()
        {
            if (unique)
                return "UNIQUE";
            else
                return "";
        }
        protected string GetCreateIndex()
        {
            return $"CREATE {GetUniquePhrase()} INDEX {IndexName}";
        }
        protected string GetON()
        {
            return $"ON {TableName} ({Columns.GetFieldNames()})";
        }

        public string GetSql()
        {
            return $"{GetCreateIndex()} {GetON()};";
        }
    }
}
