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
    
    public class SQLSelectIntoTable:SQLJoinTable
    {
       string DestinationTable = "";
        string ExternalDB = "";
        public SQLSelectIntoTable(string nm):base(nm)
        {
        }
        public SQLSelectIntoTable(string nm, string destinationtable, string externaldb = ""):base(nm)
        {
            DestinationTable = destinationtable;
            ExternalDB = externaldb;
        }
        public SQLSelectIntoTable(SQLSelectTable table, string destinationtable="", string externaldb=""):base(table)
        {
            DestinationTable = destinationtable;
            ExternalDB = externaldb;
        }
        public SQLSelectIntoTable(string maintable, SQLVars selectfields, SQLWhereVars wherefields, string destinationtable="", string externaldb="") : base(maintable, selectfields, wherefields)
        {
            DestinationTable = destinationtable;
            ExternalDB = externaldb;
        }
        public SQLSelectIntoTable(string maintable, List<string> jointables, List<SQLJoinFields> joinfields, SQLVars selectfields, SQLWhereVars wherefields, string destinationtable="", string externaldb="") : base(maintable, jointables,joinfields, selectfields, wherefields)
        {
            DestinationTable = destinationtable;
            ExternalDB = externaldb;
        }
        public void SetDestinationTable(string destinationtable)
        {
            DestinationTable = destinationtable;
        }
        public void SetExternalDB(string externaldb)
        {
            ExternalDB = externaldb;
        }
        public void SetDestination(string destinationtable, string externaldb)
        {
            DestinationTable = destinationtable;
            ExternalDB = externaldb;
        }
        public string GetINTO()
        {
            string command = $" INTO {DestinationTable}";
            if (!(ExternalDB.Equals("")))
                command += $" IN {ExternalDB}";
            return command;
            
        }
        new protected string GetSelectFrom()
        {
            return $"SELECT {GetDistinct()} {SelectFields.GetFieldNames()} {GetINTO()} FROM {MainTable}";
        }
        public override string GetSql()
        {
            return $"{GetSelectFrom()}{GetWhere()}{GetGroupBy()}{GetHaving()}{GetOrderBy()};";
        }
        public override string GetMySql()
        {
            return GetSql();
        }
        
    }
}
