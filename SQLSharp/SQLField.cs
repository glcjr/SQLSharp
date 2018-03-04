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
    /// <summary>
    /// A class that holds information about the fields for a table
    /// </summary>
    public class SQLField
    {
        private string fieldname;
        private string fieldtype;
        private int Length;
        private bool nullable;
        private bool autoincrement;
        private bool primarykey;
        private bool isunique;
        private string check = "";
        private string DefaultValue = "";

        /// <summary>
        /// A constructor that creates a SQLiteField to be used when creating a table
        /// </summary>
        /// <param name="fname">The name of the field to be created</param>
        /// <param name="ftype">The type of field to be created. Any valid SQL type can be used </param>
        /// <param name="len">The length of the field for character values like varchar</param>
        /// <param name="cannull">A bool variable that indicates if the field can contain a null</param>
        /// <param name="auto">a bool variable that indicates if the field value should be auto incremented</param>
        /// <param name="isprimary">A bool variable that indicates if the field is a primary key</param>
        public SQLField(string fname, string ftype, int len = 0, bool cannull = false, bool auto= false, bool isprimary = false, bool makeunique = false )
        {
            fieldname = fname;
            fieldtype = ftype;
            Length = len;
            nullable = cannull;
            autoincrement = auto;
            primarykey = isprimary;
            isunique = makeunique;
        }
       
        public void AddCheck(string operand, string value)
        {
            check = $"{fieldname}{operand}{value}";
        }
        /// <summary>
        /// A method that creates the SQL for creating the field in a a Create Table statement
        /// </summary>
        /// <returns></returns>
        public string GetSql()
        {
            string commandtext = $"{fieldname} {fieldtype}";
            if (!(Length == 0))
                commandtext += $"({Length})";
            if (nullable)
                commandtext += " NOT NULL";
            if (primarykey)
                commandtext += " PRIMARY KEY";
            if (autoincrement)
                commandtext += " AUTOINCREMENT";
            if (isunique)
                commandtext += " UNIQUE";
            if (check != string.Empty)
                commandtext += $" CHECK ({check})";
            if (DefaultValue != string.Empty)
                commandtext += $" DEFAULT {DefaultValue}";
            commandtext += ",";
            return commandtext;
        }
        public void SetDefaultValue(string value, bool isnum = false)
        {
            if (!(isnum))
                DefaultValue = $"'{value}'";
            else
                DefaultValue = value;
        }
        /// <summary>
        /// Returns the field name to the calling method
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return fieldname;
        }
        
    }
}
