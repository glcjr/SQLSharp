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
    public class SQLVars
    {
        protected List<SQLVar> Fields = new List<SQLVar>();

        public SQLVars()
        {
            Fields = new List<SQLVar>();
        }
        /// <summary>
        /// Creates a list of the fieldnames from the list of variables with values
        /// </summary>
        /// <param name="separator">Contains the char to use as a separater</param>
        /// <returns></returns>
        /// 
        public SQLVars(SQLVar[] fields)
        {
            foreach (var f in fields)
                Fields.Add(f);
        }
        public void Add(SQLVars vars)
        {
            foreach (var v in vars.GetFields())
                Fields.Add(v);
        }
        public void Add(params string[] fields)
        {
            foreach (var f in fields)
                Fields.Add(new SQLVar(f));
        }
        public void Add(SQLVar field)
        {
            Fields.Add(field);
        }
        
        public List<SQLVar> GetFields()
        {
            return Fields;
        }
        public string GetFieldNames(string separator = ", ")
        {
            string temp = "";
            foreach (var v in Fields)
                temp += v.GetName() + separator;
            return Utilities.RemoveLastComma(temp);
        }
        public string GetParamFieldNames(string prefix="@", string separator = ", ")
        {
            string temp = "";
            foreach (var v in Fields)
                temp += $"{prefix}{v.GetName().Trim()}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
        public int Count()
        {
            return Fields.Count;
        }
    }
}
