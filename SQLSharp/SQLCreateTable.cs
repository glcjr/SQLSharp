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
    public class SQLCreateTable
    {
        private string TableName;
        private List<SQLField> Fields = new List<SQLField>();

        /// <summary>
        /// Constructor that allows the naming of a potential table
        /// </summary>
        /// <param name="nm">The name to give the table</param>
        public SQLCreateTable(string nm)
        {
            TableName = nm;
            Fields = new List<SQLField>();
        }
        /// <summary>
        /// A constructor to begin setting up a table with a list of Fields
        /// </summary>
        /// <param name="nm">The name to give the table</param>
        /// <param name="fds">Variable that contains the fields for the table.</param>
        public SQLCreateTable(string nm, List<SQLField> fds):this(nm)
        {            
            Fields = fds;
        }
        /// <summary>
        /// Adds a field to the table
        /// </summary>
        /// <param name="field">Contains all the information for the field to be added</param>
        public void AddField(SQLField field)
        {
            Fields.Add(field);
        }
        /// <summary>
        /// Adds a field to the table
        /// </summary>
        /// <param name="fname">The name of the field</param>
        /// <param name="ftype">The type of the field</param>
        /// <param name="len">The length of the field</param>
        /// <param name="cannull">Bool that indicates if the field can be nulled</param>
        /// <param name="auto">bool that indicates if the field should be autoincremented</param>
        /// <param name="isprimary">bool that indidcates if the field is the primary key</param>
        public void AddField(string fname, string ftype, int len = 0, bool cannull = false, bool auto = false, bool isprimary = false)
        {
            Fields.Add(new SQLField(fname, ftype, len, cannull, auto, isprimary));
        }
        /// <summary>
        /// Creates the SQL to create the table based on the table name and the fields that have been added
        /// </summary>
        /// <returns>returns the SQL statement that creates the table</returns>
        public string GetCreateSql()
        {
            string commandtext = $"CREATE TABLE IF NOT EXISTS {TableName} (";
            for (int index = 0; index < Fields.Count; index++)
            {
                commandtext += Fields[index].GetSql();
            }
            commandtext = Utilities.RemoveLastCommaAddPar(commandtext) + ";";
            return commandtext;
        }
        
    }
}
