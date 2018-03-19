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
    public class SQLAlterTable
    {
        string MainTable;
        string ModifyColumnName;
        string ActionToPerform;
        string DataType;
        string NewColumnName;
        string NewTableName;

        public SQLAlterTable(string table)
        {
            MainTable = table;
        }
        public SQLAlterTable(string table, string column) : this(table)
        {
            ModifyColumnName = column;
        }
        public SQLAlterTable(string table, string column, string action) : this(table, column)
        {
            ActionToPerform = action.ToUpper();
        }
        public SQLAlterTable(string table, string column, string action, string newtype) : this(table, column, action)
        {
            DataType = newtype;
        }
        public void MakeAdd()
        {
            ActionToPerform = "ADD";
        }
        public void MakeDrop()
        {
            ActionToPerform = "DROP";
        }
        public void MakeAlter()
        {
            ActionToPerform = "ALTER";
        }
        public void MakeRenameTable()
        {
            ActionToPerform = "RENAMETABLE";
        }
        public void SetNewDataType(string newtype)
        {
            DataType = newtype;
        }
        public void SetNewColumnName(string newname)
        {
            NewColumnName = newname;
        }
        public void SetNewTableName(string newname)
        {
            NewTableName = newname;
        }
      
        protected string GetAlterTable()
        {
            return $"ALTER TABLE {MainTable}";
        }
        protected string GetAction()
        {
            if (ActionToPerform.Equals("ADD"))
                return $"ADD {ModifyColumnName} {DataType}";
            else if (ActionToPerform.Equals("DROP"))            
                return $"DROP COLUMN {ModifyColumnName}";            
            else if (ActionToPerform.Equals("ALTER"))
                return $"ALTER COLUMN {ModifyColumnName} {DataType}";
            else if (ActionToPerform.Equals("RENAMETABLE"))
                return $"RENAME TO {NewTableName}";
            else
            {
                string action = $"{ActionToPerform} {ModifyColumnName}";
                if (!(DataType.Equals("")))
                    action += DataType;
                return action;
            }
        }
        protected string GetMySqlAction()
        {
            if (ActionToPerform.Equals("ADD"))
                return $"ADD {ModifyColumnName} {DataType}";
            else if (ActionToPerform.Equals("DROP"))
                return $"DROP COLUMN {ModifyColumnName}";
            else if (ActionToPerform.Equals("RENAMECOLUMN"))
                return $"CHANGE COLUMN {ModifyColumnName} {NewColumnName}";
            else if (ActionToPerform.Equals("ALTER"))
                return $"MODIFY COLUMN {ModifyColumnName} {DataType}";
            else if (ActionToPerform.Equals("RENAMETABLE"))
                return $"RENAME TO {NewTableName}";
            else
            {
                string action = $"{ActionToPerform} {ModifyColumnName}";
                if (!(DataType.Equals("")))
                    action += DataType;
                return action;
            }
        }
        public string GetSql()
        {
            return $"{GetAlterTable()} {GetAction()};";
        }
        public string GetMySql()
        {
            return $"{GetAlterTable()} {GetMySqlAction()};";
        }
    }
}
